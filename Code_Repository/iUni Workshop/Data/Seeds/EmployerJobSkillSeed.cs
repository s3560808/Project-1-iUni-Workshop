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

        private static void CreateEmployerJobSkill(int id, int profileId, int skillId, bool required, ApplicationDbContext _context)
        {
            var newSkill = new EmployerSkill
                { Id = id, EmployerJobProfileId = profileId, SkillId = skillId, Required = required};
            try
            {
                _context.EmployerSkills.Add(newSkill);
                _context.SaveChanges();
            }
            catch (NullReferenceException ex)
            {
            }

            
        }
    }
}