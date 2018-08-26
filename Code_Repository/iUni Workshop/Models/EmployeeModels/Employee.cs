using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.SchoolModels;
using iUni_Workshop.Models.SuburbModels;
using Microsoft.AspNetCore.Identity;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class Employee
    {

        public Employee()
        {
        }

        public Employee(string id)
        {
                    Id = id;
        }
        
        [Key]
        public string Id { get; set; }
        [ForeignKey(("Id"))]
        public virtual ApplicationUser ApplicationUser { get; set; }
        
        public int? SchoolId { get; set; }
        [ForeignKey(("SchoolId"))] 
        public virtual School School { get; set; }
        
        public int? SuburbId { get; set; }
        [ForeignKey(("SuburbId"))]
        public virtual Suburb Suburb { get; set; }
        
        public string Name { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string ContactEmail { get; set; }

        public string ShortDescription { get; set; }

//        public String Photo { get; set; }

        public virtual ICollection<EmployeeCV> EmployeeCvs { get; set; }
    }
}