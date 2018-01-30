using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class BranchProductsController : Controller
    {
        private MyShopEntities db = new MyShopEntities();

        // GET: BranchProducts
        public ActionResult Index()
        {
            var branchProducts = db.BranchProducts.Include(b => b.Branch).Include(b => b.Product);
            return View(branchProducts.ToList());
        }

        // GET: BranchProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchProduct branchProduct = db.BranchProducts.Find(id);
            if (branchProduct == null)
            {
                return HttpNotFound();
            }
            return View(branchProduct);
        }

        // GET: BranchProducts/Create
        public ActionResult Create()
        {
            ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "BranchName");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name");
            return View();
        }

        // POST: BranchProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BranchProductId,ProductId,BranchId,Quantity")] BranchProduct branchProduct)
        {
            if (ModelState.IsValid)
            {
                db.BranchProducts.Add(branchProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "BranchName", branchProduct.BranchId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", branchProduct.ProductId);
            return View(branchProduct);
        }

        // GET: BranchProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchProduct branchProduct = db.BranchProducts.Find(id);
            if (branchProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "BranchName", branchProduct.BranchId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", branchProduct.ProductId);
            return View(branchProduct);
        }

        // POST: BranchProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BranchProductId,ProductId,BranchId,Quantity")] BranchProduct branchProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(branchProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchId = new SelectList(db.Branches, "BranchId", "BranchName", branchProduct.BranchId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", branchProduct.ProductId);
            return View(branchProduct);
        }

        // GET: BranchProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchProduct branchProduct = db.BranchProducts.Find(id);
            if (branchProduct == null)
            {
                return HttpNotFound();
            }
            return View(branchProduct);
        }

        // POST: BranchProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BranchProduct branchProduct = db.BranchProducts.Find(id);
            db.BranchProducts.Remove(branchProduct);
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
