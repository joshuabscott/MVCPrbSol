using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Services;

namespace MVCPrbSol.Controllers
{
    [Authorize]
    public class TicketCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketComments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TicketComments
                .Include(t => t.Ticket)
                .Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TicketComments/Details/5
        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketComment = await _context.TicketComments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketComment == null)
            {
                return NotFound();
            }

            return View(ticketComment);
        }

        // GET: TicketComments/Create
        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: TicketComments/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comment,TicketId,UserId")] TicketComment ticketComment)
        {
            if (!User.IsInRole("Demo"))
            {
                ticketComment.Created = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _context.Add(ticketComment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", ticketComment.UserId);
                return View(ticketComment);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: TicketComments/Edit/5
        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketComment = await _context.TicketComments.FindAsync(id);
            if (ticketComment == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", ticketComment.UserId);
            return View(ticketComment);
        }

        // POST: TicketComments/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comment,Created,TicketId,UserId")] TicketComment ticketComment)
        {
            if (!User.IsInRole("Demo"))
            {
                if (id != ticketComment.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(ticketComment);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TicketCommentExists(ticketComment.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));

                }
                ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", ticketComment.UserId);
                return View(ticketComment);
            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: TicketComments/Delete/5
        [Authorize(Roles = "Administrator, ProjectManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketComment = await _context.TicketComments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketComment == null)
            {
                return NotFound();
            }

            return View(ticketComment);
        }

        // POST: TicketComments/Delete/5
        [Authorize(Roles = "Administrator, ProjectManager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Demo"))
            {
                var ticketComment = await _context.TicketComments.FindAsync(id);
                _context.TicketComments.Remove(ticketComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
                return RedirectToAction(nameof(Index));
            }

        }

        private bool TicketCommentExists(int id)
        {
            return _context.TicketComments.Any(e => e.Id == id);
        }
    }
}

//namespace MVCPrbSol.Controllers     //Namespace is the outermost , Inside is a class, than a method, than the logic
//{
//    [Authorize]
//    public class TicketCommentsController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        //private readonly UserManager<PSUser> _userManager;
//        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//        //private readonly IPSNotificationService _notificationService;

//        public TicketCommentsController(ApplicationDbContext context /*, UserManager<PSUser> userManager*//*, IPSNotificationService notificationService*/)
//        {
//            _context = context;
//            //_userManager = userManager;
//            //_notificationService = notificationService;
//        }

//        // GET: TicketComments
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = _context.TicketComments
//                .Include(t => t.Ticket)
//                .Include(t => t.User);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: TicketComments/Details/5
//        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }
//            //Not sure about this link statement, might not be here or be correct.....this is correct
//            var ticketComment = await _context.TicketComments
//                .Include(t => t.Ticket)
//                .Include(t => t.User)
//                .FirstOrDefaultAsync(m => m.Id == id);

//            if (ticketComment == null)
//            {
//                return NotFound();
//            }

//            return View(ticketComment);
//        }

//        // GET: TicketComments/Create
//        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
//        public IActionResult Create()
//        {
//            //Why did I put this in?
//            //var model = new TicketComment();
//            //model.TicketId = (int)id;

//            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description");
//            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
//            return View();
//        }

//        // POST: TicketComments/Create
//        // To protect from over-posting attacks, enable the specific properties you want to bind to, for -------------------------------------Clean Up------------------------------------
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Comment,Created,TicketId,UserId")] TicketComment ticketComment, string content, int ticketId)
//        {
//            if (!User.IsInRole("Demo"))
//            {
//                ticketComment.Created = DateTime.Now;
//                if (ModelState.IsValid)
//                {
//                    _context.Add(ticketComment);
//                    await _context.SaveChangesAsync();
//                    return RedirectToAction(nameof(Index));
//                }
//                ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
//                ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", ticketComment.UserId);
//                return View(ticketComment);
//            }
//            else
//            {
//                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
//                return RedirectToAction(nameof(Index));
//            }

//        }

//        //{
//        //    if (User.IsInRole("Demo"))
//        //    {
//        //        TempData["DemoLockout"] = "Demo users can't submit data.";
//        //        return RedirectToAction("Details", "Tickets", new { id = ticketId });
//        //    }
//        //    var userId = _userManager.GetUserId(User);
//        //    ticketComment.UserId = userId;
//        //    if (content != null)
//        //    {
//        //        ticketComment.Comment = content;
//        //        ticketComment.TicketId = ticketId;
//        //        ticketComment.Created = DateTimeOffset.Now;
//        //    }

//        //    //ticketComment.Created = DateTimeOffset.Now;
//        //    //ticketComment.UserId = _userManager.GetUserId(User);

//        //    if (ModelState.IsValid)
//        //    {
//        //        _context.Add(ticketComment);
//        //        await _context.SaveChangesAsync();
//        //        var ticket = await _context.Tickets
//        //            .Include(t => t.TicketPriority)
//        //            .Include(t => t.TicketStatus)
//        //            .Include(t => t.TicketType)
//        //            .Include(t => t.DeveloperUser)
//        //            .Include(t => t.Project)
//        //            .FirstOrDefaultAsync(t => t.Id == ticketId);
//        //        if (ticket.DeveloperUserId != null)
//        //        {
//        //            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
//        //            var description = $"{user.FullName} left a comment on Ticket titled: '{ticket.Title}' saying, '{ticketComment.Comment}'";
//        //            //await _notificationService.Notify(userId, ticket, description);--------------------------------------------------------------------------------I dont know how to use this-------------------
//        //        }

//        //        return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });
//        //    }
//        //    ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
//        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketComment.UserId);
//        //    return View(ticketComment);
//        //    //else
//        //    //{
//        //    //    return NotFound();
//        //    //}
//        //}


//        // GET: TicketComments/Edit/5
//        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var ticketComment = await _context.TicketComments.FindAsync(id);
//            if (ticketComment == null)
//            {
//                return NotFound();
//            }
//            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
//            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketComment.UserId);
//            return View(ticketComment);
//        }

//        // POST: TicketComments/Edit/5
//        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [Authorize(Roles = "Administrator, ProjectManager, Developer")]
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,Comment,Created,TicketId,UserId")] TicketComment ticketComment)
//        {
//            if (!User.IsInRole("Demo"))
//            {
//                if (id != ticketComment.Id)
//                {
//                    return NotFound();
//                }

//                if (ModelState.IsValid)
//                {
//                    try
//                    {
//                        _context.Update(ticketComment);
//                        await _context.SaveChangesAsync();
//                    }
//                    catch (DbUpdateConcurrencyException)
//                    {
//                        if (!TicketCommentExists(ticketComment.Id))
//                        {
//                            return NotFound();
//                        }
//                        else
//                        {
//                            throw;
//                        }
//                    }
//                    return RedirectToAction(nameof(Index));

//                }
//                ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketComment.TicketId);
//                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketComment.UserId);
//                return View(ticketComment);
//            }
//            else
//            {
//                TempData["DemoLockout"] = "Changes made from a Demo role are not saved.";
//                return RedirectToAction(nameof(Index));
//            }

//        }

//        // GET: TicketComments/Delete/5
//        [Authorize(Roles = "Administrator, ProjectManager")]
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var ticketComment = await _context.TicketComments
//                .Include(t => t.Ticket)
//                .Include(t => t.User)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (ticketComment == null)
//            {
//                return NotFound();
//            }

//            return View(ticketComment);
//        }

//        // POST: TicketComments/Delete/5
//        [Authorize(Roles = "Administrator, ProjectManager")]
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var ticketComment = await _context.TicketComments.FindAsync(id);
//            _context.TicketComments.Remove(ticketComment);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool TicketCommentExists(int id)
//        {
//            return _context.TicketComments.Any(e => e.Id == id);
//        }
//    }
//}