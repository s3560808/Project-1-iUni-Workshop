using System;
using System.Threading.Tasks;
using iUni_Workshop.Models.EmployeeModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace iUni_Workshop.Data.Seeds
{
    public class EmployeeCvSkillSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            
            var context = serviceProvider.GetService<ApplicationDbContext>();
            CreateEmployeeCvSkill(1, 1, 1, "Certification 1", context);
            CreateEmployeeCvSkill(2, 2, 1, "Certification 2", context);
            CreateEmployeeCvSkill(3, 7, 1, "Certification 3", context);   
            CreateEmployeeCvSkill(4, 1, 2, "Certification 1", context);   
            CreateEmployeeCvSkill(5, 2, 2, "Certification 2", context);   
            CreateEmployeeCvSkill(6, 6, 2, "Certification 3", context);   
        }

        private static void CreateEmployeeCvSkill(int id, int skillId, int employeeCvId, string certification, ApplicationDbContext _context)
        {
            
            var newSkill = new EmployeeSkill
                { Id = id, SkillId = skillId, EmployeeCvId = employeeCvId, CertificationLink = certification};
            try
            {
                _context.EmployeeSkills.Add(newSkill);
                _context.SaveChanges();
            }
            catch (NullReferenceException ex)
            {
            }
            
        }
    }
}