using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Models.ViewModels
{
    public class TicketDetailsViewModel
    {
        public Ticket Ticket { get; set; }
        public TicketComment Comment { get; set; }
    }
}