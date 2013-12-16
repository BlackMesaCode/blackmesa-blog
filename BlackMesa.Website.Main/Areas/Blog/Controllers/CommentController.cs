using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Blog;

namespace BlackMesa.Website.Main.Areas.Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CommentController : BaseController
    {
        private readonly BlackMesaDbContext _db = new BlackMesaDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var comments = _db.Blog_Comments.OrderByDescending(comment => comment.DateCreated).Take(3);
            return PartialView(comments.ToList());
        }


        public ActionResult Edit(int id = 0)
        {
            var comment = _db.Blog_Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(comment).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Details", "Entry", new { Id = comment.EntryId });
            }

            return View(comment);
        }


        public ActionResult Delete(int id = 0)
        {
            Comment comment = _db.Blog_Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = _db.Blog_Comments.Find(id);
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