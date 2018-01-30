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
    public class FilePathsController : Controller
    {
        private MyShopEntities db = new MyShopEntities();

        // GET: FilePaths
        public ActionResult Index(int id)
        {
			var fileToRetrieve = db.FilePaths.Find(id);
			return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
			/* var filePaths = db.FilePaths.Include(f => f.Product);
			 return View(filePaths.ToList());*/
		}

        // GET: FilePaths/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilePath filePath = db.FilePaths.Find(id);
            if (filePath == null)
            {
                return HttpNotFound();
            }
            return View(filePath);
        }

        // GET: FilePaths/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name");
            return View();
        }

        // POST: FilePaths/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FilePathId,FileName,FileType,ProductId,ContentType,Content")] FilePath filePath)
        {
            if (ModelState.IsValid)
            {
                db.FilePaths.Add(filePath);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", filePath.ProductId);
            return View(filePath);
        }

        // GET: FilePaths/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilePath filePath = db.FilePaths.Find(id);
            if (filePath == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", filePath.ProductId);
            return View(filePath);
        }

        // POST: FilePaths/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FilePathId,FileName,FileType,ProductId,ContentType,Content")] FilePath filePath)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filePath).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", filePath.ProductId);
            return View(filePath);
        }

        // GET: FilePaths/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilePath filePath = db.FilePaths.Find(id);
            if (filePath == null)
            {
                return HttpNotFound();
            }
            return View(filePath);
        }

        // POST: FilePaths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FilePath filePath = db.FilePaths.Find(id);
            db.FilePaths.Remove(filePath);
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
