using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Models;
using MVCPrbSol.Data;
using Microsoft.AspNetCore.Identity;

namespace MVCPrbSol.Services
{
    public class PSNotificationService : IPSNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailService;
        private readonly UserManager<PSUser> _userManager;

        public PSNotificationService(ApplicationDbContext context, IEmailSender emailService, UserManager<PSUser> userManager)
        {
            _context = context;
            _emailService = emailService;
            _userManager = userManager;
        }

        //Notify
        public async Task Notify(string userId, Ticket ticket, TicketHistory change)
        {
            var notification = new Notification
            {
                TicketId = ticket.Id,
                Description = $"The {change.Property} was updated from {change.OldValue} to {change.NewValue}.",
                Created = DateTime.Now,
                SenderId = userId,
                RecipientId = ticket.DeveloperUserId
            };
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            //var to = ticket.DeveloperUser.Email;
            //var subject = $"For project: { ticket.Project.Name }, ticket: { ticket.Title }, priority: { ticket.TicketPriority.Name }";
            //await _emailService.SendEmailAsync(to, subject, notification.Description);
        }

        public async Task SendNotificationEmail(Ticket ticket, Notification notification)
        {
            var to = ticket.DeveloperUser.Email;
            var subject = $"For project: { ticket.Project.Name }, ticket: { ticket.Title }, priority: { ticket.TicketPriority.Name }";
            await _emailService.SendEmailAsync(to, subject, notification.Description);
        }

        public async Task NotifyPM(Ticket ticket, string userId)
        {
            var projectUsers = await _context.ProjectUsers.Where(pu => pu.ProjectId == ticket.ProjectId).ToListAsync();
            var pms = new List<PSUser>();
            foreach (var pu in projectUsers)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == pu.UserId);
                if (await _userManager.IsInRoleAsync(user, "ProjectManager"))
                {
                    pms.Add(user);
                }
            }
            foreach (var pm in pms)
            {
                var pu = await _context.ProjectUsers.Include(p => p.Project).FirstOrDefaultAsync(pu => pu.ProjectId == ticket.ProjectId);
                var notification = new Notification
                {
                    TicketId = ticket.Id,
                    Description = $"You have a new ticket on {pu.Project.Name}",
                    Created = DateTime.Now,
                    SenderId = userId,
                    RecipientId = pm.Id
                };
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
                await _emailService.SendEmailAsync(pm.Email, "New Ticket", notification.Description);
            }
        }

    }
}