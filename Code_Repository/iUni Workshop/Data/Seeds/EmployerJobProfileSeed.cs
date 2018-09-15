using System;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace iUni_Workshop.Data.Seeds
{
    public class EmployerJobProfileSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            
            var context = serviceProvider.GetService<ApplicationDbContext>();
            CreateEmployerJobProfile(1, "employer@example.com", DateTime.Now, DateTime.Now, 1, "Title 1", "Description 1", true, 1, 3, 20, context);
            CreateEmployerJobProfile(2, "employer@example.com", DateTime.Now, DateTime.Now, 1, "Title 2", "Description 2", false, 1, 3, (float) 19.8, context);
        }

        private static async Task CreateEmployerJobProfile(int id, string name, DateTime create, DateTime update, int field, string title, string description, bool requireExperience, int max, int min, float salary, ApplicationDbContext _context)
        {
            var user = _context.Users.First(a => a.Email == name);
            var newJobProfile = new EmployerJobProfile
                { Id = id, EmployerId = user.Id, CreateDateTime = create, LastUpdateDateTime = update, FieldId = field, Title = title, Description = description, RequireJobExperience = requireExperience, MaxDayForAWeek = max, MinDayForAWeek = min, Salary = salary};
            var check = _context.EmployerJobProfiles.Where(a => a.Id == id);
            if (check.Any())
            {
                return;
            }
           
            try
            {
                await _context.EmployerJobProfiles.AddAsync(newJobProfile);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}