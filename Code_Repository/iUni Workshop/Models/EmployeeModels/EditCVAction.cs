using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EditCVAction
    {
        public bool UpdateStatus { get; set; }
        public int CvId { get; set; }
        public bool PrimaryCv { get; set; }
        [Required(ErrorMessage = "Field name is required!")]
        public string FieldName { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
        public float MinSalary { get; set; }
    }
}