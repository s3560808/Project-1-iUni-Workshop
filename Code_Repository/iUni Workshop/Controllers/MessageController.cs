using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
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
        //Property of message controller
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        //Constructor of message controller
        public MessageController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _context = context;
        }
        
        //View of MyMessages
        [Route("/Employer/MyMessages/")]
        [Route("/Employee/MyMessages/")]
        [Route("/Administrator/MyMessages")]
        [Authorize(Roles = Roles.Employer+","+Roles.Employee+","+Roles.Administrator)]
        public async Task<IActionResult> MyMessages()
        {
            //1. Process system information
            ProcessSystemInfo();
            var user = await _userManager.GetUserAsync(User);
            //2. Get all user's conversation
            var conversations = _context.Conversations
                .Where(a => a.User1Id == user.Id ||
                            a.User2Id == user.Id);
            var messages = new List<MyMessages>();
            //3. Get user's each conversation's latest information
            foreach (var conversation in conversations)
            {
                //Current user is receiver
                //Another user is sender
                //3.1 Get sender's id
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
                //3.2 Get latest messages status
                //3.2.1. Try to get latest messages status from another user
                try
                {
                    rawMessage = _context.Messages
                        .Where(a => a.ConversationId == conversation.Id && a.receiverId == user.Id)
                        .OrderByDescending(a => a.SentTime).First();
                }
                //3.2.2. If just send but no reply,
                //Get latest messages status from current user
                catch (InvalidOperationException)
                {
                    rawMessage = _context.Messages
                        .Where(a => a.ConversationId == conversation.Id && a.receiverId == senderId)
                        .OrderByDescending(a => a.SentTime).First();
                    rawMessage.Read = true;
                }
                //3.3. Initial latest current status
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
            //4. Return to view
            return View(messages.AsEnumerable());
        }
        
        
        [Route("/Employer/MessageDetail/")]
        [Route("/Employee/MessageDetail/")]
        [Route("/Administrator/MessageDetail/")]
        public async Task<IActionResult> MessageDetail(string conversationId, int invitationId)
        {
            MessageDetail result;
            IQueryable<Message> rawMessages;
            IQueryable<Conversation> conversations;
            var messages = new List<MessageDetailMessageInfo>();
            var user = await _userManager.GetUserAsync(User);
            //1. Process system information
            ProcessSystemInfo();
            //2. Try to get conversation information and corresponding messages 
            //2.1. If use invitation id to get message detail
            //TODO redirect my invitation
            if (invitationId != 0)
            {
                var invitations= _context.Invatations.Where(a => a.Id == invitationId);
                //2.1.1. No such invitation id in database
                if (!invitations.Any())
                {
                    AddToTempDataError("Invalid invitation id.1");
                    return RedirectToAction("MyMessages");
                }
                var invitation = invitations.First();
                var cvId = invitation.EmployeeCvId;
                var profileId = invitation.EmployerJobProfileId;
                var userId1 = _context.EmployeeCvs.First(a => a.Id == cvId).EmployeeId;
                var userId2 = _context.EmployerJobProfiles.First(a => a.Id == profileId).EmployerId;
                //2.2.2. If invitation id is belong to current user
                //2.2.2.1 Invitation id is belong to current user
                if (userId1 == user.Id || userId2 == user.Id)
                {
                    //2.2.2.2 Get conversation from database
                    conversations = _context.Conversations.Where(a => a.InvatationId == invitationId);
                    //2.2.2.3 If no previous conversation, create conversation
                    if (!conversations.Any())
                    {
                        result = new MessageDetail
                        {
                            Type = MessageType.UserMessage, 
                            InvitationId = invitations.First().Id
                        };
                        return View(result);
                    }
                    //2.2.2.4 Get all message of conversation
                    rawMessages = _context.Messages.Where(a => a.ConversationId == conversations.First().Id);
                }
                else
                {
                    AddToTempDataError("Invalid invitation id.2");
                    return RedirectToAction("MyMessages");
                }
            }
            //2.2. If use conversation id to get message detail
            else if (conversationId != "")
            {
                conversations= _context.Conversations.Where(a => a.Id == conversationId);
                //2.2.1 If no previous conversation
                if (!conversations.Any())
                {
                    AddToTempDataError("Invalid conversation id.1");
                    return RedirectToAction("MyMessages");
                }
                //2.2.2 If conversation belong to current user
                if (conversations.First().User1Id == user.Id ||conversations.First().User2Id == user.Id)
                {
                    rawMessages = _context.Messages.Where(a => a.ConversationId == conversations.First().Id);
                }
                else
                {
                    AddToTempDataError("Invalid conversation id.2");
                    return RedirectToAction("MyMessages");
                }
            }
            //2.3. NO conversationId, NO invitationId
            else
            {
                AddToTempDataError("Require correct conversation id or invitation id");
                return RedirectToAction("MyMessages");
            }
            //2.4. Process raw messages
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
                //2.4.1 Update message status to read
                if (updateMessage.receiverId == user.Id) {
                    updateMessage.Read = true;
                }
            }
            //2.5. Prepare for results
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

        [HttpPost]
        public async Task<RedirectToActionResult> SendMessage(SendMessage sendMessage)
        {
            InitialSystemInfo();
            //1. Validation
            //1.1 Validate user input from front end
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("MyMessages");
            }

            Conversation conversation = null;
            Invatation invitation = null;
            string receiverId;
            var user = await _userManager.GetUserAsync(User);
            //1.2 Cannot no conversation id & no invitation id at same time
            if (sendMessage.ConversationId == "" && sendMessage.InvitationId == 0)
            {
                AddToTempDataError("Please enter correct conversation & invitation!2");
                RedirectToAction("MyMessages");
            }
            //1.3 Validate conversation id
            if (sendMessage.ConversationId != "")
            {
                try
                {
                    //1.2.1 Get Conversation
                    conversation = _context.Conversations
                        .First(a => a.Id == sendMessage.ConversationId);
                    //1.2.2 Validate conversation if user is user in the Conversation
                    if (!(conversation.User1Id == user.Id || conversation.User2Id == user.Id))
                    {
                        AddToTempDataError("Please enter correct conversation id!1");
                        RedirectToAction("MyMessages");
                    }
                }
                catch (InvalidOperationException)
                {
                    //1.2.3 If invalid conversation id
                    AddToTempDataError("Please enter correct conversation id!2");
                    RedirectToAction("MyMessages");
                } 
            }
            //1.4 Validate invitation id
            //TODO redirect to my invitation
            if (sendMessage.InvitationId != 0)
            {
                try
                {
                    //1.3.1 Get invitation
                    invitation = _context.Invatations
                        .First(a => a.Id == sendMessage.InvitationId);
                    
                    //1.3.2 Check if invitation is belong to current user 
                    var cv = _context.EmployeeCvs.First(a => a.Id == invitation.EmployeeCvId);
                    var jobProfile = _context.EmployerJobProfiles.First(a => a.Id == invitation.EmployerJobProfileId);
                    if (!(cv.EmployeeId == user.Id || jobProfile.EmployerId == user.Id))
                    {
                        AddToTempDataError("Sorry! Incorrect invitation!1");
                        return RedirectToAction("MyMessages");
                    }
                    //1.3.3 Invitation rejected
                    if (invitation.status == InvitationStatus.Rejected)
                    {
                        AddToTempDataError("Sorry! You rejected corresponding invitation. Cannot reply this message");
                        return RedirectToAction("MyMessages");
                    }
                }
                catch (InvalidOperationException)
                {
                    //If invalid invitation id
                    AddToTempDataError("Sorry! Incorrect invitation2!");
                    return RedirectToAction("MyMessages");
                }
            }        
            
            //1.5 Validate Conversation with admin
            if (conversation != null && invitation == null)
            {
                //1.5.1 Validate if have admin user?
                var roles1 = await _userManager.GetRolesAsync(_context.Users.First(a => a.Id == conversation.User1Id));
                var roles2 = await _userManager.GetRolesAsync(_context.Users.First(a => a.Id == conversation.User2Id));
                if (!(roles1.Contains(Roles.Administrator)||roles2.Contains(Roles.Administrator)))
                {
                    //Not a conversation between admin
                    AddToTempDataError("Please enter correct conversation & invitation!3");
                    return RedirectToAction("MyMessages");
                }
                //1.5.2 If conversation is System message cannot reply
                if (conversation.Type == MessageType.System)
                {
                    AddToTempDataError("This is a system message cannot reply!");
                    return RedirectToAction("MyMessages");
                }
                receiverId = conversation.User1Id == user.Id ? conversation.User2Id : conversation.User1Id;
            }
            //1.6 Validate conversation between employer & employee
            else
            {
                //1.6.1 If not a new conversation
                if (conversation != null)
                {
                    if (conversation.InvatationId != invitation.Id)
                    {
                        AddToTempDataError("Correct invitation & conversation is required!1");
                        return RedirectToAction("MyMessages");
                    }
                    receiverId = conversation.User1Id == user.Id ? conversation.User2Id : conversation.User1Id;
                }
                //1.6.2 If a new conversation
                else
                {
                    if (sendMessage.InvitationId == 0)
                    {
                        AddToTempDataError("Correct invitation & conversation is required!2");
                        return RedirectToAction("MyMessages");
                    }
                    //1.6.2.1 Make a new conversation
                    conversation = new Conversation
                    {
                        InvatationId = sendMessage.InvitationId, 
                        Title = "", 
                        Type = MessageType.UserMessage
                    };

                    var employerId = _context.EmployerJobProfiles.First(a => a.Id == invitation.EmployerJobProfileId).EmployerId; 
                    var employeeId = _context.EmployeeCvs.First(a => a.Id == invitation.EmployeeCvId).EmployeeId;
                    conversation.User1Id = employerId;
                    conversation.User2Id = employeeId;
                
                    receiverId = conversation.User1Id == user.Id ? conversation.User2Id : conversation.User1Id;
                    _context.Conversations.Add(conversation);
                    _context.SaveChanges();
                }
            }
            //2. Prepare a new message
            var message = new Message
            {
                Read = false, 
                receiverId = receiverId,
                ConversationId = conversation.Id,
                MessageDetail = sendMessage.MessageDetail,
                SentTime = DateTime.Now
            };
            //3. Add new messages
            _context.Messages.Add(message);
            _context.SaveChanges();
            AddToTempDataSuccess("Your message sent!");
            return RedirectToAction("MyMessages");

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