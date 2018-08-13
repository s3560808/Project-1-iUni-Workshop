using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace iUni_Workshop.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userRoleList = await _userManager.GetRolesAsync(user);
            var adminNum = (await _userManager.GetUsersInRoleAsync(Roles.Administrator)).Count();
            if (userRoleList.ToArray().Count() != 0)
            {
                var role = userRoleList.First();
                switch (role)
                {
                    case Roles.Administrator:
                        return RedirectToAction("Index","Administrator");
                        break;
                    case Roles.Employee:
                        return RedirectToAction("Index","Employee");
                        break;
                    case Roles.Employer:
                        return RedirectToAction("Index","Employer");
                        break;
                    default:
                        break;
                }
            }
            ViewBag.HasAdmin = Convert.ToBoolean(adminNum);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SetUserType(String UserTypeId)
        {
            var user = await _userManager.GetUserAsync(User);
            switch (UserTypeId)
            {
                    case Roles.AdministratorId:
                        await _userManager.AddToRoleAsync(user,Roles.Administrator);
                        break;
                    case Roles.EmployeeId:
                        await _userManager.AddToRoleAsync(user,Roles.Employee);
                        break;
                    case Roles.EmployerId:
                        await _userManager.AddToRoleAsync(user,Roles.Employer);
                        break;
                    default:
                        break;
            }
            return RedirectToAction("Index");
        }
    }
}