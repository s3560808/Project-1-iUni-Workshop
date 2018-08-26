using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EditCVJobHistory
    {
        [Required]
        public string JobHistoryName { get; set; }
        
        [Required]
        public string JobHistoryShortDescription { get; set; }
    }
}