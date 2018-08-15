using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.JobRelatedModels;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class ExternalMeterial
    {
        public int ExternalMeterialId { get; set; }
        [Required]
        public string ExternalMeterialName { get; set; }
        [Required]
        public string ExternalMeterialLink { get; set; }
        [Required]
        public int CVId { get; set; }
        [ForeignKey("CVId")]
        public virtual CV CV { get; set; }
    }
}