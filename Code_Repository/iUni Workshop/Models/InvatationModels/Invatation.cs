using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;

namespace iUni_Workshop.Models.InvatationModel
{
    public class Invatation
    {
        [Key]
        public string EmployerId { get; set; }
        [ForeignKey(("EmployerId"))]
        public virtual Employer Employer { get; set; }
        
        [Key]
        public string EmployeeId { get; set; }
        [ForeignKey(("EmployeeId"))]
        public virtual Employee Employee { get; set; }

        [Key] 
        public string JobProfileId { get; set; }
        [ForeignKey(("JobFrofileId"))]
        public virtual JobProfile JobProfile { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime SentDate { get; set; }
    }
}