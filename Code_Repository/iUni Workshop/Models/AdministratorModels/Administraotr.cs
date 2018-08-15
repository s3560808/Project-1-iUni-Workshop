using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployeeModels;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class Administraotr
    {
        [Key]
        public string AdministratorId { get; set; }
        [ForeignKey(("AdministratorId"))]
        public virtual ApplicationUser ApplicationUser { get; set; }
            
        public String Name { get; set; }
    }
}