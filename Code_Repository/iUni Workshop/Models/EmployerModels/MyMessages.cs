using System;
using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployerModels
{
    public class MyMessages
    {
        public string ConversationId { get; set; }
        public string Title { get; set; }
        public DateTime SentTime { get; set; }
        [Display(Name = "Conversation With")]
        public string SenderEmail { get; set; }
        public bool Read { get; set; }
    }
}