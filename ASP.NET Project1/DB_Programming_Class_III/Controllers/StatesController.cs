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
    public class StatesController : Controller
    {
        private TechSupportEntities context = new TechSupportEntities();

        // GET: States
        public ActionResult Index(string searchTerm, int top = 8, int page = 1)
        {
            int skip = ((page - 1) * top);

            List<State> states = context.States.OrderBy(s => s.StateCode).Skip(skip).Take(top).ToList();

            int totalStates = context.States.Count();

            ViewBag.totalStates = totalStates;
            ViewBag.page = page;
            ViewBag.top = top;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                states = states.Where(s => s.StateCode.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1 || 
                s.StateName.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
            }

            return View(states);
        }

        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    State state = context.States.Find(id);

        //    if (state == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(state);
        //}

        //public ActionResult AddEditState(string code)
        //{
        //    State state = context.States.FirstOrDefault(s => s.StateCode == code);

        //    if (state == null)
        //    {
        //        state = new State();
        //    }
        //    return View(state);
        //}

        //public ActionResult AddEditState(State state)
        //{
        //    try
        //    {
        //        context.States.AddOrUpdate(state);
        //        context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //    }
        //    return View(state);
        //}

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StateCode,StateName,FirstZipCode,LastZipCode")] State state)
        {
            if (ModelState.IsValid)
            {
                context.States.Add(state);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(state);
        }

        // GET: States/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            State state = context.States.Find(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StateCode,StateName,FirstZipCode,LastZipCode")] State state)
        {
            if (ModelState.IsValid)
            {
                context.Entry(state).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(state);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<State> states = context.States.ToList();
            var stateToRemove = states.FirstOrDefault(s => s.StateCode == id);

            if (stateToRemove == null)
            {
                return HttpNotFound();
            }
            return View("Delete", stateToRemove);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            List<State> states = context.States.ToList();
            var stateToRemove = states.FirstOrDefault(s => s.StateCode == id);

            context.States.Remove(stateToRemove);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            var context = new TechSupportEntities();
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
