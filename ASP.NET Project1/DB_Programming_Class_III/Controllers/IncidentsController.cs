using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DB_Programming_Class_III.Models;

namespace DB_Programming_Class_III.Controllers
{
    public class IncidentsController : Controller
    {
        private TechSupportEntities db = new TechSupportEntities();

        // GET: Incidents
        public ActionResult Index()
        {
            var incidents = db.Incidents.Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician);
            return View(incidents.ToList());
        }

        // GET: Incidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // GET: Incidents/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name");
            ViewBag.ProductCode = new SelectList(db.Products, "ProductCode", "Name");
            ViewBag.TechID = new SelectList(db.Technicians, "TechID", "Name");
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentID,CustomerID,ProductCode,TechID,DateOpened,DateClosed,Title,Description")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Incidents.Add(incident);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", incident.CustomerID);
            ViewBag.ProductCode = new SelectList(db.Products, "ProductCode", "Name", incident.ProductCode);
            ViewBag.TechID = new SelectList(db.Technicians, "TechID", "Name", incident.TechID);
            return View(incident);
        }

        // GET: Incidents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", incident.CustomerID);
            ViewBag.ProductCode = new SelectList(db.Products, "ProductCode", "Name", incident.ProductCode);
            ViewBag.TechID = new SelectList(db.Technicians, "TechID", "Name", incident.TechID);
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentID,CustomerID,ProductCode,TechID,DateOpened,DateClosed,Title,Description")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incident).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", incident.CustomerID);
            ViewBag.ProductCode = new SelectList(db.Products, "ProductCode", "Name", incident.ProductCode);
            ViewBag.TechID = new SelectList(db.Technicians, "TechID", "Name", incident.TechID);
            return View(incident);
        }

        // GET: Incidents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Incident incident = db.Incidents.Find(id);
            db.Incidents.Remove(incident);
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
