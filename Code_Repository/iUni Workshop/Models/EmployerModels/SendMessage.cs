using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iUni_Workshop.Models.EmployerModels
{
    public class SendMessage
    {

        [Required(ErrorMessage = "Message is required")]
        public string MessageDetail { get; set; }
        
        [Range(0,int.MaxValue)]
        public int InvitationId { get; set; }

        public string ConversationId { get; set; }
    }
}