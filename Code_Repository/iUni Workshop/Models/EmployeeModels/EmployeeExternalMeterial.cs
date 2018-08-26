using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.JobRelatedModels;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EmployeeExternalMeterial
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Link { get; set; }
        
        [Required]
        public int EmployeeCvId { get; set; }
        [ForeignKey("EmployeeCvId")]
        public virtual EmployeeCV EmployeeCV { get; set; }
    }
}