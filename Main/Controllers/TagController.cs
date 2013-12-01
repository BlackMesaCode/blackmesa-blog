using System;
using System.Linq;
using System.Web.Mvc;
using Blog.Main.Models;
using Blog.Main.Utilities;

namespace Blog.Main.Controllers
{
    public class TagController : BaseController
    {
        private readonly BlackMesaDb _db = new BlackMesaDb();

        [AjaxOnly]
        public ActionResult Json(int id)
        {
            var assignedTags = new string[0];
            if (id != 0)
                assignedTags = _db.Entries.Find(id).Tags.Select(t => t.Name).ToArray();

            var language = ViewBag.CurrentLanguage as string;
            var availableTags = _db.Tags.Where(t => t.Language == language).Select(t => t.Name).ToArray();

            return Json(new { availableTags = availableTags, assignedTags = assignedTags }, JsonRequestBehavior.AllowGet);
        }
        
        [AjaxOnly]
        public ActionResult JsonIndex(string selectedTags)
        {
            var assignedTags = new string[0];
            if (!String.IsNullOrEmpty(selectedTags))
               assignedTags = selectedTags.Split(',').ToArray();

            var language = ViewBag.CurrentLanguage as string;
            var availableTags = _db.Tags.Where(t => t.Language == language).Select(t => t.Name).ToArray();

            return Json(new { availableTags = availableTags, assignedTags = assignedTags }, JsonRequestBehavior.AllowGet);
        }
        

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

    }
}
