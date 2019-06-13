using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobOffersProject.Models;
using WebApplication1.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace JobOffersProject.Controllers
{
    [AllowAnonymous]
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jobs
        public ActionResult Index()
        {
            var jobs = db.Jobs.Include(j => j.categories);
            return View(jobs.ToList());
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            ViewBag.CategoriesId = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //HttpPostedFileBase is class of type file -> should have same name of (input of type file in view) to map it
        //i shold make some changes in create view form to allow me to send files(binary data)
        //he can't dont choose image becouse validation in script
        public ActionResult Create(Job job , HttpPostedFileBase upload) 
        {
            if (ModelState.IsValid)
            {
                //Path is class to deal with fiels and directories
                //combine is fun to add two paths in one path -> to get full path of image
                //Server.MapPath(path); -> get path relative of server path
                //~ is root directory
                string path = Path.Combine(Server.MapPath("~/Uploads") , upload.FileName); //Get the path of file in server
                // i will add folder with name = Uploads
                upload.SaveAs(path);//save file in path
                //so now i stored file in server
                //so i will put path of file in jop.jobimage
                job.JobImage = upload.FileName;
                job.UserId = User.Identity.GetUserId();//publisher of job
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriesId = new SelectList(db.Categories, "Id", "CategoryName", job.CategoriesId);
            return View(job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriesId = new SelectList(db.Categories, "Id", "CategoryName", job.CategoriesId);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //as create but if he dont enter new image take the last one , if he enter new image remove the old one
        public ActionResult Edit(Job job , HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                //if he enter new image
                if(upload != null)
                {
                    string oldpath = Path.Combine(Server.MapPath("~/Uploads"), job.JobImage);
                    string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                    System.IO.File.Delete(oldpath);
                    upload.SaveAs(path);
                    job.JobImage = upload.FileName;
                }
                //else keep the name of image as it is -> make sure u will recieve it in jop from view
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriesId = new SelectList(db.Categories, "Id", "CategoryName", job.CategoriesId);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
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
