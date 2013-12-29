using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Query;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    [Authorize]
    public class QueryController : BaseController
    {
        private readonly LearningRepository _learningRepo = new LearningRepository(new BlackMesaDbContext());

        public ActionResult Setup(string folderId)
        {
            var folder = _learningRepo.GetFolder(folderId);
            var selectedFolders = new List<Folder> { folder };

            var viewModel = new SetupQueryViewModel
            {
                InludeSubfolders = true,
                OrderType = OrderType.Ordered,
                QueryType = QueryType.Normal,
                SelectedFolders = selectedFolders,
            };

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setup(SetupQueryViewModel viewModel)
        {
            

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Start(SetupQueryViewModel viewModel)
        {


            return View();
        }
	}
}