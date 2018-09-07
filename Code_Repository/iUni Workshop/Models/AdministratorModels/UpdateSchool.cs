using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class UpdateSchool
    {
        [Required]
        public string DomainExtension { get; set; }
        
        [Required]
        public string SchoolName { get; set; }

        [Required] 
        public string SurburbName { get; set; }
        
        [Required] 
        public int PostCode { get; set; }

        [Required]
        public int Id { get; set; }

        [Required] public int Status { get; set; }
    }
}