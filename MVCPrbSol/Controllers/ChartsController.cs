using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Models.DemoModels;

namespace MVCPrbSol.Controllers
{
    //public class ChartsController : Controller
    //{
    //    private readonly ApplicationDbContext _context;

    //    public ChartsController(ApplicationDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public JsonResult GetDemoChartDAta()
    //    {
    //        List<DemoData> result = new List<DemoData>();

    //        var ticketPriorities = _context.TicketPriorities.ToList();

    //        foreach(var priority in ticketPriorities)
    //        {
    //            result.Add(new DemoData
    //            {
    //                Priority = priority.Name,
    //                Count = _context.Tickets.Where(t => t.TicketPriorityId = priority.Id).Count()
    //            });
    //        }
    //        return Json(result);
    //    }
    //}
}
