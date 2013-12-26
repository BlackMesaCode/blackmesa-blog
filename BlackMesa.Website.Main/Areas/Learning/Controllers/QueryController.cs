using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    [Authorize]
    public class QueryController : Controller
    {

        public ActionResult Adjust()
        {
            return View();
        }

        public ActionResult Start()
        {
            return View();
        }
	}
}