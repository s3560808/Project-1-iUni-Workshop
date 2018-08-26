using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iUni_Workshop.Models.EmployerModels
{
    public class EmployerComplusoryWorkDay
    {
        [Key]
        public int EmployerJobProfileId { get; set; }
        [ForeignKey("EmployerJobProfileId")] 
        public virtual EmployerJobProfile EmployerJobProfile { get; set; }
        
        [Key]
        public int Day { get; set; }
    }
}