using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;

namespace iUni_Workshop.Models.JobRelatedModels
{
    public class Field
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string NormalizedName { get; set; }

        public string AddedBy { get; set; }
        [ForeignKey(("AddedBy"))]
        public virtual Administraotr Administraotr { get; set; }
        
        public bool NewRequest { get; set; }
        
        public string RequestedBy { get; set; }
        [ForeignKey(("RequestedBy"))]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int Status { get; set; }

        public virtual ICollection<EmployeeCV> EmployeeCvs { get; set; }
        public virtual ICollection<EmployerJobProfile> EmployerJobProfiles { get; set; }
    }
}