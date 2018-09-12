using System;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Models.EmployeeModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace iUni_Workshop.Data.Seeds
{
    public class EmployeeCvJobHisotriesSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            
            var context = serviceProvider.GetService<ApplicationDbContext>();
            CreateEmployeeCvJobHistory(1, "History 1 of Job 1", "Description 1 of Job 1", 1, context);
            CreateEmployeeCvJobHistory(2, "History 1 of Job 1", "Description 2 of Job 1", 1, context);
        }

        private static async Task CreateEmployeeCvJobHistory(int id, string historyName, string shortDescription, int employerCvId, ApplicationDbContext _context)
        {
            
            var newHistory = new EmployeeJobHistory
                { Id = id, Name = historyName, ShortDescription = shortDescription, EmployeeCvId = employerCvId};
            var check = _context.EmployeeJobHistories.Where(a => a.Id == id);
            if (check.Any())
            {
                return;
            }
            try
            {
                await _context.EmployeeJobHistories.AddAsync(newHistory);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}