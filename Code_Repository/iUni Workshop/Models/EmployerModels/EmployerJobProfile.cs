using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.SuburbModels;
using iUniWorkshop.Models.EmployerModels;

namespace iUni_Workshop.Models.EmployerModels
{
    public class EmployerJobProfile
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int Id { get; set; }
        
        [Required] 
        public string EmployerId { get; set; }
        [ForeignKey("EmployerId")] 
        public virtual Employer Employer { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDateTime { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime LastUpdateDateTime { get; set; }
        
        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual Field Field { get; set; }
        
        [Required]
        public String Title { get; set; }

        [Required] 
        public String Description { get; set; }
        
        [Required] 
        public Boolean RequireJobExperience { get; set; }
        
        public int? MaxDayForAWeek { get; set; }
        
        public int? MinDayForAWeek { get; set; }
        
        [Required] 
        public float Salary { get; set; }


        public virtual ICollection<EmployerSkill> EmployerSkills { get; set; }
        public virtual ICollection<EmployerComplusoryWorkDay> EmployerComplusoryWorkDays { get; set; }
        public virtual ICollection<EmployerRequiredSchool> EmployerRequiredSchools { get; set; }
        public virtual ICollection<EmployerRequiredWorkLocation> EmployerRequiredWorkLocations { get; set; }
    }
}