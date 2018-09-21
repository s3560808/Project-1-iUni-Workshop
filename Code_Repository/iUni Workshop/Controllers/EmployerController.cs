using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUniWorkshop.Models.EmployerModels;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.EmployerModels;
using iUni_Workshop.Models.InvatationModel;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.SchoolModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;


namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = Roles.Employer)]
    public class EmployerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly SuburbController _suburbController;

        public EmployerController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _context = context;
            _suburbController = new SuburbController(_context);
        }
        // GET
        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> EditCompanyInfo()
        {
            ProcessSystemInfo();
            var user = await _userManager.GetUserAsync(User);
            var info = _context.Employers.Where(a => a.Id == user.Id).Select(a => new EditCompanyInfo
            {
                ABN = a.ABN,
                Address = a.Location,
                BriefDescription = a.BriefDescription,
                ContactEmail = a.ContactEmail,
                Name = a.Name,
                PostCode = a.Suburb == null ? 0 : a.Suburb.PostCode,
                SuburbName = a.Suburb == null ? "" : a.Suburb.Name,
                PhoneNumber = a.PhoneNumber,
                Certificated = a.Certificated
            }).First();
            return View(info);
        }

        //Send Email when edited
        public async Task<IActionResult> EditCompanyInfoAction(EditCompanyInfo info)
        {
            InitialSystemInfo();
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("EditCompanyInfo");
            }
            var user = await _userManager.GetUserAsync(User);
            var data = _context.Employers.First(a => a.Id == user.Id);
            try
            {
                var suburbId = _context.Suburbs.First(a => a.Name == info.SuburbName && a.PostCode == info.PostCode).Id;
                if (suburbId != data.SuburbId)
                {
                    try
                    {
                        data.SuburbId = suburbId;
                        _context.Employers.Update(data);
                        _context.SaveChanges();
                        if ((string) TempData["Success"] != "")
                        {
                            TempData["Success"] += "\n";
                        }
                        TempData["Success"] += "Your suburb updated!";
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }
                        TempData["Error"] += "Please enter correct suburb.";
                    }
                }
            }
            catch (InvalidOperationException)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }
                TempData["Error"] += "Please enter correct suburb.";
            }
            if (data.Location != info.Address)
            {
                try
                {
                    data.Location = info.Address;
                    _context.Employers.Update(data);
                    _context.SaveChanges();
                    if ((string) TempData["Success"] != "")
                    {
                        TempData["Success"] += "\n";
                    }
                    TempData["Success"] += "Your address updated!";
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                    TempData["Error"] += "Please enter correct address.";
                }
            }
            if (data.BriefDescription != info.BriefDescription)
            {
                try
                {
                    data.BriefDescription = info.BriefDescription;
                    _context.Employers.Update(data);
                    _context.SaveChanges();
                    if ((string) TempData["Success"] != "")
                    {
                        TempData["Success"] += "\n";
                    }
                    TempData["Success"] += "Your brief description updated!";
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                    TempData["Error"] += "Please enter correct brief description.";
                }
            }
            if (data.ContactEmail != info.ContactEmail)
            {
                try
                {
                    data.ContactEmail = info.ContactEmail;
                    _context.Employers.Update(data);
                    _context.SaveChanges();
                    if ((string) TempData["Success"] != "")
                    {
                        TempData["Success"] += "\n";
                    }
                    TempData["Success"] += "Your contact email updated!";
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                    TempData["Error"] += "Please enter correct contact email.";
                }
            }
            if (data.PhoneNumber != info.PhoneNumber)
            {
                try
                {
                    data.PhoneNumber = info.PhoneNumber;
                    _context.Employers.Update(data);
                    _context.SaveChanges();
                    if ((string) TempData["Success"] != "")
                    {
                        TempData["Success"] += "\n";
                    }
                    TempData["Success"] += "Your phone number updated!";
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                    TempData["Error"] += "Please enter correct phone number.";
                }
            }
            //Infom user if already has abn
            if (!data.Certificated)
            {
                if (data.ABN != info.ABN)
                {
                    try
                    {
                        data.ABN = info.ABN;
                        _context.Employers.Update(data);
                        _context.SaveChanges();
                        if ((string) TempData["Success"] != "")
                        {
                            TempData["Success"] += "\n";
                        }
                        TempData["Success"] += "Your abn updated!";
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }
                        TempData["Error"] += "Please enter correct abn.";
                    }
                }
                if (data.Name != info.Name)
                {
                    try
                    {
                        data.Name = info.Name;
                        _context.Employers.Update(data);
                        _context.SaveChanges();
                        if ((string) TempData["Success"] != "")
                        {
                            TempData["Success"] += "\n";
                        }
                        TempData["Success"] += "Your name updated!";
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }
                        TempData["Error"] += "Please enter correct name.";
                    }
                }
            }
            else
            {
                if (data.ABN != info.ABN)
                {
                    if ((string) TempData["Inform"] != "")
                    {
                        TempData["Inform"] += "\n";
                    }
                    TempData["Inform"] += "You are certificated. Your name Cannot updated";
                }
                if (data.Name != info.Name)
                {
                    if ((string) TempData["Inform"] != "")
                    {
                        TempData["Inform"] += "\n";
                    }
                    TempData["Inform"] += "You are certificated. Your name Cannot updated";
                }
            }
            return RedirectToAction("EditCompanyInfo");
        }

        //Send email
        public async Task<IActionResult> RequestToCertificateMyCompany(){
            var user = await _userManager.GetUserAsync(User);
            var rawInfo = _context.Employers.Where(a => a.Id == user.Id).Select(a => new RequestToCertificateMyCompany
            {
                ABN = a.ABN,
                Address = a.Location,
                BriefDescription = a.BriefDescription,
                ContactEmail = a.ContactEmail,
                Name = a.Name,
                PostCode = a.Suburb == null ? 0 : a.Suburb.PostCode,
                SuburbName = a.Suburb == null ? "" : a.Suburb.Name,
                PhoneNumber = a.PhoneNumber,
                Certificated = a.Certificated
            }).First();
            if(rawInfo.Certificated){
                //already Certificated
                return RedirectToAction("Index");
            }
            if (rawInfo.RequestCertification)
            {
                //already requested
                return RedirectToAction("Index");
            }
            if (rawInfo.ABN==""||rawInfo.Name==""){
                //Need to update info
                return RedirectToAction("Index");
            }
            return View(rawInfo);
        }

        public async Task<IActionResult> RequestToCertificateMyCompanyAction(){
            var user = await _userManager.GetUserAsync(User);
            var employer = _context.Employers.First(a => a.Id == user.Id);
            if (employer.Certificated)
            {
                //already Certificated
                return RedirectToAction("Index");
            }
            if (employer.RequestCertification)
            {
                //already requested
                return RedirectToAction("Index");
            }
            if (employer.ABN == "" || employer.Name == "")
            {
                //Need to update info
                return RedirectToAction("Index");
            }
            employer.RequestCertification = true;
            _context.Employers.Update(employer);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Send email
        //Database
        public IActionResult RequestToUnCertificateMyCompany() {

            return View();
        }

        public async Task<IActionResult> RequestToUnCertificateMyCompanyAction(){
            var user = await _userManager.GetUserAsync(User);
            var employer = _context.Employers.First(a => a.Id == user.Id);
            employer.RequestCertification = false;
            employer.Certificated = false;
            _context.Employers.Update(employer);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> JobProfiles()
        {
            var user = await _userManager.GetUserAsync(User);
            var list = _context.EmployerJobProfiles
                .Where(a => a.EmployerId == user.Id)
                .Select(a => new JobProfiles{ Title = a.Title, Id = a.Id, Field = a.Field.Name, LastUpdateTime = a.LastUpdateDateTime})
                .ToList();
            return View(list);
        }

        [Route("[Controller]/EditJobProfile/{jobProfileId}")]
        [Route("[Controller]/EditJobProfile/")]
        public async Task<IActionResult> EditJobProfile(int jobProfileId)
        {
            ProcessSystemInfo();
            var user = await _userManager.GetUserAsync(User);
            JobProfile jobProfile;
            try
            {
                jobProfile = _context.EmployerJobProfiles.Where(a => a.Id == jobProfileId && a.EmployerId == user.Id).Select(a => new JobProfile
                {
                    ProfileId = a.Id,
                    UpdateDate = a.LastUpdateDateTime,
                    Title = a.Title,
                    Description = a.Description,
                    RequireJobExperience = a.RequireJobExperience,
                    MaxDay = a.MaxDayForAWeek,
                    MinDay = a.MinDayForAWeek,
                    Salary = a.Salary,
                    FieldName = a.Field.Name
                }).First();
            }
            catch (InvalidOperationException)
            {
                return View();
            }

            jobProfile.JobProfileSkills = _context.EmployerSkills
                .Where(a => a.EmployerJobProfileId == jobProfile.ProfileId)
                .Select(a => new JobProfileSkill{ SkillName = a.Skill.Name,SkillRequired = a.Required})
                .ToList();

            jobProfile.JobProfileComplusoryWorkDays = _context.EmployerComplusoryWorkDays
                .Where(a => a.EmployerJobProfileId == jobProfile.ProfileId)
                .Select(a => new JobProfileComplusoryWorkDay {Day = a.Day})
                .ToList();

            jobProfile.JobProfileRequiredLocation = _context.EmployerWorkLocations
                .Where(a => a.EmployerJobProfileId == jobProfile.ProfileId)
                .Select( a=> new JobProfileRequiredLocation{ LocationName = a.Suburb.Name, PostCode = a.Suburb.PostCode})
                .ToList();

            jobProfile.EmployerRequiredSchools = _context.EmployerRequiredSchools
                .Where(a => a.EmployerJobProfileId == jobProfile.ProfileId)
                .Select(a => new JobProfileRequiredSchool { SchoolName = a.School.SchoolName, CampusName = a.School.Suburb.Name, CampusPostCode = a.School.Suburb.PostCode})
                .ToList();
            return View(jobProfile);
        }

        //If have cannot edit
        public async Task<IActionResult> EditJobProfileAction(
            JobProfile jobProfile,
            IEnumerable<JobProfileSkill> jobProfileSkills,
            //TODO 1-7
            IEnumerable<JobProfileComplusoryWorkDay> jobProfileComplusoryWorkDay,
            IEnumerable<JobProfileRequiredLocation> jobProfileRequiredLocation,
            IEnumerable<JobProfileRequiredSchool> jobProfileRequiredSchool
        ){
            InitialSystemInfo();
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("EditJobProfile", new{ JobProfileId = jobProfile.ProfileId});
            }
            var user = await _userManager.GetUserAsync(User);
            EmployerJobProfile newJobProfile = null;
            int newFieldId;
            var newSkills = new List<EmployerSkill>();
            //1. Validate correct employer job profile && if have 
            //invitation     
            if (jobProfile.ProfileId != 0)
            {
                try
                {
                    newJobProfile = _context
                        .EmployerJobProfiles
                        .First(a => a.Id == jobProfile.ProfileId && a.EmployerId == user.Id);
                }
                catch(InvalidOperationException) {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                        
                    TempData["Error"] += "Sorry. It is not your job profile!";
                    //If it is not user's job profile, redirect to job profiles
                    return RedirectToAction("JobProfiles");
                }

                try
                {
                    var profile = newJobProfile;
                    var invitation = _context.Invatations.Where(a => a.EmployerJobProfileId == profile.Id);
                    if (invitation.Any())
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }
                        
                        TempData["Error"] += "Sorry. Your job profile already has invitation. Cannot be changed!";
                        return RedirectToAction("EditJobProfile", new{ JobProfileId = jobProfile.ProfileId});
                    }
                }
                catch (InvalidOperationException)
                {
                }
            }
            //2. Validate if correct field name
            try{
                //Get new job profile field id
                newFieldId = _context.Fields.First(a => 
                    a.Status == FieldStatus.InUse && 
                    a.NormalizedName == jobProfile.FieldName.ToUpper()
                ).Id;
                
            }
            catch(InvalidOperationException)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }       
                TempData["Error"] += "Sorry. Not a valid field!";
                return RedirectToAction("EditJobProfile", new{JobProfileId = jobProfile.ProfileId});
            }
            //3. Validate skill name
            //!! No skill more than 10 skill
         
            var ndSkill  =  jobProfileSkills
                .GroupBy(p => p.SkillName)
                .Select(g => g.First())
                .ToList();
            foreach (var skill in ndSkill)
            {
                try
                {
                    var newSkillId = _context.Skills
                        .First(a => 
                            a.NormalizedName == skill.SkillName.ToUpper() && 
                            a.Status == SkillStatus.InUse
                            )
                        .Id;
                    newSkills.Add(new EmployerSkill{SkillId = newSkillId , Required = skill.SkillRequired});
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }       
                    TempData["Error"] += "Sorry. "+ skill.SkillName + " is not a valid skill";
                }
            }
            if (!newSkills.Any() || newSkills.ToList().Count>10)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }       
                TempData["Error"] += "Sorry. At least one skill at most ten skill!";
                return RedirectToAction("EditJobProfile", new {JobProfileId = jobProfile.ProfileId});
            }
            //GET NEW Days
            var newDays = jobProfileComplusoryWorkDay
                .Select(day => new EmployerComplusoryWorkDay{Day = day.Day})
                .Distinct()
                .ToList();
            //GET NEW Locations
            var ndLocation = jobProfileRequiredLocation
                .Select(a => new {a.PostCode, a.LocationName})
                .Distinct()
                .ToList();
            var newRequiredLocations = new List<EmployerRequiredWorkLocation>();
            foreach (var location in ndLocation)
            {
                try
                {
                    var newSuburbId = _context.Suburbs
                        .First(a => 
                            a.Name == location.LocationName.ToUpper() && 
                            a.PostCode == location.PostCode
                        )
                        .Id;
                    
                    newRequiredLocations.Add(new EmployerRequiredWorkLocation{ SuburbId = newSuburbId});
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }       
                    TempData["Error"] += "Sorry. "+ location.LocationName +" "+location.PostCode + " is not a valid location";
                }
            }
            newRequiredLocations = newRequiredLocations.Distinct().ToList();
            //GET NEW Schools
            var ndSchool = jobProfileRequiredSchool
                .Select(a => new {a.SchoolName, a.CampusName, a.CampusPostCode})
                .Distinct()
                .ToList();
            var newRequiredSchools = new List<EmployerRequiredSchool>();
            foreach (var school in ndSchool)
            {
                try
                {
                    var newSchoolSuburbId = _context.Suburbs
                        .First(a => 
                            a.Name == school.CampusName.ToUpper() && 
                            a.PostCode == school.CampusPostCode
                        )
                        .Id;

                    var schoolId = _context.Schools
                        .First(a => 
                            a.NormalizedName == school.SchoolName.ToUpper() &&
                            a.SuburbId == newSchoolSuburbId &&
                            a.Status == SchoolStatus.InUse
                        ).Id;
                    newRequiredSchools.Add(new EmployerRequiredSchool{ SchoolId = schoolId});
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }       
                    TempData["Error"] += "Sorry. "+ school.SchoolName +" "+ school.CampusName +" "+ school.CampusPostCode + " is not a valid school";
                }
            }
            newRequiredSchools = newRequiredSchools.Distinct().ToList();
            //if new cv, create new cv
            if (newJobProfile == null)
            {
                if (newSkills.Any() && newFieldId != 0)
                {
                    newJobProfile = new EmployerJobProfile
                    {
                        EmployerId = user.Id,
                        FieldId = newFieldId,
                        CreateDateTime = DateTime.Now,
                        Description = jobProfile.Description, 
                        Salary = jobProfile.Salary, 
                        Title = jobProfile.Title, 
                        RequireJobExperience = jobProfile.RequireJobExperience, 
                        LastUpdateDateTime = DateTime.Now, 
                        MaxDayForAWeek = jobProfile.MaxDay, 
                        MinDayForAWeek = jobProfile.MinDay
                    };
                    try
                    {
                        _context.EmployerJobProfiles.Update(newJobProfile);
                        _context.SaveChanges();
                        if ((string) TempData["Success"] != "")
                        {
                            TempData["Success"] += "\n";
                        }       
                        TempData["Success"] += "New job profile Added!";
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }       
                        TempData["Error"] += "Sorry. Failed to create a new job profile!";
                        return RedirectToAction("EditJobProfile");
                    }
                }
                else
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }       
                    TempData["Error"] += "Sorry. Failed to create a new cv!";
                    return RedirectToAction("EditJobProfile");
                }
            }

            foreach (var skill in newSkills)
            {
                skill.EmployerJobProfileId = newJobProfile.Id;
            }
            
            foreach (var day in newDays)
            {
                day.EmployerJobProfileId = newJobProfile.Id;
            }
            
            foreach (var location in newRequiredLocations)
            {
                location.EmployerJobProfileId = newJobProfile.Id;
            }
            
            foreach (var school in newRequiredSchools)
            {
                school.EmployerJobProfileId = newJobProfile.Id;
            }

            if (newJobProfile.RequireJobExperience !=jobProfile.RequireJobExperience)
            {
                try
                {
                    newJobProfile.RequireJobExperience =jobProfile.RequireJobExperience;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    if ((string) TempData["Success"] != "")
                    {
                        TempData["Success"] += "\n";
                    }
                    TempData["Success"] += "Job Profile's \"Required Job Experience\" changed!";
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                        
                    TempData["Error"] += "Sorry. Failed to update \"Required Job Experience\"";
                }
            }
            
            if (newJobProfile.Title !=jobProfile.Title)
            {
                try
                {
                    newJobProfile.Title =jobProfile.Title;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    if ((string) TempData["Success"] != "")
                    {
                        TempData["Success"] += "\n";
                    }
                    TempData["Success"] += "Job Profile's \"Title\" changed!";
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                        
                    TempData["Error"] += "Sorry. Failed to update \"Title\"";
                }
            }
            
            if (newJobProfile.Description !=jobProfile.Description)
            {
                try
                {
                    newJobProfile.Description =jobProfile.Description;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    if ((string) TempData["Success"] != "")
                    {
                        TempData["Success"] += "\n";
                    }
                    TempData["Success"] += "Job Profile's \"Description\" changed!";
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                        
                    TempData["Error"] += "Sorry. Failed to update \"Description\"";
                }
            }
            
            if (!Math.Abs(newJobProfile.Salary - jobProfile.Salary).Equals(0))
            {
                try
                {
                    newJobProfile.Salary =jobProfile.Salary;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    if ((string) TempData["Success"] != "")
                    {
                        TempData["Success"] += "\n";
                    }
                    TempData["Success"] += "Job Profile's \"Salary\" changed!";
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                        
                    TempData["Error"] += "Sorry. Failed to update \"Salary\"";
                }
            }
            
            if (newJobProfile.MaxDayForAWeek !=jobProfile.MaxDay)
            {
                try
                {
                    newJobProfile.MaxDayForAWeek =jobProfile.MaxDay;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    if ((string) TempData["Success"] != "")
                    {
                        TempData["Success"] += "\n";
                    }
                    TempData["Success"] += "Job Profile's \"Max Day For A Week\" changed!";
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                        
                    TempData["Error"] += "Sorry. Failed to update \"Max Day For A Week\"";
                }
            }
            
            if (newJobProfile.MinDayForAWeek !=jobProfile.MinDay)
            {
                try
                {
                    newJobProfile.MinDayForAWeek =jobProfile.MinDay;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    if ((string) TempData["Success"] != "")
                    {
                        TempData["Success"] += "\n";
                    }
                    TempData["Success"] += "Job Profile's \"Min Day For A Week\" changed!";
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                        
                    TempData["Error"] += "Sorry. Failed to update \"Min Day For A Week\"";
                }
            }
            
            if (newJobProfile.FieldId != newFieldId)
            {
                try
                {
                    newJobProfile.MaxDayForAWeek =jobProfile.MaxDay;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    if ((string) TempData["Success"] != "")
                    {
                        TempData["Success"] += "\n";
                    }
                    TempData["Success"] += "Job Profile's \"Field\" changed!";
                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                        
                    TempData["Error"] += "Sorry. Failed to update \"Field\"";
                }
            }
            //Update skills  
            try
            {
                var oldSkills = _context.EmployerSkills.Where(a => a.EmployerJobProfileId == newJobProfile.Id).ToList();
                var newEnumerable = newSkills.Select(a => new {a.Required, Id = a.SkillId}).OrderBy(a => a.Id).ToList();
                var oldEnumerable = oldSkills.Select(a => new {a.Required, Id = a.SkillId}).OrderBy(a => a.Id).ToList();
                
                if (!newEnumerable.SequenceEqual(oldEnumerable) && newEnumerable.Count>0)
                {
                    try
                    {
                            _context.EmployerSkills.RemoveRange(oldSkills);
                        _context.EmployerSkills.AddRange(newSkills);
                        _context.SaveChanges();
                        if ((string) TempData["Success"] != "")
                        {
                            TempData["Success"] += "\n";
                        }       
                        TempData["Success"] += "Job Profile's skills updated!";
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }       
                        TempData["Error"] += "Sorry. Failed to update your skills please retry";
                    }
                }
            }
            catch (InvalidOperationException)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }       
                TempData["Error"] += "Sorry. Failed to update your skills please retry";
            } 
            //New external materials
            try
            {
                var jobProfileId = newJobProfile.Id;
                var oldDays = _context.EmployerComplusoryWorkDays.Where(a => a.EmployerJobProfileId == jobProfileId);
                var newEnumerable = newDays.Select(a => new {a.Day}).OrderBy(a => a.Day).ToList();
                var oldEnumerable = oldDays.Select(a => new {a.Day}).OrderBy(a => a.Day).ToList();
                if (!newEnumerable.SequenceEqual(oldEnumerable) && newEnumerable.Count>0)
                {
                    try
                    {
                    
                        _context.EmployerComplusoryWorkDays.RemoveRange(oldDays);     
                                                                                              
                            _context.EmployerComplusoryWorkDays.AddRange(newDays);    
                                                                                               
                        _context.SaveChanges();                                                   
                        if ((string) TempData["Success"] != "")                                   
                        {                                                                         
                            TempData["Success"] += "\n";                                          
                        }                                                                         
                        TempData["Success"] += "Job Profile's compulsory work day updated!";                 
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }       
                        TempData["Error"] += "Sorry. Failed to update your compulsory work day please retry";
                    }
                }
                
            }
            catch (InvalidOperationException)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }       
                TempData["Error"] += "Sorry. Failed to update your compulsory work day please retry";
            }  
            //New work location
            try
            {
                var jobProfileId = newJobProfile.Id;
                var oldRequiredLocations = _context.EmployerWorkLocations.Where(a => a.EmployerJobProfileId == jobProfileId);
                var newEnumerable = newRequiredLocations.Select(a => new {a.SuburbId}).OrderBy(a => a.SuburbId).ToList();
                var oldEnumerable = oldRequiredLocations.Select(a => new {a.SuburbId}).OrderBy(a => a.SuburbId).ToList();
                if (newEnumerable.SequenceEqual(oldEnumerable) && newEnumerable.Count>0)
                {
                    try
                    {
                    
                        _context.EmployerWorkLocations.RemoveRange(oldRequiredLocations);     
                                                                                             
                            _context.EmployerWorkLocations.AddRange(newRequiredLocations);    
                                                                                               
                        _context.SaveChanges();                                                   
                        if ((string) TempData["Success"] != "")                                   
                        {                                                                         
                            TempData["Success"] += "\n";                                          
                        }                                                                         
                        TempData["Success"] += "Job Profile's compulsory work location updated!";                 
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }       
                        TempData["Error"] += "Sorry. Failed to update your compulsory work location please retry";
                    }
                }
                
            }
            catch (InvalidOperationException)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }       
                TempData["Error"] += "Sorry. Failed to update your compulsory please retry";
            }  
            //New school
            try
            {
                var jobProfileId = newJobProfile.Id;
                var oldRequiredSchools = _context.EmployerRequiredSchools.Where(a => a.EmployerJobProfileId == jobProfileId);
                var newEnumerable = newRequiredSchools.Select(a => new {a.SchoolId}).OrderBy(a => a.SchoolId).ToList();
                var oldEnumerable = oldRequiredSchools.Select(a => new {a.SchoolId}).OrderBy(a => a.SchoolId).ToList();
                if (newEnumerable.SequenceEqual(oldEnumerable) && newEnumerable.Count>0)
                {
                    try
                    {
                        _context.EmployerRequiredSchools.RemoveRange(oldRequiredSchools);     
                                                                                              
                            _context.EmployerRequiredSchools.AddRange(newRequiredSchools);    
                                                                                                
                        _context.SaveChanges();                                                   
                        if ((string) TempData["Success"] != "")                                   
                        {                                                                         
                            TempData["Success"] += "\n";                                          
                        }                                                                         
                        TempData["Success"] += "Job Profile's compulsory employee's school updated!";                 
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }       
                        TempData["Error"] += "Sorry. Failed to update your compulsory employee's school please retry";
                    }
                }
                
            }
            catch (InvalidOperationException)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }       
                TempData["Error"] += "Sorry. Failed to update your compulsory employee's school please retry";
            }  
            
            return RedirectToAction("EditJobProfile", new{ JobProfileId = jobProfile.ProfileId});
        }

        //检查本人操作
        //return if profileId is not right
        //Filter primary id
        [Route("[Controller]/SearchApplicants/{jobProfileId}")]
        public IActionResult SearchApplicants(int jobProfileId)
        {
            ViewData["App"] = jobProfileId;
            return View(SearchApplicantsCoreRanker(jobProfileId));
        }

        [Route("[Controller]/ViewApplicantCv/{jobProfileId}/{cvId}")]
        public IActionResult ViewApplicantCv(int jobProfileId, int cvId)
        {
            var validationList = ( SearchApplicantsCoreRanker(jobProfileId));
            var validationResultList = validationList.Where(a => a.CvId == cvId).ToList();
            //Not valid
            if (!validationResultList.Any())
            {
                return RedirectToAction("Index");
            }
            var raw = _context.EmployeeCvs.First(a => a.Id == validationResultList.First().CvId);
            var result = new ViewApplicantCv
            {
                Title = raw.Title, 
                Description = raw.Details, 
                EmployeeName = raw.Employee.Name
                //TODO!!More Info required
            };
            
            return View(result);
        }

        [Route("[Controller]/SendInvitation/{jobProfileId}/{cvId}")]
        public IActionResult SendInvitation(int jobProfileId, int cvId)
        {
            var validationList = SearchApplicantsCoreRanker(jobProfileId);
            var validationResultList = validationList.Where(a => a.CvId == cvId);
            //Not valid
            if (!validationResultList.Any())
            {
                return RedirectToAction("Index");
            }
            try
            {
                _context.Invatations.Update(new Invatation
                {
                    SentTime = DateTime.Now, status = InvitationStatus.Original, EmployeeCvId = cvId,
                    EmployerJobProfileId = jobProfileId
                });
                _context.SaveChanges();
            }
            catch (InvalidOperationException)
            {

            }
            return View();
        }
        public List<SearchApplicant> SearchApplicantsCoreRanker(int jobProfileId)
        {
            
            var jobProfile = _context.EmployerJobProfiles.First(a => a.Id == jobProfileId);
            var finalData = new List<EmployeeCV>();
            //1.  Filter first
            //1.1 Filter field
            //1.2 && Filter salary
            //1.3 && Filter Find Job Status
            UpdateCvFindJobStatus();
            var rawData = _context.EmployeeCvs
                .Where(a => a.FieldId == jobProfile.FieldId 
                            && a.MinSaraly <= jobProfile.Salary 
                            && a.FindJobStatus )
                .ToList();
            //Filter already got invitation's applications
            var hasInvitations = _context.Invatations.Where(a => a.EmployerJobProfileId == jobProfileId).ToList();
            foreach (var invitation in hasInvitations)
            {
                var invitationCv = _context.EmployeeCvs.Where(a => a.Id == invitation.EmployeeCvId).ToList();
                rawData = rawData.Except(invitationCv).ToList();
            }
            //1.4 Filter job experience
            if (jobProfile.RequireJobExperience)
            {
                foreach (var data in rawData)
                {
                    var experience = _context.EmployeeJobHistories.Count(a => a.EmployeeCvId == data.Id);
                    if (experience > 0)
                    {
                        finalData.Add(data);
                    }
                }  
                rawData = finalData;
                finalData = new List<EmployeeCV>();
            }
            
            //1.5 Filter compulsory work day
            var employerDays = _context.EmployerComplusoryWorkDays
                .Where(a => a.EmployerJobProfileId == jobProfile.Id)
                .Select(a => new {a.Day})
                .OrderBy(a => a.Day);
            if (employerDays.Any())
            {
                foreach (var data in rawData)
                {
                    var employeeDays = _context.EmployeeWorkDays
                        .Where(a => a.EmployeeCvId == data.Id)
                        .Select(a => new {a.Day})
                        .OrderBy(a => a.Day);
                    if (!employerDays.Except(employeeDays).Any())
                    {
                        finalData.Add(data);
                    }
                }
                rawData = finalData;
                finalData = new List<EmployeeCV>();
            }
            
            //1.6. Filter compulsory skill
            var employerSkills = _context.EmployerSkills
                .Where(a => a.EmployerJobProfileId == jobProfile.Id && a.Required)
                .Select(a => new {a.SkillId})
                .OrderBy(a =>a .SkillId)
                .ToList();
            if (employerSkills.Any())
            {
                foreach (var data in rawData)
                {
                    var employeeSkills = _context.EmployeeSkills
                        .Where(a => a.EmployeeCvId == data.Id)
                        .Select(a => new {a.SkillId})
                        .OrderBy(a =>a .SkillId)
                        .ToList();
                    if (!employerSkills.Except(employeeSkills).Any())
                    {
                        finalData.Add(data);
                    }
                }
                rawData = finalData;
                finalData = new List<EmployeeCV>();
            }
            //1.6. Filter compulsory school
            var employerSchools = _context.EmployerRequiredSchools
                .Where(a => a.EmployerJobProfileId == jobProfile.Id)
                .Select(a => new {SchoolId = (int?) a.SchoolId})
                .OrderBy(a =>a .SchoolId);
            if (employerSchools.Any())
            {
                foreach (var data in rawData)
                {
                    var employeeSchools = _context.Employees
                        .Where(a => a.Id == data.EmployeeId)
                        .Select(a => new {a.SchoolId})
                        .OrderBy(a =>a.SchoolId);
                    if (!employerSchools.Except(employeeSchools).Any())
                    {
                        finalData.Add(data);
                    }
                }
                rawData = finalData;
                finalData = new List<EmployeeCV>();
            }
            //1.6. Filter compulsory location
            var employerLocations = _context.EmployerWorkLocations
                .Where(a => a.EmployerJobProfileId == jobProfile.Id)
                .Select(a => new {SuburbId = (int?) a.SuburbId})
                .OrderBy(a =>a .SuburbId);
            if (employerLocations.Any())
            {
                foreach (var data in rawData)
                {
                    var employeeLocations = _context.Employees
                        .Where(a => a.Id == data.EmployeeId)
                        .Select(a => new {a.SuburbId})
                        .OrderBy(a =>a.SuburbId);
                    if (!employerLocations.Except(employeeLocations).Any())
                    {
                        finalData.Add(data);
                    }
                }
                rawData = finalData;
            }
            
            //Rank applicants
            var finalResults = new List<SearchApplicant>();
            foreach (var data in rawData)
            {
                var employeeName = _context.Employees.First(a => a.Id == data.EmployeeId).Name;
                finalResults.Add(new SearchApplicant{ApplicantName = employeeName, CvId = data.Id, Score = 0});
            }
            var employerSelectiveSkills = _context.EmployerSkills
                .Where(a => !a.Required && a.EmployerJobProfileId == jobProfile.Id)
                .OrderBy(a => a.Id)
                .ToList();
            if (employerSelectiveSkills.Any())
            {
                foreach (var result in finalResults)
                {
                    var employeeSkills = _context.EmployeeSkills.Where(a => a.EmployeeCvId == result.CvId).OrderBy(a => a.Id).ToList();
                    for (int i = 0; i < employerSelectiveSkills.Count; i++)
                    {
                        for (int j = 0; j < employeeSkills.Count; j++)
                        {
                            if (employerSelectiveSkills[i].SkillId == employeeSkills[j].SkillId)
                            {
                                result.Score += (11 - i) * (11 - j);
                            }
                        }
                    }
                }
            }

            finalResults = finalResults.OrderBy(a => a.Score).ToList();
            return finalResults;
        }

        public async Task<IActionResult> Invitations()
        {
            var user = await _userManager.GetUserAsync(User);
            var results = _context.Invatations
                .Where(a => a.EmployerJobProfile.EmployerId == user.Id)
                .Select(a => new EmployerInvitation{ InvitationId = a.Id, Status = a.status});
            return View(results);
        }

        public async Task<IActionResult> InvitationDetail()
        {
            
            return View();
        }

        //To employee

        public async Task<IActionResult> CertificateMyCompany()
        {
            return View();
        }
        
        private void InitialSystemInfo()
        {
            TempData["Error"] = "";
            TempData["Inform"] = "";
            TempData["Success"] = "";
        }
        
        private void ProcessModelState()
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
        
        private void ProcessSystemInfo()
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


        private void UpdateCvFindJobStatus()
        {
            var cvs = _context.EmployeeCvs.Where(a => a.FindJobStatus).ToList();
            foreach (var cv in cvs)
            {
                var days = (DateTime.Now - cv.StartFindJobDate).TotalDays;
                if (days <= 14 || cv.FindJobStatus == false)
                {
                    continue;
                }
                cv.FindJobStatus = false;
                cv.StartFindJobDate = DateTime.MinValue;
            }
            _context.EmployeeCvs.UpdateRange(cvs);
            _context.SaveChanges();
        }


    }
}