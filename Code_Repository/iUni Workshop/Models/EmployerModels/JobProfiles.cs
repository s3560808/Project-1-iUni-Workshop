using System;

namespace iUni_Workshop.Models.EmployerModels
{
    public class JobProfiles
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Field { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}