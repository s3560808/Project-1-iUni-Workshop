using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace iUni_Workshop.Models.EmployerModels
{
    public class Employer
    {
        public Employer(String userId, String companyName, String companyLocation, String phoneNumber,
            String contactEmail, String photo, String briefDescription, String abn, bool certificated)
        {
            UserId = userId;
            CompanyName = companyName;
            CompanyLocation = companyLocation;
            PhoneNumber = phoneNumber;
            ContactEmail = contactEmail;
            Photo = photo;
            BriefDescription = briefDescription;
            ABN = abn;
            Certificated = certificated;
        }

        public String UserId;
        public String CompanyName;
        public String CompanyLocation;
        public String PhoneNumber;
        public String ContactEmail;
        public String Photo;
        public String BriefDescription;
        public String ABN;
        public bool Certificated;
    }
}