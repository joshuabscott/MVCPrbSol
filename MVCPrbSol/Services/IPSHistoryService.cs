using MVCPrbSol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Services
{
    public interface IPSHistoryService
    {
        Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId);

    }
}