using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCPrbSol.Models;
using MVCPrbSol.Data;

namespace MVCPrbSol.Services
{
    public class IPSNotificationService
    {
        public Task Notify(string userId, Ticket ticket, TicketHistory change);
        public Task NotifyOfComment(string userId, Ticket ticket, TicketComment comment);
        public Task NotifyOfAttachment(string userId, Ticket ticket, TicketAttachment attachment);
    }
}
