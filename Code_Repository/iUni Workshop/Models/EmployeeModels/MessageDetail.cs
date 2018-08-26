using System;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class MessageDetail
    {
        public string SenderName { get; set; }
        public DateTime SentTime { get; set; }
        public string Detail { get; set; }
        public string ConversationId { get; set; }
    }
}