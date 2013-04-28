using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using BlackMesa.Models;

namespace BlackMesa.Controllers
{
    public class EntryController : Controller
    {
        private readonly BlackMesaDb _db = new BlackMesaDb();


        public ActionResult Index(string orderBy = "date")
        {
//            var model = new List<EntryViewModel>();
//
//            foreach (var entry in _db.Entries.ToList())
//            {
//                model.Add(new EntryViewModel
//                                  {
//                                    Id = entry.Id,
//                                    Title = entry.Title,
//                                    Tags = entry.Tags, // String.Join(", ", entry.Tags.Select(e => e.Name)),
//                                    Content = entry.Content,
//                                    DateCreated = entry.DateCreated,
//                                    DateEdited = entry.DateEdited,
//                                    Comments = entry.Comments,
//                                  });
//            }

            var model = _db.Entries.Select(e => e);
            switch (orderBy)
            {
                case "date":
                    model = model.OrderByDescending(e => e.DateCreated);
                    break;
                case "comments":
                    model = model.OrderByDescending(e => e.Comments.Count);       
                    break;
                case "views":
                    model = model.OrderByDescending(e => e.Comments.Count);
                    break;
            }

            return View(model.ToList());
        }


        [HttpGet]
        public ActionResult Details(int id = 0)
        {
            Entry entry = _db.Entries.Find(id);
            if (entry == null)
            {
                return HttpNotFound();
            }
            return View(entry);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(Comment comment)
        {
            comment.DateCreated = DateTime.Now;
            comment.DateEdited = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.Comments.Add(comment);
                _db.SaveChanges();
                return RedirectToAction("Details", "Entry", new { Id = comment.EntryId });
            }

            return View("Details", _db.Entries.Single(c => c.Id == comment.EntryId));
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        private Entry UpdateEntryTags(Entry entry, string hiddenTagList)
        {
            var tagList = hiddenTagList.Split(',').ToList();
            foreach (var tag in tagList)
            {
                // Create tag if not existing already
                if (!_db.Tags.Select(t => t.Name).Contains(tag))
                    _db.Tags.Add(new Tag { Name = tag });
            }

            try
            {
                _db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

            }

            entry.Tags = new Collection<Tag>();
            foreach (var tag in tagList)
            {
                // Search tag in tags table. connect the found tag with the entry
                var dbTag = _db.Tags.Single(t => t.Name == tag);
                entry.Tags.Add(dbTag);
            }
            return entry;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Entry entry)
        {
            if (ModelState.IsValid)
            {
                var hiddenTagList = HttpContext.Request.Form["hidden-TagList"];
                if (!String.IsNullOrEmpty(hiddenTagList))
                {
                    entry = UpdateEntryTags(entry, hiddenTagList);
                }

                _db.Entries.Add(entry);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(entry);
        }


        public ActionResult Edit(int id = 0)
        {
            Entry entry = _db.Entries.Find(id);
            if (entry == null)
            {
                return HttpNotFound();
            }
            return View(entry);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Entry entry)
        {
            if (ModelState.IsValid)
            {
                var hiddenTagList = HttpContext.Request.Form["hidden-TagList"];
                if (!String.IsNullOrEmpty(hiddenTagList))
                {
                    entry = UpdateEntryTags(entry, hiddenTagList);
                }

                _db.Entry(entry).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entry);
        }


        public ActionResult Delete(int id = 0)
        {
            Entry entry = _db.Entries.Find(id);
            if (entry == null)
            {
                return HttpNotFound();
            }
            return View(entry);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entry entry = _db.Entries.Find(id);
            _db.Entries.Remove(entry);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}