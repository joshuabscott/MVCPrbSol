using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCPrbSol.Services;
using MVCPrbSol.Models;

namespace MVCPrbSol.Services
{
    public interface IPSHistoriesService
    {
        Task AddHistory(Ticket oldTicket, Ticket newTicket, string userId);

    }
}//This is the interface that gathers information of tickets to be used for History when updating newTicket
//Sat