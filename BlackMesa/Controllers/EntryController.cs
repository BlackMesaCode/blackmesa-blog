using System;
using System.Collections.Generic;
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

            var modelMaterialized = model.ToList();
            var htmlContent = modelMaterialized.First().Content;
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//article");

                if (bodyNode != null)
                {
                    // Do something with bodyNode
                }
            }

            return View(modelMaterialized);
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Entry entry)
        {
            if (ModelState.IsValid)
            {
                var hiddenTagList = HttpContext.Request.Form["hidden-TagList"];
                if (!String.IsNullOrEmpty(hiddenTagList))
                {
                    var tagList = hiddenTagList.Split(',').ToList();

                    entry.Tags = new Collection<Tag>();
                    foreach (var tag in tagList)
                    {
                        if (!_db.Tags.Select(t => t.Name).Contains(tag))
                        {
                            var newTag = new Tag { Name = tag };
                            _db.Tags.Add(newTag);
                            entry.Tags.Add(newTag);
                        }
                        else
                        {
                            entry.Tags.Add(_db.Tags.Single(t => t.Name == tag));
                        }
                    }
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

                var dbEntry = _db.Entries.Find(entry.Id);

                TryUpdateModel(dbEntry);  // tries to map the new values from the modelbinded entry to the passed model - this is the lazy way to go, instead of manually mapping all the properties

                dbEntry.Tags.Clear();

                if (!String.IsNullOrEmpty(hiddenTagList))
                {
                    var tagList = hiddenTagList.Split(',').ToList();

                    foreach (var tag in tagList)
                    {
                        if (!_db.Tags.Select(t => t.Name).Contains(tag))
                        {
                            var newTag = new Tag { Name = tag };
                            _db.Tags.Add(newTag);
                            dbEntry.Tags.Add(newTag);
                        }
                        else
                        {
                            dbEntry.Tags.Add(_db.Tags.Single(t => t.Name == tag));
                        }
                    }
                }
                _db.Entry(dbEntry).State = EntityState.Modified;
                _db.SaveChanges();

                var tagsToDelete = _db.Tags.Where(tag => tag.Entries.Count == 0);
                foreach (var tag in tagsToDelete)
                {
                    _db.Tags.Remove(tag);
                    //_db.Entry(tag).State = EntityState.Deleted;
                }
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