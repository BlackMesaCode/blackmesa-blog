﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;
using System.Xml;
using BlackMesa.Website.Main.Areas.Learning.ViewModels;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;
using dotless.Core.Parser.Infrastructure.Nodes;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    [Authorize]
    public class SelectionController : BaseController
    {

        private readonly LearningRepository _learningRepo = new LearningRepository(new BlackMesaDbContext());


        public ActionResult AddFolder(string folderId, string returnFolderId)
        {
            var folder = _learningRepo.GetFolder(folderId);
            _learningRepo.SelectFolder(folder);
            return RedirectToAction("Details", "Folder", new {id = returnFolderId, deSelect = false });
        }

        public ActionResult RemoveFolder(string folderId, string returnFolderId)
        {
            var folder = _learningRepo.GetFolder(folderId);
            _learningRepo.DeSelectFolder(folder);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId, deSelect = false });
        }

        public ActionResult AddCard(string cardId, string returnFolderId)
        {
            var card = _learningRepo.GetCard(cardId);
            _learningRepo.SelectCard(card);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId, deSelect = false });
        }

        public ActionResult RemoveCard(string cardId, string returnFolderId)
        {
            var card = _learningRepo.GetCard(cardId);
            _learningRepo.DeSelectCard(card);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId, deSelect = false });
        }


        public ActionResult Delete(string folderId)
        {
            var folder = _learningRepo.GetFolder(folderId);

            int affectedCardsCount = 0;
            _learningRepo.GetCardCount(folder, ref affectedCardsCount, true, true);

            var viewModel = new DeleteViewModel
            {
                Id = folder.Id.ToString(),
                AffectedCardsCount = affectedCardsCount,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeleteViewModel viewModel)
        {
            var folder = _learningRepo.GetFolder(viewModel.Id);
            var parentFolderId = folder.ParentFolder != null ? folder.ParentFolder.Id.ToString() : String.Empty;
            if (folder.IsSelected)
            {
                _learningRepo.RemoveFolder(folder.Id.ToString());
                if (!String.IsNullOrEmpty(parentFolderId))
                    return RedirectToAction("Details", "Folder", new {id = parentFolderId});
                else
                    return RedirectToAction("Index", "Folder");
            }
            else
            {
                var selectedCards = folder.Cards.Where(u => u.IsSelected).ToList();
                foreach (var selectedCard in selectedCards)
                {
                    _learningRepo.RemoveCard(selectedCard.Id.ToString());
                }

                var selectedSubFolders = folder.SubFolders.Where(f => f.IsSelected).ToList();
                foreach (var selectedSubFolder in selectedSubFolders)
                {
                    _learningRepo.RemoveFolder(selectedSubFolder.Id.ToString());
                }
                return RedirectToAction("Details", "Folder", new { id = folder.Id.ToString() });
            }
        }

        public ActionResult SetMoveTarget(string sourceFolderId)
        {
            var folder = _learningRepo.GetFolder(sourceFolderId);
            var viewModel = new SetMoveTargetViewModel
            {
                SourceFolder = folder,
                SourceFolderId = folder.Id.ToString(),
                RootFolder = _learningRepo.GetRootFolder(User.Identity.GetUserId()),
            };
            return View(viewModel);
        }


        public ActionResult Move(string sourceFolderId, string targetFolderId)
        {
            if (sourceFolderId == targetFolderId)
                return RedirectToAction("Details", "Folder", new { id = sourceFolderId });

            var sourceFolder = _learningRepo.GetFolder(sourceFolderId); 

            // move sourcefolder
            if (sourceFolder.IsSelected)
            {
                _learningRepo.MoveFolder(sourceFolderId, targetFolderId);
                _learningRepo.DeSelectFolder(sourceFolder);

                return RedirectToAction("Details", "Folder", new { id = targetFolderId });
            }
            else
            {
                // move subfolders
                var selectedSubfolders = sourceFolder.SubFolders.Where(s => s.IsSelected).ToList();
                foreach (var subFolder in selectedSubfolders)
                {
                    _learningRepo.MoveFolder(subFolder.Id.ToString(), targetFolderId);
                    _learningRepo.DeSelectFolder(subFolder);
                }

                // move cards
                var cards = sourceFolder.Cards.Where(c => c.IsSelected).ToList();
                foreach (var card in cards)
                {
                    _learningRepo.MoveCard(card.Id.ToString(), targetFolderId);
                    _learningRepo.DeSelectCard(card);
                }
                return RedirectToAction("Details", "Folder", new { id = targetFolderId });
            }
            
        }


        public ActionResult SetInsertAfterCard(string sourceFolderId)
        {
            var folder = _learningRepo.GetFolder(sourceFolderId);
            var viewModel = new SetNewOrderViewModel
            {
                SourceFolder = folder,
                SourceFolderId = folder.Id.ToString(),
                Cards = folder.Cards.Where(u => !u.IsSelected).OrderBy(c => c.Position).ToList(),
            };
            return View(viewModel);
        }

        public ActionResult ChangeOrder(string sourceFolderId, string insertAfterCardId)
        {
            _learningRepo.ChangeCardOrder(sourceFolderId, insertAfterCardId);

            return RedirectToAction("Details", "Folder", new { id = sourceFolderId });
        }


        public ActionResult Search(string folderId)
        {
            var folder = _learningRepo.GetFolder(folderId);

            var viewModel = new SearchViewModel
            {
                FolderId = folder.Id.ToString(),
                SearchFrontSide = true,
                SearchBackSide = true,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SearchViewModel viewModel)
        {
            var folder = _learningRepo.GetFolder(viewModel.FolderId);
            var searchResult = new List<SearchResultViewModel>();

            if (!String.IsNullOrEmpty(viewModel.SearchText) && (viewModel.SearchFrontSide || viewModel.SearchBackSide))
            { 
                SearchInFolder(folder, viewModel.SearchText.ToLower(), viewModel.SearchFrontSide, viewModel.SearchBackSide,
                       ref searchResult);
            }

            var result = new SearchResultsViewModel
            {
                Id = viewModel.FolderId,
                SearchText = viewModel.SearchText,
                SearchResults = searchResult,
            };
            return View("SearchResults", result);
        }

        public void SearchInFolder(Folder folderToSearch, string searchText, bool searchFrontSide, bool searchBackSide, ref List<SearchResultViewModel> searchResults)
        {
            var path = new Dictionary<string, string>();
            _learningRepo.GetFolderPath(folderToSearch, ref path);
            path = path.Reverse().ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var card in folderToSearch.Cards.Where(u => u.IsSelected))
            {
                var frontSide = card.FrontSide;
                var frontSideLowered = card.FrontSide.ToLower();
                var backSide = card.BackSide;
                var backSideLowered = card.BackSide.ToLower();

                if ((searchFrontSide && frontSideLowered.Contains(searchText)) || (searchBackSide && backSideLowered.Contains(searchText)))
                {

                    const string prefix = "<span class=\"marked-search-text\">";
                    const string suffix = "</span>";

                    if (searchFrontSide && frontSideLowered.Contains(searchText))
                    {
                        var matches = Regex.Matches(frontSideLowered, searchText).Cast<Match>();
                        var offset = 0;
                        foreach (var match in matches)
                        {
                            frontSide = frontSide.Insert(match.Index + offset, prefix);
                            offset += prefix.Length;
                            frontSide = frontSide.Insert(match.Index + match.Length + offset, suffix);
                            offset += suffix.Length;
                        }
                    }


                    if (searchBackSide && backSideLowered.Contains(searchText))
                    {
                        var matches = Regex.Matches(backSideLowered, searchText).Cast<Match>();
                        var offset = 0;
                        foreach (var match in matches)
                        {
                            backSide = backSide.Insert(match.Index + offset, prefix);
                            offset += prefix.Length;
                            backSide = backSide.Insert(match.Index + match.Length + offset, suffix);
                            offset += suffix.Length;
                        }
                    }


                    var searchResult = new SearchResultViewModel
                    {
                        Id = card.Id.ToString(),
                        Path = path,
                        FrontSide = frontSide,
                        BackSide = backSide,
                    };
                    searchResults.Add(searchResult);
                }
            }

            foreach (var subFolder in folderToSearch.SubFolders.Where(f => f.IsSelected))
            {
                SearchInFolder(subFolder, searchText, searchFrontSide, searchBackSide, ref searchResults);
            }
        }

        public ActionResult Options(string id)
        {
            var folder = _learningRepo.GetFolder(id);

            var hasOnlyCardsSelected = !folder.IsSelected && !folder.SubFolders.Any(f => f.IsSelected);
            var hasOneCardSelected = (folder.Cards.Count(c => c.IsSelected) == 1);

            var hasOnlyFoldersSelected = !folder.Cards.Any(c => c.IsSelected);
            var hasOneFolderSelected = ((folder.SubFolders.Count(c => c.IsSelected) + (folder.IsSelected ? 1 : 0)) == 1);

            var viewModel = new OptionsViewModel
            {
                Id = folder.Id.ToString(),
                Folder = folder,
                HasAnyFolderSelected = (folder.IsSelected || folder.SubFolders.Any(f => f.IsSelected)),
                HasRootFolderSelected = (folder.ParentFolder == null && folder.IsSelected),
                HasOnlyCardsSelected = hasOnlyCardsSelected,
                HasOnlyOneCardSelected = hasOnlyCardsSelected && hasOneCardSelected,
                CardId = (folder.Cards.Count(c => c.IsSelected) == 1) ? folder.Cards.Find(c => c.IsSelected).Id.ToString() : String.Empty,
                HasOnlyFoldersSelected = hasOnlyFoldersSelected,
                HasOnlyOneFolderSelected = hasOneFolderSelected && hasOnlyFoldersSelected,
                FolderId = ((folder.SubFolders.Count(c => c.IsSelected) + (folder.IsSelected ? 1 : 0)) == 1) ? (folder.IsSelected ? folder.Id.ToString() : folder.SubFolders.Find(f => f.IsSelected).Id.ToString()) : String.Empty,
            };

            return View(viewModel);
        }

        public ActionResult Statistic(string id)
        {
            var folder = _learningRepo.GetFolder(id);

            var viewModel = new StatisticViewModel
            {

            };
            return View(viewModel);
        }


        public ActionResult Import(string id)
        {
            var folder = _learningRepo.GetFolder(id);

            var viewModel = new ImportViewModel
            {

            };
            return View(viewModel);
        }


        public ActionResult Export(string folderId)
        {
            var folder = _learningRepo.GetFolder(folderId);
            
            var stringBuilder = new StringBuilder();
            var settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            using (var writer = XmlWriter.Create(stringBuilder, settings))
            {
                var doc = new XmlDocument();
                SerializeFolder(folder, null, ref doc);
                doc.Save(writer);
            }

            var viewModel = new ExportViewModel
            {
                FolderId = folderId,
                SerializationResult = stringBuilder.ToString(),
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadExport(string folderId, string serializationResult)
        {
            return Content(serializationResult, "text/xml", Encoding.UTF8);
        }


        private void SerializeFolder(Folder folder, XmlElement parentFolderElement, ref XmlDocument doc)
        {
            if (parentFolderElement == null)
            {
                parentFolderElement = doc.CreateElement("Folder");
                parentFolderElement.SetAttribute("Name", folder.Name);
                doc.AppendChild(parentFolderElement);
            }

            foreach (var selectedSubFolder in folder.SubFolders.Where(f => f.IsSelected).OrderBy(f => f.Name))
            {
                var subFolderElement = doc.CreateElement("Folder");
                subFolderElement.SetAttribute("Name", selectedSubFolder.Name);
                parentFolderElement.AppendChild(subFolderElement);
                SerializeFolder(selectedSubFolder, subFolderElement, ref doc);
            }

            foreach (var selectedCard in folder.Cards.Where(c => c.IsSelected).OrderBy(f => f.Position))
            {
                var cardNode = doc.CreateElement("Card");
                cardNode.SetAttribute("FrontSide", selectedCard.FrontSide);
                cardNode.SetAttribute("BackSide", selectedCard.BackSide);
                parentFolderElement.AppendChild(cardNode);
            }
        }


    }
}
