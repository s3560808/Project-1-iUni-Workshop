using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployerModels
{
    public class EditCompanyInfo
    {
        [Required(ErrorMessage = "Your correct name is required!")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Your correct phone number is required!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter correct phone number")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Your correct contact email is required!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter your correct message")]
        public string ContactEmail { get; set; }
        
        [Required(ErrorMessage = "Your correct address is required!")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Your correct brief description is required!")]
        public string BriefDescription { get; set; }
        
        [Required(ErrorMessage = "Your correct post code is required!")] 
        public int PostCode { get; set; }
        
        [Required(ErrorMessage = "Your correct suburb name is required!")]
        public string SuburbName { get; set; }
        
        [Required(ErrorMessage = "Your correct ABN is required!")]
        public string ABN { get; set; }
        
        public bool Certificated { get; set; }
    }
}