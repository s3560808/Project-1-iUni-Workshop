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
        public IActionResult Index()
        {
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
