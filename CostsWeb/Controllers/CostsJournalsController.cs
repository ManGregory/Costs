﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CostsWeb.Helper;
using CostsWeb.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace CostsWeb.Controllers
{
    [Authorize]
    public class CostsJournalsController : Controller
    {
        private CostsContext db = new CostsContext();
        private int _pageSize = 30;

        private void SetViewBag(int? categoryId = null, int? subCategoryId = null)
        {
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.UserId == CurrentUserId), "Id", "Name", categoryId);
            ViewBag.SubCategoryId = new SelectList(db.Categories.Where(c => c.UserId == CurrentUserId), "Id", "Name", subCategoryId);
        }

        // GET: CostsJournals
        public ActionResult Index(int? page, string sortOrder)
        {
            SetSortOrderParams(sortOrder);
            var userId = CurrentUserId;
            var costsJournal =
                db.CostsJournal.Include(c => c.Category)
                    .Include(c => c.SubCategory)
                    .Where(c => c.User.Id == userId);
            switch (sortOrder)
            {
                case "date" :
                    costsJournal = costsJournal.OrderBy(c => c.Date);
                    break;
                case "category" :
                    costsJournal = costsJournal.OrderBy(c => c.Category.Name);
                    break;
                case "category_desc" :
                    costsJournal = costsJournal.OrderByDescending(c => c.Category.Name);
                    break;
                case "subcategory":
                    costsJournal = costsJournal.OrderBy(c => c.SubCategory.Name);
                    break;
                case "subcategory_desc":
                    costsJournal = costsJournal.OrderByDescending(c => c.SubCategory.Name);
                    break;
                default:
                    costsJournal = costsJournal.OrderByDescending(c => c.Date);
                    break;
            }
            return View(costsJournal.ToPagedList(page ?? 1, _pageSize));
        }

        private void SetSortOrderParams(string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "date" : "";
            ViewBag.CategorySortParam = sortOrder == "category" ? "category_desc" : "category";
            ViewBag.SubCategorySortParam = sortOrder == "subcategory" ? "subcategory_desc" : "subcategory";
        }

        private string CurrentUserId
        {
            get { return User.Identity.GetUserId(); }
        }

        // GET: CostsJournals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostsJournal costsJournal =
                db.CostsJournal.Include(c => c.Category).Include(c => c.SubCategory).FirstOrDefault(c => c.Id == id);
            if (costsJournal == null)
            {
                return HttpNotFound();
            }
            return View(costsJournal);
        }

        // GET: CostsJournals/Create
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        // POST: CostsJournals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,CategoryId,SubCategoryId,Sum,Note")] CostsJournal costsJournal)
        {
            if (ModelState.IsValid)
            {
                costsJournal.UserId = CurrentUserId;
                db.CostsJournal.Add(costsJournal);
                db.SaveChanges();
                return RedirectToAction("Create").Success("Запись успешно создана");
            }
            SetViewBag(costsJournal.CategoryId, costsJournal.SubCategoryId);
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
            SetViewBag(costsJournal.CategoryId, costsJournal.SubCategoryId);
            return View(costsJournal);
        }

        // POST: CostsJournals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,CategoryId,SubCategoryId,Sum,Note")] CostsJournal costsJournal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(costsJournal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index").Success("Данные успешно сохраненны"); ;
            }
            SetViewBag(costsJournal.CategoryId, costsJournal.SubCategoryId);
            return View(costsJournal);
        }

        // GET: CostsJournals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostsJournal costsJournal =
                db.CostsJournal.Include(c => c.Category).Include(c => c.SubCategory).FirstOrDefault(c => c.Id == id);
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
            return RedirectToAction("Index").Success("Данные успешно удалены");
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
