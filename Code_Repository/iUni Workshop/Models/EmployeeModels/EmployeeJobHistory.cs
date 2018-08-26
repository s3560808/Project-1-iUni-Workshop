using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EmployeeJobHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string ShortDescription { get; set; }
        
        [Required]
        public int EmployeeCvId { get; set; }
        [ForeignKey("EmployeeCvId")]
        public virtual EmployeeCV EmployeeCV { get; set; }
    }
}