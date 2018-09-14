using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Models.JobRelatedModels;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class AddField
    {
        [Required(ErrorMessage = "Field name is required")]
        public string Name { get; set; }

        public int Id { get; set; }
        
        [Required(ErrorMessage = "Field status is required")]
        [Range(FieldStatus.InUse, FieldStatus.NoLongerUsed, ErrorMessage = "Please enter correct status for field")]
        public int Status { get; set; }
    }
}
