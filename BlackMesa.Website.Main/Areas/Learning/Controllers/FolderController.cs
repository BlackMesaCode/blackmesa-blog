using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Website.Main.Areas.Learning.ViewModels;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    [Authorize]
    public class FolderController : BaseController
    {

        private readonly LearningRepository _learningRepo = new LearningRepository(new BlackMesaDbContext());


        public ActionResult Index()
        {
            var folders = _learningRepo.GetFolders(User.Identity.GetUserId());

            var viewModel = new FolderIndexViewModel
            {
                Folders = new List<FolderListItemViewModel>(),
            };

            var totalLearningUnits = 0;
            foreach (var folder in folders)
            {
                viewModel.Folders.Add(CreateFolderListItemViewModel(folder));
                GetAllContainingLearningUnits(folder, ref totalLearningUnits);
            }
            viewModel.TotalLearningUnits = totalLearningUnits;

            return View(viewModel);
        }

        private FolderListItemViewModel CreateFolderListItemViewModel(Folder folder)
        {
            string parentFolderId;
            int level;
            if (folder.ParentFolder == null)
            {
                parentFolderId = null;
                level = 1;
            }
            else
            {
                parentFolderId = folder.ParentFolder.Id.ToString();
                level = folder.ParentFolder.Level + 1;
            }

            int totalLearningUnits = 0;
            GetAllContainingLearningUnits(folder, ref totalLearningUnits);

            var viewModel = new FolderListItemViewModel()
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
                ParentFolderId = parentFolderId,
                Level = level,
                NumberOfLearningUnitsInSameFolder = folder.LearningUnits.Count,
                NumberOfLearningUnitsIncludingAllSubfolders = totalLearningUnits,
                SubFolders = folder.SubFolders.Select(f => CreateFolderListItemViewModel(f)).ToList(),
            };
            return viewModel;
        }


        private void GetAllContainingLearningUnits(Folder folder, ref int totalLearningUnits)
        {
            totalLearningUnits += folder.LearningUnits.Count;

            foreach (var subFolder in folder.SubFolders)
            {
                GetAllContainingLearningUnits(subFolder, ref totalLearningUnits);
            }
        }

        public ActionResult Details(string id)
        {
            var folder = _learningRepo.GetFolder(id);
            var path = new Dictionary<string, string>();
            _learningRepo.GetFolderPath(folder, ref path);
            path = path.Reverse().ToDictionary(pair => pair.Key, pair => pair.Value);
            path.Remove(path.Last().Key);
            var viewModel = new FolderDetailsViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
                SubFolders = folder.SubFolders.Select(f => CreateFolderListItemViewModel(f)).ToList(),
                IndexCards = folder.LearningUnits.OfType<IndexCard>(),
                Path = path,
            };
            return View(viewModel);
        }


        public ActionResult Create(string parentFolderId)
        {
            var viewModel = new CreateFolderViewModel()
            {
                ParentFolderId = parentFolderId,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateFolderViewModel viewModel)
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
            var viewModel = new EditFolderViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditFolderViewModel viewModel)
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


        public ActionResult Delete(string id)
        {
            var folder = _learningRepo.GetFolder(id);
            var viewModel = new DeleteFolderViewModel
            {
                Id = id,
                Name = folder.Name,
                ParentFolderId = folder.ParentFolder != null ? folder.ParentFolder.Id.ToString() : String.Empty,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeleteFolderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var folder = _learningRepo.GetFolder(viewModel.Id);
                var parentFolderId = String.Empty;
                if (folder.ParentFolder != null)
                    parentFolderId = folder.ParentFolder.Id.ToString();
                _learningRepo.RemoveFolder(viewModel.Id);

                if (!String.IsNullOrEmpty(parentFolderId))
                    return RedirectToAction("Details", "Folder", new { id = parentFolderId });
                return RedirectToAction("Index");
            }
            return View();

        }

        public ActionResult Options(string id)
        {
            var folder = _learningRepo.GetFolder(id);
            var path = new Dictionary<string, string>();
            _learningRepo.GetFolderPath(folder, ref path);
            path = path.Reverse().ToDictionary(pair => pair.Key, pair => pair.Value);
            path.Remove(path.Last().Key);
            var viewModel = new FolderOptionsViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
                Path = path,
            };
            return View(viewModel);
        }


        public ActionResult Move(string id)
        {
            var folder = _learningRepo.GetFolder(id);
            var viewModel = new MoveFolderViewModel();
            return View(viewModel);
        }


        public ActionResult Search(string id)
        {
            var folder = _learningRepo.GetFolder(id);
            var viewModel = new SearchFolderViewModel();
            return View(viewModel);
        }


    }
}
