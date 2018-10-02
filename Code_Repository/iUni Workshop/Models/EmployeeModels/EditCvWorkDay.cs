using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EditCvWorkDay
    {
        [Range(1,7, ErrorMessage = "Please enter correct work day")]
        public int Day { get; set; }
    }
}