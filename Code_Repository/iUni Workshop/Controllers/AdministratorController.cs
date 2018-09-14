using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.MessageModels;
using iUni_Workshop.Models.SchoolModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MessageDetail = iUni_Workshop.Models.AdministratorModels.MessageDetail;
using MyMessages = iUni_Workshop.Models.AdministratorModels.MyMessages;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministratorController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager
            ) {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        
        public ViewResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ViewResult AddSchool()
        {
            var result = _context.Schools
                .Select(a => new AddSchool
                {
                    DomainExtension = a.DomainExtension, 
                    Status = a.Status, 
                    PostCode = a.Suburb.PostCode,
                    SchoolName = a.SchoolName,
                    SurburbName = a.Suburb.Name,
                    Id = a.Id
                }).OrderBy(a => a.DomainExtension);
            return View(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddSchoolAction(AddSchoolAction school)
        {
            var schoolStatus = SchoolStatus.InUse;

            if (!ModelState.IsValid)
            {
                //ToDo
                //If model is not valid
            }       
            var suburbs = _context.Suburbs.Where(a => a.Name == school.SurburbName && a.PostCode == school.PostCode);
            if (suburbs.Any())
            {
                //Todo 
                //If cannot find suburb
            }

            var checkWhetherUpdate = _context.Schools.Where(a => 
                    (a.SchoolName == school.SchoolName && a.SuburbId == suburbs.First().Id) ||
                    (a.DomainExtension == school.DomainExtension  && a.SuburbId == suburbs.First().Id)
                );

            School newSchool;
            if (checkWhetherUpdate.Any())
            {
                newSchool = checkWhetherUpdate.First();
                newSchool.SchoolName = school.SchoolName;
                newSchool.DomainExtension = school.DomainExtension;
                newSchool.Status = schoolStatus;
                //ToDo
                //Need to infom administrator it is a update was no longer use or in request
            }
            else
            {
                newSchool = new School
                {
                    DomainExtension = school.DomainExtension.ToLower(),
                    SchoolName = school.SchoolName,
                    NormalizedName = school.SchoolName.ToUpper(),
                    SuburbId = suburbs.First().Id,
                    AddedBy = (await _userManager.GetUserAsync(User)).Id,
                    Status = SchoolStatus.InUse
                };
            }
            //To update school name if school name changes
            
            _context.Schools.Update(newSchool);
            await _context.SaveChangesAsync();
            await UpdateRelatedInfo(newSchool.SchoolName, newSchool.DomainExtension);
            return RedirectToAction("AddSchool");
        }

        public async Task<IActionResult> UpdateSchoolAction(UpdateSchool school)
        {
            var suburbs = _context.Suburbs.Where(a => a.Name == school.SurburbName && a.PostCode == school.PostCode);
            var update = _context.Schools.First(a => a.Id == school.Id);
            //TODO validate id
            if (suburbs.Any())
            {
                //Todo 
                //If cannot find suburb
            }
            
            //Can log in database
            if (school.SchoolName != update.SchoolName)
            {
                update.SchoolName = school.SchoolName;
                update.NormalizedName = school.SchoolName.ToUpper();
                _context.Update(update);
                _context.SaveChanges();
                await UpdateRelatedInfo(update.SchoolName, update.DomainExtension);
            }
            
            //Can log in database
            if (school.DomainExtension != update.DomainExtension)
            {
                update.DomainExtension = school.DomainExtension.ToLower();
                _context.Update(update);
                _context.SaveChanges();
                await UpdateRelatedInfo(update.SchoolName, update.DomainExtension);
            }
            
            //Can log in database
            if (suburbs.First().Id != update.SuburbId)
            {
                update.SuburbId = suburbs.First().Id;
                _context.Update(update);
                _context.SaveChanges();
            }

            //Can log in database
            if (school.Status != update.Status)
            {
                update.Status = school.Status;
                _context.Update(update);
                _context.SaveChanges();
            }
            return RedirectToAction("AddSchool");
        }

        private async Task UpdateRelatedInfo(string schoolName, string domainExtension)
        {
            var schools = _context.Schools.Where(a => 
                a.NormalizedName == schoolName.ToUpper() ||
                a.DomainExtension == domainExtension.ToLower()
            );
            
            
            foreach (var school in schools)
            {
                school.SchoolName = schoolName;
                school.NormalizedName = schoolName.ToUpper();
                school.DomainExtension = domainExtension.ToLower();
            }
            _context.Schools.UpdateRange(schools);
            await _context.SaveChangesAsync();
        }

        public async Task<IActionResult> AddField()
        {
            var result = _context.Fields
                .Select(a => 
                    new AddField
                    {
                        Id = a.Id, 
                        Name = a.Name, 
                        Status = a.Status
                    })
                .AsEnumerable();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddFieldAction(AddField field)
        {
            if (!ModelState.IsValid)
            {
            }

            var checkInDatabase = _context.Fields.Where(a => a.NormalizedName == field.Name.ToUpper());

            Field newField;
            
            if (checkInDatabase.Any())
            {
                //TODO Need to infrom administrator it is a update. Already in database.
                newField = checkInDatabase.First();
                newField.Status = FieldStatus.InUse;
            }
            else
            {
                newField = new Field
                {
                    Name = field.Name,
                    NormalizedName = field.Name.ToUpper(),
                    Status = FieldStatus.InUse,
                    AddedBy = (await _userManager.GetUserAsync(User)).Id
                };
            }
            _context.Fields.Update(newField);
            await _context.SaveChangesAsync();
            return RedirectToAction("AddField");
        }

        public void UpdateFieldAction(AddField field)
        {
            if (!ModelState.IsValid)
            {
            }
            
            var checkInDatabase = _context.Fields.Where(a => a.Id == field.Id);

            if (!checkInDatabase.Any()) return;//Todo error not in database

            var oldField = checkInDatabase.First();

            //Prepare to log
            if (oldField.Status != field.Status)
            {
                oldField.Status = field.Status;
                _context.Fields.Update(oldField);
                _context.SaveChanges();
            }
            //Prepare to log
            if (oldField.Name != field.Name)
            {
                oldField.Name = field.Name;
                oldField.NormalizedName = field.Name.ToUpper();
                _context.Fields.Update(oldField);
                _context.SaveChanges();
            }
            
            return;
        }

        public async Task<IActionResult> AddSkill()
        {
            var result = _context.Skills
                .Select(a => 
                    new AddSkill
                    {
                        Id = a.Id, 
                        Name = a.Name, 
                        Status = a.Status
                    })
                .AsEnumerable();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddSkillAction(AddSkill skill)
        {
            if (!ModelState.IsValid)
            {
            }
            
            var checkInDatabase = _context.Skills.Where(a => a.NormalizedName == skill.Name.ToUpper());

            Skill newSkill;

            if (checkInDatabase.Any())
            {
                //TODO Need to infrom administrator it is a update. Already in database.
                newSkill = checkInDatabase.First();
                newSkill.Status = SkillStatus.InUse;
            }
            else
            {
                newSkill = new Skill
                {
                    Name = skill.Name,
                    NormalizedName = skill.Name.ToUpper(),
                    AddedBy = (await _userManager.GetUserAsync(User)).Id,
                    Status = SkillStatus.InUse
                };
            }

            _context.Skills.Update(newSkill);
            await _context.SaveChangesAsync();

            return RedirectToAction("AddSkill");
        }

        public void UpdateSkillAction(AddSkill skill)
        {
            if (!ModelState.IsValid)
            {
            }
            
            var checkInDatabase = _context.Skills.Where(a => a.Id == skill.Id);

            if (!checkInDatabase.Any()) return;//Todo error not in database

            var oldField = checkInDatabase.First();

            //Prepare to log
            if (oldField.Status != skill.Status)
            {
                oldField.Status = skill.Status;
                _context.Skills.Update(oldField);
                _context.SaveChanges();
            }
            //Prepare to log
            if (oldField.Name != skill.Name)
            {
                oldField.Name = skill.Name;
                oldField.NormalizedName = skill.Name.ToUpper();
                _context.Skills.Update(oldField);
                _context.SaveChanges();
            }
            
            return;
        }

        public async Task<IActionResult> SetUserType()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        
        [HttpPost]
        public async Task<IActionResult> SetUserTypeAction(SetUserType setUserType)
        {          
            if (!ModelState.IsValid)
            {
            }
            var user = _context.Users.First(a => a.Email == setUserType.Email);
            await _userManager.AddToRoleAsync(user, Roles.Administrator);
            _context.Administraotrs.Add(new Administraotr { Id = user.Id, Name = user.UserName});
            await _context.SaveChangesAsync();
            return RedirectToAction("SetUserType");
        }

        public async Task<IActionResult> NewMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewMessageAction(NewMessage message)
        {
            if (!ModelState.IsValid)
            {
            }
            string reciverId = _context.Users.First(a => a.Email == message.Email).Id;
            //Create Conversation
            Conversation newConversation = new Conversation
            {
                User1Id = (await _userManager.GetUserAsync(User)).Id,
                User2Id = reciverId,
                Title = message.Title,
                Type = message.Type
            };
            //Create Message
            _context.Conversations.Add(newConversation);
            _context.SaveChanges();
            Message newMessage = new Message
            {
                ConversationId = newConversation.Id,
                receiverId = reciverId,
                SentTime = DateTime.Now,
                Read = false,
                MessageDetail = message.MessageDetail,
            };
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction("NewMessage");
        }

        [Route("[Controller]/GetUsers/{userName}/")]
        public IActionResult GetUsers(string userName)
        {
            var reciver = _context.Users.Where(a => a.UserName.Contains(userName)).ToList();
            return Json(reciver);
        }

        public async Task<IActionResult> MyMessages()
        {
            var _user = await _userManager.GetUserAsync(User);
            var conversations = _context.Conversations
                .Where(a => a.User1Id == _user.Id ||
                            a.User2Id == _user.Id)
                .AsEnumerable()
                .Distinct();
            List<MyMessages> messages = new List<MyMessages>();
            foreach (var conversation in conversations)
            {
                var message = _context.Messages
                    .Where(a => a.ConversationId == conversation.Id && a.receiverId == _user.Id)
                    .OrderByDescending(a => a.SentTime).First();
                string sender;
                sender = conversation.User1Id == _user.Id ? 
                    _context.Users.First(a => a.Id == conversation.User2Id).UserName : 
                    _context.Users.First(a => a.Id == conversation.User1Id).UserName;

                messages.Add(new MyMessages { ConversationId = message.ConversationId, Read = message.Read, SenderName = sender, SentTime = message.SentTime, Title = conversation.Title});
            }
            return View(messages);
        }

        public async Task<IActionResult> MessageDetail(string conversationId)
        {
            var updateMessages = _context.Messages.Where(a => a.ConversationId == conversationId).ToList();
            
            List<MessageDetail> messages = new List<MessageDetail> { };
            var user = await _userManager.GetUserAsync(User);
            var conversation = _context.Conversations.First(a => a.Id == conversationId);
            
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

                messages.Add(new MessageDetail {SenderName = senderEmail, ConversationId = updateMessage.ConversationId, Detail = updateMessage.MessageDetail, SentTime = updateMessage.SentTime, Type = conversation.Type});
                if (updateMessage.receiverId == user.Id) {
                    updateMessage.Read = true;
                }
            }
            _context.Messages.UpdateRange(updateMessages);
            await _context.SaveChangesAsync();
            return View(messages);
        }

        public async Task<IActionResult> ReplyMyMessage(ReplyMyMessage replyMyMessage)
        {
            if (!ModelState.IsValid)
            {
            }
            var user = await _userManager.GetUserAsync(User);
            var previousMessages = _context.Messages
                .Where(a => a.ConversationId == replyMyMessage.ConversationId && a.receiverId == user.Id);
            var conversation = _context.Conversations
                .First(a => a.Id == replyMyMessage.ConversationId);
            
        //Check sender, writer, conversationid
            //If no previous message it not reply
            if (!previousMessages.Any()) {
                return RedirectToAction("MyMessages");
            }
            //System conversation cannot reply
            if (conversation.Type == MessageType.System)
            {
                return RedirectToAction("MyMessages");
            }
            string receiverId;
            if (conversation.User1Id == user.Id)
            {
                receiverId = conversation.User2Id;
            }
            else
            {
                receiverId = conversation.User1Id;
            }

            var newMessage = new Message
            {
                receiverId = receiverId,
                ConversationId = conversation.Id, 
                SentTime = DateTime.Now, 
                Read = false, 
                MessageDetail = replyMyMessage.MessageDetail,
                
            };
            await _context.AddAsync(newMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyMessages");
        }
    }
}