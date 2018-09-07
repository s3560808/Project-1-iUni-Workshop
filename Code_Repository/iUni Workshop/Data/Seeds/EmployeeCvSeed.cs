using System;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Models;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace iUni_Workshop.Data.Seeds
{
    public class EmployeeCvSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            
            var context = serviceProvider.GetService<ApplicationDbContext>();
            CreateEmployeeCv("employee@example.com", 1, "Detail of Job 1", "Title of Job 1", 10, true, DateTime.Now, 1, true, context);
            CreateEmployeeCv("employee@example.com", 2, "Detail of Job 2", "Title of Job 2", (float) 9.8, true, DateTime.Now, 1, false, context);
        }

        private static void CreateEmployeeCv(string userName, int id, string detail, string title, float salary, bool findJobStatus, DateTime startFindJobDate, int fieldId, bool primary, ApplicationDbContext _context)
        {
            var user = _context.Users.First(a => a.Email == userName);
            var newCv = new EmployeeCV
            { Id = id, Details = detail, Title = title, EmployeeId = user.Id, MinSaraly = salary, FindJobStatus = findJobStatus, StartFindJobDate = startFindJobDate, FieldId = fieldId,Primary = primary};
            try
            {
                _context.EmployeeCvs.Add(newCv);
                _context.SaveChanges();
            }
            catch (System.NullReferenceException  ex)
            {
            }
            
        }
    }
}