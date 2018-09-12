using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class MessageDetail
    {
        public string SenderName { get; set; }
        public DateTime SentTime { get; set; }
        public string Detail { get; set; }
        public string ConversationId { get; set; }
        public int Type { get; set; }
    }
}
