using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class AcceptOrReject
    {
        [Required(ErrorMessage = "Accept status is required")] 
        public bool Accept { get; set; }
        
        [Required(ErrorMessage = "Please select correct invitation")] 
        public int InvitationId { get; set; }
    }
}