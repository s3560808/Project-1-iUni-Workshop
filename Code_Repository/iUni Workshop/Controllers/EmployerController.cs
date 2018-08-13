using System.Threading.Tasks;
using iUni_Workshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = Roles.Employer)]
    public class EmployerController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return
            View();
        }
        
        public async Task<IActionResult> EditCompanyInfo()
        {
            return View();
        }

        public async Task<IActionResult> EditJobProfile()
        {
            return View();
        }
        
        public async Task<IActionResult> SearchApplicants() {
            return View();
        }

        public async Task<IActionResult> ApplicantsStatus()
        {
            return View();
        }

        public async Task<IActionResult> SentInvatationDetail()
        {
            return View();
        }
        
        //To employee
        public async Task<IActionResult> MyMessages()
        {
            return View();
        }

        public async Task<IActionResult> MessageDetail()
        {
            return View();
        }

        public async Task<IActionResult> CertificateMyCompany()
        {
            return View();
        }
    }
}