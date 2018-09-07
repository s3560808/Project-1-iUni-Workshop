using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;

namespace iUni_Workshop.Models.JobRelatedModels
{
    public class SkillHistoryOperations
    {
        public const int Permit = 1;
        public const int Update = 2;
        public const int Ban = 3;
    }
}