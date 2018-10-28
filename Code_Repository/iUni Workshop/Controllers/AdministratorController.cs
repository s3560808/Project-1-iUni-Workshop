using System;
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
        //Properties of admin controller
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        //Constructor of admin controller
        public AdministratorController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager
            ) {
            _userManager = userManager;
            _context = context;
        }

        //Dashboard
        public ViewResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ViewResult AddSchool()
        {
            //1. Process system information from AddSchoolAction()/UpdateSchoolAction()
            ProcessSystemInfo();
            //2. Get all schools in database
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
            //3. Go to view
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddSchoolAction(AddSchoolAction school)
        {
            InitialSystemInfo();
            const int schoolStatus = SchoolStatus.InUse;
            //1. Check if correct input from front-end
            if (!ModelState.IsValid)
            {
                foreach (var model in ModelState)
                {
                    if (model.Value.Errors.Count == 0) continue;
                    if (model.Key == "PostCode")
                    {
                        AddToTempDataError("Correct postcode is required");
                    }
                    else
                    {
                        foreach (var error in model.Value.Errors)
                        {
                            AddToTempDataError(error.ErrorMessage);
                        }
                    }
                }
                return RedirectToAction("AddSchool");
            }       
            //2. Check if user entered correct suburb
            var suburbs = _context.Suburbs
                .Where(a => a.Name == school.SuburbName.ToUpper() && a.PostCode == school.PostCode);
            //2.1 If wrong suburb, give error and
            //return to AddSchool()
            if (!suburbs.Any())
            {
                AddToTempDataError("Cannot find your Suburb, Please select correct one");
                return RedirectToAction("AddSchool");
            }
            //3.Check if new a school or already in database 
            var checkWhetherUpdate = _context.Schools.Where(
                a => 
                    a.SchoolName == school.SchoolName && 
                    a.SuburbId == suburbs.First().Id &&
                    a.DomainExtension == school.DomainExtension.ToLower()
                );
            School newSchool;
            //3.1 Update an exist school's status to "in use"
            if (checkWhetherUpdate.Any())
            {
                var oldStatus = checkWhetherUpdate.First().Status;
                var oldLocationName = _context.Suburbs
                    .First(a => a.Id == checkWhetherUpdate.First().SuburbId).Name;
                newSchool = checkWhetherUpdate.First();
                newSchool.SchoolName = school.SchoolName;
                newSchool.DomainExtension = school.DomainExtension;
                newSchool.Status = schoolStatus;
                var information = "It is not a new school. " 
                                      + newSchool.SchoolName + " in " 
                                      + oldLocationName+" campus was in ";
                switch (oldStatus)
                {
                    case SchoolStatus.InUse:
                        information  += "\"In Use\"";
                        break;
                    case SchoolStatus.InRequest:
                        information  += "\"In Request\"";
                        break;
                    default:
                        information += "\"No Longer Used\"";
                        break;
                }
                AddToTempDataInform(information);
            }
            //3.2 Insert a new school
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
            //4. Update user required school
            _context.Schools.Update(newSchool);
            await _context.SaveChangesAsync();
            //5. To update school name if school name changes
            await UpdateRelatedInfo(newSchool.SchoolName, newSchool.DomainExtension);
            TempData["Success"] = "School "+school.SchoolName+" "+school.SuburbName+" inserted successfully";
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

        [HttpGet]
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
            //1. Check if front end input is valid
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("AddField");
            }
            //2. Check if already in database
            var checkInDatabase = _context.Fields
                .Where(a => a.NormalizedName == field.Name.ToUpper());
            Field newField;
            //2.1 if already in database
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
                AddToTempDataInform("Field \"" + field.Name + "\" is already in database. It was in " + status + ".");
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
            AddToTempDataSuccess("Field \"" + field.Name + "\" added!");
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
            //1. Check if field id is not in database
            if (!checkInDatabase.Any())
            {
                AddToTempDataError("Please enter correct field id!");
                return RedirectToAction("AddField");
            }
            //2. Check if duplicate field name with other field name
            var checkDuplication = _context.Fields
                .Where(a => a.NormalizedName == field.Name.ToUpper());
            if (checkDuplication.Any() && checkDuplication.First().Id != field.Id)
            {
                AddToTempDataError("Entered duplicate field name "+ "\""+field.Name+"\" "+"!");
                return RedirectToAction("AddField");
            }
            var oldField = checkInDatabase.First();
            //3. Prepare to change status
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
                AddToTempDataSuccess("Field "+ "\""+field.Name+"\" "+"'s status changed from "
                                    + oldStatus + " to " 
                                    + newStatus);
            }
            //4. Prepare to change field name
            if (oldField.Name != field.Name)
            {
                var newName = field.Name;
                var oldName = oldField.Name;
                oldField.Name = field.Name;
                oldField.NormalizedName = field.Name.ToUpper();
                _context.Fields.Update(oldField);
                _context.SaveChanges();
                AddToTempDataSuccess("Field"+ "\""+field.Name+"\" "+"'s name changed from "
                                     + oldName + " to " 
                                     + newName);
            }
            AddToTempDataSuccess("Field \"" + field.Name + "\" updated!");
            return RedirectToAction("AddField");
        }

        [HttpGet]
        public ViewResult AddSkill()
        {
            ProcessSystemInfo();
            var result = _context.Skills
                .Select(a => 
                    new UpdateSkill
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
            InitialSystemInfo();
            //1. Check if front-end input is valid
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("AddSkill");
            }
            //2. Check if already in database
            var checkInDatabase = _context.Skills.Where(a => a.NormalizedName == skill.Name.ToUpper());
            Skill newSkill;
            //2.1 if already in database
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
                AddToTempDataInform("Field \"" + skill.Name + "\" is already in database. It was in " + status + ".");
                newSkill = checkInDatabase.First();
                newSkill.Status = SkillStatus.InUse;
            }
            //2.2 if not in database
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
            //3. Update skill
            _context.Skills.Update(newSkill);
            await _context.SaveChangesAsync();
            AddToTempDataSuccess("Field \"" + newSkill.Name + "\" added!");
            return RedirectToAction("AddSkill");
        }
        
        [HttpPost]
        public RedirectToActionResult UpdateSkillAction(UpdateSkill skill)
        {
            InitialSystemInfo();
            //1. Check if front-end input is valid
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("AddSkill");
            }    
            //2. Check if id is correct
            var checkInDatabase = _context.Skills.Where(a => a.Id == skill.Id);
            if (!checkInDatabase.Any())
            {
                AddToTempDataError("Please enter correct skill id!");
                return RedirectToAction("AddSkill");
            }
            //3. Check if duplicate field name with other field's name
            var checkDuplication = _context.Skills
                .Where(a => a.NormalizedName == skill.Name.ToUpper());
            if (checkDuplication.Any()&&checkDuplication.First().Id != skill.Id)
            {
                AddToTempDataError("Entered duplicate skill name "+ "\""+skill.Name+"\" "+"!");
                return RedirectToAction("AddSkill");
            }
            var oldSkill = checkInDatabase.First();
            //4. Change skill status
            if (oldSkill.Status != skill.Status)
            {
                string oldStatus;
                string newStatus;
                switch (oldSkill.Status)
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
                switch (skill.Status)
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

                oldSkill.Status = skill.Status;
                _context.Skills.Update(oldSkill);
                _context.SaveChanges();
                AddToTempDataSuccess("Skill "+ "\""+oldSkill.Name+"\" "+"'s status changed from "
                                    + oldStatus + " to " 
                                    + newStatus);
            }
            //5. Change skill name
            if (oldSkill.Name != skill.Name)
            {
                var newName = skill.Name;
                var oldName = oldSkill.Name;
                oldSkill.Name = skill.Name;
                oldSkill.NormalizedName = skill.Name.ToUpper();
                _context.Skills.Update(oldSkill);
                _context.SaveChanges();
                if ((string) TempData["Inform"] != "")
                {
                    TempData["Inform"] += "\n";
                }
                TempData["Inform"] += "Skill"+ "\""+skill.Name+"\" "+"'s name changed from "
                                      + oldName + " to " 
                                      + newName;
                AddToTempDataSuccess("Skill" + "\"" + skill.Name + "\" " + "'s name changed from "
                                     + oldName + " to "
                                     + newName);
            }
            TempData["Success"] = "Skill \"" + skill.Name + "\" updated!";
            return RedirectToAction("AddSkill");
        }

        public async Task<IActionResult> SetUserType()
        {
            ProcessSystemInfo();
            var users = _userManager.Users.ToList();
            var admins = (await _userManager.GetUsersInRoleAsync(Roles.Administrator)).ToList();
            var results = users.Except(admins).ToList();
            return View(results);
        }
        
        [HttpPost]
        public async Task<IActionResult> SetUserTypeAction(SetUserType setUserType)
        {          
            InitialSystemInfo();
            if (!ModelState.IsValid)
            {
                ProcessModelState();
            }

            ApplicationUser user;
            //Check if user exist?
            try
            {
                user = _context.Users.First(a => a.Id == setUserType.Id);
            }
            catch (InvalidOperationException)
            {
                TempData["Error"] = "Please select correct user!";
                return RedirectToAction("SetUserType");
            }
            //Check if already a Administrator
            var roles = await _userManager.GetRolesAsync(user);
            if(roles.Any() && roles.Contains(Roles.Administrator))
            {
                TempData["Error"] = "User " + user.Email + "already has \"Administrator\" role! Cannot Add!";
                return RedirectToAction("SetUserType");
            }
            //Add user's role
            await _userManager.AddToRoleAsync(user, Roles.Administrator);
            _context.Administraotrs.Add(new Administraotr { Id = user.Id, Name = user.UserName});
            await _context.SaveChangesAsync();
            TempData["Success"] = "User" + user.Email + " added \"Administrator\" role!";
            return RedirectToAction("SetUserType");
        }

        public ViewResult NewMessage()
        {
            ProcessSystemInfo();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewMessageAction(NewMessage message)
        {
            InitialSystemInfo();
            if (!ModelState.IsValid)
            {
                ProcessModelState();
                return RedirectToAction("NewMessage");
            }

            string receiverId;
            try{
                receiverId = _context.Users.First(a => a.Email == message.Email).Id;
            }catch(InvalidOperationException){
                TempData["Error"] = "Please enter correct receiver!";
                return RedirectToAction("NewMessage");
            }
            
            //Create new conversation
            var newConversation = new Conversation
            {
                User1Id = (await _userManager.GetUserAsync(User)).Id,
                User2Id = receiverId,
                Title = message.Title,
                Type = message.Type
            };
            _context.Conversations.Add(newConversation);
            _context.SaveChanges();
            //Create new message
            var newMessage = new Message
            {
                ConversationId = newConversation.Id,
                receiverId = receiverId,
                SentTime = DateTime.Now,
                Read = false,
                MessageDetail = message.MessageDetail,
            };
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
            TempData["Success"] = "New message sent!";
            return RedirectToActionPermanent("MyMessages","Message");
        }

        [Route("[Controller]/GetUsers/{userName}/")]
        public IActionResult GetUsers(string userName)
        {
            var receiver = _context.Users.Where(a => a.UserName.Contains(userName)).ToList();
            return Json(receiver);
        }
        
        public IActionResult CertificateCompanies()
        {
            ProcessSystemInfo();
            var companies = _context.Employers.Where(a => a.RequestCertification);
            return View(companies.AsEnumerable());
        }
        
        public async Task<IActionResult> CertificateCompaniesAction(CertificateCompaniesAction certification)
        {
            var user = await _userManager.GetUserAsync(User);
            InitialSystemInfo();
            if (!ModelState.IsValid)
            {
                ProcessModelState();
            }
            try
            {
                var employer = _context.Employers.First(a => a.Id == certification.Id);
                if (employer.Certificated)
                {
                    AddToTempDataInform("Already certificated");
                    return RedirectToAction("CertificateCompanies");
                }
                employer.RequestCertification = false;
                employer.Certificated = true;
                employer.CertificatedBy = user.Id;
                _context.Employers.Update(employer);
                _context.SaveChanges();
                AddToTempDataSuccess("Company "+ employer.Name+" certificate successfully!");
            }
            catch (InvalidOperationException)
            {
                AddToTempDataError("Invalid company id");
            }
            return RedirectToAction("CertificateCompanies");
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

        private void AddToViewBagInform(string informMessage)
        {
            if ((string) ViewBag.Inform != "")
            {
                ViewBag.Inform += "\n";
            }
            ViewBag.Inform += informMessage;
        }
        
        private void AddToViewBagError(string errorMessage)
        {
            if ((string) ViewBag.Error != "")
            {
                ViewBag.Error+= "\n";
            }
            ViewBag.Error += errorMessage;
        }
        
        private void AddToViewBagSuccess(string successMessage)
        {
            if ((string) ViewBag.Success != "")
            {
                ViewBag.Success += "\n";
            }
            ViewBag.Success += successMessage;
        }
        
        private void AddToTempDataSuccess(string successMessage)
        {
            if ((string) TempData["Success"] != "")
            {
                TempData["Success"] += "\n";
            }
            TempData["Success"] += successMessage;
        }
        
        private void AddToTempDataInform(string informMessage)
        {
            if ((string) TempData["Inform"] != "")
            {
                TempData["Inform"] += "\n";
            }
            TempData["Inform"] += informMessage;
        }
        
        private void AddToTempDataError(string errorMessage)
        {
            if ((string) TempData["Error"] != "")
            {
                TempData["Error"] += "\n";
            }
            TempData["Error"] += errorMessage;
        }
    }
}