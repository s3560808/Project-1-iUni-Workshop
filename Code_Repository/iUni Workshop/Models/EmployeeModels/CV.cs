using System;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class CV
    {
        public CV(int cvId, String employeeId, String cvDetails, 
            ArrayList<SkillCertification> skillCertification, 
            ArrayList<ExternalMeterial> externalMeteral, 
            ArrayList<JobHisotry> jobHistory)
        {
            CVId = cvId;
            EmployeeId = employeeId;
            CVDetails = cvDetails;
            SkillCertification = skillCertification;
            ExternalMeteral = externalMeteral;
            JobHistory = jobHistory;
        }

        public CV()
        {
        }
        
        public int CVId;
        public String EmployeeId;
        public String CVDetails;
        //Certification
        public ArrayList<SkillCertification> SkillCertification;
        public ArrayList<ExternalMeterial> ExternalMeteral;
        public ArrayList<JobHisotry> JobHistory;
    }
}