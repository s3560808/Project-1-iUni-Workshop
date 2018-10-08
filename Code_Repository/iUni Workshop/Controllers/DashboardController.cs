using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;


namespace iUni_Workshop.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context
            ) {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userRoleList = await _userManager.GetRolesAsync(user);
            var adminNum = (await _userManager.GetUsersInRoleAsync(Roles.Administrator)).Count;
            if (userRoleList.ToArray().Length != 0)
            {
                var role = userRoleList.First();
                switch (role)
                {
                    case Roles.Administrator:
                        return RedirectToAction("Index","Administrator");
                    case Roles.Employee:
                        return RedirectToAction("Index","Employee");
                    case Roles.Employer:
                        return RedirectToAction("Index","Employer");
                    default:
                        break;
                }
            }
            ViewBag.HasAdmin = Convert.ToBoolean(adminNum);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SetUserType(string userTypeId)
        {
            var user = await _userManager.GetUserAsync(User);
            switch (userTypeId)
            {
                    case Roles.AdministratorId:
                        //TODO if already has admins in database return
                        await _userManager.AddToRoleAsync(user,Roles.Administrator);
                        _context.Administraotrs.Add(new Administraotr{Id=user.Id,Name=user.UserName});
                        await _context.SaveChangesAsync();
                        break;
                    case Roles.EmployeeId:
                        await _userManager.AddToRoleAsync(user,Roles.Employee);
                        _context.Employees.Add(new Employee{Id = user.Id, Name = user.UserName});
                        _context.SaveChanges();
                        break;
                    case Roles.EmployerId:
                        await _userManager.AddToRoleAsync(user,Roles.Employer);
                    _context.Employers.Add(new Employer{Id = user.Id, Name = user.UserName});
                        _context.SaveChanges();
                        break;
                    default:
                        break;
            }
            return RedirectToAction("Index");
        }

    }
}