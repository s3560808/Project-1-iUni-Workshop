using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployeeModels;

namespace iUni_Workshop.Models.JobRelatedModels
{
    public class Field
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int FieldId { get; set; }
        public string FieldName { get; set; }

        public virtual CV CV { get; set; }
    }
}