using System.Collections.Generic;
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
        
        [Route("[Controller]/GetCampus/{schoolName}")]
        public IActionResult GetCampus(string schoolName)
        {
            var rawResults = _context.Schools
                .Where(a =>a.NormalizedName == schoolName.ToUpper() && a.Status == SchoolStatus.InUse)
                .Select(b => b.SuburbId)
                .AsEnumerable()
                .Distinct()
                .ToList();
            var finalResults = new List<GetCampus>();
            foreach (var rawResult in rawResults)
            {
                finalResults.AddRange(
                    _context.Suburbs
                        .Where(a => a.Id == rawResult)
                        .Select(a => new GetCampus{ PostCode = a.PostCode, SuburbName = a.Name, SchoolName = schoolName}));
            }
            return Json(finalResults.ToList());
        }
        
    }
}