using System;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Models.EmployerModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace iUni_Workshop.Data.Seeds
{
    public class EmployerJobSkillSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            
            var context = serviceProvider.GetService<ApplicationDbContext>();
            CreateEmployerJobSkill(1, 1, 1, true, context);
            CreateEmployerJobSkill(2, 1, 2, false, context);
        }

        private static async Task CreateEmployerJobSkill(int id, int profileId, int skillId, bool required, ApplicationDbContext _context)
        {
            var newSkill = new EmployerSkill
                { Id = id, EmployerJobProfileId = profileId, SkillId = skillId, Required = required};
            var check = _context.EmployerSkills.Where(a => a.Id == id);
            if (check.Any())
            {
                return;
            }
            try
            {
                var result = await _context.EmployerSkills.AddAsync(newSkill);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
            
        }
    }
}