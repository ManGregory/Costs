using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CostsWeb.Models;

namespace CostsWeb.Controllers
{
    public class CostsJournalsController : Controller
    {
        private CostsContext db = new CostsContext();

        // GET: CostsJournals
        public ActionResult Index()
        {
            var costsJournal = db.CostsJournal.Include(c => c.Category);
            return View(costsJournal.ToList());
        }

        // GET: CostsJournals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostsJournal costsJournal = db.CostsJournal.Find(id);
            if (costsJournal == null)
            {
                return HttpNotFound();
            }
            return View(costsJournal);
        }

        // GET: CostsJournals/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: CostsJournals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,CategoryId,Sum,Note")] CostsJournal costsJournal)
        {
            if (ModelState.IsValid)
            {
                db.CostsJournal.Add(costsJournal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", costsJournal.CategoryId);
            return View(costsJournal);
        }

        // GET: CostsJournals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostsJournal costsJournal = db.CostsJournal.Find(id);
            if (costsJournal == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", costsJournal.CategoryId);
            return View(costsJournal);
        }

        // POST: CostsJournals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,CategoryId,Sum,Note")] CostsJournal costsJournal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(costsJournal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", costsJournal.CategoryId);
            return View(costsJournal);
        }

        // GET: CostsJournals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostsJournal costsJournal = db.CostsJournal.Find(id);
            if (costsJournal == null)
            {
                return HttpNotFound();
            }
            return View(costsJournal);
        }

        // POST: CostsJournals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CostsJournal costsJournal = db.CostsJournal.Find(id);
            db.CostsJournal.Remove(costsJournal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
