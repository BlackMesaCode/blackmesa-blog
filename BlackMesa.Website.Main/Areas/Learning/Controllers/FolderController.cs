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
            var folders = _learningRepo.GetFoldersWithAllSubfolders(User.Identity.GetUserId());

            var viewModel = new FolderIndexViewModel
            {
                Folders = new List<FolderListItemViewModel>(),
            };


            foreach (var folder in folders)
            {
                viewModel.Folders.Add(CreateFolderListItemViewModel(folder));
            }

            return View(viewModel);
        }

        private FolderListItemViewModel CreateFolderListItemViewModel(Folder folder)
        {
            int? parentFolderId;
            int level;
            if (folder.ParentFolder == null)
            {
                parentFolderId = null;
                level = 1;
            }
            else
            {
                parentFolderId = folder.ParentFolder.Id;
                level = folder.ParentFolder.Level + 1;
            }

            var viewModel = new FolderListItemViewModel()
            {
                Id = folder.Id,
                Name = folder.Name,
                ParentFolderId = parentFolderId,
                Level = level,
                SubFolders = folder.SubFolders.Select(f => CreateFolderListItemViewModel(f)).ToList(),
            };
            return viewModel;
        }

        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult Create(int? parentFolderId)
        {
            var viewModel = new CreateEditFolderViewModel()
            {
                ParentFolderId = parentFolderId,
                DateCreated = DateTime.Now,
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


        public ActionResult Edit(int id)
        {
            return View();
        }


        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            return View();
        }


        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
