using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Models.JobRelatedModels;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class UpdateSkill
    {
        [Required(ErrorMessage = "Correct Skill Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter correct skill id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Skill name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Skill status is required")]
        [Range(SkillStatus.InUse, SkillStatus.NoLongerUsed, ErrorMessage = "Please enter correct status for skill")]
        public int Status { get; set; }
    }
}
