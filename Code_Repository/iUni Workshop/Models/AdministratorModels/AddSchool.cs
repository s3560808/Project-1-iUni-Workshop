using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class AddSchool
    {
        [Required]
        [Display(Name = "Domain Extension of school")]
        public string DomainExtension { get; set; }
        
        [Display(Name = "School Name")]
        [Required]
        public string SchoolName { get; set; }

        [Required] 
        public string SurburbName { get; set; }
        
        [Required] 
        public int PostCode { get; set; }

        public int Status { get; set; }

        public int Id { get; set; }
    }
}