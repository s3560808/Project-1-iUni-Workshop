using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iUni_Workshop.Models.EmployerModels
{
    public class SendMessage
    {

        [Required]
        public string MessageDetail { get; set; }

        [Required] 
        public int InvitationId { get; set; }
    }
}