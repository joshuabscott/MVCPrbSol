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


namespace MVCPrbSol.Controllers      //Namespace is the outermost , Inside is a class, than a method, than the logic
{
    [Authorize]
    public class TicketAttachmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PSUser> _userManager;
        //private readonly IPSNotificationService _notificationService;

        public TicketAttachmentsController(ApplicationDbContext context, UserManager<PSUser> userManager/*, IPSNotificationService notificationService*/)
        {
            _context = context;
            _userManager = userManager;
            //_notificationService = notificationService;
        }

        // GET: TicketAttachments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TicketAttachments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketAttachment = await _context.TicketAttachments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketAttachment == null)
            {
                return NotFound();
            }

            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }


        // POST: TicketAttachments/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FormFile,Image,Description,Created,TicketId,UserId")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                MemoryStream ms = new MemoryStream();
                await ticketAttachment.FormFile.CopyToAsync(ms);

                ticketAttachment.FileData = ms.ToArray();
                ticketAttachment.FileName = ticketAttachment.FormFile.FileName;
                ticketAttachment.Created = DateTimeOffset.Now;
                ticketAttachment.UserId = _userManager.GetUserId(User);

                _context.Add(ticketAttachment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketAttachment.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        //// POST: TicketAttachments/Create
        //// To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,FilePath,FileData,Description,Created,TicketId,UserId")] TicketAttachment ticketAttachment, IFormFile attachment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (attachment != null)
        //        {
        //            var memoryStream = new MemoryStream();
        //            attachment.CopyTo(memoryStream);
        //            byte[] bytes = memoryStream.ToArray();
        //            memoryStream.Close();
        //            memoryStream.Dispose();
        //            var binary = Convert.ToBase64String(bytes);
        //            var ext = Path.GetExtension(attachment.FileName);

        //            ticketAttachment.FilePath = $"data:image/{ext};base64,{binary}";
        //            ticketAttachment.FileData = bytes;
        //            ticketAttachment.Description = attachment.FileName;
        //            ticketAttachment.Created = DateTime.Now;
        //            //Something was added here during class I believe, I don't know if this is working correctly or if this is pulled out as a service
        //            _context.Add(ticketAttachment);
        //            await _context.SaveChangesAsync();

        //            var ticket = await _context.Tickets
        //                .Include(t => t.TicketPriority)
        //                .Include(t => t.TicketStatus)
        //                .Include(t => t.TicketType)
        //                .Include(t => t.DeveloperUser)
        //                .Include(t => t.Project)
        //                .FirstOrDefaultAsync(t => t.Id == ticketAttachment.TicketId);

        //            //await _notificationService.NotifyOfAttachment(_userManager.GetUserId(User), ticket, ticketAttachment);
        //            return RedirectToAction(nameof(Index));
        //        }
        //        return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });
        //    }
        //    ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketAttachment.TicketId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketAttachment.UserId);
        //    return View(ticketAttachment);
        //}

        // GET: TicketAttachments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketAttachment = await _context.TicketAttachments.FindAsync(id);
            if (ticketAttachment == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketAttachment.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FilePath,FileData,Description,Created,TicketId,UserId")] TicketAttachment ticketAttachment)
        {
            if (id != ticketAttachment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketAttachment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketAttachmentExists(ticketAttachment.Id))
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
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketAttachment.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketAttachment = await _context.TicketAttachments
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketAttachment == null)
            {
                return NotFound();
            }

            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketAttachment = await _context.TicketAttachments.FindAsync(id);
            _context.TicketAttachments.Remove(ticketAttachment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketAttachmentExists(int id)
        {
            return _context.TicketAttachments.Any(e => e.Id == id);
        }
    }
}//Friday
//Sat
//Mon