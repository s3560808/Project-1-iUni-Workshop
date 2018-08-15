using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iUni_Workshop.Models.EmployerModels
{
    public class ComplusoryWorkingDay
    {
        [Key]
        public int JobProfileId { get; set; }
        [ForeignKey("JobProfileId")] 
        public virtual JobProfile JobProfile { get; set; }
        [Key]
        public int day { get; set; }
    }
}