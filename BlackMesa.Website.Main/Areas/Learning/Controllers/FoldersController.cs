using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BlackMesa.Website.Main.Controllers;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    public class FoldersController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
