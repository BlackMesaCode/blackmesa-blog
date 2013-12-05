using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BlackMesa.Blog.DataLayer;
using BlackMesa.Blog.Model;

namespace BlackMesa.Website.Main.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CommentController : BaseController
    {
        private readonly BlogDbContext _blogDbContext = new BlogDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var comments = _blogDbContext.BlogComments.OrderByDescending(comment => comment.DateCreated).Take(3);
            return PartialView(comments.ToList());
        }


        public ActionResult Edit(int id = 0)
        {
            var comment = _blogDbContext.BlogComments.Find(id);
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
                _blogDbContext.Entry(comment).State = EntityState.Modified;
                _blogDbContext.SaveChanges();
                return RedirectToAction("Details", "Entry", new { Id = comment.EntryId });
            }

            return View(comment);
        }


        public ActionResult Delete(int id = 0)
        {
            Comment comment = _blogDbContext.BlogComments.Find(id);
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
            Comment comment = _blogDbContext.BlogComments.Find(id);
            _blogDbContext.BlogComments.Remove(comment);
            _blogDbContext.SaveChanges();
            return RedirectToAction("Details", "Entry", new { Id = comment.EntryId });
        }

        protected override void Dispose(bool disposing)
        {
            _blogDbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}