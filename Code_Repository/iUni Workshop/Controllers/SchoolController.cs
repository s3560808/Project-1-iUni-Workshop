using System.Linq;
using iUni_Workshop.Data;
using iUni_Workshop.Models.SchoolModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    [Authorize]
    public class SchoolController : Controller
    {
        private  ApplicationDbContext _context;

        public SchoolController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [Route("[Controller]/GetSchoolName/{schoolName}")]
        public IActionResult GetSchoolName(string schoolName)
        {
            var result = _context.Schools
                .Where(a =>a.SchoolName.Contains(schoolName) && a.Status == SchoolStatus.InUse)
                .Select(b => b.SchoolName)
                .AsEnumerable()
                .Distinct()
                .ToList();
            return Json(result);
        }

        [Route("[Controller]/GetSchoolDomainExtension/{schoolName}")]
        public IActionResult GetSchoolDomainExtension(string schoolName)
        {
            var result = _context.Schools
                .Where(a =>a.NormalizedName == schoolName.ToUpper() && a.Status == SchoolStatus.InUse)
                .Select(b => b.DomainExtension)
                .AsEnumerable()
                .Distinct()
                .ToList();
            return Json(result);
        }
    }
}