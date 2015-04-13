using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CostsWeb.Helper;
using CostsWeb.Models;
using Microsoft.AspNet.Identity;

namespace CostsWeb.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private CostsContext db = new CostsContext();

        private string CurrentUserId
        {
            get { return User.Identity.GetUserId(); }
        }

        private bool CheckUserId(int recordId)
        {
            var category = db.Categories.Find(recordId);
            return ((category != null) && (category.UserId == CurrentUserId));
        }

        // GET: Categories
        public ActionResult Index()
        {
            var userId = CurrentUserId;
            return View(db.Categories.Where(c => (c.UserId == userId) && !c.IsDeleted).ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if ((category == null) || !CheckUserId(id.Value))
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.UserId = CurrentUserId;
                category.IsDeleted = false;
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Create").Success(this.CreateFlashMessage("Запись успешно добавлена", category.Id));
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if ((category == null) || !CheckUserId(id.Value))
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.UserId = CurrentUserId;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index").Success(this.CreateFlashMessage("Данные успешно удалены", category.Id));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if ((category == null) || !CheckUserId(id.Value))
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            if ((category == null) || !CheckUserId(id))
            {
                return HttpNotFound();
            }
            var flashMessage = category.IsDeleted
                ? "Запись успешно удалена"
                : this.CreateFlashMessage("Запись успешно удалена", category.Id);
            if (category.IsDeleted)
            {
                db.Categories.Remove(category);
            }
            else
            {
                category.IsDeleted = true;
            }
            db.SaveChanges();
            return RedirectToAction("Index").Success(flashMessage);
        }

        public ActionResult UndoDelete(int id)
        {
            Category category = db.Categories.Find(id);
            if ((category == null) || !CheckUserId(id))
            {
                return HttpNotFound();
            }
            category.IsDeleted = false;
            db.SaveChanges();
            return
                RedirectToAction("Index").Success(this.CreateFlashMessage("Запись успешно восстановлена", category.Id));
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
