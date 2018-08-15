using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;

namespace iUni_Workshop.Models.MessageModels
{
    public class Message
    {
        [Key]
        public string MessageId { get; set; }

        [Key]
        public string SenderId { get; set; }
        [ForeignKey(("SenderId"))]
        public virtual ApplicationUser Sender { get; set; }
        
        [Key]
        public string ReciverId { get; set; }
        [ForeignKey(("ReciverId"))]
        public virtual ApplicationUser Receiver { get; set; }
        
        [Key]
        public int JobProfileId { get; set; }
        [ForeignKey(("JobProfileId"))]
        public virtual JobProfile JobProfile { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SentTime { get; set; }

        [Required]
        public bool read { get; set; }
    }
}