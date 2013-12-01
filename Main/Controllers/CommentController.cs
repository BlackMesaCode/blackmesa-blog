using System.Data;
using System.Linq;
using System.Web.Mvc;
using Blog.Main.Models;

namespace Blog.Main.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CommentController : BaseController
    {
        private readonly BlackMesaDb _db = new BlackMesaDb();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var comments = _db.Comments.OrderByDescending(comment => comment.DateCreated).Take(3);
            return PartialView(comments.ToList());
        }


        public ActionResult Edit(int id = 0)
        {
            var comment = _db.Comments.Find(id);
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
            Comment comment = _db.Comments.Find(id);
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
            Comment comment = _db.Comments.Find(id);
            _db.Comments.Remove(comment);
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