using System.Collections.Generic;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EditCVAction
    {
        public bool UpdateStatus { get; set; }
        public int CvId { get; set; }
        public bool PrimaryCv { get; set; }
        public string FieldName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float MinSalary { get; set; }
    }
}