using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Models.EmployeeModels;
using Microsoft.AspNetCore.Identity;

namespace iUni_Workshop.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual Employee Employee { get; set; }
    }
}
