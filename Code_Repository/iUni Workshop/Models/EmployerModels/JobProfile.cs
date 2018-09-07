using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using iUni_Workshop.Models.EmployerModels;

namespace iUniWorkshop.Models.EmployerModels
{
    public class JobProfile
    {
        public int ProfileId { get; set; }
        public DateTime UpdateDate { get; set; }

        [Required]
        public string FieldName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public bool RequireJobExperience { get; set; }

        public int? MaxDay { get; set; }

        public int? MinDay { get; set; }
        [Required]
        public float Salary { get; set; }

        public List<JobProfileSkill> JobProfileSkills { get; set; }

        public List<JobProfileRequiredLocation> JobProfileRequiredLocation { get; set; }

        public List<JobProfileRequiredSchool> EmployerRequiredSchools { get; set; }

        public List<JobProfileComplusoryWorkDay> JobProfileComplusoryWorkDays { get; set; }

    }
}
