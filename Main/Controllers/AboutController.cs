using System.Web.Mvc;

namespace Blog.Main.Controllers
{
    public class AboutController : BaseController
    {
        //
        // GET: /About/

        public ActionResult Index()
        {
//            if (ViewBag.CurrentLanguage == "de")
//                return RedirectToActionPermanent("Details", "Entry", new {Id = 1});
//
//            if (ViewBag.CurrentLanguage == "en")
//                return RedirectToActionPermanent("Details", "Entry", new { Id = 2 });
//
//            return HttpNotFound();

            return View();
        }

    }
}
