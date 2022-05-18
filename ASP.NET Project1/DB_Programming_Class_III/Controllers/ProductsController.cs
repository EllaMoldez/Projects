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
    public class ProductsController : Controller
    {
        /// <summary>
        /// This method returns a list of Incident items
        /// </summary>
        /// <param name="top">how many items per page</param>
        /// <param name="page"></param>
        /// <returns></returns>
        /// 
        private TechSupportEntities context = new TechSupportEntities();

        // GET: Products
        public ActionResult Index(string searchTerm, int top = 4, int page = 1)
        {
            int skip = ((page - 1) * top);

            List<Product> products = context.Products.OrderBy(p => p.ProductCode).Skip(skip).Take(top).ToList();

            int totalProducts = context.Products.Count();

            ViewBag.totalProducts = totalProducts;
            ViewBag.page = page;
            ViewBag.top = top;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                products = products.Where(p => p.Name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1 || 
                p.ProductCode.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
            }

            return View(products);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductCode,Name,Version,ReleaseDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                context.Products.Add(product);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductCode,Name,Version,ReleaseDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                context.Entry(product).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //List<Product> products = context.Products.ToList();
            //var productToRemove = products.FirstOrDefault(p => p.ProductCode == id);
            Product product = context.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //List<Product> products = context.Products.ToList();
            //var productToRemove = products.FirstOrDefault(p => p.ProductCode == id);
            Product product = context.Products.Find(id);
            context.Products.Remove(product);
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
