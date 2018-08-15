using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.AdministratorModels;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace iUni_Workshop.Models.EmployerModels
{
    public class Employer
    {
        
        [Key]
        public string EmployerId { get; set; }
        [ForeignKey(("EmployerId"))]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required] public String CompanyName { get; set; }

        [Required] public String CompanyLocation { get; set; }

        [Required] public String PhoneNumber { get; set; }

        [Required]
        public String ContactEmail { get; set; }

        [Required] public String Photo { get; set; }

        [Required] public String BriefDescription { get; set; }

        [Required] public String ABN { get; set; }

        [Required]
        public bool Certificated { get; set; }
        
        public string CertificaedBy { get; set; }
        [ForeignKey(("CertificaedBy"))]
        public virtual Administraotr Administraotr { get; set; }
    }
}