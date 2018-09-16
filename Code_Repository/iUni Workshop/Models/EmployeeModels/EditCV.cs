using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.JobRelatedModels;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class EditCV
    {
        public int CvId { get; set; }

        public string Email { get; set; }

        public bool Primary { get; set; }
        
        public bool FindJobStatus { get; set; }
        
        public DateTime StartFindJobDate { get; set; }
        
        public string Title { get; set; }
        
        public string Details { get; set; }
        
        public float MinSaraly { get; set; }

        public int FieldId { get; set; }

        public string FieldName { get; set; }

        public List<EmployeeExternalMeterial> externalMaterials = new List<EmployeeExternalMeterial>();
        
        public List<EmployeeJobHistory> JobHistories = new List<EmployeeJobHistory>();
        
        public List<EditCVEmployeeSkill> Skills = new List<EditCVEmployeeSkill>();
        
        public List<EmployeeWorkDay> WorkDays = new List<EmployeeWorkDay>();
    }
}