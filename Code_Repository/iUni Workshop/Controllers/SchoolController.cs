using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.SchoolModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = Roles.Employer+","+Roles.Employee+","+Roles.Administrator)]
    public class SchoolController : Controller
    {
        //Property of school controller
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;     

        //Constructor of school controller
        public SchoolController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        //An API to get in use school names with partial school name
        //Return with JSON list
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

        //An API to get in use school domain extension with school name
        //Return with JSON list
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
        
        //An API to get in use school campus (suburb name & post code) with school name
        //Return with JSON list
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
        
        //View of RequestToAddSchool
        [Route("[Controller]/RequestToAddSchool")]
        public ViewResult RequestToAddSchool()
        {
            return View();
        }
        
        //TODO 如果相同学校名字，不同domainextension？
        //Real action of RequestToAddSchool
        public async Task<IActionResult> AddSchoolAction(AddSchoolAction school)
        {
            InitialSystemInfo();  
            //1. Check input from front-end
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("RequestToAddSchool");
            }
            var user = await _userManager.GetUserAsync(User);
            int suburbId;
            //2. Check if get correct suburb
            try
            {
                //2.1. try to get suburb id
                suburbId = _context.Suburbs.First(a => 
                    a.Name == school.SuburbName.ToUpper() && 
                    a.PostCode == school.PostCode
                    ).Id;
            }
            catch (InvalidOperationException)
            {
                //2.2. Cannot find suburb
                AddToTempDataError("Sorry you entered invalid suburb information");
                return RedirectToAction("RequestToAddSchool");
            }
            //3. Check if school already in database
            try
            {
                //3.1. try to find school in database
                //If in database add inform and return
                var unused = _context.Schools.First(a => 
                    a.NormalizedName == school.SchoolName.ToUpper() &&
                    a.SuburbId == suburbId);
                AddToTempDataInform("Sorry your school is already in database");
                return RedirectToAction("RequestToAddSchool");
            }
            catch (InvalidOperationException)
            {
                //3.2. Not in database
            }
            //4. Add new school and redirect
            var newSchool = new School
            {
                DomainExtension =  school.DomainExtension, 
                SchoolName = school.SchoolName, 
                NormalizedName = school.SchoolName.ToUpper(), 
                SuburbId = suburbId, 
                NewRequest = true, 
                RequestedBy = user.Id
            };
            _context.Schools.Add(newSchool);
            _context.SaveChanges();
            AddToTempDataSuccess("Your request to add "+school.SchoolName+" "+school.SuburbName+" already sent to administrator");
            return RedirectToAction("RequestToAddSchool");
        }
        
        //----------------------------------
        //----------------------------------
        //----------------------------------
        //----------------------------------
        //----------------------------------
        //----------------------------------
        //----------------------------------
        public void ProcessSystemInfo()
        {
            if ((string) TempData["Error"] != "")
            {
                ViewBag.Error = TempData["Error"];
            }
            if ((string) TempData["Inform"] != "")
            {
                ViewBag.Inform = TempData["Inform"];
            }
            if ((string) TempData["Success"] != "")
            {
                ViewBag.Success = TempData["Success"];
            }
        }

        public void InitialSystemInfo()
        {
            TempData["Error"] = "";
            TempData["Inform"] = "";
            TempData["Success"] = "";
        }

        public void ProcessModelState()
        {
            foreach (var model in ModelState)
            {
                if (model.Value.Errors.Count == 0) continue;
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }
                
                foreach (var error in model.Value.Errors)
                {
                    TempData["Error"]  += error.ErrorMessage;
                }
                
            }
        }

        public void AddToViewBagInform(string informMessage)
        {
            if ((string) ViewBag.Inform != "")
            {
                ViewBag.Inform += "\n";
            }
            ViewBag.Inform += informMessage;
        }
        
        public void AddToViewBagError(string errorMessage)
        {
            if ((string) ViewBag.Error != "")
            {
                ViewBag.Error+= "\n";
            }
            ViewBag.Error += errorMessage;
        }
        
        public void AddToViewBagSuccess(string successMessage)
        {
            if ((string) ViewBag.Success != "")
            {
                ViewBag.Success += "\n";
            }
            ViewBag.Success += successMessage;
        }
        
        public void AddToTempDataSuccess(string successMessage)
        {
            if ((string) TempData["Success"] != "")
            {
                TempData["Success"] += "\n";
            }
            TempData["Success"] += successMessage;
        }
        
        public void AddToTempDataInform(string informMessage)
        {
            if ((string) TempData["Inform"] != "")
            {
                TempData["Inform"] += "\n";
            }
            TempData["Inform"] += informMessage;
        }
        
        public void AddToTempDataError(string errorMessage)
        {
            if ((string) TempData["Error"] != "")
            {
                TempData["Error"] += "\n";
            }
            TempData["Error"] += errorMessage;
        }
    }
}