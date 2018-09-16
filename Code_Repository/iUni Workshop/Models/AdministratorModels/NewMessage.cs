using System;
using System.ComponentModel.DataAnnotations;
using iUni_Workshop.Models.MessageModels;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class NewMessage
    {
        [Required(ErrorMessage = "User email is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message detail is required.")] 
        public string MessageDetail { get; set; }

        [Required(ErrorMessage = "Title is required.")] 
        public string Title { get; set; }

        [Required(ErrorMessage = "Message Type is required.")]
        [Range(MessageType.System, MessageType.UserMessage, ErrorMessage = "Please enter correct message type.")]
        public int Type { get; set; }
    }
}