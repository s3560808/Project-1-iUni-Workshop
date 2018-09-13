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
            var user = await _userManager.GetUserAsync(User);
           
            var cvs = _context.EmployeeCvs.Where(a => a.EmployeeId == user.Id).Select(a => new MyCvs{CvTitle = a.Title, CvId = a.Id, FindJobStatus = a.FindJobStatus, FieldId = a.FieldId});
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
            var user = await _userManager.GetUserAsync(User);
            EditCV cv;
            try
            {
                cv = _context.EmployeeCvs.Where(a => a.Id == CvId && a.EmployeeId == user.Id).Select(a => new EditCV{
                        CvId = a.Id, Details = a.Details, Email = user.Email, FieldId = a.FieldId, 
                    FindJobStatus = a.FindJobStatus, MinSaraly = a.MinSaraly, StartFindJobDate = a.StartFindJobDate, 
                    Primary = a.Primary, Title = a.Title}).First();
            }
            catch (InvalidOperationException ex)
            {
                return View();
            }

            var fieldName = _context.Fields.First(a => a.Id == cv.FieldId).Name;
            cv.ExternalMeterials = _context.EmployeeExternalMeterials.Where(a => a.EmployeeCvId == cv.CvId).ToList();
            cv.JobHistories = _context.EmployeeJobHistories.Where(a => a.EmployeeCvId == cv.CvId).ToList();
            cv.Skills = _context.EmployeeSkills.Where(a => a.EmployeeCvId == cv.CvId).Select(a => new EditCVEmployeeSkill{SkillId = a.SkillId, CertificationLink = a.CertificationLink}).ToList();
            cv.WorkDays = _context.EmployeeWorkDays.Where(a => a.EmployeeCvId == cv.CvId).ToList();
            cv.FieldName = fieldName;
            for (int i = 0; i < cv.Skills.Count; i++)
            {
                cv.Skills[i].SkillName = _context.Skills.First(a => a.Id == cv.Skills[i].SkillId).Name;
            }

            return View(cv);
        }

        public async Task EditCvAction(EditCVAction cv, 
            IEnumerable<EditCVEmployeeSkill> skills, 
            IEnumerable<EditCVExternalMeterial> externalMeterials, 
            IEnumerable<EditCVJobHistory> jobHistories,
            IEnumerable<EditCvWorkDay> days)
        {
            var user = await _userManager.GetUserAsync(User);
            EmployeeCV newCv = null;
            List<EmployeeSkill> newSkills;
            List<EmployeeExternalMeterial> newExternalMeterials;
            List<EmployeeJobHistory> newJobHistories;
            List<EmployeeWorkDay> newWorkDays;
            int CvField;

            
            if (cv.CvId != 0)
            {
                try
            {
                
                EmployeeCV cvDetail = _context.EmployeeCvs.First(a => a.Id == cv.CvId);
                //Not this employee's cv
                if (cvDetail.EmployeeId != user.Id)
                {
                    return;
                }

                try
                {
                    var oldSkills = _context.EmployeeSkills.Where(a => a.EmployeeCvId == cvDetail.Id);
                    _context.EmployeeSkills.RemoveRange(oldSkills);
                    _context.SaveChanges();
                }
                catch (InvalidOperationException e)
                {
                    
                }
                try
                {
                    var oldExternalMeterials = _context.EmployeeExternalMeterials.Where(a => a.EmployeeCvId == cvDetail.Id);
                    _context.EmployeeExternalMeterials.RemoveRange(oldExternalMeterials);
                    _context.SaveChanges();
                }
                catch (InvalidOperationException e)
                {
                    
                }
                try
                {
                    var oldHistories = _context.EmployeeJobHistories.Where(a => a.EmployeeCvId == cvDetail.Id);
                    _context.EmployeeJobHistories.RemoveRange(oldHistories);
                    _context.SaveChanges();
                }
                catch (InvalidOperationException e)
                {
                    
                }
                try
                {
                    var oldDays = _context.EmployeeWorkDays.Where(a => a.EmployeeCvId == cvDetail.Id);
                    _context.EmployeeWorkDays.RemoveRange(oldDays);
                    _context.SaveChanges();
                }
                catch (InvalidOperationException e)
                {
                    
                }
                
            }
            catch (InvalidOperationException ex)
            {
                return;
            }
            }

            
            
            //!! No skill
            if (!skills.ToList().Any())
            {
                return;
            }

            //Get CV Field
            try{
                CvField = _context.Fields.First(a => a.AddedBy != null && a.NormalizedName == cv.FieldName.ToUpper()).Id;
            }catch(System.InvalidOperationException ex)
            {
                //!!! Cannot find Field
                return;
            }
            
            //Get Skills
            if (!skills.Any())
            {
                return;
            }

//            //New CV
            if (cv.CvId == 0)
            {
                newSkills = new List<EmployeeSkill>();
                newExternalMeterials = new List<EmployeeExternalMeterial>();
                newJobHistories = new List<EmployeeJobHistory>();
                newWorkDays = new List<EmployeeWorkDay>();

                newCv = new EmployeeCV
                {
                    EmployeeId = user.Id,
                    Primary = cv.PrimaryCv,
                    FindJobStatus = cv.UpdateStatus,
                    StartFindJobDate = DateTime.Now,
                    Title = cv.Title,
                    Details = cv.Description,
                    MinSaraly = cv.MinSalary,
                    FieldId = CvField
                };

                _context.EmployeeCvs.Add(newCv);
                
            }
            else
            {
                EmployeeCV cvDetail = _context.EmployeeCvs.First(a => a.Id == cv.CvId);
                cvDetail.Primary = cv.PrimaryCv;
                cvDetail.FindJobStatus = cv.UpdateStatus;
                if (cv.UpdateStatus == true)
                {
                    cvDetail.StartFindJobDate = DateTime.Now;
                }
                cvDetail.Title = cv.Title;
                cvDetail.Details = cv.Description;
                cvDetail.MinSaraly = cv.MinSalary;
                cvDetail.FieldId = CvField;
                _context.EmployeeCvs.Update(cvDetail);
                _context.SaveChanges();
                newCv = cvDetail;
            }
            
            _context.SaveChanges();
            foreach (var skill in skills)
                {
                    try
                    {
                        var skillId = _context.Skills.First(a =>
                            a.AddedBy != null && a.NormalizedName == skill.SkillName.ToUpper()).Id;
                        _context.EmployeeSkills.Add(new EmployeeSkill { SkillId = skillId, EmployeeCvId = newCv.Id, CertificationLink = skill.CertificationLink});
                        _context.SaveChanges();
                    }catch(System.InvalidOperationException ex)
                    {
                        //!!! Cannot find skill
                        continue;
                    }
                }              

                foreach (var meterial in externalMeterials)
                {
                    try
                    {
                        var test = new EmployeeExternalMeterial
                        {
                            EmployeeCvId = newCv.Id,
                            Name = meterial.ExternalMeterialName,
                            Link = meterial.ExternalMeterialLink
                        };
                        if(test.Name == null || test.Link==null){
                            continue;
                        }
                        _context.EmployeeExternalMeterials.Add(test);
                        _context.SaveChanges();
                    }
                    catch (System.InvalidOperationException ex)
                    {
                        //!!! Not valid meterial state
                        continue;
                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                    {
                        continue;
                    }
                    
                }
                
                foreach (var jobHistory in jobHistories)
                {
                    try{
                        _context.EmployeeJobHistories.Add(new EmployeeJobHistory{ EmployeeCvId = newCv.Id, Name = jobHistory.JobHistoryName, ShortDescription = jobHistory.JobHistoryShortDescription});
                        _context.SaveChanges();
                    }catch(System.InvalidOperationException ex)
                    {
                        //!!! Cannot find skill
                        continue;
                    }
                }

                foreach (var day in days)
                {
                    try{
                        _context.EmployeeWorkDays.Add(new EmployeeWorkDay{EmployeeCvId = newCv.Id, Day = day.Day});
                        _context.SaveChanges();
                    }catch(System.InvalidOperationException ex)
                    {
                        //!!! Cannot find skill
                        continue;
                    }
                }
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

        public async Task<IActionResult> AddSchoolAction(AddSchoolAction school)
        {
            var user = await _userManager.GetUserAsync(User);
            var suburbId = _context.Suburbs.First(a => a.Name == school.SurburbName && a.PostCode == school.PostCode).Id;
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

        public Task<IActionResult> InvitationDetail()
        {
            return View();
        }

        public async Task<IActionResult> MyMessages()
        {
            var user = await _userManager.GetUserAsync(User);
            var conversations = _context.Conversations
                .Where(a => a.User1Id == user.Id || a.User2Id == user.Id)
                .AsEnumerable().ToList();
            var messages = new List<MyMessages>();
            foreach (var conversation in conversations)
            {
                var receiverId = conversation.User1Id == user.Id ? conversation.User2Id : conversation.User1Id;
                var message = _context.Messages
                    .Where(a => a.ConversationId == conversation.Id && a.receiverId == receiverId)
                    .OrderByDescending(a => a.SentTime).First();
                var senderEmail = _context.Users.First(a => a.Id == receiverId).Email;
                messages.Add(new MyMessages
                {
                    ConversationId = message.ConversationId, 
                    Read = message.Read, 
                    SenderName = senderEmail, 
                    SentTime = message.SentTime, 
                    Title = conversation.Title,
                    Type = conversation.Type
                });
            }
            return View(messages);
        }


        public async Task<IActionResult> MessageDetail(string conversationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var updateMessages = _context.Messages.Where(a => a.ConversationId == conversationId).ToList();
            var conversation = _context.Conversations.First(a => a.Id == conversationId);
            var messages = new List<MessageDetail>();
            foreach (var updateMessage in updateMessages)
            {
                var senderId = user.Id == conversation.User1Id ? conversation.User2Id : conversation.User1Id;
                var email = _context.Users.First(a => a.Id == senderId).Email;
                messages.Add(new MessageDetail {SenderName = email, ConversationId = updateMessage.ConversationId, Detail = updateMessage.MessageDetail, SentTime = updateMessage.SentTime});
                if (updateMessage.receiverId == user.Id) {
                    updateMessage.Read = true;
                }
            }
            _context.Messages.UpdateRange(updateMessages);
            await _context.SaveChangesAsync();
            return View(messages);
        }
    }    
}