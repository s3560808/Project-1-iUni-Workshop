using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.SchoolModels;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class Administraotr
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }
            
        [Required]
        public string Name { get; set; }

        public virtual Employer Employer { get; set; }
        public virtual ICollection<School> Schools { get; set; }
        public virtual Field Field { get; set; }
        public virtual Skill Skill { get; set; }
    }
}