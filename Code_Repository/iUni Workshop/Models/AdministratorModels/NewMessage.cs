using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class NewMessage
    {
        [Required] 
        public string Email { get; set; }

        [Required] 
        public string MessageDetail { get; set; }

        [Required]
        public string Title { get; set; }

        [Required] 
        public int Status { get; set; }

        [Required] 
        public int Type { get; set; }
    }
}