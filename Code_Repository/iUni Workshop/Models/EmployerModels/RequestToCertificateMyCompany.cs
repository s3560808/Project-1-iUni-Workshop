using System;
using System.ComponentModel.DataAnnotations;

namespace iUniWorkshop.Models.EmployerModels
{
    public class RequestToCertificateMyCompany
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
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
        public bool RequestCertification { get; set; }
    }
}
