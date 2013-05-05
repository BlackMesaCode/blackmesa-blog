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

        [AjaxOnly]
        public ActionResult Json(int id)
        {
            var assignedTags = new string[0];
            if (id != 0)
                assignedTags = _db.Entries.Find(id).Tags.Select(t => t.Name).ToArray();
            return Json(new { availableTags = _db.Tags.Select(t => t.Name).ToArray(), assignedTags = assignedTags }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public ActionResult JsonIndex(string selectedTags)
        {
            var assignedTags = new string[0];
            if (!String.IsNullOrEmpty(selectedTags))
               assignedTags = selectedTags.Split(',').ToArray();
            var availableTags = _db.Tags.Select(t => t.Name).ToArray();
            return Json(new { availableTags = availableTags, assignedTags = assignedTags }, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

    }
}
