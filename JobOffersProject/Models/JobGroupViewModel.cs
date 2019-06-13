using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobOffersProject.Models
{
    public class JobGroupViewModel
    {
        //i dont need it in database so if it is in ApplicationDbContext Class i will remove it
        public string JobTitle { get; set; }
        public IEnumerable<ApplyForJob> ApplyForJobs { get; set; }
    }
}