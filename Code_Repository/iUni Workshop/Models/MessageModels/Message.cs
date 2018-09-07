using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.InvatationModel;

namespace iUni_Workshop.Models.MessageModels
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Key]
        public string Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ConversationId { get; set; }

        [Required] 
        public string SenderId { get; set; }
        [ForeignKey(("SenderId"))] 
        public virtual ApplicationUser Sender { get; set; }

        [Required] 
        public string ReciverId { get; set; }
        [ForeignKey(("ReciverId"))] 
        public virtual ApplicationUser Receiver { get; set; }

        public int? InvatationId { get; set; }
        [ForeignKey(("InvatationId"))] 
        public virtual Invatation Invatation { get; set; }

        [DataType(DataType.Time)] 
        public DateTime SentTime { get; set; }

        [Required] 
        public bool Read { get; set; }

        [Required]
        public string Title { get; set; }
        
        [Required]
        [DataType(DataType.Text)]
        public string MessageDetail { get; set; }

        [Required] 
        public int Type { get; set; }
    }
}