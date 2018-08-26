using System;
using System.Threading.Tasks;
using iUni_Workshop.Models;
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
            await CreateUser("admin@example.com", Roles.Administrator, userManager, logger);
            await CreateUser("employee@example.com", Roles.Employee, userManager, logger);
            await CreateUser("employer@example.com", Roles.Employer, userManager, logger);
        }

        private static async Task CreateUser(String userName, String role, UserManager<ApplicationUser> userManager, ILogger<Program> logger) {
            var user = new ApplicationUser { UserName = userName, Email = userName};
            var result = await userManager.CreateAsync(user, "1234qwAS./");
            logger.LogCritical(result.Succeeded.ToString());
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}