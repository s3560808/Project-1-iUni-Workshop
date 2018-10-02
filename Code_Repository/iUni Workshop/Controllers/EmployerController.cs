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


namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = Roles.Employer)]
    public class EmployerController : Controller
    {
        //Property of employer controller
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        //Constructor of employer controller
        public EmployerController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _context = context;
        }

        //View of index page of employer
        [Route("[Controller]/Index")]
        public IActionResult Index()
        {
            return View();
        }

        //View of edit company info
        [Route("[Controller]/EditCompanyInfo")]
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

        //Edit company inforation
        public async Task<IActionResult> EditCompanyInfoAction(EditCompanyInfo info)
        {
            InitialSystemInfo();
            //1. Validate front-end user input
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("EditCompanyInfo");
            }
            var user = await _userManager.GetUserAsync(User);
            var data = _context.Employers.First(a => a.Id == user.Id);
            //2. Update suburb
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
                        AddToTempDataSuccess("Your suburb updated!");
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Suburb failed to update, please retry.");
                    }
                }
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Please enter correct suburb.");
            }
            //3. Update address
            if (data.Location != info.Address)
            {
                try
                {
                    data.Location = info.Address;
                    _context.Employers.Update(data);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Your address updated!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Address failed to update, please retry.");
                }
            }
            //4. Update brief description
            if (data.BriefDescription != info.BriefDescription)
            {
                try
                {
                    data.BriefDescription = info.BriefDescription;
                    _context.Employers.Update(data);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Your brief description updated!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Please enter correct brief description.");
                }
            }
            //5. Update contact email
            if (data.ContactEmail != info.ContactEmail)
            {
                try
                {
                    data.ContactEmail = info.ContactEmail;
                    _context.Employers.Update(data);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Your contact email updated!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Please enter correct contact email.");
                }
            }
            //5. Update contact email
            if (data.PhoneNumber != info.PhoneNumber)
            {
                try
                {
                    data.PhoneNumber = info.PhoneNumber;
                    _context.Employers.Update(data);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Your phone number updated!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Please enter correct phone number.");
                }
            }
            //If certificated ABN & company cannot be changed
            if (!data.Certificated)
            {
                //6. Update ABN
                if (data.ABN != info.ABN)
                {
                    try
                    {
                        data.ABN = info.ABN;
                        _context.Employers.Update(data);
                        _context.SaveChanges();
                        AddToTempDataSuccess("Your ABN updated!");
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Please enter correct ABN.");
                    }
                }
                //7. Update company name
                if (data.Name == info.Name) return RedirectToAction("EditCompanyInfo");
                try
                {
                    data.Name = info.Name;
                    _context.Employers.Update(data);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Your company updated!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Please enter correct company name.");
                }
            }
            else
            {
                if (data.ABN != info.ABN)
                {
                    AddToTempDataInform("You are certificated. Your name Cannot updated");
                }
                if (data.Name != info.Name)
                {
                    AddToTempDataInform("You are certificated. Your name Cannot updated");
                }
            }
            return RedirectToAction("EditCompanyInfo");
        }

        //View of RequestToCertificateMyCompany 
        [Route("[Controller]/RequestToCertificateMyCompany")]
        public async Task<IActionResult> RequestToCertificateMyCompany(){
            ProcessSystemInfo();
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
                if ((string) ViewBag.Inform != "")
                {
                    ViewBag.Inform += "\n";
                }
                ViewBag.Inform += "You already certificated.";
            }
            if (rawInfo.RequestCertification)
            {
                if ((string) ViewBag.Inform != "")
                {
                    ViewBag.Inform += "\n";
                }
                ViewBag.Inform += "You already requested certification.";
            }
            if (rawInfo.ABN==""||rawInfo.Name==""){
                if ((string) ViewBag.Inform != "")
                {
                    ViewBag.Inform += "\n";
                }
                ViewBag.Inform += "You need update your company's ABN and Name first.";
            }
            return View(rawInfo);
        }

        //Action of RequestToCertificateMyCompany 
        public async Task<IActionResult> RequestToCertificateMyCompanyAction(){
            var user = await _userManager.GetUserAsync(User);
            var employer = _context.Employers.First(a => a.Id == user.Id);
            if (employer.Certificated)
            {
                AddToTempDataInform("Your company already certificated.");
                return RedirectToAction("RequestToCertificateMyCompany");
            }
            if (employer.RequestCertification)
            {
                AddToTempDataInform("Your company already requested to certificate company.\n Please wait.");
                return RedirectToAction("RequestToCertificateMyCompany");
            }
            if (employer.ABN == "" || employer.Name == "")
            {
                AddToTempDataInform("Please update ABN & company name first");
                return RedirectToAction("RequestToCertificateMyCompany");
            }
            employer.RequestCertification = true;
            _context.Employers.Update(employer);
            _context.SaveChanges();
            AddToTempDataSuccess("Request to certificate company sent!");
            return RedirectToAction("RequestToCertificateMyCompany");
        }

        //View of RequestToUnCertificateMyCompany 
        [Route("[Controller]/RequestToUnCertificateMyCompany")]
        public IActionResult RequestToUnCertificateMyCompany() 
        {
            ProcessSystemInfo();
            return View();
        }

        //Action of RequestToUnCertificateMyCompany 
        public async Task<IActionResult> RequestToUnCertificateMyCompanyAction(){
            var user = await _userManager.GetUserAsync(User);
            var employer = _context.Employers.First(a => a.Id == user.Id);
            employer.RequestCertification = false;
            employer.Certificated = false;
            _context.Employers.Update(employer);
            _context.SaveChanges();
            AddToTempDataSuccess("Your company changed to not certificated status!");
            return RedirectToAction("RequestToUnCertificateMyCompany");
        }

        //View of Job Profiles
        [Route("[Controller]/JobProfiles")]
        public async Task<IActionResult> JobProfiles()
        {
            var user = await _userManager.GetUserAsync(User);
            var list = _context.EmployerJobProfiles
                .Where(a => a.EmployerId == user.Id)
                .Select(a => new JobProfiles
                {
                    Title = a.Title, 
                    Id = a.Id, 
                    Field = a.Field.Name, 
                    LastUpdateTime = a.LastUpdateDateTime
                })
                .ToList();
            return View(list);
        }

        //View of EditJobProfile
        [Route("[Controller]/EditJobProfile/{jobProfileId}")]
        [Route("[Controller]/EditJobProfile/")]
        public async Task<IActionResult> EditJobProfile(int jobProfileId)
        {
            ProcessSystemInfo();
            InitialSystemInfo();
            var user = await _userManager.GetUserAsync(User);
            JobProfile jobProfile;
            //1.1. Get specific user profile
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
            //1.2. New user profile
            catch (InvalidOperationException)
            {
                return View();
            }
            //2. Get profile's skills 
            jobProfile.JobProfileSkills = _context.EmployerSkills
                .Where(a => a.EmployerJobProfileId == jobProfile.ProfileId)
                .Select(a => new JobProfileSkill{ SkillName = a.Skill.Name,SkillRequired = a.Required})
                .ToList();
            //3. Get profile's compulsory work days 
            jobProfile.JobProfileComplusoryWorkDays = _context.EmployerComplusoryWorkDays
                .Where(a => a.EmployerJobProfileId == jobProfile.ProfileId)
                .Select(a => new JobProfileCompulsoryWorkDay {Day = a.Day})
                .ToList();
            //4. Get profile's required location 
            jobProfile.JobProfileRequiredLocation = _context.EmployerWorkLocations
                .Where(a => a.EmployerJobProfileId == jobProfile.ProfileId)
                .Select( a=> new JobProfileRequiredLocation{ LocationName = a.Suburb.Name, PostCode = a.Suburb.PostCode})
                .ToList();
            //4. Get profile's required schools 
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
            IEnumerable<JobProfileCompulsoryWorkDay> jobProfileCompulsoryWorkDay,
            IEnumerable<JobProfileRequiredLocation> jobProfileRequiredLocation,
            IEnumerable<JobProfileRequiredSchool> jobProfileRequiredSchool
        ){
            InitialSystemInfo();
            //0. Validate front-end page
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
                //1.1. Get correct user's job profile
                try
                {
                    newJobProfile = _context
                        .EmployerJobProfiles
                        .First(a => a.Id == jobProfile.ProfileId && a.EmployerId == user.Id);
                }
                catch(InvalidOperationException) {
                    AddToTempDataError("Sorry. It is not your job profile!");
                    //If it is not user's job profile, redirect to job profiles
                    return RedirectToAction("JobProfiles");
                }
                //1.2. Get correct user's invitation
                try
                {
                    var profile = newJobProfile;
                    var invitation = _context.Invatations.Where(a => a.EmployerJobProfileId == profile.Id);
                    if (invitation.Any())
                    {
                        AddToTempDataError("Sorry. Your job profile already has invitation. Cannot be changed!");
                        return RedirectToAction("EditJobProfile", new{ JobProfileId = jobProfile.ProfileId});
                    }
                }
                catch (InvalidOperationException)
                {
                }
            }
            //2. Validate if correct field name
            try{
                //2.1 Get new job profile field id
                newFieldId = _context.Fields.First(a => 
                    a.Status == FieldStatus.InUse && 
                    a.NormalizedName == jobProfile.FieldName.ToUpper()
                ).Id;
            }
            catch(InvalidOperationException)
            {
                //2.3 Not valid filed
                AddToTempDataError("Sorry. Not a valid field!");
                return RedirectToAction("EditJobProfile", new{JobProfileId = jobProfile.ProfileId});
            }
            //3. Get related information
            //3.1 Validate skill name & Get skill id list
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
                    AddToTempDataError("Sorry. "+ skill.SkillName + " is not a valid skill");
                }
            }
            if (!newSkills.Any() || newSkills.ToList().Count>10)
            {
                AddToTempDataError("Sorry. At least one skill at most ten skill!");
                return RedirectToAction("EditJobProfile", new {JobProfileId = jobProfile.ProfileId});
            }
            //3.2 GET NEW Days
            var newDays = jobProfileCompulsoryWorkDay
                .Select(day => new EmployerComplusoryWorkDay{Day = day.Day})
                .Distinct()
                .ToList();
            //3.3 GET required Locations
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
            //3.4 GET required Schools
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
                    AddToTempDataError("Sorry. "+ school.SchoolName +" "+ school.CampusName +" "+ school.CampusPostCode + " is not a valid school");
                }
            }
            newRequiredSchools = newRequiredSchools.Distinct().ToList();
            //4. if new cv, create new cv
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
                        AddToTempDataSuccess("New job profile Added!");
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Sorry. Failed to create a new job profile!");
                        return RedirectToAction("EditJobProfile");
                    }
                }
                else
                {
                    AddToTempDataError("Sorry. Failed to create a new cv!");
                    return RedirectToAction("EditJobProfile");
                }
            }

            //5. Set job profile id
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

            //6.Update info
            //6.1 Update Require Job Experience
            if (newJobProfile.RequireJobExperience !=jobProfile.RequireJobExperience)
            {
                try
                {
                    newJobProfile.RequireJobExperience =jobProfile.RequireJobExperience;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Job Profile's \"Required Job Experience\" changed!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry. Failed to update \"Required Job Experience\"");
                }
            }
            //6.2 Update job profile title
            if (newJobProfile.Title !=jobProfile.Title)
            {
                try
                {
                    newJobProfile.Title =jobProfile.Title;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Job Profile's \"Title\" changed!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry. Failed to update \"Title\"");
                }
            }
            //6.2 Update job profile description
            if (newJobProfile.Description !=jobProfile.Description)
            {
                try
                {
                    newJobProfile.Description =jobProfile.Description;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Job Profile's \"Description\" changed!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry. Failed to update \"Description\"");
                }
            }
            
            //6.3 Update job profile salary
            if (!Math.Abs(newJobProfile.Salary - jobProfile.Salary).Equals(0))
            {
                try
                {
                    newJobProfile.Salary =jobProfile.Salary;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Job Profile's \"Salary\" changed!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry. Failed to update \"Salary\"");
                }
            }
            
            //6.4 Update job profile max day
            if (newJobProfile.MaxDayForAWeek !=jobProfile.MaxDay)
            {
                try
                {
                    newJobProfile.MaxDayForAWeek =jobProfile.MaxDay;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Job Profile's \"Max Day For A Week\" changed!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry. Failed to update \"Max Day For A Week\"");
                }
            }
            
            //6.5 Update job profile min day
            if (newJobProfile.MinDayForAWeek !=jobProfile.MinDay)
            {
                try
                {
                    newJobProfile.MinDayForAWeek =jobProfile.MinDay;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Job Profile's \"Min Day For A Week\" changed!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry. Failed to update \"Min Day For A Week\"");
                }
            }
            //6.6 Update job profile field
            if (newJobProfile.FieldId != newFieldId)
            {
                try
                {
                    newJobProfile.MaxDayForAWeek =jobProfile.MaxDay;
                    _context.EmployerJobProfiles.Update(newJobProfile);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Job Profile's \"Field\" changed!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry. Failed to update \"Field\"");
                }
            }
            //6.7 Update job skills
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
                        AddToTempDataSuccess("Job Profile's skills updated!");
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Sorry. Failed to update your skills please retry");
                    }
                }
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Sorry. Failed to update your skills please retry");
            } 
            //6.7 Update external materials
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
                        AddToTempDataSuccess("Job Profile's compulsory work day updated!");                 
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Sorry. Failed to update your compulsory work day please retry");
                    }
                }
                
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Sorry. Failed to update your compulsory work day please retry");
            }  
            //6.8 Update compulsory work location
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
                        AddToTempDataSuccess("Job Profile's compulsory work location updated!");                 
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Sorry. Failed to update your compulsory work location please retry");
                    }
                }
                
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Sorry. Failed to update your compulsory please retry");
            }  
            //6.9 Update required school
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
                        AddToTempDataSuccess("Job Profile's compulsory employee's school updated!");                 
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Sorry. Failed to update your compulsory employee's school please retry");
                    }
                }
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Sorry. Failed to update your compulsory employee's school please retry");
            }  
            return RedirectToAction("EditJobProfile", new{ JobProfileId = newJobProfile.Id});
        }

        //TODO Filter primary id
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
            //1. Not valid jobProfileId & CvId
            if (!validationResultList.Any())
            {
                AddToTempDataError("Wrong CV Request");
                return RedirectToAction("Index");
            }
            var raw = _context.EmployeeCvs.First(a => a.Id == validationResultList.First().CvId);
            //2. Get cv info
            var result = new ViewApplicantCv
            {
                Title = raw.Title, 
                Description = raw.Details, 
                EmployeeName = raw.Employee.Name
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
                return RedirectToAction("SearchApplicants", new {jobProfileId = jobProfileId});
            }
            try
            {
                _context.Invatations.Update(new Invatation
                {
                    SentTime = DateTime.Now, status = InvitationStatus.Original, 
                    EmployeeCvId = cvId,
                    EmployerJobProfileId = jobProfileId
                });
                _context.SaveChanges();
                if ((string) ViewBag.Sucess != "")
                {
                    ViewBag.Sucess  += "\n";
                }       
                ViewBag.Sucess += "Invitation Sent!";
            }
            catch (InvalidOperationException)
            {
                if ((string) ViewBag.Error != "")
                {
                    ViewBag.Error += "\n";
                }       
                ViewBag.Error += "Invitation sent failed";
            }
            return View();
        }
        
        public List<SearchApplicant> SearchApplicantsCoreRanker(int jobProfileId)
        {

            EmployerJobProfile jobProfile;
            var finalData = new List<EmployeeCV>();
            try
            {
                jobProfile = _context.EmployerJobProfiles.First(a => a.Id == jobProfileId);
            }
            catch(InvalidOperationException)
            {
                AddToTempDataError("It is not your job profile");
                return new List<SearchApplicant>();
            }
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

        [Route("[Controller]/Invitations")]
        public async Task<IActionResult> Invitations()
        {
            var user = await _userManager.GetUserAsync(User);
            var results = _context.Invatations
                .Where(a => a.EmployerJobProfile.EmployerId == user.Id)
                .Select(a => new EmployerInvitation{ InvitationId = a.Id, Status = a.status});
            return View(results);
        }

        [Route("[Controller]/InvitationDetail/{invitationId}")]
        public async Task<IActionResult> InvitationDetail(int invitationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var invitation =
                _context.Invatations.Where(a => a.Id == invitationId && a.EmployerJobProfile.EmployerId == user.Id);
            if (!invitation.Any())
            {
                AddToTempDataError("Invalid invitation id.");
                RedirectToAction("Invitations");
            }
            return View(invitation);
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