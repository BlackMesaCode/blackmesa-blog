using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Website.Main.Areas.Learning.ViewModels;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.IndexCard;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    [Authorize]
    public class IndexCardController : BaseController
    {

        private readonly LearningRepository _learningRepo = new LearningRepository(new BlackMesaDbContext());



        public ActionResult Create(string folderId)
        {
            var viewModel = new CreateIndexCardViewModel()
            {
                FolderId = folderId,
            };

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Create(CreateIndexCardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _learningRepo.AddIndexCard(viewModel.FolderId, User.Identity.GetUserId(), viewModel.Question, viewModel.Answer);
                return RedirectToAction("Details", "Folder", new { id = viewModel.FolderId });
            }
            return View(viewModel);
        }


        //public ActionResult Edit(string id)
        //{
        //    var folder = _learningRepo.GetFolder(User.Identity.GetUserId(), id);
        //    var viewModel = new EditFolderViewModel
        //    {
        //        Id = folder.Id.ToString(),
        //        Name = folder.Name,
        //    };
        //    return View(viewModel);
        //}


        //[HttpPost]
        //public ActionResult Edit(EditFolderViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var folder = _learningRepo.GetFolder(User.Identity.GetUserId(), viewModel.Id);
        //        _learningRepo.EditFolder(viewModel.Id, viewModel.Name);
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}


        //public ActionResult Delete(string id)
        //{
        //    var folder = _learningRepo.GetFolder(User.Identity.GetUserId(), id);
        //    var viewModel = new DeleteFolderViewModel
        //    {
        //        Id = id,
        //        Name = folder.Name,
        //    };
        //    return View(viewModel);
        //}


        //[HttpPost]
        //public ActionResult Delete(DeleteFolderViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    return View();

        //}

    }
}
