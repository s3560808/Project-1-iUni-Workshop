using System.Linq;
using iUni_Workshop.Data;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.SchoolModels;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    public class SkillController : Controller
    {
        private  ApplicationDbContext _context;

        public SkillController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [Route("[Controller]/GetSkillName/{skillName}")]
        public IActionResult GetSkillName(string skillName)
        {
            var result = _context.Skills
                .Where(a =>a.NormalizedName.Contains(skillName.ToUpper()) && a.Status == SkillStatus.InUse)
                .Select(b => b.Name)
                .AsEnumerable()
                .Distinct()
                .ToList();
            return Json(result);
        }
    }
}