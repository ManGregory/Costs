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

        private bool CheckUserId(int recordId)
        {
            var costsJournal = db.CostsJournal.Find(recordId);
            return ((costsJournal != null) && (costsJournal.UserId == CurrentUserId));
        }

        private void SetViewBag(int? categoryId = null, int? subCategoryId = null)
        {
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.UserId == CurrentUserId), "Id", "Name", categoryId);
            ViewBag.SubCategoryId = new SelectList(db.Categories.Where(c => c.UserId == CurrentUserId), "Id", "Name", subCategoryId);
        }

        // GET: CostsJournals
        public ActionResult Index(int? page, string sortOrder, DateTime? dateFrom, DateTime? dateTo,
            DateTime? currentDateFrom, DateTime? currentDateTo, int? categoryId, int? subCategoryId, string note,
            int? currentCategoryId, int? currentSubCategoryid, string currentNote)
        {
            SetSortOrderParams(sortOrder);
            if ((dateFrom != null) || (dateTo != null) || (categoryId != null) || 
                (subCategoryId != null) || !string.IsNullOrEmpty(note))
            {
                page = 1;
            }
            dateFrom = dateFrom ?? currentDateFrom;
            dateTo = dateTo ?? currentDateTo;
            categoryId = categoryId ?? currentCategoryId;
            subCategoryId = subCategoryId ?? currentSubCategoryid;
            note = note ?? currentNote;
            SetFilter(dateFrom, dateTo, categoryId, subCategoryId, note);            
            var userId = CurrentUserId;
            var costsJournal =
                db.CostsJournal.Include(c => c.Category)
                    .Include(c => c.SubCategory)
                    .Where(c => (c.User.Id == userId) && !c.IsDeleted);
            if ((dateFrom != null) && (dateTo != null))
            {
                costsJournal = costsJournal.Where(c => c.Date >= dateFrom && c.Date <= dateTo);
            }
            if (categoryId != null)
            {
                costsJournal = costsJournal.Where(c => c.CategoryId == categoryId);
            }
            if (subCategoryId != null)
            {
                costsJournal = costsJournal.Where(c => c.SubCategoryId == subCategoryId);
            }
            if (!string.IsNullOrWhiteSpace(note))
            {
                costsJournal = costsJournal.Where(c => c.Note.ToLower().Contains(note));
            }
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

        private void SetFilter(DateTime? dateFrom, DateTime? dateTo, int? categoryId, int? subCategoryId, string note)
        {
            ViewBag.CurrentDateFrom = dateFrom;
            ViewBag.CurrentDateTo = dateTo;
            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name", categoryId);
            ViewBag.SubCategories = new SelectList(db.Categories, "Id", "Name", subCategoryId);
            ViewBag.CurrentCategoryId = categoryId;
            ViewBag.CurrentSubCategoryId = subCategoryId;
            ViewBag.CurrentNote = note;
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
            if ((costsJournal == null) || !CheckUserId(id.Value))
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
                return RedirectToAction("Create")
                    .Success(this.CreateFlashMessage("Запись успешно создана", costsJournal.Id));
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
            if ((costsJournal == null) || !CheckUserId(id.Value))
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
                costsJournal.UserId = CurrentUserId;
                db.Entry(costsJournal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index").Success(
                    this.CreateFlashMessage("Данные успешно сохранены", costsJournal.Id));
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
            if ((costsJournal == null) || !CheckUserId(id.Value))
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
            if ((costsJournal == null) || !CheckUserId(id))
            {
                return HttpNotFound();
            }
            var flashMessage = costsJournal.IsDeleted
                ? "Запись успешно удалена"
                : this.CreateFlashMessage("Запись успешно удалена", costsJournal.Id);
            if (costsJournal.IsDeleted)
            {
                db.CostsJournal.Remove(costsJournal);   
            }
            else
            {
                costsJournal.IsDeleted = true;
            }
            db.SaveChanges();
            return RedirectToAction("Index").Success(flashMessage);                
        }

        public ActionResult UndoDelete(int id)
        {
            var costsJournal = db.CostsJournal.Find(id);
            if ((costsJournal == null) || !CheckUserId(id))
            {
                return HttpNotFound();
            }
            costsJournal.IsDeleted = false;
            db.SaveChanges();
            return
                RedirectToAction("Index").Success(this.CreateFlashMessage("Запись успешно восстановлена", costsJournal.Id));
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
