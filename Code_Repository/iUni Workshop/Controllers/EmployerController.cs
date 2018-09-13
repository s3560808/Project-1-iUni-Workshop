using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.EmployerModels;
using iUniWorkshop.Models.EmployerModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.InvatationModel;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.MessageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MySql.Data.MySqlClient;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using MessageDetail = iUni_Workshop.Models.EmployerModels.MessageDetail;

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

        //如果认证了则不可改变
        public async Task<IActionResult> EditCompanyInfo()
        {
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
            if (!ModelState.IsValid)
            {
            }
            int suburbId;
            if ((suburbId = _suburbController.hasSuburb(info.SuburbName, info.PostCode)) < 1)
            {
            }
            var user = await _userManager.GetUserAsync(User);
            var employerInfo = _context.Employers.First(a => a.Id == user.Id);
            //Infom user if already has abn
            if (!employerInfo.Certificated)
            {
                employerInfo.ABN = info.ABN;
                employerInfo.Name = info.Name;
            }
            employerInfo.Location = info.Address;
            employerInfo.BriefDescription = info.BriefDescription;
            employerInfo.ContactEmail = info.ContactEmail;
            employerInfo.PhoneNumber = info.PhoneNumber;
            employerInfo.SuburbId = suburbId;
            _context.Employers.Update(employerInfo);
            _context.SaveChanges();
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
            if(rawInfo.Certificated==true){
                //already Certificated
                return RedirectToAction("Index");
            }
            if (rawInfo.RequestCertification == true)
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
            if (employer.Certificated == true)
            {
                //already Certificated
                return RedirectToAction("Index");
            }
            if (employer.RequestCertification == true)
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
        public async Task<IActionResult> RequestToUnCertificateMyCompany() {

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

        [Route("[Controller]/EditJobProfile/{JobProfileId}")]
        [Route("[Controller]/EditJobProfile/")]
        public async Task<IActionResult> EditJobProfile(int JobProfileId)
        {
            var user = await _userManager.GetUserAsync(User);
            JobProfile jobProfile;
            try
            {
                jobProfile = _context.EmployerJobProfiles.Where(a => a.Id == JobProfileId && a.EmployerId == user.Id).Select(a => new JobProfile
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
            catch (InvalidOperationException ex)
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
                .Select(a => new JobProfileRequiredSchool { SchoolName = a.School.SchoolName, Campus = a.School.Suburb.Name })
                .ToList();
            return View(jobProfile);
        }

        //如果有邀请则不可更改
        //检查是不是本employer的JobProfile
        //
        public async Task<IActionResult> EditJobProfileAction(
            JobProfile jobProfile,
            IEnumerable<JobProfileSkill> jobProfileSkill,
            IEnumerable<JobProfileComplusoryWorkDay> jobProfileComplusoryWorkDay,
            IEnumerable<JobProfileRequiredLocation> jobProfileRequiredLocation,
            IEnumerable<JobProfileRequiredSchool> jobProfileRequiredSchool
        ){
            var user = await _userManager.GetUserAsync(User);
            var field = new Field{};
            
            //0. Validate input data
            //0.1
            //Validate previous profile
            EmployerJobProfile preProfile = null;
            if (jobProfile.ProfileId != 0)
            {
                try
                {
                    preProfile = _context
                        .EmployerJobProfiles
                        .First(a => a.Id == jobProfile.ProfileId);
                }
                catch(System.InvalidOperationException ex) {
                    //No Such profile
                    return RedirectToAction("EditJobProfile");
                }
                //Try to get others profile
                if (user.Id != preProfile.EmployerId)
                {
                    return RedirectToAction("EditJobProfile");
                }
            }
            //0.2
            //If does not have skills
            var newSkills = new List<EmployerSkill>();
            foreach (var skill in jobProfileSkill)
            {
                try
                {
                    var newSkillId = _context.Skills.First(a => a.NormalizedName ==skill.SkillName.ToUpper()).Id;
                    newSkills.Add(new EmployerSkill { SkillId = newSkillId, Required = skill.SkillRequired});  
                }
                catch (InvalidOperationException ex)
                {
                    continue;
                }
            }
            if (!newSkills.Any())
            {
                return RedirectToAction("EditJobProfile");
            }    
            //0.3
            //Validate field
            try
            {
                field = _context.Fields.First(a => a.Name == jobProfile.FieldName);
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToAction("EditJobProfile");
            }
            //0.4
            //Validate Title Validate Description
            //@TODO

            //1. Insert Or Update new profile
            //1.1
            //Set up newer version profile
            var profile = new EmployerJobProfile
            {
                EmployerId = user.Id,
                //需要检查之前
                CreateDateTime = DateTime.Now,
                LastUpdateDateTime = DateTime.Now,
                FieldId = field.Id,
                Title = jobProfile.Title,
                Description = jobProfile.Description,
                RequireJobExperience = jobProfile.RequireJobExperience,
                MaxDayForAWeek = jobProfile.MaxDay,
                MinDayForAWeek = jobProfile.MinDay,
                Salary = jobProfile.Salary
            };
            //1.2.1
            //If does not have pre profile
            if (preProfile == null)
            {
                preProfile = profile;
                _context.EmployerJobProfiles.Add(preProfile);
                
            }
            //1.2.2.
            //If has pre profile
            else
            {
                preProfile.LastUpdateDateTime = DateTime.Now;
                preProfile.FieldId = field.Id;
                preProfile.Title = jobProfile.Title;
                preProfile.Description = jobProfile.Description;
                preProfile.RequireJobExperience = jobProfile.RequireJobExperience;
                preProfile.MaxDayForAWeek = jobProfile.MaxDay;
                preProfile.MinDayForAWeek = jobProfile.MinDay;
                preProfile.Salary = jobProfile.Salary;
                _context.EmployerJobProfiles.Update(preProfile);
            }
            _context.SaveChanges();
            
            //2. Update Skills
            //2.1
            //Try to find & remove previous Skills
            try
            {
                var preSkills = _context.EmployerSkills.Where(a => a.EmployerJobProfileId == jobProfile.ProfileId).ToList();
                _context.EmployerSkills.RemoveRange(preSkills);
                _context.SaveChanges();
            }
            catch (System.InvalidOperationException ex)
            {
                
            }
            //2.2
            //Insert new skills
            foreach (var skill in newSkills)
            {
                skill.EmployerJobProfileId = preProfile.Id;
            }
            _context.EmployerSkills.AddRange(newSkills);
            _context.SaveChanges();
            
            //3. Update Compulsory work Day
            //3.1
            //Try to find & remove previous Compulsory work Day
            try
            {
                var preDays = _context.EmployerComplusoryWorkDays.Where(a => a.EmployerJobProfileId == jobProfile.ProfileId).ToList();
                _context.EmployerComplusoryWorkDays.RemoveRange(preDays);
                _context.SaveChanges();
            }
            catch (System.InvalidOperationException ex)
            {
                
            }
            //3.2
            //Add new Compulsory work Day
            var newDays = new List<EmployerComplusoryWorkDay>();
            foreach (var day in jobProfileComplusoryWorkDay)
            {
                newDays.Add(new EmployerComplusoryWorkDay{EmployerJobProfileId = preProfile.Id,Day = day.Day});
            }
            if (newDays.Any())
            {
                _context.EmployerComplusoryWorkDays.AddRange(newDays);
                _context.SaveChanges();
            }
            
            //4. Update Location
            //4.1
            //Try to find & remove previous Location
            try
            {
                var preLocation = _context.EmployerWorkLocations.Where(a => a.EmployerJobProfileId == jobProfile.ProfileId).ToList();
                _context.EmployerWorkLocations.RemoveRange(preLocation);
                _context.SaveChanges();
            }
            catch (System.InvalidOperationException ex)
            {
                
            }
            //4.2
            //Add new Location
            var newLocations = new List<EmployerRequiredWorkLocation>();
            foreach (var location in jobProfileRequiredLocation)
            {
                try
                {
                    var suburbId = _context.Suburbs.First(
                        a => a.Name == location.LocationName &&
                             a.PostCode == location.PostCode).Id;
                    newLocations.Add(new EmployerRequiredWorkLocation{EmployerJobProfileId = preProfile.Id,SuburbId = suburbId});
                }
                catch(InvalidOperationException ex)
                {
                }

            }
            if (newLocations.Any())
            {
                _context.EmployerWorkLocations.AddRange(newLocations);
                _context.SaveChanges();
            }
            
            //5. Update School
            //5.1
            //Try to find & remove previous School
            try
            {
                var preSchools = _context.EmployerRequiredSchools.Where(a => a.EmployerJobProfileId == jobProfile.ProfileId).ToList();
                _context.EmployerRequiredSchools.RemoveRange(preSchools);
                _context.SaveChanges();
            }
            catch (System.InvalidOperationException ex)
            {
                
            }
            //5.2
            //Add new School
            var newSchools = new List<EmployerRequiredSchool>();
            foreach (var school in jobProfileRequiredSchool)
            {
                try
                {
                    var schoolId = _context.Schools.First(
                        a => a.SchoolName == school.SchoolName &&
                             a.Suburb.Name == school.Campus).Id;
                    newSchools.Add(new EmployerRequiredSchool{EmployerJobProfileId = preProfile.Id, SchoolId = schoolId});
                }
                catch(InvalidOperationException ex)
                {
                }

            }
            if (newLocations.Any())
            {
                _context.EmployerRequiredSchools.AddRange(newSchools);
                _context.SaveChanges();
            }
            
            
            return RedirectToAction("EditJobProfile");
        }

        //检查本人操作
        //return if profileId is not right
        //Filter primary id
        //去除已发送invatation
        [Route("[Controller]/SearchApplicants/{jobProfileId}")]
        public async Task<IActionResult> SearchApplicants(int jobProfileId)
        {
            return View(SearchApplicantsCoreRanker(jobProfileId));
        }

        [Route("[Controller]/ViewApplicantCv/{jobProfileId}/{cvId}")]
        public async Task<IActionResult> ViewApplicantCv(int jobProfileId, int cvId)
        {
            var validationList = SearchApplicantsCoreRanker(jobProfileId);
            var validationResultList = validationList.Where(a => a.CvId == cvId);
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
                EmployeeName = raw.Employee.Name,
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
            catch (InvalidOperationException ex)
            {

            }
            catch (DbUpdateException exception)
            {
                
            }
            return View();
        }
        
        public List<SearchApplicant> SearchApplicantsCoreRanker(int jobProfileId)
        {
            var jobProfile = _context.EmployerJobProfiles.First(a => a.Id == jobProfileId);
            var rawData = new List<EmployeeCV> {};
            var finalData = new List<EmployeeCV> {};
            //1.  Filter first
            //1.1 Filter field
            //1.2 && Filter salary
            //1.3 && Filter Find Job Status
            rawData = _context.EmployeeCvs
                .Where(a => a.FieldId == jobProfile.FieldId 
                            && a.MinSaraly <= jobProfile.Salary 
                            && a.FindJobStatus 
                            && a.StartFindJobDate.AddDays(14).Day >= DateTime.Today.Day)
                .ToList();
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
                .Where(a => a.EmployerJobProfileId == jobProfile.Id).ToList();
            if (employerDays.Any())
            {
                foreach (var data in rawData)
                {
                    var employeeDays = _context.EmployeeWorkDays.Where(a => a.EmployeeCvId == data.Id).ToList();
                    var query =
                        (from employer in employerDays
                            join employee in employeeDays
                                on employer.Day equals employee.Day
                            where employer.Day == employee.Day
                            select new EmployeeCV{Id = employee.EmployeeCvId}).ToList();
                    if (query.Count() == employerDays.Count())
                    {
                        finalData.Add(data);
                    }
                }
                rawData = finalData;
                finalData = new List<EmployeeCV>();
            }
            
            //2. Filter Applicants
            var employerSkills = _context.EmployerSkills.Where(a => a.Required).ToList();
            if (employerSkills.Any())
            {
                foreach (var data in rawData)
                {
                    var employeeSkills = _context.EmployeeSkills.Where(a => a.EmployeeCvId == data.Id).ToList();
                    var query =
                        (from employer in employerSkills
                            join employee in employeeSkills
                                on employer.SkillId equals employee.SkillId
                            where employee.SkillId == employer.SkillId
                            select new {id = employee.Id}).ToList();
                    if (query.Count() == employerSkills.Count())
                    {
                        finalData.Add(data);
                    }
                }
                rawData = finalData;
                finalData = new List<EmployeeCV>();
            }
            //@TODO Filter School
            //@TODO Filter Location
            
            //Rank applicants
            var finalResults = new List<SearchApplicant>();
            foreach (var data in rawData)
            {
                var employeeName = _context.Employees.First(a => a.Id == data.EmployeeId).Name;
                finalResults.Add(new SearchApplicant{ApplicantName = employeeName, CvId = data.Id, Score = 0});
            }
            employerSkills = _context.EmployerSkills
                .Where(a => !a.Required && a.EmployerJobProfileId == jobProfile.Id)
                .OrderBy(a => a.Id)
                .ToList();
            if (employerSkills.Any())
            {
                foreach (var result in finalResults)
                {
                    var employeeSkills = _context.EmployeeSkills.Where(a => a.EmployeeCvId == result.CvId).OrderBy(a => a.Id).ToList();
                    for (int i = 0; i < employerSkills.Count; i++)
                    {
                        for (int j = 0; j < employeeSkills.Count; j++)
                        {
                            if (employerSkills[i].SkillId == employeeSkills[j].SkillId)
                            {
                                result.Score += (11 - i) * (11 - j);
                            }
                        }
                    }
                }
            }
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
        public async Task<IActionResult> MyMessages()
        {
            
            return View();
        }

        //一旦reject不可发送
        //验证发送者和接收者
        [Route("[Controller]/MessageDetail/{invitationId}")]
        public async Task<IActionResult> MessageDetail(int invitationId)
        {
            var updateConversations= _context.Conversations.Where(a => a.InvatationId == invitationId);
            //New conversation
            MessageDetail result;
            if (!updateConversations.Any())
            {
                result = new MessageDetail { Type = MessageType.UserMessage, InvitationId = invitationId};
                return View(result);
            }

            var updateMessages = _context.Messages.Where(a => a.ConversationId == updateConversations.First().Id);
            
            List<MessageDetailMessageInfo> messages = new List<MessageDetailMessageInfo> { };
            var user = await _userManager.GetUserAsync(User);
            var conversation = _context.Conversations.First(a => a.InvatationId == invitationId);
            
            foreach (var updateMessage in updateMessages) {
                var receiver = _context.Users.First(a => a.Id == updateMessage.receiverId);
                string senderEmail;
                if (conversation.User1Id == receiver.Id)
                {
                    senderEmail = _context.Users.First(a => a.Id == conversation.User2Id).Email;
                }
                else
                {
                    senderEmail = _context.Users.First(a => a.Id == conversation.User1Id).Email;
                }

                messages.Add(new MessageDetailMessageInfo {SenderName = senderEmail, Detail = updateMessage.MessageDetail, SentTime = updateMessage.SentTime});
                if (updateMessage.receiverId == user.Id) {
                    updateMessage.Read = true;
                }
            }

            result  = new MessageDetail { Type = MessageType.UserMessage, InvitationId = invitationId, Messages = messages.AsEnumerable()};
            _context.Messages.UpdateRange(updateMessages);
            await _context.SaveChangesAsync();
            return View(result);
        }

        public async Task SendMessage(SendMessage sendMessage)
        {
            if (!ModelState.IsValid)
            {
            }
            var user = await _userManager.GetUserAsync(User);
            Conversation conversation = null;
            try
            {
                conversation = _context.Conversations
                    .First(a => a.InvatationId == sendMessage.InvitationId);
            }
            catch (InvalidOperationException ex)
            {
            }
            var invitation = _context.Invatations
                .First(a => a.Id == sendMessage.InvitationId);
            string receiverId;
            
            if (conversation != null)
            {
                if (conversation.User1Id == user.Id)
                {
                    receiverId = conversation.User2Id;
                }
                else
                {
                    receiverId = conversation.User1Id;
                }

            }
            else
            {
                conversation = new Conversation();

                conversation.InvatationId = sendMessage.InvitationId;
                conversation.Title = "";
                conversation.Type = MessageType.UserMessage;
                var employerId = _context.EmployerJobProfiles.First(a => a.Id == invitation.EmployerJobProfileId).EmployerId; 
                conversation.User1Id = employerId;
                var employeeId = _context.EmployeeCvs.First(a => a.Id == invitation.EmployeeCvId).EmployeeId;
                conversation.User2Id = employeeId;
                
                receiverId = conversation.User1Id;
                _context.Conversations.Add(conversation);
                _context.SaveChanges();
            }

            var message = new Message
            {
                Read = false, 
                receiverId = receiverId,
                ConversationId = conversation.Id,
                MessageDetail = sendMessage.MessageDetail,
                SentTime = DateTime.Now
            };

            _context.Messages.Add(message);
            _context.SaveChanges();
            
        }

        public async Task<IActionResult> CertificateMyCompany()
        {
            return View();
        }


    }
}