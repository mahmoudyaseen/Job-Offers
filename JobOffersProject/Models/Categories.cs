using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobOffersProject.Models
{
    public class Categories
    {
        //first i create this class with its prop and build the project
        //second i will create controller with views and entityframework
        //then i should make migrate and update database

        public int Id { get; set; }

        [Required]
        [Display(Name ="Category Name")]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name ="Category Description")]
        public string Description { get; set; }

        //we will put virtual to activate lazy loading to get data for this object -> search on it in entity framework
        public virtual ICollection<Job> Jobs { get; set; }
    }
}