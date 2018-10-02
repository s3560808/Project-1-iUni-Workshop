using System.Collections.Generic;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class Index
    {
        public string Name { get; set; }
        public string BriefDescription { get; set; }
        public List<IndexCv> Cvs { get; set; }
    }
}