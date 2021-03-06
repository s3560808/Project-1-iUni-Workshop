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
    public class EmployeeCvWorkDaySeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            
            var context = serviceProvider.GetService<ApplicationDbContext>();
            CreateEmployeeWorkDay(1, 1, context);
            CreateEmployeeWorkDay(2, 1, context);
            CreateEmployeeWorkDay(7, 1, context);   
            CreateEmployeeWorkDay(1, 2, context);   
            CreateEmployeeWorkDay(2, 2, context);   
            CreateEmployeeWorkDay(6, 2, context);   
        }

        private static async Task CreateEmployeeWorkDay(int day, int employeeCvId, ApplicationDbContext _context)
        {
            
            var newDay = new EmployeeWorkDay
                { Day = day, EmployeeCvId = employeeCvId};
            var check = _context.EmployeeWorkDays.Where(a => a.Day == day && a.EmployeeCvId == employeeCvId);
            if (check.Any())
            {
                return;
            }
            try
            {
                await _context.EmployeeWorkDays.AddAsync(newDay);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }
    }
}