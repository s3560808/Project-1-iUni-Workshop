using System;
using System.Collections.Generic;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class IndexCv
    {
        public string Description { get; set; }
        public string FieldName { get; set; }
        public readonly List<string> SkillNames = new List<string>();
    }
}