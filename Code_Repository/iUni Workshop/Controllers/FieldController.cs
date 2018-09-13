using System.Linq;
using iUni_Workshop.Data;
using iUni_Workshop.Models.JobRelatedModels;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    public class FieldController : Controller
    {
        private  ApplicationDbContext _context;

        public FieldController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [Route("[Controller]/GetFieldName/{fieldName}")]
        public IActionResult GetFieldName(string fieldName)
        {
            var result = _context.Fields
                .Where(a =>a.NormalizedName.Contains(fieldName.ToUpper()) && a.Status == SkillStatus.InUse)
                .Select(b => b.Name)
                .AsEnumerable()
                .Distinct()
                .ToList();
            return Json(result);
        }

        [Route("[Controller]/GetAllFields/")]
        public IActionResult GetAllFields()
        {
            var result = _context.Fields
                .Where(a => a.Status == SkillStatus.InUse)
                .Select(b => b.Name)
                .AsEnumerable()
                .Distinct()
                .ToList();
            return Json(result);
        }
    }
}