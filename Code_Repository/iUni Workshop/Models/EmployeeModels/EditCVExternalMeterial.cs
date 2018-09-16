using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EditCVExternalMeterial
    {
        [Required(ErrorMessage = "External material name is required!")]
        public string ExternalMaterialName { get; set; }
        
        [Required(ErrorMessage = "External material certification link is required!")]
        public string ExternalMaterialLink { get; set; }
    }
}