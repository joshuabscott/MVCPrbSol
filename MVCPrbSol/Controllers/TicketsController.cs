using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Services;
using MVCPrbSol.Utilities;

namespace MVCPrbSol.Controllers   //Namespace is the outermost , Inside is a class, than a method, than the logic
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PSUser> _userManager;
        private readonly IPSHistoriesService _historiesService;
        private readonly IPSRolesService _rolesService;
        private readonly IPSAccessService _accessService;

       
        public TicketsController(ApplicationDbContext context, UserManager<PSUser> userManager, IPSHistoriesService historiesService, IPSRolesService rolesService, IPSAccessService accessService)
        {
            _userManager = userManager;
            _context = context;
            _historiesService = historiesService;
            _rolesService = rolesService;
            _accessService = accessService;
        }

       
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


        // GET: MyTickets Index
        public IActionResult MyTickets()
        {   //Create an item of type "new List" of type Ticket
            var model = new List<Ticket>();
            var userId = _userManager.GetUserId(User);

            #region Method for showing tickets to User who owns them (MyTickets method + associated view)
            if (User.IsInRole("Admin"))
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
                    //If and if else are useful for ranges
                }
            }
            else if (User.IsInRole("Developer"))
            {
                model = _context.Tickets
                    //This will grab ONLY criteria
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


            
        }

        // GET: Tickets/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var userId = _userManager.GetUserId(User);
            var roleName = (await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))).FirstOrDefault();

            
            
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

            //return View(ticket);

            
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



           
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");


           
            if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
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
                model.DeveloperUserId = null; //Null by default
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

                    
                    ticket.Created = DateTimeOffset.Now;
                    ticket.OwnerUserId = _userManager.GetUserId(User);

                    if (attachment != null)
                    {
                        AttachmentHandler attachmentHandler = new AttachmentHandler();
                        ticket.Attachments.Add(attachmentHandler.Attach(attachment));
                    }

                    _context.Add(ticket);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("MyTickets", "Tickets");
                   
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
        //[Authorize(Roles = "Admin, ProjectManager, Developer")]
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
                await _historiesService.AddHistory(oldTicket, newTicket, userId);


                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
                ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);



                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
                //return RedirectToAction(name of(Index));
            }
            else
            {
                TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database, please log in as a full user.";
                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
            }

        }


        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
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
//Friday
//Sat