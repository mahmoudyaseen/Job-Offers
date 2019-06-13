using JobOffersProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult Details(int JobId)
        {
            var job = db.Jobs.Find(JobId);
            if (job == null)
                return HttpNotFound();
            Session["JobId"] = JobId;
            return View(job);
        }

        //We create ApplyForJob Model we can get all his attributes eccept the Message so we will send it from view to apply post message
        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }

        //parameter should have same name of textarea in view
        [HttpPost]
        public ActionResult Apply(string Message)
        {
            //ApplyForJob need id -> will generate auto,jobid -> from session,userid -> from identity,datetime -> now ,message -> from view
            var jobId = (int)Session["JobId"];//what happen if he enter to this action throw link without enter Details ->no session
            //i think -> better we can send jobid as query to apply fun(get) and view send message\id to this fun 
            var userId = User.Identity.GetUserId();
            //now we will migration to add ApplyForJob Table
            //u will have error say u have two repeted tables -> go to ApplicationDbContext and remove ApplicationUsers becouse there is one already in asp
            //now u can deal with ApplyForJob table

            //makesure the applier dont already applied for whis job
            var check = db.ApplyForJobs.Where(a => a.JobId == jobId && a.UserId == userId).ToList();
            //user can apply
            if (check.Count < 1)
            {
                ApplyForJob job = new ApplyForJob();
                job.JobId = jobId;
                job.UserId = userId;
                job.Message = Message;
                job.ApplyDate = DateTime.Now;

                db.ApplyForJobs.Add(job);
                db.SaveChanges();
                //ViewBag is keep data for Http Request only (send from action to view)
                ViewBag.Result = "Apply Done Successfully";
            }
            else
            {
                ViewBag.Result = "You Already Applied For this Job";
            }

            return View();
        }

        [Authorize]
        public ActionResult GetJobByPublisher()
        {
            var userid = User.Identity.GetUserId();
            var applyForJobs = from app in db.ApplyForJobs
                               join job in db.Jobs
                               on app.JobId equals job.Id
                               where job.UserId == userid
                               select app;

            //i need to group data to be more readable so i need data like -> 
            //job title + ApplyForThisJob.list
            //so i need class on this view and i will not put him in database i will use it only here 
            //to enhance to view of data


            var Groups = from applyJob in applyForJobs
                         group applyJob by applyJob.job.JobName
                         into Group
                         select new JobGroupViewModel
                         {
                             JobTitle = Group.Key, // key is the selected item sorted by
                             ApplyForJobs = Group
                         };


            return View(Groups.ToList());
        }


        [Authorize]
        public ActionResult GetJobByUser()
        {
            var userid = User.Identity.GetUserId();
            var jobs = db.ApplyForJobs.Where(a => a.UserId == userid);
            return View(jobs.ToList());
            //i have error say 
            //There was an error running the selected code generator: 'Unable to retrieve metadata for 'JobOffers.Models.ApplyForJob
            //so i add public virtual ICollection<ApplyForJob> ApplyForJobs { get; set; } in job and ApplicationUser class
        }

        [Authorize]
        public ActionResult DetailsOFJob(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
                return HttpNotFound();
            return View(job);
        }

        public ActionResult Edit(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
                return HttpNotFound();
            return View(job);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(ApplyForJob job)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    //but new datetime
                    job.ApplyDate = DateTime.Now;
                    db.Entry(job).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("GetJobByUser");
                }
                return View(job);
            }
            catch
            {
                return View(job);
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
                return HttpNotFound();
            return View(job);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(ApplyForJob job)
        {
            try
            {
                // TODO: Add delete logic here
                ApplyForJob myjob = db.ApplyForJobs.Find(job.Id);
                db.ApplyForJobs.Remove(myjob);
                db.SaveChanges();
                return RedirectToAction("GetJobByUser");
            }
            catch
            {
                return View(job);
            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    

        [HttpGet]
        [Authorize]
        public ActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("mahmoudyaseen6977@gmail.com", "12345Mody12345");//someone/company responsaple to send mails -> manager/company mail
            mail.From = new MailAddress(model.Email);
            mail.To.Add(new MailAddress("mahmoudyaseen6977@gmail.com"));
            mail.Subject = model.Subject;
            mail.IsBodyHtml = true; //if u want to send html
            string body = "User Name : " + model.UserName + "<br>" + model.Message;
            mail.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);//host and port for gmail
            smtpClient.EnableSsl = true; //safe mode to move data from browser to webserver
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail);
            return RedirectToAction("Index");
        }


        //work in get -> when user write in uri
        //public ActionResult Search()
        //{
        //    return View();
        //}

        [HttpPost]
        //work when user submit form of search -> click enter in search textbox
        public ActionResult Search(string searchtxt)
        {
            //contains means the text in the text -> Ex: star - oban star race
            var jobs = db.Jobs.Where(a => a.JobName.Contains(searchtxt)
                                       || a.JobDescription.Contains(searchtxt)
                                       || a.categories.CategoryName.Contains(searchtxt)
                                       || a.categories.Description.Contains(searchtxt)).ToList();
            return View(jobs);
        }
    }
}