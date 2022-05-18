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
    public class TechniciansController : Controller
    {
        private TechSupportEntities context = new TechSupportEntities();

        // GET: Technicians
        public ActionResult Index(string searchTerm, int top = 5, int page = 1)
        {
            int skip = ((page - 1) * top);

            List<Technician> technicians = context.Technicians.OrderBy(t => t.Name).Skip(skip).Take(top).ToList();

            int totalTechnicians = context.Technicians.Count();

            ViewBag.totalTechnicians = totalTechnicians;
            ViewBag.page = page;
            ViewBag.top = top;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                technicians = technicians.Where(t => t.Name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
            }

            return View(technicians);
        }

        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Technician technician = context.Technicians.Find(id);
        //    if (technician == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(technician);
        //}

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TechID,Name,Email,Phone")] Technician technician)
        {
            if (ModelState.IsValid)
            {
                context.Technicians.Add(technician);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(technician);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Technician technician = context.Technicians.Find(id);

            if (technician == null)
            {
                return HttpNotFound();
            }
            return View(technician);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TechID,Name,Email,Phone")] Technician technician)
        {
            if (ModelState.IsValid)
            {
                context.Entry(technician).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(technician);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Technician technician = context.Technicians.Find(id);
            
            if (technician == null)
            {
                return HttpNotFound();
            }
            return View(technician);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<Technician> technicians = context.Technicians.ToList();
            var techToRemove = technicians.FirstOrDefault(t => t.TechID == id);

            context.Technicians.Remove(techToRemove);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
