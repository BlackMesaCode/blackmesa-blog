using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Models;

namespace BlackMesa.Controllers
{
    public class TagController : Controller
    {
        private readonly BlackMesaDb _db = new BlackMesaDb();

        public ActionResult Index()
        {
            return PartialView("_Index", _db.Tags.ToList());
        }

        
        public ActionResult Json()
        {
            return Json(_db.Tags.Select(t => t.Name).ToList());
        }


        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

    }
}
