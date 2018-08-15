using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.InvatationModel;
using iUni_Workshop.Models.JobRelatedModels;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class CV
    {   
        [Key]
        public int CVId { get; set; }
        [Required]
        public String CVTitle { get; set; }
        [Required]
        public String CVDetails { get; set; }
        [Required]
        public bool FindJobStatus { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime FJSSetDate { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual Field Field { get; set; }
        
        [Required]
        public String EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        //Minimum salary
        
//      Certification        
        public virtual ICollection<ExternalMeterial> ExternalMeterials { get; set; }
        
        public virtual ICollection<SkillForJob_Employee> SkillForJobEmployees { get; set; }

        public virtual ICollection<JobHistory> JobHisotries { get; set; }
    }
}