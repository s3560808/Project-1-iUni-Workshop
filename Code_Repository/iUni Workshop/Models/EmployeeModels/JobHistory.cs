using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class JobHistory
    {
        public int JobHistoryId { get; set; }
        [Required]
        public string PreviousJobName { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public int CVId { get; set; }
        [ForeignKey("CVId")]
        public virtual CV CV { get; set; }
    }
}