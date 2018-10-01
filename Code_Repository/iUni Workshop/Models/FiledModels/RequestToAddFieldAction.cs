using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.FiledModels
{
    public class RequestToAddFieldAction
    {
        [Display(Name = "Field Name")]
        [Required]
        public string FieldName { get; set; }
    }
}