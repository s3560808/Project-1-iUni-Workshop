using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.InvatationModel;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.SchoolModels;
using iUni_Workshop.Models.SuburbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = Roles.Employee)]
    public class EmployeeController : Controller
    {
        //Property of employer controller
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        //Constructor of employer controller
        public EmployeeController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager
        ) {
            _userManager = userManager;
            _context = context;
        }
        
        //View of index page of employee
        [Route("[Controller]/Index")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var employee = _context.Employees.First(a => a.Id == user.Id);
            var employeeCvs = _context.EmployeeCvs.Where(a => a.EmployeeId == user.Id);
            var result = new Index {Name = employee.Name, BriefDescription = employee.ShortDescription};
            foreach (var cv in employeeCvs)
            {
                var indexCv = new IndexCv();
                var cvId = cv.Id;
                indexCv.Description = cv.Details;
                var cvFieldId = cv.FieldId;
                indexCv.FieldName = _context.Fields.First(a => a.Id == cvFieldId).Name;
                var cvSkillIds = _context.EmployeeSkills.Where(a => a.EmployeeCvId == cvId);
                foreach (var skillId in cvSkillIds)
                {
                    indexCv.SkillNames.Add(_context.Skills.First(a => a.Id == skillId.SkillId).Name);
                }
                result.Cvs.Add(indexCv);
            }
            return View(result);
        }
  
        //View of EditPersonalInfo
        [Route("[Controller]/EditPersonalInfo")]
        public async Task<IActionResult> EditPersonalInfo()
        {
            ProcessSystemInfo();
            var user = await _userManager.GetUserAsync(User);
            var employee = _context.Employees.First(a => a.Id == user.Id);
            var info = new EditPersonalInfo
            {
                Name = employee.Name,
                ContactEmail = employee.ContactEmail,
                PhoneNumber = employee.PhoneNumber,
                ShortDescription = employee.ShortDescription
            };
            if (employee.SchoolId != null)
            {
                var school = _context.Schools.First(a => a.Id == employee.SchoolId );
                info.SchoolName = school.SchoolName;
                var schoolSuburb = _context.Suburbs.First(a => a.Id == school.SuburbId);
                info.CampusName = schoolSuburb.Name;
                info.CampusPostCode = schoolSuburb.PostCode;
            }
            if (employee.SuburbId != null)
            {
                var suburb = _context.Suburbs.First(a => a.Id == employee.SuburbId);
                info.LivingDistrict = suburb.Name;
                info.PostCode = suburb.PostCode;
            }

            return View(info);
        }
        
        //Action of EditPersonalInfo
        public async Task<IActionResult> EditPersonalInfoAction(EditPersonalInfo personalInfo)
        {
            InitialSystemInfo();
            var user = await _userManager.GetUserAsync(User);
            //1. Validation
            //1.1. Validate user front-end input
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("EditPersonalInfo");
            }

            Suburb suburb = null;
            School school = null;
            //1.2. Validate living suburb
            try
            {
                suburb = _context
                    .Suburbs
                    .First(a => 
                        a.Name == personalInfo.LivingDistrict && 
                        a.PostCode == personalInfo.PostCode
                    );
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Please enter correct living area information!");
            }
            //1.2. Validate school
            try
            {
                var suburbId = _context.Suburbs.First(a =>
                    a.Name == personalInfo.CampusName.ToUpper() && a.PostCode == personalInfo.PostCode).Id;
                school = _context.Schools.First(a => a.NormalizedName == personalInfo.SchoolName.ToUpper() && 
                                                     a.SuburbId == suburbId &&
                                                     a.Status == SchoolStatus.InUse);
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Please enter correct school information!");
            }

            var employee = _context.Employees.First(a => a.Id == user.Id);
            //2. Update info
            //2.1 Update contact email
            if (employee.ContactEmail != personalInfo.ContactEmail)
            {
                try
                {
                    employee.ContactEmail = personalInfo.ContactEmail;
                    _context.Employees.Update(employee);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Your email successfully updated!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry, fail to update your contact email");
                }
            }
            //2.2 Update user name
            if (employee.Name != personalInfo.Name)
            {
                try
                {
                    employee.Name = personalInfo.Name;
                    _context.Employees.Update(employee);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Your name successfully updated!");
                    
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry, fail to update your name");
                }
            }
            //2.3 Update phone number
            if (employee.PhoneNumber != personalInfo.PhoneNumber)
            {
                try
                {
                    employee.PhoneNumber = personalInfo.PhoneNumber;
                    _context.Employees.Update(employee);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Your phone number successfully updated!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry, fail to update your phone number");
                }
            }
            //2.4 Update short description
            if (employee.ShortDescription != personalInfo.ShortDescription)
            {
                try
                {
                    employee.ShortDescription = personalInfo.ShortDescription;
                    _context.Employees.Update(employee);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Your description successfully updated!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry, fail to update your description");
                }
            }
            //2.6 Update suburb
            if (suburb != null)
            {
                try
                {
                    employee.SuburbId = suburb.Id;
                    _context.Employees.Update(employee);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Your suburb successfully updated!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry, fail to update your suburb");
                }
            }
            //2.7 Update school
            if (school != null)
            {
                try
                {
                    employee.SchoolId = school.Id;
                    _context.Employees.Update(employee);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Your school successfully updated!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry, fail to update your school");
                }
            }

            return RedirectToAction("EditPersonalInfo");
        }
        
        //View of MyCVs
        [Route("[Controller]/MyCVs")]
        public async Task<IActionResult> MyCVs()
        {
            ProcessSystemInfo();
            await UpdateCvFindJobStatus();
            var user = await _userManager.GetUserAsync(User);
            var cvs = _context.EmployeeCvs
                .Where(a => a.EmployeeId == user.Id)
                .Select(a => new MyCvs
                {
                    CvTitle = a.Title, 
                    CvId = a.Id, 
                    FindJobStatus = a.FindJobStatus, 
                    FieldId = a.FieldId
                });
            foreach (var cv in cvs)
            {
                var fieldName = _context.Fields.First(a => a.Id == cv.FieldId).Name;
                cv.FieldName = fieldName;
                
            }
            return View(cvs);
        }

        //View of EditCv
        [Route("[Controller]/EditCv/{CvId}")]
        [Route("[Controller]/EditCv/")]
        public async Task<IActionResult> EditCv(int cvId)
        {
            ProcessSystemInfo();
            InitialSystemInfo();
            var user = await _userManager.GetUserAsync(User);
            EditCV cv;
            //1. Get Cv with CV id
            try
            {
                //1.1 get exist cv
                cv = _context.EmployeeCvs
                    .Where(a => a.Id == cvId && a.EmployeeId == user.Id)
                    .Select(a => new EditCV{
                        CvId = a.Id, 
                        Details = a.Details, 
                        Email = user.Email, 
                        FieldId = a.FieldId,
                        FindJobStatus = a.FindJobStatus, 
                        MinSaraly = a.MinSaraly, 
                        StartFindJobDate = a.StartFindJobDate,
                        Primary = a.Primary, 
                        Title = a.Title}
                    ).First();
            }
            catch (InvalidOperationException)
            {
                //1.2 not user's cv
                if (cvId != 0)
                {
                    AddToTempDataError("Please select correct CV!");
                    return RedirectToAction("MyCVs");
                }
                //1.3 New cv
                else
                {
                    return View();
                }
                
            }
            //2. Get related cv info
            //2.1 Get cv field
            var fieldName = _context.Fields.First(a => a.Id == cv.FieldId).Name;
            //2.2 Get cv external materials
            cv.externalMaterials = _context.EmployeeExternalMeterials.Where(a => a.EmployeeCvId == cv.CvId).ToList();
            //2.3 Get cv job histories
            cv.JobHistories = _context.EmployeeJobHistories.Where(a => a.EmployeeCvId == cv.CvId).ToList();
            //2.4 Get cv skills
            cv.Skills = _context.EmployeeSkills.Where(a => a.EmployeeCvId == cv.CvId).Select(a => new EditCVEmployeeSkill{SkillId = a.SkillId, CertificationLink = a.CertificationLink}).ToList();
            //2.5 Get cv work day
            cv.WorkDays = _context.EmployeeWorkDays.Where(a => a.EmployeeCvId == cv.CvId).ToList();
            //2.6 Get cv field name
            cv.FieldName = fieldName;
            for (var i = 0; i < cv.Skills.Count; i++)
            {
                cv.Skills[i].SkillName = _context.Skills.First(a => a.Id == cv.Skills[i].SkillId).Name;
            }
            return View(cv);
        }

        //Action of EditCv
        public async Task<RedirectToActionResult> EditCvAction(EditCVAction cv, 
            IEnumerable<EditCVEmployeeSkill> skills, 
            IEnumerable<EditCVExternalMeterial> externalMaterials, 
            IEnumerable<EditCVJobHistory> jobHistories,
            IEnumerable<EditCvWorkDay> days)
        {
            InitialSystemInfo();
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToActionPermanent("EditCv","Employee",new {cv.CvId });
            }  
            var user = await _userManager.GetUserAsync(User);
            EmployeeCV newCv = null;
            int newFieldId;
            var newSkills = new List<EmployeeSkill>();
            //1. Validate if correct cv id
            if (cv.CvId != 0)
            {
                //Validate if is correct cv id
                try{
                    newCv = _context.EmployeeCvs.First(a => a.Id == cv.CvId);
                    //Validate if is current Employee's CV
                    if (newCv.EmployeeId != user.Id)
                    {
                        AddToTempDataError("Sorry. It is not your CV1!");
                        return RedirectToActionPermanent("MyCVs");
                    }
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry. It is not your CV2!");
                    return RedirectToAction("EditCv");
                }
            }  
            //2. Validate if correct field name
            try{
                //Get new CV field id
                newFieldId = _context.Fields.First(a => 
                    a.Status == FieldStatus.InUse && 
                    a.NormalizedName == cv.FieldName.ToUpper()
                    ).Id;
                
            }
            catch(InvalidOperationException)
            {
                AddToTempDataError("Sorry. Not a valid field!");
                return RedirectToAction("EditCv", new{cv.CvId});
            }
            //3. Validate skill name
            //!! No skill more than 10 skill
            var ndSkills = skills
                .Select(a => new{a.SkillName, a.CertificationLink})
                .Distinct()
                .ToList();
            foreach (var skill in ndSkills)
            {
                try
                {
                    var newSkillId = _context.Skills
                        .First(a => 
                            a.NormalizedName == skill.SkillName.ToUpper()&& 
                            a.Status == SkillStatus.InUse
                            )
                        .Id;
                    newSkills.Add(new EmployeeSkill{ CertificationLink = skill.CertificationLink, SkillId = newSkillId});
                }
                catch (InvalidCastException)
                {
                    AddToTempDataError("Sorry. "+ skill.SkillName + " is not a valid skill");
                }
            }
            if (!newSkills.Any() || newSkills.ToList().Count>10)
            {
                AddToTempDataError("Sorry. At least one skill at most ten skill!");
                return RedirectToAction("EditCv", new {cv.CvId});
            }
            //4. Process related info
            //4.1 Get new external materials
            var ndExternalMaterials = externalMaterials
                .Select(a => new {exLink = a.ExternalMaterialLink, exName = a.ExternalMaterialName})
                .Distinct()
                .ToList();
            var newExternalMaterials = ndExternalMaterials.Select(a => new EmployeeExternalMeterial {Name = a.exName, Link = a.exLink}).ToList();
            //4.2 GET JOB HISTORIES
            var ndJobHis = jobHistories
                    .Select(a => new{jhName = a.JobHistoryName, jhDescri = a.JobHistoryShortDescription})
                    .Distinct()
                    .ToList();
            var newJobHistories = ndJobHis.Select(jobHistory => new EmployeeJobHistory {Name = jobHistory.jhName, ShortDescription = jobHistory.jhDescri}).ToList();
            newJobHistories = newJobHistories.Distinct().ToList();
            //4.3 GET Days
            var ndDays = days
                .Select(a => new{a.Day})
                .Distinct()
                .ToList();
            var newDays = ndDays.Select(day => new EmployeeWorkDay {Day = day.Day}).ToList();
            newDays = newDays.Distinct().ToList();
            //4.4 if new cv, create new cv
            if (newCv == null)
            {
                if (newSkills.Any() && newFieldId != 0)
                {
                    newCv = new EmployeeCV
                    {
                        EmployeeId = user.Id,
                        Primary = cv.PrimaryCv,
                        FindJobStatus = cv.UpdateStatus,
                        StartFindJobDate = DateTime.Now,
                        Title = cv.Title,
                        Details = cv.Description,
                        MinSaraly = cv.MinSalary,
                        FieldId = newFieldId
                    };
                    try
                    {
                        
                        var otherCvs = _context.EmployeeCvs.Where(a => a.EmployeeId == user.Id);
                        if (cv.PrimaryCv)
                        {
                            foreach (var otherCv in otherCvs)
                            {
                                otherCv.Primary = false;
                            }
                            _context.EmployeeCvs.UpdateRange(otherCvs);
                        }
                        _context.EmployeeCvs.Update(newCv);
                        _context.SaveChanges();
                        AddToTempDataSuccess("New Cv Added!");
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Sorry. Failed to create a new cv!");
                        return RedirectToAction("EditCv");
                    }
                }
                else
                {
                    AddToTempDataError("Sorry. Failed to create a new cv!");
                    return RedirectToAction("EditCv");
                }
            }
            foreach (var newSkill in newSkills)
            {
                newSkill.EmployeeCvId = newCv.Id;
            }
            foreach (var newExternalMaterial in newExternalMaterials)
            {
                newExternalMaterial.EmployeeCvId = newCv.Id;
            }
            foreach (var newJobHistory in newJobHistories)
            {
                newJobHistory.EmployeeCvId = newCv.Id;
            }
            foreach (var day in newDays)
            {
                day.EmployeeCvId = newCv.Id;
            }
            //5. Update new cv 
            //5.1 Update primary status
            if (newCv.Primary != cv.PrimaryCv)
             {
                 try
                 {
                     var otherCvs = _context.EmployeeCvs.Where(a => a.EmployeeId == user.Id);
                     if (cv.PrimaryCv)
                     {
                         foreach (var otherCv in otherCvs)
                         {
                             otherCv.Primary = false;
                         }
                     }
                     newCv.Primary = cv.PrimaryCv;
                     _context.EmployeeCvs.UpdateRange(otherCvs);
                     _context.EmployeeCvs.Update(newCv);
                     _context.SaveChanges();
                     AddToTempDataSuccess("Primary Cv status changed.");
                 }
                 catch (InvalidOperationException)
                 {
                     AddToTempDataError("Sorry. Failed to update primary cv status!");
                 }                 
             }
            //5.2 Update find job status
            if (cv.UpdateStatus != newCv.FindJobStatus)
            {
                if (cv.UpdateStatus)
                {
                    try
                    {
                        newCv.StartFindJobDate = DateTime.Now;
                        newCv.FindJobStatus = true;
                        _context.EmployeeCvs.Update(newCv);
                        _context.SaveChanges();
                        AddToTempDataSuccess("Start to find job!");
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Sorry. Failed to update find job status");
                    }
                }
                else
                {
                    try
                    {
                        newCv.FindJobStatus = false;
                        newCv.StartFindJobDate = DateTime.MinValue;
                        _context.EmployeeCvs.Update(newCv);
                        _context.SaveChanges();
                        AddToTempDataSuccess("Stop to find job!");
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Sorry. Failed to update find job status");
                    }
                }
            }
            //5.3 Update title
            if (newCv.Title != cv.Title)
            {
                try
                {
                    newCv.Title = cv.Title;
                    _context.EmployeeCvs.Update(newCv);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Curriculum Vitae title changed!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry. Failed to update title");
                }
            }
            //5.4 Update description
            if (newCv.Details != cv.Description)
            {
                try
                {
                    newCv.Details = cv.Description;
                    _context.EmployeeCvs.Update(newCv);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Curriculum Vitae description changed!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry. Failed to update description");
                }
            }
            //5.5 Update min salary
            if (!Math.Abs(newCv.MinSaraly - cv.MinSalary).Equals(0))
            {
                try
                {
                    newCv.MinSaraly = cv.MinSalary;
                    _context.EmployeeCvs.Update(newCv);
                    _context.SaveChanges();
                    AddToTempDataSuccess("Curriculum Vitae min salary changed!");
                }
                catch (InvalidOperationException)
                {
                    AddToTempDataError("Sorry. Failed to update min salary");
                }
            }
            //5.6 Update cv field
            try
            {
                var oldFieldId = _context.Fields.First(a => 
                    a.Id == newCv.FieldId
                ).Id;
                //Check if new field
                if (newFieldId != oldFieldId)
                {
                    //Update existed Curriculum Vitae field
                    try
                    {
                        newCv.FieldId = newFieldId;
                        _context.EmployeeCvs.Update(newCv);
                        _context.SaveChanges();
                        AddToTempDataSuccess("Curriculum Vitae field updated!");
                    }
                    catch (InvalidCastException)
                    {
                        AddToTempDataError("Sorry. Failed to change field! Please retry!");
                    } 
                }
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Sorry. Failed to change field! Please retry!");
            }
            //5.7 Update skills  
            try
            {
                var oldSkills = _context.EmployeeSkills.Where(a => a.EmployeeCvId == newCv.Id).ToList();
                var newEnumerable = newSkills.Select(a => new {Link = a.CertificationLink, Id = a.SkillId}).OrderBy(a => a.Id).ToList();
                var oldEnumerable = oldSkills.Select(a => new {Link = a.CertificationLink, Id = a.SkillId}).OrderBy(a => a.Id).ToList();
                if (!newEnumerable.SequenceEqual(oldEnumerable) && newEnumerable.Count>0)
                {
                    try
                    {
                        _context.EmployeeSkills.RemoveRange(oldSkills);
                        _context.EmployeeSkills.AddRange(newSkills);
                        _context.SaveChanges();
                        AddToTempDataSuccess("cv skills updated!");
                    }
                    catch (InvalidCastException)
                    {
                        AddToTempDataError("Sorry. Failed to update your skills please retry");
                    }
                }
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Sorry. Failed to update your skills please retry");
            }    
            //5.8 Update external materials
            try
            {
                var cvId = newCv.Id;
                var oldExternalMaterials = _context.EmployeeExternalMeterials.Where(a => a.EmployeeCvId == cvId);
                var newEnumerable = newExternalMaterials.Select(a => new {a.Name, a.Link}).OrderBy(a => a.Name).ToList();
                var oldEnumerable = oldExternalMaterials.Select(a => new {a.Name, a.Link}).OrderBy(a => a.Name).ToList();
                if (!newEnumerable.SequenceEqual(oldEnumerable) && newEnumerable.Count>0)
                {
                    try
                    {
                        _context.EmployeeExternalMeterials.RemoveRange(oldExternalMaterials);  
                                                                                              
                            _context.EmployeeExternalMeterials.AddRange(newExternalMaterials);  
                        
                        _context.SaveChanges();                                                  
                        AddToTempDataSuccess("CV external material updated!");                 
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Sorry. Failed to update your external material please retry");
                    }
                }
                
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Sorry. Failed to update your external material please retry");
            }     
            //5.9 Update Job histories
            try
            {
                var cvId = newCv.Id;
                var oldJobHistories = _context.EmployeeJobHistories.Where(a => a.EmployeeCvId == cvId);
                var newEnumerable = newJobHistories.Select(a => new {a.Name, Link = a.ShortDescription}).OrderBy(a => a.Name).ToList();
                var oldEnumerable = oldJobHistories.Select(a => new {a.Name, Link = a.ShortDescription}).OrderBy(a => a.Name).ToList();
                if (!newEnumerable.SequenceEqual(oldEnumerable) && newEnumerable.Count>0)
                {
                    try
                    {
                        _context.EmployeeJobHistories.RemoveRange(oldJobHistories);
                        
                            _context.EmployeeJobHistories.AddRange(newJobHistories);
                        
                        _context.SaveChanges();
                        AddToTempDataSuccess("CV job histories updated!");
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Sorry. Failed to update your job histories please retry");
                    }
                }
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Sorry. Failed to update your job histories please retry");
            }
            //5.10 Update Days
            try
            {
                    
                var cvId = newCv.Id;
                var oldDays = _context.EmployeeWorkDays.Where(a => a.EmployeeCvId == cvId);
                var newEnumerable = newDays.Select(a => new {a.Day}).OrderBy(a => a.Day).ToList();
                var oldEnumerable = oldDays.Select(a => new {a.Day}).OrderBy(a => a.Day).ToList();
                if (!newEnumerable.SequenceEqual(oldEnumerable) && newEnumerable.Count>0)
                {
                    try
                    {
                        _context.EmployeeWorkDays.RemoveRange(oldDays);
                            _context.EmployeeWorkDays.AddRange(newDays);
                        _context.SaveChanges();
                        AddToTempDataSuccess("CV work day updated!");
                    }
                    catch (InvalidOperationException)
                    {
                        AddToTempDataError("Sorry. Failed to update your work day please retry");
                    }
                }   
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Sorry. Failed to update your work day please retry");
            }

            return RedirectToAction("EditCv", new {CvId = newCv.Id});
        }
        
        //View of MyInvitations
        [Route("[Controller]/MyInvitations/")]
        public async Task<IActionResult> MyInvitations()
        {
            ProcessSystemInfo();
            var user = await _userManager.GetUserAsync(User);
            var invitations = _context.Invatations.Where(a => a.EmployeeCV.EmployeeId == user.Id).AsEnumerable();
            var results = new List<MyInvitations>();
            results.AddRange(
                invitations.Select(
                    invitation => new MyInvitations
                    {
                        Id = invitation.Id, 
                        EmployeeCvTitle = _context.EmployeeCvs.First(a => a.Id == invitation.EmployeeCvId).Title, 
                        EmployerJobProfileTitle = _context.EmployerJobProfiles
                            .First(a => a.Id == invitation.EmployerJobProfileId).Title
                    }
                )
            );
            return View(results.AsEnumerable());
        }

        //View of InvitationDetail
        [Route("[Controller]/InvitationDetail/{invitationId}")]
        public async Task<IActionResult> InvitationDetail(int invitationId)
        {
            ProcessSystemInfo();
            Invatation invitation;
            var user = await _userManager.GetUserAsync(User);
            try
            {
                invitation = _context.Invatations.First(a => a.Id == invitationId);
                var employeeId = _context.EmployeeCvs.First(a => a.Id == invitation.EmployeeCvId).EmployeeId;
                if (user.Id != employeeId)
                {
                    TempData["Error"] = "Please select correct invitation";
                    return RedirectToAction("MyInvitations");
                }
            }
            catch (InvalidOperationException)
            {
                TempData["Error"] = "Please select correct invitation";
                return RedirectToAction("MyInvitations");
            }
            var jobProfile = _context.EmployerJobProfiles.First(a => a.Id == invitation.EmployerJobProfileId);
            var employer = _context.Employers.First(a => a.Id == jobProfile.EmployerId);
            var result = new InvitationDetail
            {
                SentDate = invitation.SentTime, 
                CompanyDescription = employer.BriefDescription, 
                JobDescription = jobProfile.Description,
                Title = jobProfile.Title,
                InvitationId = invitation.Id
            };
            
            return View(result);
        }

        //Accept or Reject invitation
        public async Task<IActionResult> AcceptOrReject(AcceptOrReject model)
        {
            var user = await _userManager.GetUserAsync(User);
            InitialSystemInfo();
            if (!ModelState.IsValid)
            {
                ProcessModelState();
            }
            try
            {
                var invitation = _context.Invatations.First(a => a.Id == model.InvitationId);
                var employeeId = _context.EmployeeCvs.First(a => a.Id == invitation.EmployeeCvId).EmployeeId;
                if (employeeId != user.Id)
                {
                    TempData["Error"] = "Please select correct invitation";
                    return RedirectToAction("MyInvitations");
                }

                if (invitation.status != InvitationStatus.Original)
                {
                    TempData["Error"] = "Cannot change status";
                    return RedirectToAction("InvitationDetail", new{invitationId = invitation.Id});
                }

                invitation.status = model.Accept ? InvitationStatus.Accepted : InvitationStatus.Rejected;
                _context.Invatations.Update(invitation);
                _context.SaveChanges();
                var status = model.Accept ? "accepted" : "rejected";

                TempData["Success"] = "Invitation successfully "+status;
                return RedirectToAction("InvitationDetail", new { invitationId = invitation.Id});
            }
            catch (InvalidOperationException)
            {
                TempData["Error"] = "Please select correct invitation";
                return RedirectToAction("MyInvitations");
            }
        }
        
        //Update old job id
        private async Task UpdateCvFindJobStatus()
        {
            var user = await _userManager.GetUserAsync(User);
            var cvs = _context.EmployeeCvs.Where(a => a.EmployeeId == user.Id);
            foreach (var cv in cvs)
            {
                if ((DateTime.Now - cv.StartFindJobDate).TotalDays <= 14||cv.FindJobStatus==false) 
                    continue;
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