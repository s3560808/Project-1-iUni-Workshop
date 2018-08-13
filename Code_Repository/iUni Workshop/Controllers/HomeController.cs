using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using iUni_Workshop.Models;
using Microsoft.AspNetCore.Identity;

namespace iUni_Workshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            bool exitAdmin = await _roleManager.RoleExistsAsync("Administrator");
            bool exitEmployer = await _roleManager.RoleExistsAsync("Employer");
            bool exitEmployee = await _roleManager.RoleExistsAsync("Employee");
            if (!exitAdmin)
            {
                var role = new IdentityRole();
                role.Name = "Administrator";
                await _roleManager.CreateAsync(role);
            }
            if (!exitEmployer)
            {
                var role = new IdentityRole();
                role.Name = "Employer";
                await _roleManager.CreateAsync(role);
            }
            if (!exitEmployee)
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                await _roleManager.CreateAsync(role);
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
