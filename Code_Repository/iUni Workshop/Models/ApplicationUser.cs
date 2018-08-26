using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using iUni_Workshop.Models.MessageModels;
using iUni_Workshop.Models.SchoolModels;
using Microsoft.AspNetCore.Identity;

namespace iUni_Workshop.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //public virtual ICollection<Employee> Employees { get; set; }
        //public virtual ICollection<Employer> Employers { get; set; }
        //public virtual ICollection<Administraotr> Administraotrs { get; set; }
        //public virtual ICollection<School> Schools { get; set; }
    }
}
