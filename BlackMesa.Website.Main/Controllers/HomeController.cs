using System.Web.Mvc;
using System.Web.Routing;

namespace BlackMesa.Website.Main.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Entry", new { area = "Blog" });
        }

    }
}
