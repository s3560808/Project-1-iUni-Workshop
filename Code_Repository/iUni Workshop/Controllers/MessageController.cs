using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUni_Workshop.Data;
using iUni_Workshop.Models;
using iUni_Workshop.Models.EmployeeModels;
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
    [Authorize(Roles = Roles.Employer+","+Roles.Employee)]
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
        public async Task<IActionResult> MyMessages()
        {
            var user = await _userManager.GetUserAsync(User);
            var conversations = _context.Conversations
                .Where(a => a.User1Id == user.Id ||
                            a.User2Id == user.Id);
            var messages = new List<MyMessages>();
            foreach (var conversation in conversations)
            {
                string sender;
                if (conversation.User1Id == user.Id)
                {
                    sender = _context.Users.First(a => a.Id == conversation.User2Id).Email;
                }
                else
                {
                    sender = _context.Users.First(a => a.Id == conversation.User1Id).Email;
                }

                var rawMessage = _context.Messages
                    .Where(a => a.ConversationId == conversation.Id && a.receiverId == user.Id)
                    .OrderByDescending(a => a.SentTime).First();
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
                string senderEmail;
                if (conversation.User1Id == receiver.Id)
                {
                    senderEmail = _context.Users.First(a => a.Id == conversation.User2Id).Email;
                }
                else
                {
                    senderEmail = _context.Users.First(a => a.Id == conversation.User1Id).Email;
                }

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
                Type = MessageType.UserMessage, 
                InvitationId = conversation.InvatationId, 
                Messages = messages.AsEnumerable().OrderByDescending(a => a.SentTime)
            };
            
            _context.Messages.UpdateRange(rawMessages);
            await _context.SaveChangesAsync();
            return View(result);
        }

        public async Task<RedirectToActionResult> SendMessage(SendMessage sendMessage)
        {
            if (!ModelState.IsValid)
            {
            }
            var user = await _userManager.GetUserAsync(User);
            Conversation conversation = null;
            //Check whether has pre-conversation
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
            return RedirectToAction("MyMessages");

        }
    }
}