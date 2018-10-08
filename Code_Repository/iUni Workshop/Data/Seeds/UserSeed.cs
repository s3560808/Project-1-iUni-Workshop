using System;
using System.Threading.Tasks;
using iUni_Workshop.Controllers;
using iUni_Workshop.Models;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace iUni_Workshop.Data.Seeds
{
    public class UserSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetService<ApplicationDbContext>();
            await CreateUser("admin@example.com", Roles.Administrator, userManager, logger, context);
            await CreateUser("employee@example.com", Roles.Employee, userManager, logger, context);
            await CreateUser("employer@example.com", Roles.Employer, userManager, logger, context);
        }

        private static async Task CreateUser(String userName, String role, UserManager<ApplicationUser> userManager, ILogger<Program> logger, ApplicationDbContext applicationDbContext) {
            var user = new ApplicationUser { UserName = userName, Email = userName};
            
            try
            {
                var result = await userManager.CreateAsync(user, "1234qwAS./");
                logger.LogCritical(result.Succeeded.ToString());
                if (result.Succeeded)
                {
                    switch (role)
                    {
                        case Roles.Administrator:
                            await userManager.AddToRoleAsync(user, Roles.Administrator);
                            applicationDbContext.Administraotrs.Add(new Administraotr
                                {Id = user.Id, Name = user.UserName});
                            await applicationDbContext.SaveChangesAsync();
                            break;
                        case Roles.Employee:
                            await userManager.AddToRoleAsync(user, Roles.Employee);
                            applicationDbContext.Employees.Add(new Employee {Id = user.Id, Name = user.UserName});
                            applicationDbContext.SaveChanges();
                            break;
                        case Roles.Employer:
                            await userManager.AddToRoleAsync(user, Roles.Employer);
                            applicationDbContext.Employers.Add(new Employer {Id = user.Id, Name = user.UserName});
                            applicationDbContext.SaveChanges();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}