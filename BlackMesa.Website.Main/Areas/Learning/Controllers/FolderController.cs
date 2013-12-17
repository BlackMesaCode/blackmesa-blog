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
            var folder = _learningRepo.GetFolder(User.Identity.GetUserId(), id);
            var viewModel = new FolderDetailsViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
                SubFolders = folder.SubFolders.Select(f => CreateFolderListItemViewModel(f)).ToList(),
                TextCards = folder.LearningUnits.OfType<TextCard>(),
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
        public ActionResult Create(CreateFolderViewModel viewModel)
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
            var viewModel = new EditFolderViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
            };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Edit(EditFolderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var folder = _learningRepo.GetFolder(User.Identity.GetUserId(), viewModel.Id);
                _learningRepo.EditFolder(viewModel.Id, viewModel.Name);
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
