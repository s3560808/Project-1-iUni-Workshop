using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.SuburbModels;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace iUni_Workshop.Models.EmployerModels
{
    public class Employer
    {
        
        [Key]
        public string Id { get; set; }
        [ForeignKey(("Id"))]
        public virtual ApplicationUser ApplicationUser { get; set; }

        
        public string Name { get; set; }

        
        public string Location { get; set; }
        
        
        public int? SuburbId { get; set; }
        [ForeignKey(("SuburbId"))]
        public virtual Suburb Suburb { get; set; }

        
        public string PhoneNumber { get; set; }

        
        public string ContactEmail { get; set; }

//        public String Photo { get; set; }

        
        public string BriefDescription { get; set; }

       
        public string ABN { get; set; }

        
        public bool Certificated { get; set; }

        public bool RequestCertification { get; set; }

        public string CertificatedBy { get; set; }
        [ForeignKey(("CertificatedBy"))]
        public virtual Administraotr Administraotr { get; set; }
    }
}