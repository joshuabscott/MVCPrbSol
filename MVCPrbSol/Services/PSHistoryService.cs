using MVCPrbSol.Data;
using MVCPrbSol.Models;
using Microsoft.AspNetCore.Identity;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Services
{
    public class PSHistoryService : IPSHistoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PSUser> _userManager;

        public PSHistoryService(ApplicationDbContext context, UserManager<PSUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {
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
                await _context.TicketHistorys.AddAsync(history);
            }

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
                await _context.TicketHistorys.AddAsync(history);
            }

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
                await _context.TicketHistorys.AddAsync(history);
            }

            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "TicketPriorityId",
                    OldValue = _context.TicketPriorities.Find(oldTicket.TicketPriorityId).Name,
                    NewValue = _context.TicketPriorities.Find(newTicket.TicketPriorityId).Name,
                    Created = DateTimeOffset.Now,
                    UserId = userId

                };
                await _context.TicketHistorys.AddAsync(history);
            }

            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                TicketHistory history = new TicketHistory
                {
                    TicketId = newTicket.Id,
                    Property = "TicketStatusId",
                    OldValue = _context.TicketStatuses.Find(oldTicket.TicketStatusId).Name,
                    NewValue = _context.TicketStatuses.Find(newTicket.TicketStatusId).Name,
                    Created = DateTimeOffset.Now,
                    UserId = userId

                };
                await _context.TicketHistorys.AddAsync(history);
            }

            if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
            {
                if (String.IsNullOrWhiteSpace(oldTicket.DeveloperUserId))
                {
                    var oldval = oldTicket.DeveloperUserId == null ? "Unassigned" : _context.Users.Find(oldTicket.DeveloperUserId).FullName;
                    TicketHistory history = new TicketHistory
                    {
                        TicketId = newTicket.Id,
                        Property = "DeveloperUserId",
                        OldValue = "No Developer Assigned",
                        NewValue = _context.Users.Find(newTicket.DeveloperUserId).FullName,
                        Created = DateTimeOffset.Now,
                        UserId = userId

                    };
                    await _context.TicketHistorys.AddAsync(history);

                }

                //else if (String.IsNullOrWhiteSpace(newTicket.DeveloperUserId))
                //{
                //    TicketHistory history = new TicketHistory
                //    {
                //        TicketId = newTicket.Id,
                //        Property = "Developer",
                //        OldValue = _context.Users.Find(newTicket.DeveloperUserId).FullName,
                //        NewValue = "No Developer Assigned",
                //        Created = DateTimeOffset.Now,
                //        UserId = userId

                //    };
                //    await _context.TicketHistorys.AddAsync(history);

                //}
                //else
                //{
                //    TicketHistory history = new TicketHistory
                //    {
                //        TicketId = newTicket.Id,
                //        Property = "Developer",
                //        OldValue = _context.Users.Find(oldTicket.DeveloperUserId).FullName,
                //        NewValue = _context.Users.Find(newTicket.DeveloperUserId).FullName,
                //        Created = DateTimeOffset.Now,
                //        UserId = userId

                //    };
                //    await _context.TicketHistorys.AddAsync(history);
                //}
            }
            await _context.SaveChangesAsync();

        }
    }
}