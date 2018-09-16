using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.SchoolModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MessageDetail = iUni_Workshop.Models.EmployeeModels.MessageDetail;
using MyMessages = iUni_Workshop.Models.EmployeeModels.MyMessages;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = Roles.Employee)]
    public class EmployeeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EmployeeController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager
        ) {
            _userManager = userManager;
            _context = context;
        }
        
        // Index of page
        public IActionResult Index()
        {
            return View();
        }
  
        //
        public async Task<IActionResult> EditPersonalInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            var employee = _context.Employees.First(a => a.Id == user.Id);
            return View(employee);
        }
        
        public async Task<IActionResult> EditPersonalInfoAction(EditPersonalInfo personalInfo)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
            }

            var suburb = _context.Suburbs.First(a => a.Name == personalInfo.LivingDistrict && a.PostCode == personalInfo.PostCode);
            if (suburb == null)
            {
            }

            var employee = _context.Employees.First(a => a.Id == user.Id);
            employee.ContactEmail = personalInfo.ContactEmail;
            employee.Name = personalInfo.Name;
            employee.PhoneNumber = personalInfo.PhoneNumber;
            employee.ShortDescription = personalInfo.ShortDescription;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        
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

        [Route("[Controller]/EditCv/{CvId}")]
        [Route("[Controller]/EditCv/")]
        public async Task<IActionResult> EditCV(int CvId)
        {
            ProcessSystemInfo();
            InitialSystemInfo();
            var user = await _userManager.GetUserAsync(User);
            EditCV cv;
            //Get Cv with CV id
            try
            {
                cv = _context.EmployeeCvs
                    .Where(a => a.Id == CvId && a.EmployeeId == user.Id)
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
                //Validate CV id & if user's CV
                if (CvId != 0)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                    TempData["Error"] += "Please select correct CV!";
                    return RedirectToAction("MyCVs");
                }
                else
                {
                    return View();
                }
                
            }
            
            var fieldName = _context.Fields.First(a => a.Id == cv.FieldId).Name;
            cv.externalMaterials = _context.EmployeeExternalMeterials.Where(a => a.EmployeeCvId == cv.CvId).ToList();
            cv.JobHistories = _context.EmployeeJobHistories.Where(a => a.EmployeeCvId == cv.CvId).ToList();
            cv.Skills = _context.EmployeeSkills.Where(a => a.EmployeeCvId == cv.CvId).Select(a => new EditCVEmployeeSkill{SkillId = a.SkillId, CertificationLink = a.CertificationLink}).ToList();
            cv.WorkDays = _context.EmployeeWorkDays.Where(a => a.EmployeeCvId == cv.CvId).ToList();
            cv.FieldName = fieldName;
            for (var i = 0; i < cv.Skills.Count; i++)
            {
                cv.Skills[i].SkillName = _context.Skills.First(a => a.Id == cv.Skills[i].SkillId).Name;
            }
            return View(cv);
        }

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
                return RedirectToActionPermanent("EditCV","Employee",new { CvId = cv.CvId });
            }  
            var user = await _userManager.GetUserAsync(User);
            EmployeeCV newCv = null;
            var newFieldId = 0;
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
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }
                        
                        TempData["Error"] += "Sorry. It is not your CV1!";
                        return RedirectToActionPermanent("MyCVs");
                    }

                   

                }
                catch (InvalidOperationException)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                        
                    TempData["Error"] += "Sorry. It is not your CV2!";
                    return RedirectToAction("EditCV");
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
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }       
                TempData["Error"] += "Sorry. Not a valid field!";
                return RedirectToAction("EditCV", new{CvId = cv.CvId});
            }
            //3. Validate skill name
            //!! No skill more than 10 skill
            foreach (var skill in skills)
            {
                try
                {
                    var newSkillId = _context.Skills
                        .First(a => 
                            a.NormalizedName == skill.SkillName.ToUpper())
                        .Id;
                    newSkills.Add(new EmployeeSkill{ CertificationLink = skill.CertificationLink, SkillId = newSkillId});
                }
                catch (InvalidCastException)
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
                return RedirectToAction("EditCV", new {CvId = cv.CvId});
            }
            //Get new external materials
            var newExternalMaterials = externalMaterials.Select(externalMaterial => new EmployeeExternalMeterial {Name = externalMaterial.ExternalMaterialName, Link = externalMaterial.ExternalMaterialLink}).ToList();
            //GET NEW JOB HISTORIES
            var newJobHistories = jobHistories.Select(jobHistory => new EmployeeJobHistory {Name = jobHistory.JobHistoryName, ShortDescription = jobHistory.JobHistoryShortDescription}).ToList();
            //GET NEW Days
            var newDays = days.Select(day => new EmployeeWorkDay {Day = day.Day}).ToList();
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
                        _context.EmployeeCvs.Update(newCv);
                        _context.SaveChanges();
                        if ((string) TempData["Success"] != "")
                        {
                            TempData["Success"] += "\n";
                        }       
                        TempData["Success"] += "New Cv Added!";
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }       
                        TempData["Error"] += "Sorry. Failed to create a new cv!";
                        return RedirectToAction("EditCV");
                    }
                }
                else
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }       
                    TempData["Error"] += "Sorry. Failed to create a new cv!";
                    return RedirectToAction("EditCV");
                }
            }
            //TODO for
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
             if (newCv.Primary != cv.PrimaryCv)
                    {
                        try
                        {
                            var otherCvs = _context.EmployeeCvs.Where(a => a.EmployeeId == user.Id);
                            if (cv.PrimaryCv == true)
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
                            if ((string) TempData["Success"] != "")
                            {
                                TempData["Success"] += "\n";
                            }
                            TempData["Success"] += "Primary Cv status changed.";
                        }
                        catch (InvalidOperationException)
                        {
                            if ((string) TempData["Error"] != "")
                            {
                                TempData["Error"] += "\n";
                            }
                        
                            TempData["Error"] += "Sorry. Failed to update primary cv status!";
                        }
                        
                    }

            if (cv.UpdateStatus != newCv.FindJobStatus)
            {
                if (cv.UpdateStatus == true)
                    {
                        try
                        {
                            newCv.StartFindJobDate = DateTime.Now;
                            newCv.FindJobStatus = true;
                            _context.EmployeeCvs.Update(newCv);
                            _context.SaveChanges();
                            if ((string) TempData["Success"] != "")
                            {
                                TempData["Success"] += "\n";
                            }
                            TempData["Success"] += "Start to find job!";
                        }
                        catch (InvalidOperationException)
                        {
                            if ((string) TempData["Error"] != "")
                            {
                                TempData["Error"] += "\n";
                            }
                        
                            TempData["Error"] += "Sorry. Failed to update find job status";
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
                            if ((string) TempData["Success"] != "")
                            {
                                TempData["Success"] += "\n";
                            }
                            TempData["Success"] += "Stop to find job!";
                        }
                        catch (InvalidOperationException)
                        {
                            if ((string) TempData["Error"] != "")
                            {
                                TempData["Error"] += "\n";
                            }
                        
                            TempData["Error"] += "Sorry. Failed to update find job status";
                        }
                    }
            }

            

                    if (newCv.Title != cv.Title)
                    {
                        try
                        {
                            newCv.Title = cv.Title;
                            _context.EmployeeCvs.Update(newCv);
                            _context.SaveChanges();
                            if ((string) TempData["Success"] != "")
                            {
                                TempData["Success"] += "\n";
                            }
                            TempData["Success"] += "CV's title changed!";
                        }
                        catch (InvalidOperationException)
                        {
                            if ((string) TempData["Error"] != "")
                            {
                                TempData["Error"] += "\n";
                            }
                        
                            TempData["Error"] += "Sorry. Failed to update title";
                        }
                    }

                    if (newCv.Details != cv.Description)
                    {
                        try
                        {
                            newCv.Details = cv.Description;
                            _context.EmployeeCvs.Update(newCv);
                            _context.SaveChanges();
                            if ((string) TempData["Success"] != "")
                            {
                                TempData["Success"] += "\n";
                            }
                            TempData["Success"] += "CV's description changed!";
                        }
                        catch (InvalidOperationException)
                        {
                            if ((string) TempData["Error"] != "")
                            {
                                TempData["Error"] += "\n";
                            }
                        
                            TempData["Error"] += "Sorry. Failed to update description";
                        }
                    }

                    if (!Math.Abs(newCv.MinSaraly - cv.MinSalary).Equals(0))
                    {
                        try
                        {
                            newCv.MinSaraly = cv.MinSalary;
                            _context.EmployeeCvs.Update(newCv);
                            _context.SaveChanges();
                            if ((string) TempData["Success"] != "")
                            {
                                TempData["Success"] += "\n";
                            }
                            TempData["Success"] += "CV's min salary changed!";
                        }
                        catch (InvalidOperationException)
                        {
                            if ((string) TempData["Error"] != "")
                            {
                                TempData["Error"] += "\n";
                            }
                        
                            TempData["Error"] += "Sorry. Failed to update min salary";
                        }
                    }
            //Update CV's field
            try
            {
                var oldFieldId = _context.Fields.First(a => 
                    a.Id == newCv.FieldId
                ).Id;
                //Check if new field
                if (newFieldId != oldFieldId)
                {
                    //Update existed cv's field
                    try
                    {
                        newCv.FieldId = newFieldId;
                        _context.EmployeeCvs.Update(newCv);
                        _context.SaveChanges();
                        if ((string) TempData["Success"] != "")
                        {
                            TempData["Success"] += "\n";
                        }       
                        TempData["Success"] += "CV's field updated!";
                    }
                    catch (InvalidCastException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }       
                        TempData["Error"] += "Sorry. Failed to change field! Please retry!";
                    } 
                }
            }
            catch (InvalidOperationException)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }       
                TempData["Error"] += "Sorry. Failed to change field! Please retry!";
            }
            //Update skills  
            try
            {
                var oldSkills = _context.EmployeeSkills.Where(a => a.EmployeeCvId == newCv.Id).ToList();
                var newEnumerable = newSkills.Select(a => new {Link = a.CertificationLink, Id = a.SkillId}).OrderBy(a => a.Id);
                var oldEnumerable = oldSkills.Select(a => new {Link = a.CertificationLink, Id = a.SkillId}).OrderBy(a => a.Id);
                if (newEnumerable == oldEnumerable)
                {
                    try
                    {
                        if (oldSkills.Any())
                        {
                            _context.EmployeeSkills.RemoveRange(oldSkills);
                        }
                        _context.EmployeeSkills.AddRange(newSkills);
                        _context.SaveChanges();
                        if ((string) TempData["Success"] != "")
                        {
                            TempData["Success"] += "\n";
                        }       
                        TempData["Success"] += "CV's skills updated!";
                    }
                    catch (InvalidCastException)
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
                var cvId = newCv.Id;
                var oldExternalMaterials = _context.EmployeeExternalMeterials.Where(a => a.EmployeeCvId == cvId);
                var newEnumerable = newExternalMaterials.Select(a => new {Name = a.Name, Link = a.Link}).OrderBy(a => a.Name).ToList();
                var oldEnumerable = oldExternalMaterials.Select(a => new {Name = a.Name, Link = a.Link}).OrderBy(a => a.Name).ToList();
                var except = newEnumerable.Except(oldEnumerable);
                if (except.Any())
                {
                    try
                    {
                    
                        _context.EmployeeExternalMeterials.RemoveRange(oldExternalMaterials);     
                        if (newExternalMaterials.Any())                                           
                        {                                                                         
                            _context.EmployeeExternalMeterials.AddRange(newExternalMaterials);    
                        }                                                                         
                        _context.SaveChanges();                                                   
                        if ((string) TempData["Success"] != "")                                   
                        {                                                                         
                            TempData["Success"] += "\n";                                          
                        }                                                                         
                        TempData["Success"] += "CV's external material updated!";                 
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }       
                        TempData["Error"] += "Sorry. Failed to update your external material please retry";
                    }
                }
                
            }
            catch (InvalidOperationException)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }       
                TempData["Error"] += "Sorry. Failed to update your external material please retry";
            }     
            //New Job histories
            try
            {
                var cvId = newCv.Id;
                var oldJobHistories = _context.EmployeeJobHistories.Where(a => a.EmployeeCvId == cvId);
                var newEnumerable = newJobHistories.Select(a => new {Name = a.Name, Link = a.ShortDescription}).OrderBy(a => a.Name).ToList();
                var oldEnumerable = oldJobHistories.Select(a => new {Name = a.Name, Link = a.ShortDescription}).OrderBy(a => a.Name).ToList();
                var except = newEnumerable.Except(oldEnumerable);
                if (except.Any())
                {
                    try
                    {
                        _context.EmployeeJobHistories.RemoveRange(oldJobHistories);
                        if (newJobHistories.Any())
                        {
                            _context.EmployeeJobHistories.AddRange(newJobHistories);
                        }
                        _context.SaveChanges();
                        if ((string) TempData["Success"] != "")
                        {
                            TempData["Success"] += "\n";
                        }       
                        TempData["Success"] += "CV's job histories updated!";
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }       
                        TempData["Error"] += "Sorry. Failed to update your job histories please retry";
                    }
                }
            }
            catch (InvalidOperationException)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }       
                TempData["Error"] += "Sorry. Failed to update your job histories please retry";
            }
            //New Days
            try
            {
                    
                var cvId = newCv.Id;
                var oldDays = _context.EmployeeWorkDays.Where(a => a.EmployeeCvId == cvId);
                var newEnumerable = newDays.Select(a => new {Day = a.Day}).OrderBy(a => a.Day).ToList();
                var oldEnumerable = oldDays.Select(a => new {Day = a.Day}).OrderBy(a => a.Day).ToList();
                var except = newEnumerable.Except(oldEnumerable);
                if (except.Any())
                {
                    try
                    {
                        _context.EmployeeWorkDays.RemoveRange(oldDays);
                        if (newDays.Any())
                        {
                            _context.EmployeeWorkDays.AddRange(newDays);
                        }
                        _context.SaveChanges();
                        if ((string) TempData["Success"] != "")
                        {
                            TempData["Success"] += "\n";
                        }       
                        TempData["Success"] += "CV's work day updated!";
                    }
                    catch (InvalidOperationException)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }       
                        TempData["Error"] += "Sorry. Failed to update your work day please retry";
                    }
                }   
            }
            catch (InvalidOperationException)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }       
                TempData["Error"] += "Sorry. Failed to update your work day please retry";
            }

            return RedirectToAction("EditCV", new {CvId = cv.CvId});
        }


        public ViewResult RequestToAddField()
        {
            return View();
        }

        public RedirectToActionResult RequestToAddFieldAction()
        {
            return RedirectToAction("RequestToAddField");
        }

        public ViewResult RequestToAddSkill()
        {
            return View();
        }
        
        public RedirectToActionResult RequestToAddSkillAction()
        {
            return RedirectToAction("RequestToAddSkill");
        }

        public ViewResult RequestToAddSchool()
        {
            return View();
        }
        
        //要避免重复加入

        public async Task<IActionResult> AddSchoolAction(AddSchoolAction school)
        {
            var user = await _userManager.GetUserAsync(User);
            var suburbId = _context.Suburbs.First(a => a.Name == school.SuburbName && a.PostCode == school.PostCode).Id;
            var newSchool = new School {DomainExtension =  school.DomainExtension, SchoolName = school.SchoolName, NormalizedName = school.SchoolName.ToUpper(), SuburbId = suburbId, NewRequest = true, RequestedBy = user.Id};
            _context.Schools.Add(newSchool);
            _context.SaveChanges();
            return RedirectToAction("EditPersonalInfo");
        }

        
        public async Task<IActionResult> MyInvitations()
        {
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

        //验证是否是这个人的invitation
        public async Task<IActionResult> InvitationDetail(int invitationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var invitation = _context.Invatations.First(a => a.Id == invitationId);
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

        //Validation 是否是自己
        public IActionResult AcceptOrReject(bool accept)
        {
            return RedirectToAction("MyInvitations");
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
    }    
}