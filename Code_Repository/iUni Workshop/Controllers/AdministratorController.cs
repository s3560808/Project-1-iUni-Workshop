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
using MessageDetail = iUni_Workshop.Models.AdministratorModels.MessageDetail;
using MyMessages = iUni_Workshop.Models.AdministratorModels.MyMessages;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdministratorController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager
            ) {
            _userManager = userManager;
            _context = context;
        }

        
        public ViewResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ViewResult AddSchool()
        {
            ProcessSystemInfo();
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

        [HttpPost]
        public async Task<IActionResult> AddSchoolAction(AddSchoolAction school)
        {
            const int schoolStatus = SchoolStatus.InUse;
            InitialSystemInfo();
            //1. Check if correct input from front-end
            if (!ModelState.IsValid)
            {
                foreach (var model in ModelState)
                {
                    if (model.Value.Errors.Count == 0) continue;
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                    if (model.Key == "PostCode")
                    {
                        TempData["Error"]  += "Correct postcode is required";
                    }
                    else
                    {
                        foreach (var error in model.Value.Errors)
                        {
                            TempData["Error"]  += error.ErrorMessage;
                        }
                    }
                }
                return RedirectToAction("AddSchool");
            }       
            //2. Check if user entered correct suburb
            var suburbs = _context.Suburbs
                .Where(a => a.Name == school.SuburbName.ToUpper() && a.PostCode == school.PostCode);
            if (!suburbs.Any())
            {
                TempData["Error"] = "Cannot find your Suburb, Please select correct one";
                return RedirectToAction("AddSchool");
            }
            var checkWhetherUpdate = _context.Schools.Where(a => 
                    (a.SchoolName == school.SchoolName && a.SuburbId == suburbs.First().Id) &&
                    (a.DomainExtension == school.DomainExtension)
                );
            School newSchool;
            //2.Check if new a school or already in database 
            //2.1Update a school
            if (checkWhetherUpdate.Any())
            {
                var oldStatus = checkWhetherUpdate.First().Status;
                var oldLocationName = _context.Suburbs
                    .First(a => a.Id == checkWhetherUpdate.First().SuburbId).Name;
                newSchool = checkWhetherUpdate.First();
                newSchool.SchoolName = school.SchoolName;
                newSchool.DomainExtension = school.DomainExtension;
                newSchool.Status = schoolStatus;
                if ((string) TempData["Inform"] != "")
                {
                    TempData["Inform"] += "\n";
                }
                TempData["Inform"] += "It is not a new school. " 
                                      + newSchool.SchoolName + " in " 
                                      + oldLocationName+" campus was in ";
                if (oldStatus == SchoolStatus.InUse)
                {
                    TempData["Inform"]  += "\"In Use\"";
                }else if (oldStatus == SchoolStatus.InRequest)
                {
                    TempData["Inform"]  += "\"In Request\"";
                }
                else
                {
                    TempData["Inform"]  += "\"No Longer Used\"";
                }
            }
            //2.2Insert a new school
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
            //3. Update user required school
            _context.Schools.Update(newSchool);
            await _context.SaveChangesAsync();
            //4. To update school name if school name changes
            await UpdateRelatedInfo(newSchool.SchoolName, newSchool.DomainExtension);
            TempData["Success"] = "School inserted successfully";
            return RedirectToAction("AddSchool");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSchoolAction(UpdateSchool school)
        {
            InitialSystemInfo();
            //1. Check if have required model
            if (!ModelState.IsValid)
            {
                foreach (var model in ModelState)
                {
                    if (model.Value.Errors.Count == 0) continue;
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                    if (model.Key == "PostCode")
                    {
                        TempData["Error"]  += "Correct postcode is required";
                    }
                    else
                    {
                        foreach (var error in model.Value.Errors)
                        {
                            TempData["Error"]  += error.ErrorMessage;
                        }
                    }
                }
                return RedirectToAction("AddSchool");
            }  
            
            var suburbs = _context.Suburbs
                .Where(a => a.Name == school.SuburbName && a.PostCode == school.PostCode);
            var updateRaw = _context.Schools
                .Where(a => a.Id == school.Id);
            //2. Check if correct suburb
            if (!suburbs.Any())
            {
                TempData["Error"] = "Cannot find your suburb. Please select correct one.";
                return RedirectToAction("AddSchool");
            }
            //3. Check if correct school id
            if (!updateRaw.Any())
            {
                TempData["Error"] = "Cannot find your school. Pleas enter correct one.";
                return RedirectToAction("AddSchool");
            }

            var duplication = _context.Schools.Where(a =>
                a.SuburbId == suburbs.First().Id &&
                a.DomainExtension == updateRaw.First().DomainExtension &&
                a.NormalizedName == updateRaw.First().NormalizedName
                );
            //4. Check duplicate entry for school & campus
            if (duplication.Any())
            {
                if (duplication.First().Id != school.Id)
                {
                    TempData["Error"] = "School "+ duplication.First().SchoolName
                                                 + " & Campus " +suburbs.First().Name + " " + suburbs.First().PostCode
                                                 +" already in database.";
                    return RedirectToAction("AddSchool");
                }
            }
            var update = updateRaw.First();
            //5. Update school Name
            //TODO Can make a log in database in later version
            if (school.SchoolName != update.SchoolName)
            {
                if ((string) TempData["Inform"] != "")
                {
                    TempData["Inform"] += "\n";
                }
                TempData["Inform"] += "School " + update.SchoolName +"'s Name updated"
                                      +" from " + update.SchoolName
                                      +" to " + school.SchoolName;
                update.SchoolName = school.SchoolName;
                update.NormalizedName = school.SchoolName.ToUpper();
                _context.Update(update);
                _context.SaveChanges();
                await UpdateRelatedInfo(update.SchoolName, update.DomainExtension);
            }
            var finalSchoolName = school.SchoolName;
            //6. Update domain extension
            if (school.DomainExtension != update.DomainExtension)
            {
                if ((string) TempData["Inform"] != "")
                {
                    TempData["Inform"] += "\n";
                }
                TempData["Inform"] += "School "+finalSchoolName+"'s Domain Extension updated" 
                                      +" from " + update.DomainExtension 
                                      +" to "+ school.DomainExtension;
                update.DomainExtension = school.DomainExtension.ToLower();
                _context.Update(update);
                _context.SaveChanges();
                await UpdateRelatedInfo(update.SchoolName, update.DomainExtension);
            }
            
            //7. Update school suburb
            if (suburbs.First().Id != update.SuburbId)
            {
                if ((string) TempData["Inform"] != "")
                {
                    TempData["Inform"] += "\n";
                }

                var oldSuburbId = update.SuburbId;
                var newSuburbId = suburbs.First().Id;
                var oldSuburbName = _context.Suburbs.First(a => a.Id == oldSuburbId).Name;
                var oldSuburbPostCode = _context.Suburbs.First(a => a.Id == oldSuburbId).PostCode;
                var newSuburbName = _context.Suburbs.First(a => a.Id == newSuburbId).Name;
                var newSuburbPostCode = _context.Suburbs.First(a => a.Id == newSuburbId).PostCode;
                TempData["Inform"] += "School " + finalSchoolName+ "'s suburb updated"
                                                +" from "+ oldSuburbName + " "+ oldSuburbPostCode
                                                +" to "+ newSuburbName + " "+ newSuburbPostCode;
                update.SuburbId = newSuburbId;
                _context.Update(update);
                _context.SaveChanges();
            }

            //8. Update school status
            if (school.Status != update.Status)
            {
                if ((string) TempData["Inform"] != "")
                {
                    TempData["Inform"] += "\n";
                }
                string oldStatus;
                string newStatus;
                switch (update.Status)
                {
                    case SchoolStatus.InUse:
                        oldStatus = "In Use";
                        break;
                    case SchoolStatus.InRequest:
                        oldStatus = "In Request";
                        break;
                    default:
                        oldStatus = "No longer used";
                        break;
                }

                switch (school.Status)
                {
                    case SchoolStatus.InUse:
                        newStatus = "In Use";
                        break;
                    case SchoolStatus.InRequest:
                        newStatus = "In Request";
                        break;
                    default:
                        newStatus = "No longer used";
                        break;
                }
                TempData["Inform"] += "School " + finalSchoolName + "'s status updated"
                                      +" from "+ oldStatus
                                      +" to "+ newStatus;
                
                update.Status = school.Status;
                _context.Update(update);
                _context.SaveChanges();
            }
            TempData["Success"] = "School updated successfully";
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

        public ViewResult AddField()
        {
            ProcessSystemInfo();
            var result = _context.Fields
                .Select(a => 
                    new UpdateField
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
            InitialSystemInfo();
            //Check if front end input is valid
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("AddField");
            }
            //2. Check if already in database
            var checkInDatabase = _context.Fields
                .Where(a => a.NormalizedName == field.Name.ToUpper());
            Field newField;
            //2.1 Already in database
            if (checkInDatabase.Any())
            {
                string status;
                switch (checkInDatabase.First().Status)
                {
                    case FieldStatus.InUse:
                        status = "\"In Use\"";
                        break;
                    case FieldStatus.InRequest:
                        status = "\"In Request\"";
                        break;
                    default:
                        status = "\"No Longer Used\"";
                        break;
                }
                TempData["Inform"] = "Field \"" + field.Name + "\" is already in database. It was in " + status + ".";
                newField = checkInDatabase.First();
                newField.Status = FieldStatus.InUse;
            }
            //2.2 Not in database
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
            TempData["Success"] = "Field \"" + field.Name + "\" added!";
            return RedirectToAction("AddField");
        }

        [HttpPost]
        public RedirectToActionResult UpdateFieldAction(UpdateField field)
        {
            InitialSystemInfo();
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("AddField");
            }    
            var checkInDatabase = _context.Fields.Where(a => a.Id == field.Id);
            //Check if field id is not in database
            if (!checkInDatabase.Any())
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }

                TempData["Error"] += "Please enter correct field id!";
                return RedirectToAction("AddField");
            }
            //Check if duplicate field name with other field name
            var checkDuplication = _context.Fields
                .Where(a => a.NormalizedName == field.Name.ToUpper());
            if (checkDuplication.First().Id != field.Id)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }
                TempData["Error"] += "Entered duplicate field name "+ "\""+field.Name+"\" "+"!";
                return RedirectToAction("AddField");
            }
            var oldField = checkInDatabase.First();
            //Prepare to change status
            if (oldField.Status != field.Status)
            {
                string oldStatus;
                string newStatus;
                switch (oldField.Status)
                {
                    case FieldStatus.InUse:
                        oldStatus = "\"In Use\"";
                        break;
                    case FieldStatus.InRequest:
                        oldStatus = "\"In Request\"";
                        break;
                    default:
                        oldStatus = "\"No Longer Used\"";
                        break;
                }
                switch (field.Status)
                {
                    case FieldStatus.InUse:
                        newStatus = "\"In Use\"";
                        break;
                    case FieldStatus.InRequest:
                        newStatus = "\"In Request\"";
                        break;
                    default:
                        newStatus = "\"No Longer Used\"";
                        break;
                }
                oldField.Status = field.Status;
                _context.Fields.Update(oldField);
                _context.SaveChanges();
                if ((string) TempData["Inform"] != "")
                {
                    TempData["Inform"] += "\n";
                }
                TempData["Inform"] += "Field "+ "\""+field.Name+"\" "+"'s status changed from "
                                     + oldStatus + " to " 
                                     + newStatus;
            }
            //Prepare to change field name
            if (oldField.Name != field.Name)
            {
                var newName = field.Name;
                var oldName = oldField.Name;
                oldField.Name = field.Name;
                oldField.NormalizedName = field.Name.ToUpper();
                _context.Fields.Update(oldField);
                _context.SaveChanges();
                if ((string) TempData["Inform"] != "")
                {
                    TempData["Inform"] += "\n";
                }
                TempData["Inform"] += "Field"+ "\""+field.Name+"\" "+"'s name changed from "
                                     + oldName + " to " 
                                     + newName;
            }
            
            return RedirectToAction("AddField");
        }

        public ViewResult AddSkill()
        {
            ProcessSystemInfo();
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
        
        //检查是否已经是admin
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