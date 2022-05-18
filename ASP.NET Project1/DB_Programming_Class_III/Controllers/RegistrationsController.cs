using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DB_Programming_Class_III.Models;

namespace DB_Programming_Class_III.Controllers
{
    public class RegistrationsController : Controller
    {
        private TechSupportEntities context = new TechSupportEntities();

        // GET: Registrations
        public ActionResult Index(string searchTerm, int top = 8, int page = 1)
        {
            int skip = ((page - 1) * top);

            List<Registration> registrations = context.Registrations.OrderBy(r => r.RegistrationDate).Skip(skip).Take(top).ToList();

            int totalRegistrations = context.Registrations.Count();

            ViewBag.totalRegistrations = totalRegistrations;
            ViewBag.page = page;
            ViewBag.top = top;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                registrations = registrations.Where(r => r.Customer.Name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                r.ProductCode.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
            }

            return View(registrations);
        }


        //// GET: Registrations/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Registration registration = db.Registrations.Find(id);
        //    if (registration == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(registration);
        //}

        // GET: Registrations/Create

        //public ActionResult Create(int id = 0)
        //{
        //    var context = new TechSupportEntities();

        //    Registration registrations = context.Registrations.FirstOrDefault(x => x.CustomerID == id);

        //    if (registrations == null)
        //    {
        //        registrations = new Registration();
        //    }

        //    return View(registrations);
        //}


        //[HttpPost]
        //public ActionResult Create(Registration registrations)
        //{
        //    var context = new TechSupportEntities();
        //    try
        //    {
        //        context.Registrations.AddOrUpdate(registrations);
        //        context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //    }
        //    return View(registrations);
        //}

        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(context.Customers, "CustomerID", "Name");
            ViewBag.ProductCode = new SelectList(context.Products, "ProductCode", "Name");
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,ProductCode,RegistrationDate")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                context.Registrations.Add(registration);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(context.Customers, "CustomerID", "Name", registration.CustomerID);
            ViewBag.ProductCode = new SelectList(context.Products, "ProductCode", "Name", registration.ProductCode);
            return View(registration);
        }

        // GET: Registrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = context.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(context.Customers, "CustomerID", "Name", registration.CustomerID);
            ViewBag.ProductCode = new SelectList(context.Products, "ProductCode", "Name", registration.ProductCode);
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,ProductCode,RegistrationDate")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                context.Entry(registration).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(context.Customers, "CustomerID", "Name", registration.CustomerID);
            ViewBag.ProductCode = new SelectList(context.Products, "ProductCode", "Name", registration.ProductCode);
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = context.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Registration registration = context.Registrations.Find(id);
            context.Registrations.Remove(registration);
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
