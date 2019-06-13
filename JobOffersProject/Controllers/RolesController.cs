using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace JobOffersProject.Controllers
{
    [Authorize(Roles ="Admins")]
    public class RolesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //i make rolemviewodel class similar to IdentityRole to make views based on it then i will delete it -> to generate views auto
        //when i create view i pass to him roleviewmodel as model and dont pass databasecontext to avoid of create another class in database
        //becouse i already hava Roles table
        // GET: Roles
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
                return HttpNotFound();
            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            try
            {
                // TODO: Add insert logic here
                if(ModelState.IsValid)
                {
                    db.Roles.Add(role);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(role);
            }
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
                return HttpNotFound();
            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include ="Id,Name")]IdentityRole role)
        {
            try
            {
                // TODO: Add update logic here
                if(ModelState.IsValid)
                {
                    db.Entry(role).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(role);
            }
            catch
            {
                return View(role);
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
                return HttpNotFound();
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(IdentityRole role)
        {
            try
            {
                // TODO: Add delete logic here
                IdentityRole myrole = db.Roles.Find(role.Id);
                db.Roles.Remove(myrole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(role);
            }
        }
    }
}
