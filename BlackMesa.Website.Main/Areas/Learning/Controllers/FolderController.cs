using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Folder;
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
            var path = new List<Folder>();

            _learningRepo.GetFolderPath(folder, ref path);
            path.Reverse();
            path.Remove(path.Last());

            if (deSelect)
                _learningRepo.DeSelectFolder(folder);

            var dueCardsPerFolder = new Dictionary<string, int>();
            foreach (var subFolder in folder.SubFolders)
            {
                var cardCount = 0;
                _learningRepo.GetCardCount(subFolder, ref cardCount, true, false, true);
                dueCardsPerFolder.Add(subFolder.Id.ToString(), cardCount);
            }

            var dueCards = 0;
            _learningRepo.GetCardCount(folder, ref dueCards, false, false, true);
            dueCards += dueCardsPerFolder.Values.Sum();

            var viewModel = new DetailsViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
                IsSelected = folder.IsSelected,
                IsRootFolder = (folder.ParentFolder == null),
                Level = folder.Level,
                HasAnySelection = (folder.IsSelected || folder.SubFolders.Any(f => f.IsSelected) || folder.Cards.Any(u => u.IsSelected)),
                SubFolders = folder.SubFolders.OrderBy(f => f.Name),
                Cards = folder.Cards.OrderBy(c => c.Position),
                Path = path,
                ParentFolderId = (!folder.IsRootFolder ? folder.ParentFolder.Id.ToString() : String.Empty),
                DueCards = dueCards,
                DueCardsPerSubfolder = dueCardsPerFolder,
            };
            return View(viewModel);
        }


        public ActionResult Create(string parentFolderId)
        {
            var viewModel = new CreateViewModel()
            {
                ParentFolderId = parentFolderId,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel viewModel)
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
            var viewModel = new EditViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var folder = _learningRepo.GetFolder(viewModel.Id);
                _learningRepo.EditFolder(viewModel.Id, viewModel.Name);
                if (!folder.IsRootFolder)
                    return RedirectToAction("Details", "Folder", new { id = folder.ParentFolder.Id });
                return RedirectToAction("Index");
            }
        return View();
        }


        public ActionResult AddOptions(string folderId)
        {
            var viewModel = new AddOptionsViewModel
            {
                FolderId = folderId,
            };
            return View(viewModel);
        }

    }
}
