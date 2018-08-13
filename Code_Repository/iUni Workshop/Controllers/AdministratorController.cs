using System.Threading.Tasks;
using iUni_Workshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministratorController(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
        }
        
        // GET
        
        public async Task<IActionResult> Index()
        {
            var role = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));
            return
            View();
        }
        
        public async Task<IActionResult> AddSchool()
        {
            return View();
        }

        public async Task<IActionResult> AddField()
        {
            return View();
        }

        public async Task<IActionResult> AddSkill()
        {
            return View();
        }

        public async Task<IActionResult> SetUserType()
        {
            return View();
        }

        public async Task<IActionResult> MyMessages()
        {
            return View();
        }

        public async Task<IActionResult> MessageDetail()
        {
            return View();
        }

        public async Task<IActionResult> SystemStatus()
        {
            return View();
        }
    }
}