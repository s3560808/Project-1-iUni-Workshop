using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using iUni_Workshop.Models.MessageModels;

namespace iUni_Workshop.Models.InvatationModel
{
    public class Invatation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        public int EmployeeCvId { get; set; }
        [ForeignKey(("EmployeeCvId"))]
        public virtual EmployeeCV EmployeeCV { get; set; }

        public int EmployerJobProfileId { get; set; }
        [ForeignKey(("EmployerJobProfileId"))]
        public virtual EmployerJobProfile EmployerJobProfile { get; set; }
        
        [Required]
        public int status { get; set; }

        [Required]
        public DateTime SentTime { get; set; }

        private ICollection<Message> Messages { get; set; }
    }
}