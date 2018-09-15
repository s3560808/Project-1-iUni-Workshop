using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class AddSchool
    {
        [Display(Name = "Domain Extension of school")]
        [Required(ErrorMessage = "Domain extension is required")]
        public string DomainExtension { get; set; }
        
        [Display(Name = "School Name")]
        [Required(ErrorMessage = "School Name is required")]
        public string SchoolName { get; set; }

        [Required(ErrorMessage = "Suburb Name is required")]
        public string SurburbName { get; set; }
        
        [Required(ErrorMessage = "Post code is required")]
        public int PostCode { get; set; }

        public int Status { get; set; }

        public int Id { get; set; }
    }
}