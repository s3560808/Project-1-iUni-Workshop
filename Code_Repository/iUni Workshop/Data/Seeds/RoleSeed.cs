using System;
using System.Threading.Tasks;
using iUni_Workshop;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace iUniWorkshop.Data.Seeds
{
    public class RoleSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetService<ApplicationDbContext>();
            await CreateRole(Roles.Administrator, roleManager, logger);
            await CreateRole(Roles.Employee, roleManager, logger);
            await CreateRole(Roles.Employer, roleManager, logger);
        }

        private static async Task CreateRole(string role, RoleManager<IdentityRole> roleManager, ILogger<Program> logger)
        {
            var result = await roleManager.CreateAsync(new IdentityRole { Name = role });
            logger.LogCritical(result.ToString());
        }
    }
}
