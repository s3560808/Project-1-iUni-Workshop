using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.SchoolModels;

namespace iUni_Workshop.Models.EmployerModels
{
    public class EmployerRequiredSchool
    {
        [Key]
        public int EmployerJobProfileId { get; set; }
        [ForeignKey(("EmployerJobProfileId"))]
        public virtual EmployerJobProfile EmployerJobProfile { get; set; }
        
        [Key]
        public int SchoolId { get; set; }
        [ForeignKey(("SchoolId"))] 
        public virtual School School { get; set; }
    }
}