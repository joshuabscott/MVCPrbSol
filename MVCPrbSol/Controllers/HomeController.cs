using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Models.ViewModels;
using MVCPrbSol.Services;

namespace MVCPrbSol.Controllers  //Namespace is the outermost , Inside is a class, than a method, than the logic
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PSUser> _userManager;
        private readonly IPSProjectService _projectService;
        private readonly IPSRolesService _rolesService;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<PSUser> userManager, IPSProjectService projectService, IPSRolesService rolesService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _projectService = projectService;
            _rolesService = rolesService;
        }

        public IActionResult Temp()
        {
            return View();
        }

        public IActionResult FourOhFour()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var vm = new HomePMViewModel();
            var tickets = await _context.Tickets
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.Comments).ThenInclude(tc => tc.User)
                .Include(t => t.Attachments)
                .Include(t => t.Notifications)
                .Include(t => t.Histories).ThenInclude(h => h.User)
                .ToListAsync();
            vm.numTickets = tickets.Count;
            vm.numCritical = tickets.Where(t => t.TicketPriority.Name == "Critical").ToList().Count;
            vm.numOpen = tickets.Where(t => t.TicketStatus.Name == "Opened").ToList().Count;
            vm.numAssigned = tickets.Where(t => t.DeveloperUserId != null).ToList().Count;
            vm.numUnassigned = tickets.Where(t => t.DeveloperUserId == null).ToList().Count;
            vm.UsersOnProject = await _context.Users.ToListAsync();
            var notifications = new List<ICollection<Notification>>();
            foreach (var ticket in tickets)
            {
                notifications.Add(ticket.Notifications);
            }
            vm.Notifications = notifications.SelectMany(n => n)
                .Where(n => n.RecipientId == user.Id)
                .Where(n => n.Viewed == false)
                .ToList();
            // give pm personalized data
            if (await _userManager.IsInRoleAsync(user, "ProjectManager"))
            {
                // all pm projects
                var projects = await _projectService.ListUserProjects(user.Id);
                // all users on all projects --> list of lists
                var users = new List<ICollection<PSUser>>();
                var ticketSet = new List<List<Ticket>>();
                foreach (var project in projects)
                {
                    users.Add(await _projectService.UsersOnProject(project.Id));
                    ticketSet.Add(tickets.Where(t => t.Project.Id == project.Id).ToList());
                }
                // flatten list of lists
                tickets = ticketSet.SelectMany(t => t).ToList();
                vm.UsersOnProject = users.SelectMany(u => u).Distinct().ToList();

                // remove users that are not developers
                List<PSUser> devs = new List<PSUser>();
                foreach (var flatuser in vm.UsersOnProject)
                {
                    if (await _rolesService.IsUserInRole(flatuser, "Developer"))
                    {
                        devs.Add(flatuser);
                    }
                }

                // reassign view model properties if you're a pm
                vm.numTickets = ticketSet.SelectMany(t => t).ToList().Count;
                vm.numCritical = tickets.Where(t => t.TicketPriority.Name == "Critical").ToList().Count;
                vm.numUnassigned = tickets.Where(t => t.DeveloperUserId == null).ToList().Count;
                vm.numOpen = tickets.Where(t => t.TicketStatus.Name == "Opened").ToList().Count;

                // if we have developers make suggestion
                if (devs.Count > 0)
                {
                    // maximum suggestions
                    var max = 5;
                    // minimum suggestions = number of tickets
                    var min = tickets.Where(t => t.DeveloperUserId == null).ToList().Count;
                    // if it's less than max
                    var num = min < max ? min : max;
                    for (var i = 0; i < num; i++)
                    {
                        // get ticket
                        var ticket = tickets.Where(t => t.DeveloperUserId == null)
                            .OrderBy(t => t.TicketPriorityId).ThenBy(t => t.TicketStatusId)
                            .Skip(i).Take(1).ToList()[0];
                        vm.Tickets.Add(ticket);

                        // get dev
                        //devs = _projectService.SortListOfDevsByTicketCountAsync(devs, tickets);
                        var dev = devs.Count > i ? devs[i] : devs[0];
                        vm.Developers.Add(devs[0]);
                        // get task count
                        vm.Count.Add(tickets.Where(t => t.DeveloperUserId == devs[0].Id).ToList().Count);
                    }
                }
            }

            // give developer personalized data
            if (await _userManager.IsInRoleAsync(user, "Developer"))
            {
                vm.Tickets = tickets;

                var projects = await _projectService.ListUserProjects(user.Id);
                var ticketSet = new List<List<Ticket>>();
                foreach (var project in projects)
                {
                    ticketSet.Add(tickets.Where(t => t.Project.Id == project.Id).ToList());
                }
                vm.TicketsOnDevProjs = ticketSet.SelectMany(t => t).ToList();
                vm.TicketsAssignedToDev = vm.TicketsOnDevProjs.Where(t => t.DeveloperUserId == user.Id).ToList();
            }

            // give submitter personalized data
            if (await _userManager.IsInRoleAsync(user, "Submitter"))
            {
                vm.Tickets = tickets;
                vm.TicketsCreatedByMe = tickets.Where(t => t.OwnerUserId == user.Id).ToList();
            }

            // give submitter personalized data
            if (await _userManager.IsInRoleAsync(user, "NewUser"))
            {
                vm.Tickets = tickets;
            }

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult LandingPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}