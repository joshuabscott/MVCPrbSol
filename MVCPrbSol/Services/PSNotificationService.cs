using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Models;
using MVCPrbSol.Data;

namespace MVCPrbSol.Services
{
    public class PSNotificationService : IPSNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailService;

        public PSNotificationService(ApplicationDbContext context, IEmailSender emailService)
        {
            _context = context;
            _emailService = emailService;
        }
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
            var to = ticket.DeveloperUser.Email;
            var subject = $"For project: { ticket.Project.Name }, ticket: { ticket.Title }, priority: { ticket.TicketPriority.Name }";
            await _emailService.SendEmailAsync(to, subject, notification.Description);
        }

        public async Task NotifyOfComment(string userId, Ticket ticket, TicketComment comment)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var notification = new Notification
            {
                TicketId = ticket.Id,
                Description = $"{user.FullName} left a comment on Ticket titled: '{ticket.Title}' saying, '{comment.Comment}'",
                Created = DateTime.Now,
                SenderId = userId,
                RecipientId = ticket.DeveloperUserId
            };
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            var to = ticket.DeveloperUser.Email;
            var subject = $"For project: { ticket.Project.Name }, ticket: { ticket.Title }, priority: { ticket.TicketPriority.Name }";
            await _emailService.SendEmailAsync(to, subject, notification.Description);
        }

        public async Task NotifyOfAttachment(string userId, Ticket ticket, TicketAttachment attachment)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var notification = new Notification
            {
                TicketId = ticket.Id,
                Description = $"{user.FullName} added an attachment on Ticket titled: '{ticket.Title}', named, '{attachment.Description}'",
                Created = DateTime.Now,
                SenderId = userId,
                RecipientId = ticket.DeveloperUserId
            };
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            var to = ticket.DeveloperUser.Email;
            var subject = $"For project: { ticket.Project.Name }, ticket: { ticket.Title }, priority: { ticket.TicketPriority.Name }";
            await _emailService.SendEmailAsync(to, subject, notification.Description);
        }
    }
}