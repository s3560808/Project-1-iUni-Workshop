using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EditCVEmployeeSkill
    {
        [Required(ErrorMessage = "Skill name is required!")]
        public string SkillName { get; set; }
        
        [Required(ErrorMessage = "Skill certification link is required!")]
        public string CertificationLink { get; set; }

        public int SkillId { get; set; }
    }
}