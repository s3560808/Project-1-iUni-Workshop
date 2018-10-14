using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class CertificateCompaniesAction
    {
        [Required]
        public bool Accept { get; set; }

        [Required] 
        public string Id { get; set; }
    }
}