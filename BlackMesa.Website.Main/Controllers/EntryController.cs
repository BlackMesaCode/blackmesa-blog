﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BlackMesa.Blog.Model;
using BlackMesa.Blog.DataLayer.DbContext;
using BlackMesa.Website.Main.Utilities;
using BlackMesa.Website.Main.ViewModels;
using Microsoft.Ajax.Utilities;
using PagedList;
using WebGrease.Css.Extensions;

namespace BlackMesa.Website.Main.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EntryController : BaseController
    {
        private readonly BlogContext _blogContext = new BlogContext();

        [AllowAnonymous]
        public ActionResult Index(int? page, string orderBy, string selectedTag, int? selectedYear, int? selectedMonth)
        {
            var language = ViewBag.CurrentLanguage as string;

            var model = User.IsInRole("Admin") ? _blogContext.Entries.Where(e => e.Language == language) : _blogContext.Entries.Where(e => e.Published && e.Language == language);

            // Filter
            if (!String.IsNullOrEmpty(selectedTag)) // by tag
            {
                model = model.Where(e => e.Tags.Select(t => t.Name).Contains(selectedTag));
            }
            else if (selectedYear.HasValue && selectedMonth.HasValue) // by date
            {
                model = model.Where(e => e.DateCreated.Year == selectedYear && e.DateCreated.Month == selectedMonth);
            }



            // Order
            orderBy = orderBy ?? "date"; // set default ordering
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
                default:
                    model = model.OrderByDescending(e => e.DateCreated);
                    break;
            }

            var viewModel = new EntryIndexViewModel
            {
                OrderBy = orderBy,
                SelectedTag = selectedTag,
                SelectedYear = selectedYear,
                SelectedMonth = selectedMonth,
                EntriesFound = model.Count(),
                Entries = model.ToPagedList(page ?? 1, 3),

            };

            return View(viewModel);


        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(string title, int id = 0)
        {
            Entry entry = _blogContext.Entries.Find(id);

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
                _blogContext.Comments.Add(comment);
                _blogContext.SaveChanges();
                return RedirectToAction("Details", "Entry", new { Id = comment.EntryId });
            }

            return View("Details", _blogContext.Entries.Single(c => c.Id == comment.EntryId));
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
                _blogContext.Entries.Add(entry);
                _blogContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(entry);
        }


        public ActionResult Edit(int id = 0)
        {
            Entry entry = _blogContext.Entries.Find(id);

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
                var dbEntry = _blogContext.Entries.Find(entry.Id);

                TryUpdateModel(dbEntry);  // tries to map the new values from the modelbinded entry to the passed dbmodel - this is the lazy way to go, instead of manually mapping all the properties

                AddTags(entry.TagsAsString, dbEntry);
                
                _blogContext.SaveChanges();

                DeleteTagsWithNoEntries();

                _blogContext.SaveChanges();
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

                if (!_blogContext.Tags.Where(t => t.Language == entry.Language).Select(t => t.Name).Contains(trimmedTag))
                {
                    var newTag = new Tag { Name = trimmedTag, Language = entry.Language };
                    _blogContext.Tags.Add(newTag);
                    entry.Tags.Add(newTag);
                }
                else
                {
                    entry.Tags.Add(_blogContext.Tags.Single(t => t.Name == trimmedTag && t.Language == entry.Language));
                }
            }
        }


        private void DeleteTagsWithNoEntries()
        {
            var tagsToDelete = _blogContext.Tags.Where(tag => tag.Entries.Count == 0);
            foreach (var tag in tagsToDelete)
            {
                _blogContext.Tags.Remove(tag);
            }
        }


        public ActionResult Delete(int id = 0)
        {
            Entry entry = _blogContext.Entries.Find(id);
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
            Entry entry = _blogContext.Entries.Find(id);
            _blogContext.Entries.Remove(entry);
            _blogContext.SaveChanges();

            DeleteTagsWithNoEntries();
            _blogContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult Archive()
        {
            var language = RouteData.Values["language"].ToString();
            var viewModel = new EntryArchiveViewModel()
            {
                AvailableYears = _blogContext.Entries.Where(e => e.Language == language).Select(e => e.DateCreated.Year).Distinct().ToList().Select(year =>
                                    new SelectListItem
                                    {
                                        Selected = true,
                                        Text = year.ToString(),
                                        Value = year.ToString(),
                                    }
                                    ),
                AvailableMonths = DateTimeFormatInfo.CurrentInfo.MonthNames.Where(month => !String.IsNullOrEmpty(month)).Select(month =>
                                    new SelectListItem
                                    {
                                        Selected = true,
                                        Text = month,
                                        Value = DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month.ToString(),
                                    }
                                    ),
                Tags = _blogContext.Tags.Where(e => e.Language == language).OrderByDescending(tag => tag.Entries.Count).
                                    ToDictionary(tag => tag.Name, tag => tag.Entries.Count),

            };

            return View(viewModel);
        }


        protected override void Dispose(bool disposing)
        {
            _blogContext.Dispose();
            base.Dispose(disposing);
        }
    }
}