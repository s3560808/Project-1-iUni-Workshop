using System;
using System.Collections.Generic;

namespace iUni_Workshop.Models.EmployerModels
{
    public class MessageDetail
    {
        public string ConversationId { get; set; }
        public int Type { get; set; }
        public int InvitationId { get; set; }
        public IEnumerable<MessageDetailMessageInfo> Messages { get; set; }
    }
}