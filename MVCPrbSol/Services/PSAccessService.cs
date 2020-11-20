using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Data;

namespace MVCPrbSol.Services
{
    public class PSAccessService : IPSAccessService
    {
        private readonly ApplicationDbContext _context;
        public PSAccessService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CanInteractProject(string userId, int projectId, string roleName)
        {
            switch (roleName)
            {
                case "Administrator":
                    return true;
                case "ProjectManager":
                    if (await _context.ProjectUsers.Where(pu => pu.UserId == userId && pu.ProjectId == projectId).AnyAsync())
                    {
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }
        public async Task<bool> CanInteractTicket(string userId, int ticketId, string roleName)
        {
            bool result = false;
            switch (roleName)
            {
                case "Administrator":
                    result = true;
                    break;
                case "ProjectManager":
                    var projectId = (await _context.Tickets.FindAsync(ticketId)).ProjectId;
                    if (await _context.ProjectUsers.Where(pu => pu.UserId == userId && pu.ProjectId == projectId).AnyAsync())
                    {
                        result = true;
                    }
                    break;
                case "Developer":
                    if (await _context.Tickets.Where(t => t.DeveloperUserId == userId && t.Id == ticketId).AnyAsync())
                    {
                        result = true;
                    }
                    break;
                case "Submitter":
                    if (await _context.Tickets.Where(t => t.OwnerUserId == userId && t.Id == ticketId).AnyAsync())
                    {
                        result = true;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}//Allowing the access of the different roles of Administrator, Project Manager, Developer, Submitter with Project and Ticket