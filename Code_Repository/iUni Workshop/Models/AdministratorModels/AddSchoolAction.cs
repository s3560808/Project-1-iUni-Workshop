using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class AddSchoolAction
    {
        [Required(ErrorMessage = "Correct school name is required")]
        public string SchoolName { get; set; }
        
        [Display(Name = "Domain Extension of school")]
        [Required(ErrorMessage = "Correct domain extension is required")]
        public string DomainExtension { get; set; }
        
        [Required(ErrorMessage = "Correct suburb name is required")]
        public string SuburbName { get; set; }
        
        [Required(ErrorMessage = "Correct postcode is required")]
        public int PostCode { get; set; }

    }
}