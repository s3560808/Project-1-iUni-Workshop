using System;
using System.ComponentModel.DataAnnotations;

namespace iUni_Workshop.Models.InvatationModel
{
    public class Invatation
    {
        public Invatation(String employeeId, String employerId, int jobProfileId, DateTime sentDate, DateTime expireDate, bool accepted)
        {
            EmployeeId = employeeId;
            EmployerId = employerId;
            JobProfileId = jobProfileId;
            SentDate = sentDate;
            ExpireDate = expireDate;
            Accepted = accepted;
        }

        public String EmployeeId;
        public String EmployerId;
        public int JobProfileId;
        public DateTime SentDate;
        public DateTime ExpireDate;
        public bool Accepted;
    }
}