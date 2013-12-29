using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Website.Main.Controllers;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}