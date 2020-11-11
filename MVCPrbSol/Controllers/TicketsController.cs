using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MVCPrbSol.Services;
using Microsoft.AspNetCore.Authorization;

namespace MVCPrbSol.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PSUser> _userManager;
        private readonly IPSHistoryService _historiesService;
        private readonly IPSRolesService _rolesService;
        #region Constructor Comments
        // The method below is a constructor. The purpose of the constructor is to initialize objects. 
        // Constructor's name MUST MATCH the class name, and cannot have a return type(like void or int).
        // A good way to think about setting up a contructor:
        // 1) Inject the service I want to reference as a parameter in the Constructor.
        // 2) Create a local, private readonly reference to the service injected. This goes above the Constructor.
        // 3) Assign the value of the injection to local reference 
        #endregion
        public TicketsController(ApplicationDbContext context, UserManager<PSUser> userManager, IPSHistoryService historyService, IPSRolesService rolesService)
        {
            _userManager = userManager;
            _context = context;
            _historiesService = historyService;
            _rolesService = rolesService;
        }

        // GET: Tickets/Index
        public async Task<IActionResult> Index(string userId)
        {
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
            var projectIds = new List<int>();
            var model = new List<Ticket>();
            var userProjects = _context.ProjectUsers.Where(pu => pu.UserId == userId).ToList();
            foreach (var record in userProjects)
            {
                projectIds.Add(_context.Projects.Find(record.ProjectId).Id);

            }
            foreach (var id in projectIds)
            {
                var tickets = _context.Tickets.Where(t => t.ProjectId == id).ToList();
                model.AddRange(tickets);
            }



            #region Linq Statements
            // The method below could be written out in one long string, but it is easier to read and understand
            // when it is divided like it is. The code below is saying "create a variable (applicationDbContext).
            // Then 
            #endregion
            var applicationDbContext = _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tickets/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
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
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create(int? id)
        {
            var model = new Ticket();
            model.ProjectId = (int)id;
            // Problem: When I try to create a ticket from the Ticket index view, an error occurs that pulls me to the line above. How do I fix that?

            #region ViewData/SelectLists
            // These are the fields that a user can select through when creating a ticket. For example, the user should see a select list for the following things; 
            // Developer assigned to ticket, owner of ticket, the project the ticket is assigned to, etc.
            //in parameters of new SelectList, second var. is what prints out
            #endregion
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");


            #region Role Authentication
            // This is an example of role authentication; Since the common user should not be selecting the ticket priority or status, this if UserIsInRole method
            // hides those select lists for anyone who is not logged in and in the role of Admin or PM. The else statement sets the ticket priority and status to a default
            // pending, which the admin/PM will later assess and change to the appropriate priority or status.
            #endregion
            if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
            {
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName");
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId")] Ticket ticket, IFormFile attachment)
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
                return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });       //When user creates a new ticket, they will be redirected back to the Projects/Details View instead of the default, which was "Index"
            }
            else
            {
                return NotFound();
            }
            //ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            //ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
            //ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            //ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            //return View(ticket);
        }


        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin,ProjecManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            //Need ticket comments to show in edit view so I can edit/archive them(as admin/PM)
            var ticketComment = await _context.TicketComments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            //Need to be able to edit ticket comments; Do I need a linq statement in the get/post or just ViewDataFields?
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId")] Ticket ticket)
        {

            ticket.Updated = DateTimeOffset.Now;



            if (id != ticket.Id)
            {
                return NotFound();
            }

            //Grabbing the Id of the old ticket
            Ticket oldTicket = await _context.Tickets
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

                //Add history; 
                string userId = _userManager.GetUserId(User);
                await _historiesService.AddHistory(oldTicket, ticket, userId);


                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
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