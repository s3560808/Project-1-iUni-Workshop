using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class AddSkill
    {
        [Required(ErrorMessage = "Field name is required")]
        public string Name { get; set; }
    }
}