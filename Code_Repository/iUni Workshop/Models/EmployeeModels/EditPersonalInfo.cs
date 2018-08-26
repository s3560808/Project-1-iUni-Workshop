using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EditPersonalInfo
    {
        [Required] 
        public string Name { get; set; }

        [Required] 
        public string PhoneNumber { get; set; }
        
        [Required] 
        public string ContactEmail { get; set; }
        
        public string SchoolName { get; set; }
        
        public string Campus { get; set; }
        
        public string LivingDistrict { get; set; }
        
        public int PostCode { get; set; }

        public string ShortDescription { get; set; }
    }
}