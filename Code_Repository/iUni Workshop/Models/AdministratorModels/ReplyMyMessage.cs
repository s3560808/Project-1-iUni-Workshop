using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class ReplyMyMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public string ConversationId { get; set; }

        [Required]
        public string MessageDetail { get; set; }
    }
}
