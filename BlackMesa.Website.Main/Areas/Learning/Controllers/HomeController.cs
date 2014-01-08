using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;
using Microsoft.AspNet.Identity;

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