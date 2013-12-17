using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Website.Main.Areas.Learning.ViewModels;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
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

            int totalLearningUnits = 0;
            foreach (var folder in folders)
            {
                viewModel.Folders.Add(CreateFolderListItemViewModel(folder));
                totalLearningUnits += GetNumberOfLearningUnitsInAllSubfolders(folder);
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

            var viewModel = new FolderListItemViewModel()
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
                ParentFolderId = parentFolderId,
                Level = level,
                SubFolders = folder.SubFolders.Select(f => CreateFolderListItemViewModel(f)).ToList(),
            };
            return viewModel;
        }


        private int GetNumberOfLearningUnitsInAllSubfolders(Folder folder, int result = 0)
        {
            foreach (var subFolder in folder.SubFolders)
            {
                result += GetNumberOfLearningUnitsInAllSubfolders(subFolder, result + folder.LearningUnits.Count);
            }

            return result;
        }

        public ActionResult Details(string id)
        {
            var folder = _learningRepo.GetFolder(User.Identity.GetUserId(), id);
            var viewModel = new FolderDetailsViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
            };
            return View(viewModel);
        }


        public ActionResult Create(string parentFolderId)
        {
            var viewModel = new CreateEditFolderViewModel()
            {
                ParentFolderId = parentFolderId,
            };

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Create(CreateEditFolderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _learningRepo.AddFolder(viewModel.Name, User.Identity.GetUserId(), viewModel.ParentFolderId);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }


        public ActionResult Edit(string id)
        {
            var folder = _learningRepo.GetFolder(User.Identity.GetUserId(), id);
            var viewModel = new CreateEditFolderViewModel
            {
                Id = folder.Id.ToString(),
                ParentFolderId = folder.ParentFolder.Id.ToString(),
                Name = folder.Name,
            };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Edit(CreateEditFolderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var folder = _learningRepo.GetFolder(User.Identity.GetUserId(), viewModel.Id);
                TryUpdateModel(folder);
                 return RedirectToAction("Index");
            }
        return View();
        }


        public ActionResult Delete(string id)
        {
            var folder = _learningRepo.GetFolder(User.Identity.GetUserId(), id);
            var viewModel = new DeleteFolderViewModel
            {
                Id = id,
                Name = folder.Name,
            };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Delete(DeleteFolderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            return View();

        }

    }
}
