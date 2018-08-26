using System;
using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class MyMessages
    {
        [Required]
        public string ConversationId { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required]
        public DateTime SentTime { get; set; }

        [Required]
        public bool Read { get; set; }

        [Required]
        public string Title { get; set; }
    }
}