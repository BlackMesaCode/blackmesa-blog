using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Selection;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;
using BlackMesa.Website.Main.Resources;
using Microsoft.AspNet.Identity;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    [Authorize]
    public class FolderController : BaseController
    {

        private readonly LearningRepository _learningRepo = new LearningRepository(new BlackMesaDbContext());


        public ActionResult Index()
        {
            var rootFolder = _learningRepo.GetRootFolder(User.Identity.GetUserId());

            if (rootFolder == null)
            {
                _learningRepo.CreateRootFolder(Strings.Root, User.Identity.GetUserId());
                rootFolder = _learningRepo.GetRootFolder(User.Identity.GetUserId());
            }

            return RedirectToAction("Details", "Folder", new {id = rootFolder.Id.ToString()});
        }


        public ActionResult Details(string id, bool deSelect = true)
        {
            var folder = _learningRepo.GetFolder(id);
            var path = new Dictionary<string, string>();

            _learningRepo.GetFolderPath(folder, ref path);
            path = path.Reverse().ToDictionary(pair => pair.Key, pair => pair.Value);
            path.Remove(path.Last().Key);

            if (deSelect)
                _learningRepo.DeSelectFolder(id);
            

            var viewModel = new FolderDetailsViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
                IsSelected = folder.IsSelected,
                IsRootFolder = (folder.ParentFolder == null),
                Level = folder.Level,
                HasAnySelection = (folder.IsSelected || folder.LearningUnits.Any(u => u.IsSelected) || folder.SubFolders.Any(f => f.IsSelected)),
                HasAnyFolderSelection = (folder.IsSelected || folder.SubFolders.Any(f => f.IsSelected)),
                HasRootFolderSelected = (folder.ParentFolder == null && folder.IsSelected),
                HasOnlyIndexCardsSelected = (folder.LearningUnits.Any(u => u.IsSelected) && !folder.IsSelected && !folder.SubFolders.Any(f => f.IsSelected)),
                SubFolders = folder.SubFolders,
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



    }
}
