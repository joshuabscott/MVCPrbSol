using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Identity;
using MVCPrbSol.Data;
using MVCPrbSol.Models.ViewModels;
using MVCPrbSol.Services;
using MVCPrbSol.Models;


namespace MVCPrbSol.Controllers
{
    //@*------C# Notes------*@
    //[HandleError]
    //[Authorize(Roles = "Administrator")]
    // public class ProjectsController : Controller
    public class ProjectUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPSProjectService _projectService;
        private readonly UserManager<PSUser> _userManager;

        //This is the Class Constructor and // Set the initial value for model
        public ProjectUsersController(ApplicationDbContext context, IPSProjectService projectService, UserManager<PSUser> userManager)
        {
            _context = context;
            _projectService = projectService;
            _userManager = userManager;
        }

        //AddUser - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToProject(ManageProjectUsersViewModel model)
        {
            foreach (var userId in model.SelectedUsers)
            {
                if (!await _projectService.IsUserOnProject(userId, model.Project.Id))
                {
                    await _projectService.AddUserToProject(userId, model.Project.Id);
                }
            }
            return RedirectToAction("AddUserToProject");
        }

        //RemoveUserFromProject - GET
        public async Task<IActionResult> RemoveUserFromProject(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Projects");
            }
            var model = new ManageProjectUsersViewModel();
            model.Project = await _context.Projects.FindAsync((int)id);
            var users = await _context.Users.Where(u => _projectService.IsUserOnProject(u.Id, (int)id).Result).ToListAsync();
            model.Users = new MultiSelectList(users, "Id", "FullName");

            return View(model);
        }

        //RemoveUserFromProject - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserFromProject(ManageProjectUsersViewModel model)
        {
            foreach (var userId in model.SelectedUsers)
            {
                if (!await _projectService.IsUserOnProject(userId, model.Project.Id))
                {
                    await _projectService.RemoveUserFromProject(userId, model.Project.Id);
                }
            }
            return RedirectToAction("RemoveUserFromProject");
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}

//----------------------------------------------------------------------------OLD---------------------------------------------
//        //Assign ProjectUsers GET Action
//        public async Task<IActionResult> AssignUsers(int id)
//        {
//            var model = new ManageProjectUsersViewModel();
//            var project = _context.Projects.Find(id);

//            model.Project = project;
//            List<PSUser> users = await _context.Users.ToListAsync();
//            List<PSUser> members = (List<PSUser>)await _projectService.UsersOnProject(id);
//            model.Users = new MultiSelectList(users, "Id", "FullName", members);
//            return View(model);
//        }

//        //Assign ProjectUsers POST Action
//        [HttpPost]
//        [ValidateAntiForgeryToken]

//        public async Task<IActionResult> AssignUsers(ManageProjectUsersViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                if (model.SelectedUsers != null)
//                {
//                    var currentMembers = await _context.Projects.Include(p => p.ProjectUsers).FirstOrDefaultAsync(p => p.Id == model.Project.Id);
//                    List<string> memberIds = currentMembers?.ProjectUsers.Select(u => u.UserId).ToList();

//                    foreach (string id in memberIds)
//                    {
//                        await _projectService.RemoveUserFromProject(id, model.Project.Id);
//                    }

//                    foreach (string id in model.SelectedUsers)
//                    {
//                        await _projectService.AddUserToProject(id, model.Project.Id);
//                    }
//                    return RedirectToAction("Details", "Projects", new { id = model.Project.Id });
//                }
//                else
//                {
//                    Debug.WriteLine("Error assigning user");
//                }
//            }
//            return View(model);
//        }

//        //Remove ProjectUsers GET Action
//        public async Task<IActionResult> RemoveUsers(int? id)
//        {
//            if (id == null)
//            {
//                return RedirectToAction("Index, Projects");
//            }

//            var model = new ManageProjectUsersViewModel();
//            model.Project = await _context.Projects.FindAsync((int)id);
//            var users = await _context.Users.Where(u => _projectService.IsUserOnProject(u.Id, (int)id).Result).ToListAsync();
//            model.Users = new MultiSelectList(users, "Id", "FullName");

//            return View(model);
//        }

//        //Remove ProjectUsers POST Action
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> RemoveUser(ManageProjectUsersViewModel model)
//        {
//            foreach (var userId in model.SelectedUsers)
//            {
//                if (!await _projectService.IsUserOnProject(userId, model.Project.Id))
//                {
//                    await _projectService.RemoveUserFromProject(userId, model.Project.Id);
//                }
//            }
//            return RedirectToAction("RemoveUserFromProject");
//        }
//        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//        // GET: Projects
//        public async Task<IActionResult> Index()
//        {
//            return View(await _context.Projects.ToListAsync());
//        }

//        // GET: Projects/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var vm = new ProjectTicketsViewModel();

//            var project = await _context.Projects
//                .Include(p => p.ProjectUsers)
//                .ThenInclude(p => p.User)
//                .FirstOrDefaultAsync(p => p.Id == id);

//            var tickets = await _context.Tickets
//                .Where(t => t.ProjectId == id)
//                .Include(t => t.TicketStatus)
//                .Include(t => t.TicketPriority)
//                .Include(t => t.TicketType)
//                .Include(t => t.DeveloperUser)
//                .ToListAsync();

//            vm.Project = project;
//            vm.Tickets = tickets;

//            if (project == null)
//            {
//                return NotFound();
//            }

//            return View(vm);
//        }

//        // GET: Projects/Create
//        [Authorize(Roles = "Administrator")]
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Projects/Create
//        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,Name,ImagePath,ImageData")] Project project)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(project);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(project);
//        }

//        // GET: Projects/Edit/5
//        //[Authorize(Roles = "Administrator, ProjectManager")]
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var project = await _context.Projects.FindAsync(id);
//            if (project == null)
//            {
//                return NotFound();
//            }
//            return View(project);
//        }

//        // POST: Projects/Edit/5
//        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImagePath,ImageData")] Project project)
//        {
//            if (id != project.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(project);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ProjectExists(project.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(project);
//        }

//        // GET: Projects/Delete/5
//        //[Authorize(Roles = "Administrator")]
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var project = await _context.Projects
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (project == null)
//            {
//                return NotFound();
//            }

//            return View(project);
//        }

//        // POST: Projects/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var project = await _context.Projects.FindAsync(id);
//            _context.Projects.Remove(project);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ProjectExists(int id)
//        {
//            return _context.Projects.Any(e => e.Id == id);
//        }
//    }
//}