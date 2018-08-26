using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployerModels;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EmployeeWorkDay
    {
        [Key]
        public int EmployeeCvId { get; set; }
        [ForeignKey("EmployeeCvId")] 
        public virtual EmployeeCV EmployeeCv { get; set; }
        
        [Key]
        public int Day { get; set; }
    }
}