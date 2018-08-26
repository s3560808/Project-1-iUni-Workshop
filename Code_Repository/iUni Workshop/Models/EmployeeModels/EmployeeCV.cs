using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.InvatationModel;
using iUni_Workshop.Models.JobRelatedModels;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EmployeeCV
    {   
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int Id { get; set; }
        
        [Required]
        public String EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        
        [Required]
        public bool Primary { get; set; }
        
        [Required]
        public bool FindJobStatus { get; set; }
        
        [Required]
        public DateTime StartFindJobDate { get; set; }
        
        [Required]
        public String Title { get; set; }
        
        [Required]
        public String Details { get; set; }
        
        public float MinSaraly { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual Field Field { get; set; }
        
        
        
//      Certification        
        public virtual ICollection<EmployeeExternalMeterial> EmployeeExternalMeterials { get; set; }
        
        public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; }

        public virtual ICollection<EmployeeJobHistory> EmployeeJobHistories { get; set; }
        
        public virtual ICollection<Invatation> Invatations { get; set; }

        public virtual ICollection<EmployeeWorkDay> EmployeeWorkDays { get; set; }
    }
}