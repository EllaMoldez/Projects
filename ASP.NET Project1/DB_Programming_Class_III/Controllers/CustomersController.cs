using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DB_Programming_Class_III.Models;

namespace DB_Programming_Class_III.Controllers
{
    public class CustomersController : Controller
    {
        private TechSupportEntities context = new TechSupportEntities();

        // GET: Customers
        public ActionResult Index(string searchTerm, int top = 8, int page = 1)
        {
            int skip = ((page - 1) * top);

            List<Customer> customers = context.Customers.OrderBy(c => c.Name).Skip(skip).Take(top).ToList();

            //List<Customer> customers = context.Customers.ToList();

            int totalCustomers = context.Customers.Count();

            ViewBag.totalCustomers = totalCustomers;
            ViewBag.page = page;
            ViewBag.top = top;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                customers = customers.Where(c => c.Name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1 || c.City.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
            }

            return View(customers);
        }


        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = context.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.State = new SelectList(context.States, "StateCode", "StateName");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,Name,Address,City,State,ZipCode,Phone,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                context.Customers.Add(customer);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.State = new SelectList(context.States, "StateCode", "StateName", customer.State);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = context.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.State = new SelectList(context.States, "StateCode", "StateName", customer.State);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,Name,Address,City,State,ZipCode,Phone,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                context.Entry(customer).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.State = new SelectList(context.States, "StateCode", "StateName", customer.State);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = context.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = context.Customers.Find(id);
            context.Customers.Remove(customer);
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
