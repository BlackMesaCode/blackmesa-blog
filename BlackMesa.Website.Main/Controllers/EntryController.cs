using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BlackMesa.Blog.DataLayer;
using BlackMesa.Blog.Model;
using BlackMesa.Website.Main.Utilities;
using BlackMesa.Website.Main.ViewModels;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace BlackMesa.Website.Main.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EntryController : BaseController
    {
        private readonly BlogDbContext _blogDbContext = new BlogDbContext();

        [AllowAnonymous]
        public ActionResult Index(EntryIndexViewModel viewModel)
        {
            var language = ViewBag.CurrentLanguage as string;
            var model = User.IsInRole("Admin") ? _blogDbContext.Entries.Where(e => e.Language == language) : _blogDbContext.Entries.Where(e => e.Published && e.Language == language);

            // Filter
            var selectedTags = viewModel.SelectedTags;

            if (!String.IsNullOrEmpty(selectedTags))
            {
                var selectedTagList = selectedTags.Split(',').ToList();

                model = model.Where(e => selectedTagList.All(tagString => (e.Tags.Select(t => t.Name).Contains(tagString))));
            }
            /* SelectedTags has to be stored in the ViewBag to make it accessible in a PartialView */
            ViewBag.SelectedTags = viewModel.SelectedTags;
            ViewBag.EntriesFound = model.Count();


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
            ViewBag.OrderBy = viewModel.OrderBy; 


            // Paging
            const int pageSize = 3;
            ViewBag.PageSize = pageSize;
            var pageNumber = (viewModel.Page ?? 1);
            viewModel.Page = pageNumber;
            viewModel.Entries = model.ToPagedList(pageNumber, pageSize);


            if (Request.IsAjaxRequest())
                return PartialView(viewModel);

            return View(viewModel);


        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(string title, int id = 0)
        {
            Entry entry = _blogDbContext.Entries.Find(id);

            if (entry == null || (!entry.Published && !User.IsInRole("Admin")))
            {
                return HttpNotFound();
            }

            /* if the title passed with the http-request doesnt equal the current entry title - we response with a moved permanent code to 
             * inform the search engines */

            var currentSeoFriendlyTitle = Utilities.Utilities.MakeUrlFriendly(entry.Title);
            if (title != currentSeoFriendlyTitle)
                return RedirectToActionPermanent("Details", new { Id = id, Title = currentSeoFriendlyTitle });

            return View(entry);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Details(Comment comment)
        {
            comment.DateCreated = DateTime.Now;
            comment.DateEdited = DateTime.Now;

            if (ModelState.IsValid)
            {
                _blogDbContext.Comments.Add(comment);
                _blogDbContext.SaveChanges();
                return RedirectToAction("Details", "Entry", new { Id = comment.EntryId });
            }

            return View("Details", _blogDbContext.Entries.Single(c => c.Id == comment.EntryId));
        }


        [HttpGet]
        public ActionResult Create()
        {
            var entry = new Entry();
            entry.DateCreated = DateTime.Now;
            entry.DateEdited = DateTime.Now;
            entry.Content = this.GetHtmlHelper().Partial("_ContentTemplate").ToHtmlString();

            return View(entry);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] 
        public ActionResult Create(Entry entry)
        {

            if (ModelState.IsValid)
            {
                AddTags(entry.TagsAsString, entry);
                _blogDbContext.Entries.Add(entry);
                _blogDbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(entry);
        }


        public ActionResult Edit(int id = 0)
        {
            Entry entry = _blogDbContext.Entries.Find(id);
            entry.DateEdited = DateTime.Now;

            if (entry == null)
            {
                return HttpNotFound();
            }
            return View(entry);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Entry entry)
        {
            if (ModelState.IsValid)
            {
                var dbEntry = _blogDbContext.Entries.Find(entry.Id);

                TryUpdateModel(dbEntry);  // tries to map the new values from the modelbinded entry to the passed dbmodel - this is the lazy way to go, instead of manually mapping all the properties

                AddTags(entry.TagsAsString, dbEntry);
                
                _blogDbContext.SaveChanges();

                DeleteTagsWithNoEntries();

                _blogDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entry);
        }

        
        private void AddTags(string tagsAsString, Entry entry)
        {
            var tags = tagsAsString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

            entry.Tags = new Collection<Tag>();
            entry.Tags.Clear(); // this line seems redundant, but is necessary, as otherwise, the existing tags wont be removed from the entry object
            

            foreach (var tag in tags.Where(tag => !tag.IsNullOrWhiteSpace())) // RemoveEmptyEntries does only remove strings with NO conent, but not string with only spaces as content
            {
                var trimmedTag = tag.TrimStart().TrimEnd(); // Remove leading and ending spaces from string

                if (!_blogDbContext.Tags.Where(t => t.Language == entry.Language).Select(t => t.Name).Contains(trimmedTag))
                {
                    var newTag = new Tag { Name = trimmedTag, Language = entry.Language };
                    _blogDbContext.Tags.Add(newTag);
                    entry.Tags.Add(newTag);
                }
                else
                {
                    entry.Tags.Add(_blogDbContext.Tags.Single(t => t.Name == trimmedTag && t.Language == entry.Language));
                }
            }
        }


        private void DeleteTagsWithNoEntries()
        {
            var tagsToDelete = _blogDbContext.Tags.Where(tag => tag.Entries.Count == 0);
            foreach (var tag in tagsToDelete)
            {
                _blogDbContext.Tags.Remove(tag);
            }
        }


        public ActionResult Delete(int id = 0)
        {
            Entry entry = _blogDbContext.Entries.Find(id);
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
            Entry entry = _blogDbContext.Entries.Find(id);
            _blogDbContext.Entries.Remove(entry);
            _blogDbContext.SaveChanges();

            DeleteTagsWithNoEntries();
            _blogDbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            _blogDbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}