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
        private readonly ApplicationDbContext _context;

        public SuburbController(ApplicationDbContext context)
        {
            _context = context;
        }

        //This is an API to get suburb names with partial suburb name
        //Return with JSON list, if result more than 10 will only return first 10 results
        [Route("[Controller]/GetSuburb/{suburbName}")]
        public IActionResult GetSuburb(string suburbName)
        {
            var allSuburbs = _context.Suburbs.Where(a =>
                    a.Name.Contains(suburbName.ToUpper()) &&
                    (
                        (a.PostCode > 0 && a.PostCode < 200) ||
                        (a.PostCode > 299 && a.PostCode < 900) ||
                        (a.PostCode > 1999 && a.PostCode < 5800) ||
                        (a.PostCode > 5999 && a.PostCode < 6800) ||
                        (a.PostCode > 6999 && a.PostCode < 7800)
                    )
                ).ToList();
            var result = new List<string> { };
            if (allSuburbs.Count > 10)
            {
                for (var i = 0; i < 10; i++)
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
        
        //This is an API to get post-codes with a suburb name
        //Return with JSON
        [Route("[Controller]/GetPostCode/{suburbName}")]
        public IActionResult GetPostCode(string suburbName)
        {
            var result = _context.Suburbs.Where(a =>a.Name == suburbName).Select(b => b.PostCode).ToList();
            return Json(result);
        }

        //This is an function to check if has a suburb
        //If cannot find corresponding suburb will return -1
        //If find corresponding suburb successfully return suburb id
        [Route("[Controller]/HasSuburb/{suburbName}/{postCode}")]
        public int HasSuburb(string suburbName, int postCode)
        {
            var suburb = _context.Suburbs.Where(a => a.Name == suburbName && a.PostCode == postCode);
            if (!suburb.Any())
            {
                return -1;
            }
            else
            {
                return suburb.First().Id;
            }
        }
        
        [Route("[Controller]/HasSuburb/{suburbName}/{postCode}")]
        public int HasSuburb(string suburbName, int postCode)
        {
            var suburb = _context.Suburbs.Where(a => a.Name == suburbName && a.PostCode == postCode);
            if (!suburb.Any())
            {
                return -1;
            }
            else
            {
                return suburb.First().Id;
            }
        }
    }
}