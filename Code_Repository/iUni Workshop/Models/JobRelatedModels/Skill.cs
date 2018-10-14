using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Xml;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace iUni_Workshop.Models.JobRelatedModels
{
    public class Skill
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string NormalizedName { get; set; }

        public string AddedBy { get; set; }
        [ForeignKey(("AddedBy"))]
        public virtual Administraotr Administrator { get; set; }
        
        public string RequestedByUserId { get; set; }
        [ForeignKey(("RequestedByUserId"))]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int Status { get; set; }
        
        public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; }
        public virtual ICollection<EmployerSkill> EmployerSkills { get; set; }
        public virtual ICollection<SkillHistory> SkillHistories { get; set; }
    }
}