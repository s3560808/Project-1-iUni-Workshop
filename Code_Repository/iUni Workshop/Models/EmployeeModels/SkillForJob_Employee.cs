using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class SkillForJob_Employee
    {
        [Key]
        public int SkillId { get; set; }
        [ForeignKey("SkillId")]
        public virtual Skill.Skill Skill { get; set; }
        
        [Key]
        public int CVId { get; set; }
        [ForeignKey("CVId")]
        public virtual CV CV { get; set; }

        [Required]
        public string CertificationLink { get; set; }
    }
}