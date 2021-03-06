﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BlackMesa.Blog.Controllers;
using BlackMesa.Blog.DataLayer;
using BlackMesa.Blog.Models.Blog;
using BlackMesa.Blog.Utilities;
using BlackMesa.Blog.ViewModels;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace BlackMesa.Blog.Areas.Blog.Controllers
{

    public class EntryController : BaseController
    {
        private readonly BlackMesaDbContext _dbContext = new BlackMesaDbContext();

        [AllowAnonymous]
        public ActionResult IndexClean(int? page, string orderBy)
        {
            return RedirectToActionPermanent("Index");
        }


        [AllowAnonymous]
        public ActionResult Index(int? page, string orderBy, string selectedTag, int? selectedYear, int? selectedMonth, string searchText)
        {
            var language = ViewBag.CurrentLanguage as string;

            var model = User.IsInRole("Admin") ? _dbContext.Blog_Entries.Where(e => e.Language == language) : _dbContext.Blog_Entries.Where(e => e.Published && e.Language == language);

            // Filter
            if (!String.IsNullOrEmpty(selectedTag)) // by tag
            {
                model = model.Where(e => e.Tags.Select(t => t.Name).Contains(selectedTag));
            }
            else if (selectedYear.HasValue) // by date
            {
                model = model.Where(e => e.DateCreated.Year == selectedYear);
                if (selectedMonth.HasValue)
                {
                    model = model.Where(e => e.DateCreated.Month == selectedMonth);
                }
            }
            else if (!String.IsNullOrEmpty(searchText)) // by search text
            {
                model =
                    model.Where(
                        e =>
                            e.Title.Contains(searchText) || e.Preview.Contains(searchText) ||
                            e.Content.Contains(searchText));
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
                SearchText = searchText,
                EntriesFound = model.Count(),
                Entries = model.ToPagedList(page ?? 1, 3),

            };

            return View(viewModel);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(string title, int id = 0)
        {
            Entry entry = _dbContext.Blog_Entries.Find(id);

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



        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Entry entry)
        {

            if (ModelState.IsValid)
            {
                AddTags(entry.TagsAsString, entry);
                _dbContext.Blog_Entries.Add(entry);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(entry);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            Entry entry = _dbContext.Blog_Entries.Find(id);

            if (entry == null)
            {
                return HttpNotFound();
            }
            return View(entry);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Entry entry)
        {
            if (ModelState.IsValid)
            {
                var dbEntry = _dbContext.Blog_Entries.Find(entry.Id);

                TryUpdateModel(dbEntry);  // tries to map the new values from the modelbinded entry to the passed dbmodel - this is the lazy way to go, instead of manually mapping all the properties

                AddTags(entry.TagsAsString, dbEntry);
                
                _dbContext.SaveChanges();

                DeleteTagsWithNoEntries();

                _dbContext.SaveChanges();
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

                if (!_dbContext.Blog_Tags.Where(t => t.Language == entry.Language).Select(t => t.Name).Contains(trimmedTag))
                {
                    var newTag = new Tag { Name = trimmedTag, Language = entry.Language };
                    _dbContext.Blog_Tags.Add(newTag);
                    entry.Tags.Add(newTag);
                }
                else
                {
                    entry.Tags.Add(_dbContext.Blog_Tags.Single(t => t.Name == trimmedTag && t.Language == entry.Language));
                }
            }
        }


        private void DeleteTagsWithNoEntries()
        {
            var tagsToDelete = _dbContext.Blog_Tags.Where(tag => tag.Entries.Count == 0);
            foreach (var tag in tagsToDelete)
            {
                _dbContext.Blog_Tags.Remove(tag);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            Entry entry = _dbContext.Blog_Entries.Find(id);
            if (entry == null)
            {
                return HttpNotFound();
            }
            return View(entry);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Entry entry = _dbContext.Blog_Entries.Find(id);
            _dbContext.Blog_Entries.Remove(entry);
            _dbContext.SaveChanges();

            DeleteTagsWithNoEntries();
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Search()
        {
            var language = RouteData.Values["language"].ToString();
            var viewModel = new EntryArchiveViewModel()
            {
                AvailableYears = _dbContext.Blog_Entries.Where(e => e.Language == language).Select(e => e.DateCreated.Year).Distinct().ToList().Select(year =>
                                    new SelectListItem
                                    {
                                        Selected = false,
                                        Text = year.ToString(),
                                        Value = year.ToString(),
                                    }
                                    ),
                AvailableMonths = DateTimeFormatInfo.CurrentInfo.MonthNames.Select(month =>  //.Where(month => !String.IsNullOrEmpty(month))
                                    new SelectListItem
                                    {
                                        Selected = String.IsNullOrEmpty(month),
                                        Text = month,
                                        Value = !String.IsNullOrEmpty(month) ? DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month.ToString() : String.Empty,
                                    }
                                    ),
                Tags = _dbContext.Blog_Tags.Where(e => e.Language == language).OrderByDescending(tag => tag.Entries.Count).
                                    ToDictionary(tag => tag.Name, tag => tag.Entries.Count),

            };

            return View(viewModel);
        }

        [AllowAnonymous]
        public ActionResult Rss()
        {
            var language = RouteData.Values["language"].ToString();
            var entries = _dbContext.Blog_Entries.Where(e => e.Language == language).OrderBy(e => e.DateCreated).Take(25).ToList()
                .Select(e => new SyndicationItem
                {
                    Title = new TextSyndicationContent(e.Title),
                    Content = new TextSyndicationContent(e.Preview),
                    LastUpdatedTime = new DateTimeOffset(e.DateEdited),
                    BaseUri = new Uri(Url.Action("Details", "Entry", new { Id = e.Id }, Request.Url.Scheme)), 
                });

            var feed = new SyndicationFeed(WebConfigurationManager.AppSettings.Get("SiteTitle"),
                WebConfigurationManager.AppSettings.Get("SiteDescription"), new Uri(HttpContext.ApplicationInstance.Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/")), entries)
            {
                Copyright = new TextSyndicationContent(Url.Action("Index", "LegalNotice")),
                Language = language
            };

            return new FeedResult(new Rss20FeedFormatter(feed));
        }


        protected override void Dispose(bool disposing)
        {
            _dbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}