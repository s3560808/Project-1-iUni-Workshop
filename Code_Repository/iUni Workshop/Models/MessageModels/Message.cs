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

        [Required]
        public string receiverId { get; set; }
        [ForeignKey(("receiverId"))] 
        public virtual ApplicationUser User { get; set; }

        [ForeignKey(("ConversationId"))]
        [Required]
        public string ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }

        [DataType(DataType.Time)] 
        public DateTime SentTime { get; set; }

        [Required] 
        public bool Read { get; set; }

        
        
        [Required]
        [DataType(DataType.Text)]
        public string MessageDetail { get; set; }
    }
}