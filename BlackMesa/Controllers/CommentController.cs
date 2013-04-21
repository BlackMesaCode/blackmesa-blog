using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Models;

namespace BlackMesa.Controllers
{
    public class CommentController : Controller
    {
        private BlackMesaDb db = new BlackMesaDb();

        //
        // GET: /Comment/

//        public ActionResult Index()
//        {
//            var comments = db.Comments.Include(c => c.Entry);
//            return View(comments.ToList());
//        }
//
//        //
//        // GET: /Comment/Details/5
//
//        public ActionResult Details(int id = 0)
//        {
//            Comment comment = db.Comments.Find(id);
//            if (comment == null)
//            {
//                return HttpNotFound();
//            }
//            return View(comment);
//        }

        //
        // POST: /Comment/Create

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(Comment comment)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Comments.Add(comment);
//                db.SaveChanges();
//                return RedirectToAction("Details", "Entry", new { comment.Id });
//            }
//
//            ViewBag.EntryId = new SelectList(db.Entries, "Id", "Title", comment.EntryId);
//            return View("../Entry/Details", comment.Entry);
//        }

        //
        // GET: /Comment/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        //
        // POST: /Comment/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Entry", new { Id = comment.EntryId });
            }

            return View(comment);
        }

        //
        // GET: /Comment/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //
        // POST: /Comment/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "Entry", new { Id = comment.EntryId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}