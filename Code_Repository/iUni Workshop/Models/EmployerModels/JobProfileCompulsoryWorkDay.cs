using System;
using System.ComponentModel.DataAnnotations;

namespace iUniWorkshop.Models.EmployerModels
{
    public class JobProfileCompulsoryWorkDay
    {
        [Range(1,7, ErrorMessage = "Please enter correct day")]
        public int Day { get; set; }
    }
}
