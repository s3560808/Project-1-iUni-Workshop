using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployeeModels;

namespace iUni_Workshop.Models.EmployerModels
{
    public class SkillForJob_Employer
    {
        [Key]
        public int JobProfileId { get; set; }
        [ForeignKey("JobProfileId")]
        public virtual JobProfile JobProfile { get; set; }
        
        [Key]
        public int SkillId { get; set; }
        [ForeignKey("SkillId")]
        public virtual Skill.Skill Skill { get; set; }

        public bool required { get; set; }
    }
}