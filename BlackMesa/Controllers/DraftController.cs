using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace BlackMesa.Controllers
{
    public class DraftController : Controller
    {
        //
        // GET: /Entries/

        public ActionResult Index()
        {
            var directory = new DirectoryInfo(Server.MapPath(@"~\Views\Draft\Archive"));
            var files = directory.GetFiles().ToList();

            return View(files);
        }

        public ActionResult Details(string viewName)
        {
            return View("Archive\\" + viewName);
        }

    }
}
