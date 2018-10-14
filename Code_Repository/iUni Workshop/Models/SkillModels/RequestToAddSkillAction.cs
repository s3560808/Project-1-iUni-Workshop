using System.ComponentModel.DataAnnotations;


namespace iUni_Workshop.Models.SkillModels
{
    public class RequestToAddSkillAction
    {
        [Display(Name = "Skill Name")]
        [Required(ErrorMessage = "Skill name is required!")]
        public string SkillName { get; set; }
    }
}