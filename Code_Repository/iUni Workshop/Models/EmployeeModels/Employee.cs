using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class Employee
    {

//        public Employee(String employeeId, String name, 
//            String phoneNumber, String contactEmail, 
//            String photo, String schooName, 
//            String livingSuburb, ICollection<CV>cVs)
//        {
//            EmployeeId = employeeId;
//            Name = name;
//            PhoneNumber = phoneNumber;
//            ContactEmail = contactEmail;
//            
//            
//            //Follwing will be showed to employer when searching
//            Photo = photo;
//            SchooName = schooName;
//            LivingSuburb = livingSuburb;
//            CVs = cVs;
//        }
//
        public Employee()
        {
        }

            public Employee(string id)
            {
                    EmployeeId = id;
            }
        
        [Key]
        public string EmployeeId { get; set; }
        [ForeignKey(("EmployeeId"))]
        public virtual ApplicationUser ApplicationUser { get; set; }
            
        public String Name { get; set; }
        public String PhoneNumber { get; set; }
        public String ContactEmail { get; set; }
        public String SchooName { get; set; }
        public String LivingSuburb { get; set; }
        public String Photo { get; set; }

        public virtual ICollection<CV> CVs { get; set; }
    }
}