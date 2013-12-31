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
    public class SelectionController : BaseController
    {

        private readonly LearningRepository _learningRepo = new LearningRepository(new BlackMesaDbContext());

        public ActionResult Index()
        {

            return View();
        }


        public ActionResult AddFolder(string folderId, string returnFolderId)
        {
            // Set Folder selected attribute
            // auto select all subfolders and subitems
            return RedirectToAction("Details", "Folder", new {id = returnFolderId});
        }

        public ActionResult AddUnit(string unitId, string returnFolderId)
        {
            // Set unit selected attribute
            return RedirectToAction("Details", "Folder", new { id = returnFolderId });
        }

        public ActionResult Paste()
        {

            return View();
        }



    }
}
