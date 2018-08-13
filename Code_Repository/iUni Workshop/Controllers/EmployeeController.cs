using System.Threading.Tasks;
using iUni_Workshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = Roles.Employee)]
    public class EmployeeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return
            View();
        }

        public async Task<IActionResult> EditPersonalInfo()
        {
            return View();
        }

        public async Task<IActionResult> EditCV()
        {
            return View();
        }

        public async Task<IActionResult> MyInvatations()
        {
            return View();
        }

        public async Task<IActionResult> InvatationDetail()
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
    }
    
}