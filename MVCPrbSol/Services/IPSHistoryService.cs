using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCPrbSol.Models;

namespace MVCPrbSol.Services
{
    public interface IPSHistoryService
    {   //the interface for 2 types of xTicket? with the Id
        Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId);
    }
}