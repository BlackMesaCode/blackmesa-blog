using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;
using BlackMesa.Website.Main.Resources;
using Microsoft.AspNet.Identity;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    [Authorize]
    public class FolderController : BaseController
    {

        private readonly LearningRepository _learningRepo = new LearningRepository(new BlackMesaDbContext());



        public void WalkDirectoryTree(DirectoryInfo folder, bool isRootFolder = false)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            var createdFolderId = "";
            if (!isRootFolder)
            {
                createdFolderId = _learningRepo.AddFolder(folder.Name, User.Identity.GetUserId(),
                    _learningRepo.GetRootFolder(User.Identity.GetUserId()).Id.ToString());
            }


            try
            {
                files = folder.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException e)
            {

            }

            catch (System.IO.DirectoryNotFoundException e)
            {

            }


            if (files != null)
            {
                foreach (var file in files)
                {
                    var createdSubFolderId = _learningRepo.AddFolder(file.Name.Remove(file.Name.IndexOf(".txt"), 4), User.Identity.GetUserId(),
                        createdFolderId);
                    //using ()
                    //{
                    var stream = file.OpenRead();
                    var reader = new StreamReader(stream);
                    var text = reader.ReadToEnd();
                    CreateNextCard(text, createdSubFolderId);
                    stream.Close();
                    //    stream.Dispose();
                    //}
                }


                subDirs = folder.GetDirectories();

                foreach (var subDir in subDirs)
                {
                    WalkDirectoryTree(subDir);
                }
            }
        }


        public void CreateNextCard(string text, string folderId)
        {
            var divider = "---------------------------------------------------------------------";
            var positionOfFirstDivider = text.IndexOf(divider);

            if (positionOfFirstDivider != -1)
            {
                var positionOfFrontSideStart = positionOfFirstDivider + divider.Length;
                var positionOfSecondDivider = text.IndexOf(divider, positionOfFirstDivider + divider.Length);
                var frontSide = text.Substring(positionOfFrontSideStart,
                    positionOfSecondDivider - positionOfFrontSideStart)
                    .Trim(new char[] { ' ', '\n', '\r', '\t' });
                var positionOfBackSideStart = positionOfSecondDivider + divider.Length;
                var positionOfThirdDivider = text.IndexOf(divider, positionOfSecondDivider + divider.Length);
                var backSide = String.Empty;
                if (positionOfThirdDivider == -1)
                {
                    backSide = text.Substring(positionOfBackSideStart).Trim(new char[] { ' ', '\n', '\r', '\t' });
                }
                else
                {
                    backSide = text.Substring(positionOfBackSideStart, positionOfThirdDivider - positionOfBackSideStart)
                        .Trim(new char[] { ' ', '\n', '\r', '\t' });
                }

                _learningRepo.AddCard(folderId, User.Identity.GetUserId(), frontSide, backSide);

                if (positionOfThirdDivider != -1)
                    CreateNextCard(text.Substring(positionOfThirdDivider), folderId);
            }

        }



        public ActionResult Index()
        {
            var rootFolder = _learningRepo.GetRootFolder(User.Identity.GetUserId());

            if (rootFolder == null)
            {
                _learningRepo.CreateRootFolder(Strings.Root, User.Identity.GetUserId());
                rootFolder = _learningRepo.GetRootFolder(User.Identity.GetUserId());
            }

            var rootDir = new DirectoryInfo(@"D:\RootFolder");
            WalkDirectoryTree(rootDir, true);

            return RedirectToAction("Details", "Folder", new {id = rootFolder.Id.ToString()});
        }


        public ActionResult Details(string id, bool deSelect = true)
        {
            var folder = _learningRepo.GetFolder(id);
            var path = new Dictionary<string, string>();

            _learningRepo.GetFolderPath(folder, ref path);
            path = path.Reverse().ToDictionary(pair => pair.Key, pair => pair.Value);
            path.Remove(path.Last().Key);

            if (deSelect)
                _learningRepo.DeSelectFolder(id);
            

            var viewModel = new FolderViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
                IsSelected = folder.IsSelected,
                IsRootFolder = (folder.ParentFolder == null),
                Level = folder.Level,
                HasAnySelection = (folder.IsSelected || folder.SubFolders.Any(f => f.IsSelected) || folder.Cards.Any(u => u.IsSelected)),
                SubFolders = folder.SubFolders.OrderBy(f => f.Name),
                Cards = folder.Cards.OrderBy(c => c.Position),
                Path = path,
                ParentFolderId = (folder.ParentFolder != null ? folder.ParentFolder.Id.ToString() : String.Empty),
            };
            return View(viewModel);
        }


        public ActionResult Create(string parentFolderId)
        {
            var viewModel = new CreateViewModel()
            {
                ParentFolderId = parentFolderId,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _learningRepo.AddFolder(viewModel.Name, User.Identity.GetUserId(), viewModel.ParentFolderId);
                if (!String.IsNullOrEmpty(viewModel.ParentFolderId))
                    return RedirectToAction("Details", "Folder", new { id = viewModel.ParentFolderId });
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }


        public ActionResult Edit(string id)
        {
            var folder = _learningRepo.GetFolder(id);
            var viewModel = new EditViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var folder = _learningRepo.GetFolder(viewModel.Id);
                _learningRepo.EditFolder(viewModel.Id, viewModel.Name);
                if (folder.ParentFolder != null)
                    return RedirectToAction("Details", "Folder", new { id = folder.ParentFolder.Id });
                return RedirectToAction("Index");
            }
        return View();
        }


    }
}
