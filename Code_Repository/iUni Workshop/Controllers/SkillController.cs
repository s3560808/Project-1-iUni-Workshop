using System;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.SkillModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = Roles.Employer+","+Roles.Employee+","+Roles.Administrator)]
    public class SkillController : Controller
    {
        //Property of skill controller
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        //Constructor of skill controller
        public SkillController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        //An API to get skill names with partial skill name
        //Return with JSON list
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
        
        //View of RequestToAddSkill
        [Route("[Controller]/RequestToAddSkill")]
        [Authorize(Roles = Roles.Employer+","+Roles.Employee)]
        public IActionResult RequestToAddSkill()
        {
            ProcessSystemInfo();
            return View();
        }
        
        //Real action of RequestToAddSkill
        [Authorize(Roles = Roles.Employer+","+Roles.Employee)]
        public async Task<RedirectToActionResult> RequestToAddSkillAction(RequestToAddSkillAction skill)
        {
            InitialSystemInfo();
            //1. Check input from front-end
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("RequestToAddSkill");
            }
            //2. Check if request is already in database
            try
            {
                //2.1 Already in database
                //Give user information back
                var inDatabase = _context.Skills.First(a => a.NormalizedName == skill.SkillName.ToUpper());
                switch (inDatabase.Status)
                {
                    case SkillStatus.InRequest:
                        AddToTempDataInform(skill.SkillName + " is in request already");
                        break;
                    case SkillStatus.InUse:
                        AddToTempDataInform(skill.SkillName + " is in use already");
                        break;
                    case SkillStatus.NoLongerUsed:
                        AddToTempDataInform(skill.SkillName + " is no longer used already");
                        break;
                    // ReSharper disable once RedundantEmptySwitchSection
                    default:
                        break;
                }
            }
            catch (InvalidOperationException)
            {
                //2.2. Not in database
                var user = await _userManager.GetUserAsync(User);
                //2.2.1. Create new skill object
                var newSkill = new Skill
                {
                    Name = skill.SkillName, 
                    NormalizedName = skill.SkillName.ToUpper(), 
                    RequestedByUserId = user.Id,
                    Status = SkillStatus.InRequest
                };
                //2.2.2. Add new skill to database and save changes
                _context.Skills.Add(newSkill);
                _context.SaveChanges();
                //2.2.3. Add success information
                AddToTempDataSuccess(skill.SkillName + ". Skill request sent successfully");
            }
            //3. Redirect
            return RedirectToAction("RequestToAddSkill");
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