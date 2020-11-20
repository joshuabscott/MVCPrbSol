using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Services;
using MVCPrbSol.Utilities;

namespace MVCPrbSol.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PSUser> _userManager;
        private readonly IPSHistoryService _historyService;
        private readonly IPSAccessService _accessService;
        private readonly IPSRolesService _rolesService;
        private readonly IPSTicketService _ticketService;

        #region Constructor Comments
        // The method below is a constructor. The purpose of the constructor is to initialize objects. 
        // Constructor's name MUST MATCH the class name, and cannot have a return type(like void or int).
        // A good way to think about setting up a contructor:
        // 1) Inject the service I want to reference as a parameter in the Constructor.
        // 2) Create a local, private readonly reference to the service injected. This goes above the Constructor.
        // 3) Assign the value of the injection to local reference 
        #endregion
        public TicketsController(ApplicationDbContext context, UserManager<PSUser> userManager, IPSHistoryService historyService, IPSRolesService rolesService, IPSAccessService accessService)
        {
            _userManager = userManager;
            _context = context;
            _historyService = historyService;
            _rolesService = rolesService;
            _accessService = accessService;
        }

        #region All Tickets Index
        // Shows all tickets created in the Tickets/Index View.
        //
        // The code block for this action declares a local var. called applicationDbContext.
        // The local var. is assigned the value of a LINQ statement that queries the database for 
        // info. 
        // 
        // The block reads something like this: "local var applicationDbContext is going into
        // the db (_context is our reference to the db that we injected in our constructor) and grabbing 
        // the Tickets table. The .Include statements are including the information we want from the Tickets
        // table, such as the DeveloperUser column info, OwnerUser column info, etc. Finally, the return 
        // statement at the end of the block returns the view of all of the information we asked our local
        // var to hold and puts in in an async list with the .ToListAsync() method (this is why the await is 
        // needed in front of the local var)".
        #endregion
        // GET: Tickets Index
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tickets
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType);

            return View(await applicationDbContext.ToListAsync());
        }


        #region My Tickets Action (For users to see their tickets)
        // Shows a filtered version of all tickets created for specific users; For example, if i'm signed in
        // as Joe Shmoe and go to the appropriate view (Tickets/MyTickets), I will only see the tickets that 
        // Joe Shmoe has created. This method allows a user to quickly navigate to tickets that they've submitted
        // instead of having to look through all tickets (Tickets/Index)
        //
        // The codeblock for this action declares two local var's in the beginning: model is assigned a new list of type
        // Ticket, while userId is assigned the value of User by using the injected reference of UserManager and the method
        // .GetUserId. The following code reads something like "If the user is in the role of Admin, model will be assigned the
        // value of a list of info queried from the Tickets table" (using a LINQ statment). Since Admin is GodMode, this is all
        // of the info. The codeblock continues "else if the user in role is the Project Manager, only show a list of tickets
        // that are associated with the ProjectManager"
        //
        //Previous Action name that was asynchronous: public async Task<IActionResult> MyTickets()
        #endregion
        // GET: MyTickets Index
        public IActionResult MyTickets()
        {   //Create an item of type "new List" of type Ticket
            var model = new List<Ticket>();
            var userId = _userManager.GetUserId(User);

            #region Method for showing tickets to User who owns them (MyTickets method + associated view)
            if (User.IsInRole("Administrator"))
            {
                model = _context.Tickets
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType).ToList();
            }
            else if (User.IsInRole("ProjectManager"))
            {
                var projectIds = new List<int>();
                //Actively grabbing data I want NOW;
                var userProjects = _context.ProjectUsers.Where(pu => pu.UserId == userId).ToList();


                foreach (var record in userProjects)
                {
                    projectIds.Add(record.ProjectId);

                }
                foreach (var id in projectIds)
                {
                    var tickets = _context.Tickets.Where(t => t.ProjectId == id)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType).ToList();
                    model.AddRange(tickets);
                    //If and if elses are useful for ranges
                }
            }
            else if (User.IsInRole("Developer"))
            {
                model = _context.Tickets
                    //This will grab ONLY criteria i'm trying to match
                    .Where(t => t.DeveloperUserId == userId || t.OwnerUserId == userId)

                    .Include(t => t.OwnerUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType).ToList();
            }
            else if (User.IsInRole("Submitter") || User.IsInRole("NewUser"))
            {
                model = _context.Tickets
                    .Where(t => t.OwnerUserId == userId)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.Project)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType).ToList();

            }
            else
            {
                return NotFound();
            }
            return View(model);
            #endregion


            #region 11/10 Code that Drew said had something to do with showing tickets to specific users;

            // What is this? why is this? what the heck is going on

            //var user = _userManager.GetUserId(User);
            //var myRole = await _rolesService.ListUserRoles(_context.Users.Find(user));
            //var test = myRole.FirstOrDefault();
            //switch (test)
            //{
            //    case "Admin":
            //        var model = _context.Tickets.ToList();
            //        break;
            //    case "ProjectManager":
            //        model = 1; //The foreach loops
            //        break;
            //    case "Developer":
            //    case "NewUser":
            //        model = _context.Tickets.Where(t => t.DeveloperUserId == user).ToList(); //The foreach loops
            //        break;
            //    case "Submitter":
            //        model = _context.Tickets.Where(t => t.OwnerUserId == user).ToList(); //The foreach loops
            //        break;
            //}

            //This is supposed to have something to do with PM's getting tickets? 
            //var projectIds = new List<int>();
            //var model = new List<Ticket>();
            //var userProjects = _context.ProjectUsers.Where(pu => pu.UserId == userId).ToList();
            //foreach (var record in userProjects)
            //{
            //    projectIds.Add(_context.Projects.Find(record.ProjectId).Id);

            //}
            //foreach (var id in projectIds)
            //{
            //    var tickets = _context.Tickets.Where(t => t.ProjectId == id).ToList();
            //    model.AddRange(tickets);
            //}
            #endregion


            #region Link Statements
            // The method below could be written out in one long string, but it is easier to read and understand
            // when it is divided like it is. The code below is saying "create a variable (applicationDbContext).
            // Then 

            //var applicationDbContext = _context.Tickets
            //    .Include(t => t.DeveloperUser)
            //    .Include(t => t.OwnerUser)
            //    .Include(t => t.Project)
            //    .Include(t => t.TicketPriority)
            //    .Include(t => t.TicketStatus)
            //    .Include(t => t.TicketType);
            //return View(await applicationDbContext.ToListAsync());
            #endregion
        }

        // GET: Tickets/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //These are referencing the services we injected into the constructor. We'll need these
            //for the code below
            var userId = _userManager.GetUserId(User);
            var roleName = (await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))).FirstOrDefault();

            //if (await _accessService.CanInteractTicket(userId, (int)id, roleName))
            //{
            #region Lamda Expression
            //Lamda Expression Notes: 
            //var ticket, which we create, Says wer're going to the database and inside the ticket table , 
            //and we go into Ticket Table. From there we include Developer User, Owner User columns, etc. first or default is a statement saying 
            //"give me the first row where the first row id is the same as the id's we were given."
            #endregion
            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.Comments).ThenInclude(tc => tc.User)
                .Include(t => t.Histories)
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);

            //}
            //Possible Returns, 2 lines below
            //TempData["InvalidAccess"] = "You do not have access to this ticket. Contact administrator for help";
            //return RedirectToAction("Index");
            return RedirectToAction("MyTickets", "Tickets");
        }



        // GET: Tickets/Create
        public IActionResult Create(int? id)
        {
            var model = new Ticket();

            //This if statement is used(along with view logic) to let me create a new ticket and show the drop-down menu
            if (id != null)
            {
                model.ProjectId = (int)id;
            }



            #region ViewData/SelectLists
            // These are the fields that a user can select through when creating a ticket. For example, the user should see a select list for the following things; 
            // Developer assigned to ticket, owner of ticket, the project the ticket is assigned to, etc.
            //in parameters of new SelectList, second var. is what prints out
            #endregion
            //ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");


            #region Role Authentication
            // This is an example of role authentication; Since the common user should not be selecting the ticket priority or status, this if UserIsInRole method
            // hides those select lists for anyone who is not logged in and in the role of Administrator or PM. The else statement sets the ticket priority and status to a default
            // pending, which the administrator/PM will later assess and change to the appropriate priority or status.
            #endregion
            if (User.IsInRole("Administrator") || User.IsInRole("ProjectManager"))
            {
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName"); //Need to filter this to only developers
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
            }
            else
            {
                model.TicketTypeId = _context.TicketTypes.Where(tp => tp.Name == "Pending").FirstOrDefault().Id; ;
                model.TicketPriorityId = _context.TicketPriorities.Where(tp => tp.Name == "Pending").FirstOrDefault().Id;
                model.TicketStatusId = _context.TicketStatuses.Where(tp => tp.Name == "Pending").FirstOrDefault().Id; ;
                model.DeveloperUserId = null; //Nullable by default
            }
            return View(model);
        }


        // POST: Tickets/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId")] Ticket ticket, IFormFile attachment)
        {
            if (!User.IsInRole("Demo"))
            {

                if (ModelState.IsValid)
                {
                    #region Default Settings set in the Controller
                    //We don't want the ticker submitter to be able to set the date/submitter name, so we are doing it by default for them.
                    // The constructor is partly responsible for being able to reference getting user id, the ticket model, etc
                    #endregion
                    ticket.Created = DateTimeOffset.Now;
                    ticket.OwnerUserId = _userManager.GetUserId(User);

                    #region Using our Attachment Handler Class
                    // This block is saying "If my attachment is not null, var attachmentHandler 
                    // is a new instance of class AttachmentHandler. var ticket goes to Attachments and adds the attachment in the second line
                    #endregion
                    if (attachment != null)
                    {
                        AttachmentHandler attachmentHandler = new AttachmentHandler();
                        ticket.Attachments.Add(attachmentHandler.Attach(attachment));
                    }

                    _context.Add(ticket);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("MyTickets", "Tickets");
                    /*return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });*/       //When user creates a new ticket, they will be redirected back to the Projects/Details View instead of the default, which was "Index"
                }
                else
                {
                    return NotFound();
                }

            }
            else
            {
                TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database, please log in as a full user.";
                return RedirectToAction("MyTickets", "Tickets");

            }
        }

        // GET: Tickets/Edit/5
        //[Authorize(Roles = "Administrator, ProjectManager, Developer")]
        public async Task<IActionResult> Edit(int? id)
        {
            //Need ticket comments to show in edit view so I can edit/archive them(as administrator/PM)
            var ticketComment = await _context.TicketComments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.OwnerUser)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            //Need to be able to edit ticket comments; Do I need a link statement in the get/post or just ViewDataFields?
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
            //OwnerUserId is retained in the get when editing a ticket, but as soon as the person tries to
            //save the changes (in the post) they are removed as the ticket owner(OwnerUserId/OwnerUser)
        }

        // POST: Tickets/Edit
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,OwnerUserId,TicketStatusId,DeveloperUserId")] Ticket ticket)
        {
            //Does not save changes for a Demo User
            if (!User.IsInRole("Demo"))
            {
                ticket.Updated = DateTimeOffset.Now;

                if (id != ticket.Id)
                {
                    return NotFound();
                }

                //"Snapshot" of the old ticket

                Ticket oldTicket = await _context.Tickets
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.Project)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticket.Id);

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(ticket);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TicketExists(ticket.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                //Add history; 
                string userId = _userManager.GetUserId(User);
                Ticket newTicket = await _context.Tickets
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.DeveloperUser)
                .Include(t => t.Project)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == ticket.Id);
                await _historyService.AddHistory(oldTicket, newTicket, userId);




                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
                ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);



                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
                //return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["DemoLockout"] = "Your changes have not beeen saved. To make changes to the database, please log in as a full user.";
                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
            }

        }



        // GET: Tickets/Delete/5
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}





//The Logic to create an instance of an Object : Ticket