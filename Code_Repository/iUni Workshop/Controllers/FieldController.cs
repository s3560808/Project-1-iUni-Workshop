using System;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.FiledModels;
using iUni_Workshop.Models.JobRelatedModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    public class FieldController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FieldController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        //An API to get in use Filed Name with partial Filed Name
        //Return with JSON list
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

        //An API to get ALL in use Filed Name
        //Return with JSON list
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
        
        //View of RequestToAddField
        [Route("[Controller]/RequestToAddField/")]
        public IActionResult RequestToAddField()
        {
            ProcessSystemInfo();
            return View();
        }

        //Action of RequestToAddField
        public async Task<RedirectToActionResult> RequestToAddFieldAction(RequestToAddFieldAction field)
        {
            InitialSystemInfo();
            //1. Validate front-end user input
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("RequestToAddField");
            }
            //2. Check if field is in database
            try
            {
                //2.1 In database
                var inDatabase = _context.Fields.First(a => a.NormalizedName == field.FieldName.ToUpper());
                switch (inDatabase.Status)
                {
                    case FieldStatus.InRequest:
                        AddToTempDataInform(field.FieldName + " is in request already");
                        break;
                    case FieldStatus.InUse:
                        AddToTempDataInform(field.FieldName + " is in use already");
                        break;
                    case FieldStatus.NoLongerUsed:
                        AddToTempDataInform(field.FieldName + " is no longer used already");
                        break;
                    default:
                        break;
                }
            }
            catch (InvalidOperationException)
            {
                //2.2 Not in database then add field
                var user = await _userManager.GetUserAsync(User);
                var newField = new Field
                {
                    Name = field.FieldName, 
                    NormalizedName = field.FieldName.ToUpper(), 
                    RequestedBy = user.Id,
                    Status = SkillStatus.InRequest
                };
                _context.Fields.Add(newField);
                _context.SaveChanges();
                AddToTempDataSuccess(field.FieldName + ". Skill request sent successfully");
                
            }
            //3. Redirect
            return RedirectToAction("RequestToAddField");
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