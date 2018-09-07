using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.JobRelatedModels;

namespace iUni_Workshop.Models.EmployerModels
{
    public class EmployerSkill
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }

        public int EmployerJobProfileId { get; set; }
        [ForeignKey("EmployerJobProfileId")]
        public virtual EmployerJobProfile EmployerJobProfile { get; set; }
        
        public int SkillId { get; set; }
        [ForeignKey("SkillId")]
        public virtual Skill Skill { get; set; }

        public bool Required { get; set; }
    }
}