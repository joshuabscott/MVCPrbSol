﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using MVCPrbSol.Data;
using MVCPrbSol.Models;

namespace MVCPrbSol.Services
{
    public class PSHistoriesService : IPSHistoriesService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PSUser> _userManager;
        private readonly IEmailSender _emailSender;
        //private readonly IPSNotificationService _notificationService;

        public PSHistoriesService(ApplicationDbContext context, UserManager<PSUser> userManager, IEmailSender emailSender/*, IPSNotificationService notificationService*/)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            //_notificationService = notificationService;

        }
        public async Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {
            //.Title
            if (oldTicket.Title != newTicket.Title)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Title",
                    OldValue = oldTicket.Title,
                    NewValue = newTicket.Title,
                    Created = DateTimeOffset.Now,
                    UserId = userId
                };
                //await _context.TicketHistories.AddAsync(history);
                _context.TicketHistories.Add(history);
                // notify developer that someone else made a change
                if (oldTicket.DeveloperUserId != userId && oldTicket.DeveloperUserId != null)
                {
                    var description = $"The {history.Property} was updated from {history.OldValue} to {history.NewValue}.";
                    //await _notificationService.Notify(userId, newTicket, description);
                }
            }

            //.Description
            if (oldTicket.Description != newTicket.Description)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "Description",
                    OldValue = oldTicket.Description,
                    NewValue = newTicket.Description,
                    Created = DateTimeOffset.Now,
                    UserId = userId
                };
                //await _context.TicketHistories.AddAsync(history);
                _context.TicketHistories.Add(history);
                // notify developer that someone else made a change
                if (oldTicket.DeveloperUserId != userId && oldTicket.DeveloperUserId != null)
                {
                    var description = $"The {history.Property} was updated from {history.OldValue} to {history.NewValue}.";
                    //await _notificationService.Notify(userId, newTicket, description);
                }
            }

            //.TicketTypeId
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "TicketTypeId",
                    OldValue = _context.TicketTypes.Find(oldTicket.TicketTypeId).Name,
                    NewValue = _context.TicketTypes.Find(newTicket.TicketTypeId).Name,
                    Created = DateTimeOffset.Now,
                    UserId = userId
                };
                //await _context.TicketHistories.AddAsync(history);
                _context.TicketHistories.Add(history);
                // notify developer that someone else made a change
                if (oldTicket.DeveloperUserId != userId && oldTicket.DeveloperUserId != null)
                {
                    var description = $"The {history.Property} was updated from {history.OldValue} to {history.NewValue}.";
                    //await _notificationService.Notify(userId, newTicket, description);
                }
            }

            //.TicketPriorityId
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "TicketPriorityId",
                    OldValue = _context.TicketTypes.Find(oldTicket.TicketPriorityId).Name,
                    NewValue = _context.TicketTypes.Find(newTicket.TicketPriorityId).Name,
                    Created = DateTimeOffset.Now,
                    UserId = userId
                };
                //await _context.TicketHistories.AddAsync(history);
                _context.TicketHistories.Add(history);
                // notify developer that someone else made a change
                if (oldTicket.DeveloperUserId != userId && oldTicket.DeveloperUserId != null)
                {
                    var description = $"The {history.Property} was updated from {history.OldValue} to {history.NewValue}.";
                    //await _notificationService.Notify(userId, newTicket, description);
                }
            }

            //.TicketStatusId
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "TicketStatusId",
                    OldValue = _context.TicketTypes.Find(oldTicket.TicketStatusId).Name,
                    NewValue = _context.TicketTypes.Find(newTicket.TicketStatusId).Name,
                    Created = DateTimeOffset.Now,
                    UserId = userId
                };
                //await _context.TicketHistories.AddAsync(history);
                _context.TicketHistories.Add(history);
                // notify developer that someone else made a change
                if (oldTicket.DeveloperUserId != userId && oldTicket.DeveloperUserId != null)
                {
                    var description = $"The {history.Property} was updated from {history.OldValue} to {history.NewValue}.";
                    //await _notificationService.Notify(userId, newTicket, description);
                }
            }
            
            
            //.DeveloperUserId
            if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
            {
                //if this, do this
                if (String.IsNullOrWhiteSpace(oldTicket.DeveloperUserId))
                {
                    TicketHistory history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "DeveloperUserId",
                        OldValue = "No developer assigned.",
                        NewValue = _context.Users.Find(newTicket.DeveloperUserId).FullName,
                        Created = DateTimeOffset.Now,
                        UserId = userId
                    };
                    await _context.TicketHistories.AddAsync(history);
                }
                //unless this, do this
                else if (String.IsNullOrWhiteSpace(newTicket.DeveloperUserId))
                {
                    TicketHistory history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "DeveloperUserId",
                        OldValue = _context.Users.Find(oldTicket.DeveloperUserId).FullName,
                        NewValue = "No developer assigned.",
                        Created = DateTimeOffset.Now,
                        UserId = userId
                    };
                    await _context.TicketHistories.AddAsync(history);
                }
                //if neither of those "if", do this instead
                else
                {
                    TicketHistory history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "Developer",
                        OldValue = _context.Users.Find(oldTicket.DeveloperUserId).FullName,
                        NewValue = _context.Users.Find(newTicket.DeveloperUserId).FullName,
                        Created = DateTimeOffset.Now,
                        UserId = userId
                    };
                    await _context.TicketHistories.AddAsync(history);
                    Notification notification = new Notification
                    {
                        TicketId = newTicket.Id,
                        Description = "You have a new ticket.",
                        Created = DateTimeOffset.Now,
                        SenderId = userId,
                        RecipientId = newTicket.DeveloperUserId
                    };
                    await _context.Notifications.AddAsync(notification);

                    //Then, Send an email now to Dev
                    string devEmail = newTicket.DeveloperUser.Email;
                    string subject = "New Ticket Assignment";
                    string message = $"You have a new ticket for project: {newTicket.Project.Name}";
                    await _emailSender.SendEmailAsync(devEmail, subject, message);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}//keeping track of a tickets data and tracking it as it is changed and updated, and Sending an email if New Assignment