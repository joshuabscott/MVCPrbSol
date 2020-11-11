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
        private readonly IPSHistoryService _historyService;
        private readonly IPSRolesService _rolesService;
      
        public TicketsController(ApplicationDbContext context, UserManager<PSUser> userManager, IPSHistoryService historyService, IPSRolesService rolesService)
        {
            _userManager = userManager;
            _context = context;
            _historyService = historyService;
            _rolesService = rolesService;
        }

        // GET: Tickets/Index
        public async Task<IActionResult> Index(string userId)
        {
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
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        // GET: Tickets/Create
        public IActionResult Create(int? id)
        {
            //var model = new Ticket();
            //model.ProjectId = (int)id;

            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");

            if (User.IsInRole("Administrator") || User.IsInRole("ProjectManager"))
            {
                ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
                ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name");
                ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName");
                ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
            }
            //else
            //{
            //    model.TicketTypeId = _context.TicketTypes.Where(tp => tp.Name == "Pending").FirstOrDefault().Id; ;
            //    model.TicketPriorityId = _context.TicketPriorities.Where(tp => tp.Name == "Pending").FirstOrDefault().Id;
            //    model.TicketStatusId = _context.TicketStatuses.Where(tp => tp.Name == "Pending").FirstOrDefault().Id; ;
            //    model.DeveloperUserId = null; //Null by default
            //}
            //return View(model);
            return View();
        }

        // POST: Tickets/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
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
                return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });      
                //When user creates a new ticket, they will be redirected back to the Projects/Details View instead of the default, which was "Index"
            }
            else
            {
                return NotFound();
            }
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Administrator,ProjecManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            //Need ticket comments to show in edit view so I can edit/archive them(as AD/PM)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId")] Ticket ticket)
        {
            ticket.Updated = DateTimeOffset.Now;

            if (id != ticket.Id)
            {
                return NotFound();
            }

            //Id of the old ticket
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
                await _historyService.AddHistory(oldTicket, ticket, userId);

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
        [Authorize(Roles = "Administrator")]
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