using DB_Programming_Class_III.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DB_Programming_Class_III.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// This method returns a list of Incident items
        /// </summary>
        /// <param name="top">how many items per page</param>
        /// <param name="page"></param>
        /// <returns></returns>
        
        private TechSupportEntities context = new TechSupportEntities();

        public ActionResult Index(string searchTerm,  int top = 8, int page = 1)
        {
            int skip = ((page - 1) * top);
            
            List<Incident> incident = context.Incidents.OrderByDescending(i => i.DateOpened).Skip(skip).Take(top).ToList();

            int totalIncidents = context.Incidents.Count();

            ViewBag.totalIncidents = totalIncidents;
            ViewBag.page = page;
            ViewBag.top = top;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                incident = incident.Where(i => i.Title.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1 || 
                i.Customer.Name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1 || 
                i.ProductCode.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
            }

            return View(incident);
        }

        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(context.Customers, "CustomerID", "Name");
            ViewBag.ProductCode = new SelectList(context.Products, "ProductCode", "Name");
            ViewBag.TechID = new SelectList(context.Technicians, "TechID", "Name");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentID,CustomerID,ProductCode,TechID,DateOpened,DateClosed,Title,Description")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                context.Incidents.Add(incident);
                context.SaveChanges();
                
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(context.Customers, "CustomerID", "Name", incident.CustomerID);
            ViewBag.ProductCode = new SelectList(context.Products, "ProductCode", "Name", incident.ProductCode);
            ViewBag.TechID = new SelectList(context.Technicians, "TechID", "Name", incident.TechID);
            
            return View(incident);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = context.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(context.Customers, "CustomerID", "Name", incident.CustomerID);
            ViewBag.ProductCode = new SelectList(context.Products, "ProductCode", "Name", incident.ProductCode);
            ViewBag.TechID = new SelectList(context.Technicians, "TechID", "Name", incident.TechID);
            
            return View(incident);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentID,CustomerID,ProductCode,TechID,DateOpened,DateClosed,Title,Description")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                context.Entry(incident).State = EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(context.Customers, "CustomerID", "Name", incident.CustomerID);
            ViewBag.ProductCode = new SelectList(context.Products, "ProductCode", "Name", incident.ProductCode);
            ViewBag.TechID = new SelectList(context.Technicians, "TechID", "Name", incident.TechID);
            
            return View(incident);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Incident incident = context.Incidents.Find(id);

            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Incident incident = context.Incidents.Find(id);
            context.Incidents.Remove(incident);
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

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        public ActionResult Contact(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("ellemoldez@gmail.com", "Elle Moldez");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "test123";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
        }
    }
}