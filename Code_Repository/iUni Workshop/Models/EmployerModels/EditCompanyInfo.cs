using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployerModels
{
    public class EditCompanyInfo
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string ContactEmail { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string BriefDescription { get; set; }
        [Required]
        public int PostCode { get; set; }
        [Required]
        public string SuburbName { get; set; }
        [Required]
        public string ABN { get; set; }
        public bool Certificated { get; set; }
    }
}