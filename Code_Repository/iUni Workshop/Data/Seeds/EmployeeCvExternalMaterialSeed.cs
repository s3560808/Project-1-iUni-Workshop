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
    public class EmployeeCvExternalMaterialSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            
            var context = serviceProvider.GetService<ApplicationDbContext>();
            CreateEmployeeCvExternalMeterial(1, "Material 1", 1, "Link 1", context);
            CreateEmployeeCvExternalMeterial(2, "Material 2", 1, "Link 2", context);
            CreateEmployeeCvExternalMeterial(3, "Material 3", 2, "Link 3", context);
            CreateEmployeeCvExternalMeterial(4, "Material 4", 2, "Link 4", context);
        }

        private static void CreateEmployeeCvExternalMeterial(int id, string materialName, int employeeCvId, string link, ApplicationDbContext _context)
        {
            var newCv = new EmployeeExternalMeterial
                { Id = id, Name = materialName, EmployeeCvId = employeeCvId, Link = link};
            try
            {
                _context.EmployeeExternalMeterials.Add(newCv);
                _context.SaveChanges();
               
            }
            catch (NullReferenceException ex)
            {
            }
        }
    }
}