using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using iUni_Workshop.Models.StateModels;
using iUni_Workshop.Models.SuburbModels;

namespace iUni_Workshop.Models.SchoolModels
{
    public class School
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Domain Extension of school")]
        [Required]
        public string DomainExtension { get; set; }
        
        [Display(Name = "School Name")]
        [Required]
        public string SchoolName { get; set; }
        
        [Required]
        public string NormalizedName { get; set; }

        [Required]
        public int SuburbId { get; set; }
        [ForeignKey(("SuburbId"))]
        public virtual Suburb Suburb { get; set; }

        public string AddedBy { get; set; }
        [ForeignKey(("AddedBy"))]
        public virtual Administraotr Administrator { get; set; }

        public bool NewRequest { get; set; }

        public string RequestedBy { get; set; }
        [ForeignKey(("RequestedBy"))]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int Status { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<EmployerRequiredSchool> EmployerRequiredSchools { get; set; }
    }
}