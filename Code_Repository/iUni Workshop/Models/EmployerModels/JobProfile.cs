using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.InvatationModel;
using iUni_Workshop.Models.JobRelatedModels;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;

namespace iUni_Workshop.Models.EmployerModels
{
    public class JobProfile
    {


        public int JobProfileId { get; set; }
        [Required] 
        public string EmployerId { get; set; }
        [ForeignKey("EmployerId")] 
        public virtual Employer Employer { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EstablishDateTime { get; set; }
        [Required]
        public String JobTitleP { get; set; }

        [Required] public String JobDescription { get; set; }
        public Boolean RequireJobExperience { get; set; }
        public int MaxDayForAWeek { get; set; }
        public int MinDayForAWeek { get; set; }
        [Required] 
        public Double Salary { get; set; }
        [Required] 
        public String SalaryUnit { get; set; }

        public virtual ICollection<SkillForJob_Employer> SkillForJobEmployers { get; set; }
        public virtual ICollection<ComplusoryWorkingDay> ComplusoryWorkingDay { get; set; }
    }
}