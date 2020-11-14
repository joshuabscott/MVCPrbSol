using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCPrbSol.Models.ViewModels
{
    public class ProjectTicketsViewModel
    {
        public Project Project { get; set; }
        public Ticket Ticket { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
