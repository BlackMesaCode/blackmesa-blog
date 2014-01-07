using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Website.Main.Areas.Learning.ViewModels;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Card;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    [Authorize]
    public class CardController : BaseController
    {

        private readonly LearningRepository _learningRepo = new LearningRepository(new BlackMesaDbContext());

        public ActionResult Create(string folderId)
        {
            var viewModel = new CreateCardViewModel()
            {
                FolderId = folderId,
            };

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Create(CreateCardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _learningRepo.AddCard(viewModel.FolderId, User.Identity.GetUserId(), viewModel.FrontSide, viewModel.BackSide);
                return RedirectToAction("Details", "Folder", new { id = viewModel.FolderId });
            }
            return View(viewModel);
        }


        public ActionResult Edit(string id)
        {
            var card = _learningRepo.GetCard(id);
            var viewModel = new EditCardViewModel
            {
                Id = card.Id.ToString(),
                FolderId = card.FolderId.ToString(),
                FrontSide = card.FrontSide,
                BackSide = card.BackSide,
            };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Edit(EditCardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var card = _learningRepo.GetCard(viewModel.Id);
                _learningRepo.EditCard(viewModel.Id, viewModel.FrontSide, viewModel.BackSide, viewModel.Hint, viewModel.CodeSnipped, viewModel.ImageUrl);
                return RedirectToAction("Details", "Folder", new  { id = card.FolderId });
            }
            return View(viewModel);
        }


    }
}
