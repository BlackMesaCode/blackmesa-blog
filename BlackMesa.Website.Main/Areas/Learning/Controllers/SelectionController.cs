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
            _learningRepo.SelectFolder(folderId);
            return RedirectToAction("Details", "Folder", new {id = returnFolderId});
        }

        public ActionResult RemoveFolder(string folderId, string returnFolderId)
        {
            _learningRepo.DeSelectFolder(folderId);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId });
        }

        public ActionResult AddUnit(string unitId, string returnFolderId)
        {
            _learningRepo.SelectUnit(unitId);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId });
        }

        public ActionResult RemoveUnit(string unitId, string returnFolderId)
        {
            _learningRepo.DeSelectUnit(unitId);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId });
        }

        public ActionResult Delete(string folderId)
        {
            var folder = _learningRepo.GetFolder(folderId);
            var parentFolderId = folder.ParentFolder != null ? folder.ParentFolder.Id.ToString() : String.Empty;
            if (folder.IsSelected)
            {
                _learningRepo.RemoveFolder(folderId);
                if (!String.IsNullOrEmpty(parentFolderId))
                    return RedirectToAction("Details", "Folder", new {id = parentFolderId});
                else
                    return RedirectToAction("Index", "Folder");
            }
            else
            {
                var selectedLearningUnits = folder.LearningUnits.Where(u => u.IsSelected).ToList();
                foreach (var selectedUnit in selectedLearningUnits)
                {
                    _learningRepo.RemoveLearningUnit(selectedUnit.Id.ToString());
                }
                //for (int i = selectedLearningUnits.Count(u => u.IsSelected) - 1; i >= 0; i--)
                //{
                //    _learningRepo.RemoveLearningUnit(selectedUnit.Id.ToString());
                //}

                var selectedSubFolders = folder.SubFolders.Where(f => f.IsSelected).ToList();
                foreach (var selectedSubFolder in selectedSubFolders)
                {
                    _learningRepo.RemoveFolder(selectedSubFolder.Id.ToString());
                }
                return RedirectToAction("Details", "Folder", new { id = folderId });
            }
        }

        public ActionResult Move(string sourceFolder)
        {

            return View();
        }

        [HttpPost]
        public ActionResult Move(string sourceFolderId, string targetFolderId)
        {

            return View();
        }

    }
}
