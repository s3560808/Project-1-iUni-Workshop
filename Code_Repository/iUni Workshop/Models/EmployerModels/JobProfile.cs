using System;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;

namespace iUni_Workshop.Models.EmployerModels
{
    public class JobProfile
    {
        public JobProfile(int jobProfileId, String employerId, String jobTitle, String jobDescription,
            ArrayList<int> requredSkill, 
            ArrayList<int> optionalSkill, 
            Boolean requireJobExperience, 
            int maxDayForAWeek,
            int minDayForAWeek,
            ArrayList<int> complusoryWorkingDays,
            Double salary,
            String salaryUnit
            )
        {
            JobProfileId = jobProfileId;
            EmployerId = employerId;
            JobTitle = jobTitle;
            JobDescription = jobDescription;
            RequredSkill = requredSkill;
            OptionalSkill = optionalSkill;
            RequireJobExperience = requireJobExperience;
            MaxDayForAWeek = maxDayForAWeek;
            MinDayForAWeek = minDayForAWeek;
            ComplusoryWorkingDays = complusoryWorkingDays;
            Salary = salary;
            SalaryUnit = salaryUnit;
        }

        public JobProfile()
        {
        }

        public int JobProfileId;
        public String EmployerId;
        public String JobTitle;
        public String JobDescription;
        public ArrayList<int> RequredSkill;
        public ArrayList<int> OptionalSkill;
        public Boolean RequireJobExperience;
        public int MaxDayForAWeek;
        public int MinDayForAWeek;
        public ArrayList<int> ComplusoryWorkingDays;
        public Double Salary;
        public String SalaryUnit;
    }
}