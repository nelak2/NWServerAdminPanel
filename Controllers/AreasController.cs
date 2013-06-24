using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWServerAdminPanel.Models;

namespace NWServerAdminPanel.Controllers
{
    public class AreasController : Controller
    {
        private AreaDbContext db = new AreaDbContext();

        //
        // GET: /Areas/

        public ActionResult Index()
        {
            return View(db.Areas.ToList());
        }

        //
        // GET: /Areas/Details/5

        public ActionResult Details(int id = 0)
        {
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        //
        // GET: /Areas/Create

        public ActionResult Create()
        {

            return View();
        }

        //
        // POST: /Areas/Create

        [HttpPost]
        public ActionResult Create(Area area)
        {
            if (ModelState.IsValid)
            {
                db.Areas.Add(area);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(area);
        }

        //
        // GET: /Areas/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        //
        // POST: /Areas/Edit/5

        [HttpPost]
        public ActionResult Edit(Area area)
        {
            if (ModelState.IsValid)
            {
                db.Entry(area).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(area);
        }

        //
        // GET: /Areas/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        //
        // POST: /Areas/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Area area = db.Areas.Find(id);
            db.Areas.Remove(area);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}