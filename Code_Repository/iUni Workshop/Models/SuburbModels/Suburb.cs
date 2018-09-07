using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using iUni_Workshop.Models.SchoolModels;
using iUni_Workshop.Models.StateModels;
using iUniWorkshop.Models.EmployerModels;

namespace iUni_Workshop.Models.SuburbModels
{
    public class Suburb
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Display(Name = "State")]
        [Required]
        public int StateId { get; set; }
        [ForeignKey(("StateId"))]
        public virtual State State { get; set; }

        public int PostCode { get; set; }

        public string AdditionalInfo { get; set; }

        public virtual ICollection<School> Schools { get; set; }
        public virtual ICollection<EmployerJobProfile> EmployerJobProfile { get; set; }
        public virtual ICollection<EmployerRequiredWorkLocation> EmployerRequiredWorkLocations { get; set; }
        public virtual ICollection<Employer> Employers { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

    }
}