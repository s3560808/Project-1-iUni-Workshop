using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models.SuburbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    [Authorize]
    public class SuburbController : Controller
    {
        private  ApplicationDbContext _context;

        public SuburbController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("[Controller]/GetSuburb/{surburbName}")]
        public IActionResult GetSuburb(string surburbName)
        {
            var allSuburbs = _context.Suburbs.Where(a =>
                    a.Name.Contains(surburbName.ToUpper()) &&
                    (
                        (a.PostCode > 0 && a.PostCode < 200) ||
                        (a.PostCode > 299 && a.PostCode < 900) ||
                        (a.PostCode > 1999 && a.PostCode < 5800) ||
                        (a.PostCode > 5999 && a.PostCode < 6800) ||
                        (a.PostCode > 6999 && a.PostCode < 7800)
                    )
                ).ToList();
            List<string> result = new List<string> { };
            if (allSuburbs.Count > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    result.Add(allSuburbs[i].Name);
                }
            }
            else
            {
                result = allSuburbs.Select(v => v.Name).ToList();
            }
            result = result.ToAsyncEnumerable().ToEnumerable().Distinct().ToList();
            return Json(result);
        }
        
        [Route("[Controller]/GetPostCode/{surburbName}")]
        public IActionResult GetPostCode(string surburbName)
        {
            var result = _context.Suburbs.Where(a =>a.Name == surburbName).Select(b => b.PostCode).ToList();
            return Json(result);
        }

        public int hasSuburb(string SuburbName, int PostCode)
        {
            var suburb = _context.Suburbs.First(a => a.Name == SuburbName && a.PostCode == PostCode);
            if (suburb == null)
            {
                return -1;
            }
            return suburb.Id;
        }

    }
}