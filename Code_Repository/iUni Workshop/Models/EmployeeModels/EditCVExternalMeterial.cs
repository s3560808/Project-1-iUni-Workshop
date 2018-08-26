using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EditCVExternalMeterial
    {
        [Required]
        public string ExternalMeterialName { get; set; }
        
        [Required]
        public string ExternalMeterialLink { get; set; }
    }
}