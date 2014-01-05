using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;
using BlackMesa.Website.Main.Areas.Learning.ViewModels;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;
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
            _learningRepo.SelectFolder(folderId);
            return RedirectToAction("Details", "Folder", new {id = returnFolderId});
        }

        public ActionResult RemoveFolder(string folderId, string returnFolderId)
        {
            _learningRepo.DeSelectFolder(folderId);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId });
        }

        public ActionResult AddUnit(string unitId, string returnFolderId)
        {
            _learningRepo.SelectUnit(unitId);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId });
        }

        public ActionResult RemoveUnit(string unitId, string returnFolderId)
        {
            _learningRepo.DeSelectUnit(unitId);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId });
        }


        public ActionResult Delete(string folderId)
        {
            var folder = _learningRepo.GetFolder(folderId);

            var selectedFolders = new List<Folder>();
            var selectedIndexCards = new List<IndexCard>();

            if (folder.IsSelected)
                selectedFolders.Add(folder);
            else
            {
                selectedFolders = folder.SubFolders.Where(f => f.IsSelected).ToList();
                selectedIndexCards = folder.LearningUnits.OfType<IndexCard>().Where(u => u.IsSelected).ToList();
            }

            int affectedFolders = 0;
            int affectedIndexCards = 0;

            _learningRepo.GetFolderCount(folder, ref affectedFolders, true, true);
            _learningRepo.GetUnitCount<IndexCard>(folder, ref affectedIndexCards, true, true);

            var viewModel = new DeleteSelectionViewModel
            {
                Id = folder.Id.ToString(),
                SelectedFolders = selectedFolders,
                SelectedIndexCards = selectedIndexCards,
                AffectedFolders = affectedFolders,
                AffectedIndexCards = affectedIndexCards,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeleteSelectionViewModel viewModel)
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
                var selectedLearningUnits = folder.LearningUnits.Where(u => u.IsSelected).ToList();
                foreach (var selectedUnit in selectedLearningUnits)
                {
                    _learningRepo.RemoveUnit(selectedUnit.Id.ToString());
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
                _learningRepo.DeSelectFolder(sourceFolderId);

                return RedirectToAction("Details", "Folder", new { id = targetFolderId });
            }
            else
            {
                // move subfolders
                var selectedSubfolders = sourceFolder.SubFolders.Where(s => s.IsSelected).ToList();
                foreach (var subFolder in selectedSubfolders)
                {
                    _learningRepo.MoveFolder(subFolder.Id.ToString(), targetFolderId);
                    _learningRepo.DeSelectFolder(subFolder.Id.ToString());
                }

                // move units
                var learningUnits = sourceFolder.LearningUnits.Where(u => u.IsSelected).ToList();
                foreach (var unit in learningUnits)
                {
                    _learningRepo.MoveUnit(unit.Id.ToString(), targetFolderId);
                    _learningRepo.DeSelectUnit(unit.Id.ToString());
                }
                return RedirectToAction("Details", "Folder", new { id = targetFolderId });
            }
            
        }

        public ActionResult Search(string folderId)
        {
            var folder = _learningRepo.GetFolder(folderId);

            var viewModel = new SearchSelectionViewModel
            {
                FolderId = folder.Id.ToString(),
                SearchFrontSide = true,
                SearchBackSide = true,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SearchSelectionViewModel viewModel)
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

            foreach (var unit in folderToSearch.LearningUnits.OfType<IndexCard>().Where(u => u.IsSelected))
            {
                var frontSide = unit.FrontSide.ToLower();
                var backSide = unit.BackSide.ToLower();

                if ((searchFrontSide && frontSide.Contains(searchText)) || (searchBackSide && backSide.Contains(searchText)))
                {

                    const string prefix = "<span class=\"marked-search-text\">";
                    const string suffix = "</span>";

                    if (searchFrontSide && frontSide.Contains(searchText))
                    {
                        var matches = Regex.Matches(frontSide, searchText).Cast<Match>();
                        var offset = 0;
                        foreach (var match in matches)
                        {
                            frontSide = unit.FrontSide.Insert(match.Index + offset, prefix);
                            offset += prefix.Length;
                            frontSide = frontSide.Insert(match.Index + match.Length + offset, suffix);
                            offset += suffix.Length;
                        }
                    }


                    if (searchBackSide && backSide.Contains(searchText))
                    {
                        var matches = Regex.Matches(backSide, searchText).Cast<Match>();
                        var offset = 0;
                        foreach (var match in matches)
                        {
                            backSide = unit.BackSide.Insert(match.Index + offset, prefix);
                            offset += prefix.Length;
                            backSide = backSide.Insert(match.Index + match.Length + offset, suffix);
                            offset += suffix.Length;
                        }
                    }


                    var searchResult = new SearchResultViewModel
                    {
                        Id = unit.Id.ToString(),
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
    }
}
