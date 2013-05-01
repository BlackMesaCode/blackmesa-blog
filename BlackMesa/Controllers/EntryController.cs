using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using BlackMesa.Models;
using BlackMesa.ViewModels;
using PagedList;

namespace BlackMesa.Controllers
{
    public class EntryController : Controller
    {
        private readonly BlackMesaDb _db = new BlackMesaDb();


        public ActionResult Index(EntryIndexViewModel viewModel)
        {
            var model = _db.Entries.Select(e => e);
            

            // Filter
            var selectedTags = viewModel.SelectedTags;

            if (!String.IsNullOrEmpty(selectedTags))
            {
                var selectedTagList = selectedTags.Split(',').ToList();

                model = model.Where(e => selectedTagList.All(tagString => (e.Tags.Select(t => t.Name).Contains(tagString))));
            }
            /* SelectedTags has to be stored in the ViewBag to make it accessible in a PartialView */
            ViewBag.SelectedTags = viewModel.SelectedTags; 


            // Order
            switch (viewModel.OrderBy)
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
                default:
                    model = model.OrderByDescending(e => e.DateCreated);
                    break;
            }

            // Paging
            const int pageSize = 3;
            var pageNumber = (viewModel.Page ?? 1);
            viewModel.Page = pageNumber;
            viewModel.Entries = model.ToPagedList(pageNumber, pageSize);

            
            /*  Problem: Ajax requests wont trigger the RenderScripts in the above view */
            if (Request.IsAjaxRequest())
                return PartialView("_Entries", viewModel.Entries);

            return View(viewModel);


//            var htmlContent = modelMaterialized.First().Content;
//            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
//            htmlDoc.LoadHtml(htmlContent);
//
//            if (htmlDoc.DocumentNode != null)
//            {
//                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//article");
//
//                if (bodyNode != null)
//                {
//                    // Do something with bodyNode
//                }
//            }

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
                var selectedTags = HttpContext.Request.Form["SelectedTags"];
                if (!String.IsNullOrEmpty(selectedTags))
                {
                    var selectedTagsList = selectedTags.Split(',').ToList();

                    entry.Tags = new Collection<Tag>();
                    foreach (var tag in selectedTagsList)
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
                var selectedTags = HttpContext.Request.Form["SelectedTags"];

                var dbEntry = _db.Entries.Find(entry.Id);

                TryUpdateModel(dbEntry);  // tries to map the new values from the modelbinded entry to the passed model - this is the lazy way to go, instead of manually mapping all the properties

                dbEntry.Tags.Clear();

                if (!String.IsNullOrEmpty(selectedTags))
                {
                    var selectedTagsList = selectedTags.Split(',').ToList();

                    foreach (var tag in selectedTagsList)
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