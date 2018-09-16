using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iUni_Workshop.Models.AdministratorModels
{
    public class SetUserType
    {
        [Required(ErrorMessage = "Correct user email is required")]
        public string Email { get; set; }
    }
}
