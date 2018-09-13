using System;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class InvitationDetail
    {
        public string Title { get; set; }
        public DateTime SentDate { get; set; }
        public string CompanyDescription { get; set; }
        public string JobDescription { get; set; }
        public int InvitationId { get; set; }
    }
}