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
                _learningRepo.AddIndexCard(viewModel.FolderId, User.Identity.GetUserId(), viewModel.FrontSide, viewModel.BackSide);
                return RedirectToAction("Details", "Folder", new { id = viewModel.FolderId });
            }
            return View(viewModel);
        }


        public ActionResult Edit(string id)
        {
            var indexCard = _learningRepo.GetIndexCard(id);
            var viewModel = new EditIndexCardViewModel
            {
                Id = indexCard.Id.ToString(),
                FolderId = indexCard.FolderId.ToString(),
                FrontSide = indexCard.FrontSide,
                BackSide = indexCard.BackSide,
                Hint = indexCard.Hint,
                CodeSnipped = indexCard.CodeSnipped,
                ImageUrl = indexCard.ImageUrl,
            };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Edit(EditIndexCardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var indexCard = _learningRepo.GetIndexCard(viewModel.Id);
                _learningRepo.EditIndexCard(viewModel.Id, viewModel.FrontSide, viewModel.BackSide, viewModel.Hint, viewModel.CodeSnipped, viewModel.ImageUrl);
                return RedirectToAction("Details", "Folder", new  { id = indexCard.FolderId });
            }
            return View(viewModel);
        }


    }
}
