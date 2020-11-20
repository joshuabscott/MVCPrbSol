using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCPrbSol.Models;
using MVCPrbSol.Models.ViewModels;
using MVCPrbSol.Data;
using MVCPrbSol.Services;
using MVCPrbSol.Utilities;

namespace MVCPrbSol.Utilities
{
    public class BubbleSort
    {
        public static List<PSUser> SortListOfDevsByTicketCount(List<PSUser> users, ApplicationDbContext _context)
        {
            int i, j;
            int N = users.Count;

            for (j = N - 1; j > 0; j--)
            {
                for (i = 0; i < j; i++)
                {
                    var pu1 = users[i].ProjectUsers;
                    int tc1 = 0;
                    foreach (var pu in pu1)
                    {
                        tc1 += _context.Tickets.Where(t => t.DeveloperUserId == users[i].Id).ToList().Count;
                    }
                    var pu2 = users[i + 1].ProjectUsers;
                    int tc2 = 0;
                    foreach (var pu in pu2)
                    {
                        tc2 += _context.Tickets.Where(t => t.DeveloperUserId == users[i + 1].Id).ToList().Count;
                    }
                    if (tc2 < tc1)
                    {
                        var temp = users[i];
                        users[i] = users[i + 1];
                        users[i + 1] = users[i];
                    }
                }
            }
            return users;
        }
    }
}