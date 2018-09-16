using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EditPersonalInfo
    {
        [Required(ErrorMessage = "Your correct name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field name is required!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter correct phone number")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Field name is required!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter your correct message")]
        public string ContactEmail { get; set; }
        
        public string SchoolName { get; set; }
        
        public string CampusName { get; set; }
        
        public int CampusPostCode { get; set; }
        
        [Required(ErrorMessage = "Living district is required!")]
        public string LivingDistrict { get; set; }
        
        [Required(ErrorMessage = "Postcode is required!")]
        public int PostCode { get; set; }

        [Required(ErrorMessage = "Short description is required!")]
        public string ShortDescription { get; set; }
    }
}