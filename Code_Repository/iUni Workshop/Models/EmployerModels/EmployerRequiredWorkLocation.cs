using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iUni_Workshop.Models.EmployerModels;
using iUni_Workshop.Models.SuburbModels;

namespace iUniWorkshop.Models.EmployerModels
{
    public class EmployerRequiredWorkLocation
    {

        [Display(Name = "Suburb"),Key]
        public int SuburbId { get; set; }
        [ForeignKey(("SuburbId"))]
        public virtual Suburb Suburb { get; set; }

        [Key]
        public int EmployerJobProfileId { get; set; }
        [ForeignKey("EmployerJobProfileId")]
        public virtual EmployerJobProfile EmployerJobProfile { get; set; }
    }
}
