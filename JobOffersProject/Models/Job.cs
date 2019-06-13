using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace JobOffersProject.Models
{
    public class Job
    {
        //add new job class with its attributes and make association between jobs and categories 
        //add some attributes to link in job and categories
        //build and makecontroller and views and entityframework and update database

        public int Id { get; set; }

        [Required]
        [Display(Name ="Job Name")]
        public string JobName { get; set; }

        [Required]
        [Display(Name ="Job Description")]
        [AllowHtml]//to make this allow html -> CKEditor
        public string JobDescription { get; set; }

        [Display(Name ="Job Image")]
        public string JobImage { get; set; }
        
        //important information -> to make this field is foreign key for any navigation prop u should name it the same name of nav prop + Id
        public int CategoriesId { get; set; } //need it as ForeignKey

        public string UserId { get; set; }//to know who publish the job

        //this prop is Navigation prop and use to bind two tables
        //we will put virtual to activate lazy loading to get data for this object -> search on it in entity framework
        public virtual Categories categories { get; set; }

        public virtual ApplicationUser User { get; set; }//the publisher   -> and add the rest of relation in applicationuser
        //and update the create method to add publisher 

        public virtual ICollection<ApplyForJob> ApplyForJobs { get; set; }

    }
}