using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class AddSchoolAction
    {
        [Required]
        public string SchoolName { get; set; }
        
        [Required]
        public string DomainExtension { get; set; }
        
        [Required]
        public string SurburbName { get; set; }
        
        [Required]
        public int PostCode { get; set; }
    }
}