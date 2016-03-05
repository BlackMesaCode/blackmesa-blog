using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BlackMesa.Blog.Areas.Blog.ViewModels;
using BlackMesa.Blog.Controllers;
using BlackMesa.Blog.DataLayer;
using BlackMesa.Blog.Models.Blog;
using BlackMesa.Blog.Models.Identity;
using BlackMesa.Blog.Resources;
using Microsoft.AspNet.Identity;

namespace BlackMesa.Blog.Areas.Blog.Controllers
{
    public class CommentController : BaseController
    {

        private readonly BlackMesaDbContext _db = new BlackMesaDbContext();

        public ActionResult Index(int entryId)
        {
            var comments = _db.Blog_Comments.Where(c => c.EntryId == entryId).OrderBy(comment => comment.DateCreated).AsEnumerable();
            return PartialView("_Index", comments);
        }


        public ActionResult Create(int entryId)
        {
            var viewModel = new CreateCommentViewModel()
            {
                EntryId = entryId,
                Name = User.Identity.GetUserName(),
            };
            return PartialView("Create", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(CreateCommentViewModel commentViewModel)
        {
            var newComment = new Comment
            {
                Id = commentViewModel.Id,
                Name = commentViewModel.Name,
                Content = commentViewModel.Content,
                DateCreated = DateTime.Now,
                DateEdited = DateTime.Now,
                EntryId = commentViewModel.EntryId,
            };

            if (User.Identity.IsAuthenticated)
            {
                var user = _db.Users.Find(User.Identity.GetUserId());
                
                if (user == null)
                    return new HttpUnauthorizedResult();

                newComment.Owner = user;
                newComment.OwnerId = user.Id;
                newComment.Name = user.UserName;
            }

            if (!User.Identity.IsAuthenticated && _db.Users.Select(u => u.UserName).Contains(commentViewModel.Name))
                ModelState.AddModelError("NameAlreadyTaken", Strings.NameAlreadyTaken);


            if (ModelState.IsValid)
            {
                _db.Blog_Comments.Add(newComment);
                _db.SaveChanges();
                return RedirectToAction("Details", "Entry", new { Id = commentViewModel.EntryId });
            }

            return View("Create", commentViewModel);
        }

        public ActionResult Edit(int id = 0)
        {
            var comment = _db.Blog_Comments.Find(id);

            if (comment == null)
                return HttpNotFound();

            var viewModel = new EditCommentViewModel
            {
                Id = comment.Id,
                Content = comment.Content,
                EntryId = comment.EntryId,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCommentViewModel viewModel)
        {
            var comment = _db.Blog_Comments.Find(viewModel.Id);

            if (comment == null)
                return HttpNotFound();

            if (!User.IsInRole("Admin") && (String.IsNullOrEmpty(comment.OwnerId) || User.Identity.GetUserId() != comment.OwnerId))
                return new HttpUnauthorizedResult();

            comment.Content = viewModel.Content;
            comment.DateEdited = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.Entry(comment).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Details", "Entry", new { Id = comment.EntryId });
            }

            return View(viewModel);
        }


        public ActionResult Delete(int id = 0)
        {
            var comment = _db.Blog_Comments.Find(id);
            if (comment == null)
                return HttpNotFound();

            return View(comment);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var comment = _db.Blog_Comments.Find(id);

            if (comment == null)
                return HttpNotFound();

            if (!User.IsInRole("Admin") && (String.IsNullOrEmpty(comment.OwnerId) || User.Identity.GetUserId() != comment.OwnerId))
                return new HttpUnauthorizedResult();

            _db.Blog_Comments.Remove(comment);
            _db.SaveChanges();
            return RedirectToAction("Details", "Entry", new { Id = comment.EntryId });
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}