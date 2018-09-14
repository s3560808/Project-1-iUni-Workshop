using System;

namespace iUni_Workshop.Models.EmployerModels
{
    public class MyMessages
    {
        public string ConversationId { get; set; }
        public string Title { get; set; }
        public DateTime SentTime { get; set; }
        public string SenderEmail { get; set; }
        public bool Read { get; set; }
    }
}