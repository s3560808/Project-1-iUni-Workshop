using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.EmployerModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = Roles.Employer)]
    public class EmployerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EmployerController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager
        ) {
            _userManager = userManager;
            _context = context;
        }
        // GET
        public IActionResult Index()
        {
            
            return View();
        }
        
        public async Task<IActionResult> EditCompanyInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            var info = _context.Employers.Where(a => a.Id == user.Id).Select(a => new EditCompanyInfo {
                ABN = a.ABN,
                Address = a.Location,
                BriefDescription = a.BriefDescription,
                Certificated = a.Certificated,
                ContactEmail = a.ContactEmail,
                Name = a.Name,
                PostCode = a.Suburb.PostCode,
                SuburbName = a.Suburb.Name,
                RequestionCertification = a.RequestCertification,
                PhoneNumber = a.PhoneNumber
            }).First();
            return View(info);
        }

        public RedirectToActionResult EditCompanyInfoAction(EditCompanyInfo info)
        {
            
            return RedirectToAction("EditCompanyInfo");
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