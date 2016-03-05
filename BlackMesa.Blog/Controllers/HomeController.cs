using System.Web.Mvc;
using System.Web.Routing;

namespace BlackMesa.Blog.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //return RedirectToActionPermanent("Index", "Folder", new { area = "Learning" });
            return RedirectToActionPermanent("Index", "Entry", new { area = "Blog" });
        }

    }
}
