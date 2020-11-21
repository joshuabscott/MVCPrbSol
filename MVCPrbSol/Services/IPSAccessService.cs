using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Services
{
    public interface IPSAccessService
    {
        public Task<bool> CanInteractProject(string userId, int projectId, string roleName);
        public Task<bool> CanInteractTicket(string userId, int ticketId, string roleName);
    }
}//This is the interface for gathering the ID's to be added to Projects and Tickets
//Sat