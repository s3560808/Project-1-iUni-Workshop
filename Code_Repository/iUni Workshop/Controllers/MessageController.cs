using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.EmployeeModels;
using iUni_Workshop.Models.InvatationModel;
using iUni_Workshop.Models.MessageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMessages = iUni_Workshop.Models.EmployerModels.MyMessages;
using MessageDetail = iUni_Workshop.Models.EmployerModels.MessageDetail;
using MessageDetailMessageInfo =  iUni_Workshop.Models.EmployerModels.MessageDetailMessageInfo;
using SendMessage = iUni_Workshop.Models.EmployerModels.SendMessage;

namespace iUni_Workshop.Controllers
{
    [Authorize(Roles = Roles.Employer+","+Roles.Employee+","+Roles.Administrator)]
    public class MessageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly SuburbController _suburbController;

        public MessageController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _context = context;
            _suburbController = new SuburbController(_context);
        }
        
        
        [Route("/Employer/MyMessages/")]
        [Route("/Employee/MyMessages/")]
        [Route("/Administrator/MyMessages")]
        [Authorize(Roles = Roles.Employer+","+Roles.Employee+","+Roles.Administrator)]
        public async Task<IActionResult> MyMessages()
        {
            ProcessSystemInfo();
            var user = await _userManager.GetUserAsync(User);
            var conversations = _context.Conversations
                .Where(a => a.User1Id == user.Id ||
                            a.User2Id == user.Id);
            var messages = new List<MyMessages>();
            foreach (var conversation in conversations)
            {
                string sender;
                string senderId;
                if (conversation.User1Id == user.Id)
                {
                    sender = _context.Users.First(a => a.Id == conversation.User2Id).Email;
                    senderId = _context.Users.First(a => a.Id == conversation.User2Id).Id; 
                }
                else
                {
                    sender = _context.Users.First(a => a.Id == conversation.User1Id).Email;
                    senderId = _context.Users.First(a => a.Id == conversation.User2Id).Id;
                }
                Message rawMessage;
                try
                {
                    rawMessage = _context.Messages
                        .Where(a => a.ConversationId == conversation.Id && a.receiverId == user.Id)
                        .OrderByDescending(a => a.SentTime).First();
                }
                //If just send but no reply
                catch (InvalidOperationException)
                {
                    rawMessage = _context.Messages
                        .Where(a => a.ConversationId == conversation.Id && a.receiverId == senderId)
                        .OrderByDescending(a => a.SentTime).First();
                    rawMessage.Read = true;
                }
                
                var message = new MyMessages
                {
                    ConversationId = conversation.Id, 
                    SenderEmail = sender,
                    Title = conversation.Title,
                    SentTime = rawMessage.SentTime,
                    Read = rawMessage.Read
                };
                messages.Add(message);
            }
            return View(messages.AsEnumerable());
        }
        
        
        //一旦reject不可发送
        //验证发送者和接收者
        [Route("/Employer/MessageDetail/")]
        [Route("/Employee/MessageDetail/")]
        public async Task<IActionResult> MessageDetail(string conversationId, int invitationId)
        {
            ProcessSystemInfo();
            MessageDetail result;
            IQueryable<Message> rawMessages;
            IQueryable<Conversation> conversations;
            List<MessageDetailMessageInfo> messages = new List<MessageDetailMessageInfo> { };
            var user = await _userManager.GetUserAsync(User);
            //From my invitations page
            if (invitationId != 0)
            {
                var invitations= _context.Invatations.Where(a => a.Id == invitationId);
                //No such invitation 
                if (!invitations.Any())
                {
                    //TODO no such invitation
                    return View();
                }
                
                conversations = _context.Conversations.Where(a => a.InvatationId == invitationId);
                //If no previous conversation, create conversation
                if (!conversations.Any())
                {
                    result = new MessageDetail
                    {
                        Type = MessageType.UserMessage, 
                        InvitationId = invitations.First().Id
                    };
                    return View(result);
                }
                rawMessages = _context.Messages.Where(a => a.ConversationId == conversations.First().Id);
            //From my messages page    
            }else if (conversationId != "")
            {
                conversations= _context.Conversations.Where(a => a.Id == conversationId);
                //If no previous conversation
                if (!conversations.Any())
                {
                    return View();
                }
                rawMessages = _context.Messages.Where(a => a.ConversationId == conversations.First().Id);
            }
            else
            {
                //No thing for this page
                return View();
            }

            var conversation = conversations.First();
            foreach (var updateMessage in rawMessages) {
                var receiver = _context.Users.First(a => a.Id == updateMessage.receiverId);
                var senderEmail = conversation.User1Id == receiver.Id ? 
                    _context.Users.First(a => a.Id == conversation.User2Id).Email : 
                    _context.Users.First(a => a.Id == conversation.User1Id).Email;
                messages.Add(new MessageDetailMessageInfo
                {
                    SenderName = senderEmail, 
                    Detail = updateMessage.MessageDetail, 
                    SentTime = updateMessage.SentTime
                });
                if (updateMessage.receiverId == user.Id) {
                    updateMessage.Read = true;
                }
            }

            result  = new MessageDetail
            {
                Type = conversation.Type, 
                InvitationId = conversation.InvatationId, 
                ConversationId = conversation.Id,
                Messages = messages.AsEnumerable().OrderByDescending(a => a.SentTime)
            };
            
            _context.Messages.UpdateRange(rawMessages);
            await _context.SaveChangesAsync();
            return View(result);
        }

        public async Task<RedirectToActionResult> SendMessage(SendMessage sendMessage)
        {
            InitialSystemInfo();
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("MyMessages");
            }
            ApplicationUser user = null;
            Conversation conversation = null;
            Invatation invitation = null;
            string receiverId;
            
            user = await _userManager.GetUserAsync(User);
            if (sendMessage.ConversationId != null)
            {
                try
                {
                    //Get Conversaiton
                    conversation = _context.Conversations
                        .First(a => a.Id == sendMessage.ConversationId);
                    //Validate conversation if user is user in the Conversation
                    if (!(conversation.User1Id == user.Id || conversation.User2Id == user.Id))
                    {
                        TempData["Error"] += "Please enter correct conversation id!";
                    }
                }
                catch (InvalidOperationException)
                {
                    //If invalid conversation id
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                    TempData["Error"] += "Please enter correct conversation id!";
                } 
            }
            if (sendMessage.InvitationId != 0)
            {
                try
                {
                    //Get invitation
                    invitation = _context.Invatations
                        .First(a => a.Id == sendMessage.InvitationId);
                    if (invitation.status == InvitationStatus.Rejected)
                    {
                        {
                            TempData["Error"] += "\n";
                        }
                        TempData["Error"] += "Sorry! You rejected corresponding invitation. Cannot reply this message";
                        return RedirectToAction("MessageDetail", new{invitationId = invitation.Id});
                    }
                    //If invitation is not null validate invitation
                    //If have invitation it means not system message
                    var cv = _context.EmployeeCvs.First(a => a.Id == invitation.EmployeeCvId);
                    var jobProfile = _context.EmployerJobProfiles.First(a => a.Id == invitation.EmployerJobProfileId);
                    if (!(cv.EmployeeId == user.Id || jobProfile.EmployerId == user.Id))
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }
                        TempData["Error"] += "Please enter correct invitation id!";
                    }
                }
                catch (InvalidOperationException)
                {
                    //If invalid invitation id
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                    TempData["Error"] += "Please enter correct invitation id!";
                }
            }        
            if (sendMessage.ConversationId == "" && sendMessage.InvitationId == 0)
            {
                if ((string) TempData["Error"] != "")
                {
                    TempData["Error"] += "\n";
                }
                TempData["Error"] += "Please enter correct conversation & invitation!2";
            }
            if ((string) TempData["Error"]!="")
            {
                return RedirectToAction("MyMessages");
            }
            //Validate Conversation with admin?
            if (conversation != null && invitation == null)
            {
                //Validate if have admin user?
                var roles1 = await _userManager.GetRolesAsync(_context.Users.First(a => a.Id == conversation.User1Id));
                var roles2 = await _userManager.GetRolesAsync(_context.Users.First(a => a.Id == conversation.User2Id));
                if (!(roles1.Contains(Roles.Administrator)||roles2.Contains(Roles.Administrator)))
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                    TempData["Error"] += "Please enter correct conversation & invitation!3";
                    return RedirectToAction("MyMessages");
                }
                //Check if system message?
                if (conversation.Type == MessageType.System)
                {
                    if ((string) TempData["Error"] != "")
                    {
                        TempData["Error"] += "\n";
                    }
                    TempData["Error"] += "Please enter correct conversation & invitation!4";
                    return RedirectToAction("MyMessages");
                }
                receiverId = conversation.User1Id == user.Id ? conversation.User2Id : conversation.User1Id;
            }
            //Validate conversation between employer & employee
            else
            {
                if (conversation != null)
                {
                    if (conversation.InvatationId != invitation.Id)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }
                        TempData["Error"] += "Please enter correct conversation & invitation!5";
                        return RedirectToAction("MyMessages");
                    }

                    if (conversation.InvatationId != invitation.Id)
                    {
                        if ((string) TempData["Error"] != "")
                        {
                            TempData["Error"] += "\n";
                        }
                        TempData["Error"] += "Please enter correct conversation & invitation!6";
                        return RedirectToAction("MyMessages");
                    }
                    receiverId = conversation.User1Id == user.Id ? conversation.User2Id : conversation.User1Id;
                }
                else
                {
                    conversation = new Conversation();
                    if (sendMessage.InvitationId != 0)
                    conversation.InvatationId = sendMessage.InvitationId;
                    conversation.Title = "";
                    conversation.Type = MessageType.UserMessage;
                    var employerId = _context.EmployerJobProfiles.First(a => a.Id == invitation.EmployerJobProfileId).EmployerId; 
                    conversation.User1Id = employerId;
                    var employeeId = _context.EmployeeCvs.First(a => a.Id == invitation.EmployeeCvId).EmployeeId;
                    conversation.User2Id = employeeId;
                
                    receiverId = conversation.User1Id == user.Id ? conversation.User2Id : conversation.User1Id;
                    _context.Conversations.Add(conversation);
                    _context.SaveChanges();
                }
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
            return RedirectToAction("MyMessages");

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

    }
}