using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.JobRelatedModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EmployeeSkill
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }

        public int SkillId { get; set; }
        [ForeignKey("SkillId")]
        public virtual Skill Skill { get; set; }
        
        public int EmployeeCvId { get; set; }
        [ForeignKey("EmployeeCvId")]
        public virtual EmployeeCV EmployeeCV { get; set; }

        [Required]
        public string CertificationLink { get; set; }
    }
}