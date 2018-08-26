using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.AdministratorModels;
using iUni_Workshop.Models.JobRelatedModels;
using iUni_Workshop.Models.MessageModels;
using iUni_Workshop.Models.SchoolModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministratorController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager
            ) {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        
        public async Task<IActionResult> Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> AddSchool()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddSchoolAction(AddSchool school)
        {
            if (!ModelState.IsValid)
            {
            }
            var suburbId = _context.Suburbs.First(a => a.Name == school.SurburbName && a.PostCode == school.PostCode).Id;
            var newSchool = new School
            {
                DomainExtension = school.DomainExtension,
                SchoolName = school.SchoolName,
                NormalizedName = school.SchoolName.ToUpper(),
                SuburbId = suburbId,
                AddedBy = (await _userManager.GetUserAsync(User)).Id
            };
            _context.Schools.Add(newSchool);
            await _context.SaveChangesAsync();
            return RedirectToAction("AddSchool");
        }

        public async Task<IActionResult> AddField()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFieldAction(AddField field)
        {
            if (!ModelState.IsValid)
            {
            }
            var newField = new Field
            {
                Name = field.Name,
                NormalizedName = field.Name.ToUpper(),
                AddedBy = (await _userManager.GetUserAsync(User)).Id
            };
            _context.Fields.Add(newField);
            await _context.SaveChangesAsync();
            return RedirectToAction("AddField");
        }

        public async Task<IActionResult> AddSkill()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSkillAction(AddSkill skill)
        {
            if (!ModelState.IsValid)
            {
            }
            var newSkill = new Skill
            {
                Name = skill.Name,
                NormalizedName = skill.Name.ToUpper(),
                AddedBy = (await _userManager.GetUserAsync(User)).Id
            };
            
            _context.Skills.Add(newSkill);
            await _context.SaveChangesAsync();
            return RedirectToAction("AddSkill");
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
            Message newMessage = new Message
            {
                SenderId = (await _userManager.GetUserAsync(User)).Id,
                ReciverId = reciverId,
                SentTime = DateTime.Now,
                Title = message.Title,
                MessageDetail = message.MessageDetail,
                Read = false
            };
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction("NewMessage");
        }

        [Route("[Controller]/GetUsers/{email}/")]
        public IActionResult GetUsers(string userName)
        {
            var reciver = _context.Users.Where(a => a.UserName.Contains(userName)).ToList();
            return Json(reciver);
        }

        public async Task<IActionResult> MyMessages()
        {
            var _user = await _userManager.GetUserAsync(User);
            var conversations = _context.Messages.Where(a => a.ReciverId == _user.Id).Select(b => b.ConversationId).AsEnumerable().Distinct();
            List<MyMessages> messages = new List<MyMessages>();
            foreach (var conversation in conversations)
            {
                var message = _context.Messages.Where(a => a.ConversationId == conversation && a.ReciverId == _user.Id).OrderByDescending(a => a.SentTime).First();

                messages.Add(new MyMessages { ConversationId = message.ConversationId, Read = message.Read, SenderName = _user.Email, SentTime = message.SentTime, Title = message.Title});
            }
            return View(messages);
        }

        public async Task<IActionResult> MessageDetail(string conversationId)
        {
            var updateMessages = _context.Messages.Where(a => a.ConversationId == conversationId).ToList();
            List<MessageDetail> messages = new List<MessageDetail> { };
            var user = await _userManager.GetUserAsync(User);
            foreach (var updateMessage in updateMessages) {
                var Email = _context.Users.First(a => a.Id == updateMessage.SenderId).Email;
                messages.Add(new MessageDetail {SenderName = Email, ConversationId = updateMessage.ConversationId, Detail = updateMessage.MessageDetail, SentTime = updateMessage.SentTime});
                if (updateMessage.ReciverId == user.Id) {
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
            var previousMessage = _context.Messages.Where(a => a.ConversationId == replyMyMessage.ConversationId && a.ReciverId == user.Id);
            //Check sender, writer, conversationid
            if (previousMessage.Count() == 0) {
                return RedirectToAction("MyMessages");
            }
            var newMessage = new Message {ConversationId = previousMessage.First().ConversationId, SenderId = user.Id, ReciverId = previousMessage.First().SenderId, SentTime = DateTime.Now, Read = false, Title = previousMessage.First().Title, MessageDetail = replyMyMessage.MessageDetail};
            await _context.AddAsync(newMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyMessages");
    }

        //        public async Task<IActionResult> SystemStatus()
        //        {
        //            return View();
        //        }
    }
}