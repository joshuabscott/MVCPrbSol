using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCPrbSol.Data;
using MVCPrbSol.Models;

namespace MVCPrbSol.Services
{
    public class IPSNotificationService
    {
        public Task Notify(string userId, Ticket ticket, string description);
        public Task SendNotificationEmail(Ticket ticket, Notification notification);
        public Task NotifyPM(Ticket ticket, string userId);
        //public Task Notify(string userId, Ticket ticket, TicketHistory change);
        //public Task NotifyOfComment(string userId, Ticket ticket, TicketComment comment);
        //public Task NotifyOfAttachment(string userId, Ticket ticket, TicketAttachment attachment);
    }
}
