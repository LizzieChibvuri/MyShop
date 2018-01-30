using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyShop.Models;
using System.Data.Entity.Infrastructure;

namespace MyShop.Controllers
{
    public class ProductsController : Controller
    {
        private MyShopEntities db = new MyShopEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			Product product = db.Products.Include(p => p.FilePaths).SingleOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,Name,Description,Price,CategoryId")] Product product,HttpPostedFileBase upload)
        {
			Console.WriteLine("inside product create");
			try
			{
				if (ModelState.IsValid)
				{
					Console.WriteLine("model valid");

					if (upload != null && upload.ContentLength > 0)
					{
						var avatar = new FilePath
						{
							FileName = System.IO.Path.GetFileName(upload.FileName),
							FileType = 1,
							ContentType = upload.ContentType
						};
						using (var reader = new System.IO.BinaryReader(upload.InputStream))
						{
							avatar.Content = reader.ReadBytes(upload.ContentLength);
						}
						product.FilePaths = new List<FilePath> { avatar };
					}

					db.Products.Add(product);
					db.SaveChanges();
					return RedirectToAction("Index");
				}

			}
			catch(RetryLimitExceededException /* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
           

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			Product product = db.Products.Include(p => p.FilePaths).SingleOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int? id, HttpPostedFileBase upload)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var product = db.Products.Find(id);
			if (TryUpdateModel(product, "",
				new string[] { "Name", "Description", "Price","CategoryId" }))
			{
				try
				{
        
					
					if (upload != null && upload.ContentLength > 0)
					{
						/* This is used when storing photos in the filesystem not in database.FilePath tbl has FilePathID,FileName,FileType,ProductID
						var photo = new FilePath
						{
							FileName = System.IO.Path.GetFileName(upload.FileName),
						   //FileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(upload.FileName), use this to guarantee unique filename so files dont overide each other
							FileType = 2
						};
						product.FilePaths = new List<FilePath>();
						product.FilePaths.Add(photo);
						*/

						if (product.FilePaths.Any(f => f.FileType == 1))
						{
							db.FilePaths.Remove(product.FilePaths.First(f => f.FileType == 1));
						}
						var photo = new FilePath
						{
							FileName = System.IO.Path.GetFileName(upload.FileName),
							FileType = 1,
							ContentType = upload.ContentType
						};
						using (var reader = new System.IO.BinaryReader(upload.InputStream))
						{
							photo.Content = reader.ReadBytes(upload.ContentLength);
						}
						product.FilePaths = new List<FilePath> { photo };
					}

					db.Entry(product).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				catch (RetryLimitExceededException /* dex */)
				{
					//Log the error (uncomment dex variable name and add a line here to write a log.
					ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");

				}
			}
			ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
			return View(product);
		}
		/*
		// POST: Products/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,Name,Description,Price,CategoryId")] Product product,HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
				db.Entry(product).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");	
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }*/

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
