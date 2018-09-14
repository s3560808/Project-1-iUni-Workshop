using System.ComponentModel.DataAnnotations;
using iUni_Workshop.Models.SchoolModels;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class UpdateSchool
    {
        [Required(ErrorMessage = "Correct domain extension is required")]
        public string DomainExtension { get; set; }
        
        [Required(ErrorMessage = "Correct school name is required")]
        public string SchoolName { get; set; }

        [Required(ErrorMessage = "Correct suburb name is required")]
        public string SuburbName { get; set; }
        
        [Required(ErrorMessage = "Correct post code is required")]
        public int PostCode { get; set; }

        [Required(ErrorMessage = "Correct school id name is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Correct status is required")]
        [Range(SchoolStatus.InUse, SchoolStatus.NoLongerUsed, ErrorMessage = "Please enter correct status for school")]
        public int Status { get; set; }
    }
}