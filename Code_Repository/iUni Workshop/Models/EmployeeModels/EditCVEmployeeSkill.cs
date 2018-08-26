using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EditCVEmployeeSkill
    {
        [Required]
        public string SkillName { get; set; }
        
        [Required]
        public string CertificationLink { get; set; }

        public int SkillId { get; set; }
    }
}