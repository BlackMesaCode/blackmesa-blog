using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Website.Main.Areas.Learning.ViewModels;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    public class FolderController : Controller
    {

        private readonly LearningRepository _learningRepo = new LearningRepository(new BlackMesaDbContext());


        public ActionResult Index()
        {
            var folders1 = _learningRepo.GetFoldersWithAllSubfolders(User.Identity.GetUserId());

            var viewModel = new FolderIndexViewModel
            {
                Folders = new List<FolderListItemViewModel>(),
            };

            //foreach (var folder1 in folders1)
            //{
            //    var folderListItem1 = new FolderListItemViewModel();
            //    foreach (var folder2 in folder1.SubFolders)
            //    {
            //        var folderListItem2 = CreateFolderListItemViewModel(subFolder);
            //        foreach (var folder3 in folder2.SubFolders)
            //        {
            //            var folderListItem3 = CreateFolderListItemViewModel(folder3);
            //            folderListItem2.SubFolders.Add(folderListItem3);
            //        }
            //        folderListItem1.SubFolders.Add(folderListItem2);
            //    }
                
            //    viewModel.Folders.Add(folderListItem1);
            //}

            foreach (var folder in folders1)
            {
                viewModel.Folders.Add(CreateFolderListItemViewModel(folder));
            }

            return View(viewModel);
        }

        private FolderListItemViewModel CreateFolderListItemViewModel(Folder folder)
        {
            int? parentFolderId;
            if (folder.ParentFolder == null)
                parentFolderId = null;
            else
                parentFolderId = folder.ParentFolder.Id;

            var viewModel = new FolderListItemViewModel()
            {
                Id = folder.Id,
                Name = folder.Name,
                ParentFolderId = parentFolderId,
                SubFolders = folder.SubFolders.Select(f => CreateFolderListItemViewModel(f)).ToList(),
            };
            return viewModel;
        }

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}


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


        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

    }
}
