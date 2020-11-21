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
using MVCPrbSol.Models;
using MVCPrbSol.Models.ViewModels;
using MVCPrbSol.Services;

namespace MVCPrbSol.Controllers
{
    //------------------------------------------------------------------------------------error-----------------------------------------
    [Authorize]
    public class ProjectsController : Controller
    {

        private readonly ApplicationDbContext _context;                 
        private readonly UserManager<PSUser> _userManager;
        private readonly IPSAccessService _accessService;
        private readonly IPSProjectService _projectService;
        private readonly IPSRolesService _rolesService;
        private readonly IPSProjectService _PSProjectService;

        public ProjectsController(ApplicationDbContext context, UserManager<PSUser> userManager, IPSAccessService accessService, IPSProjectService projectService, IPSRolesService rolesService, IPSProjectService PSProjectService)        
        {
            _context = context;
            _userManager = userManager;
            _accessService = accessService;
            _projectService = projectService;
            _rolesService = rolesService;
            _PSProjectService = PSProjectService;
        }


        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }

        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> MyProjects()
        {
            var userId = _userManager.GetUserId(User);
            var projectUserRecords = await _context.ProjectUsers
                                    .Where(p => p.UserId == userId)
                                    .Include(pu => pu.Project)
                                    .ToListAsync();
            var projects = new List<Project>();
            foreach (var projectUserRecord in projectUserRecords)
            {
                projects.Add(projectUserRecord.Project);
            }
            return View(projects);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.ProjectUsers).ThenInclude(p => p.User)
                .Include(p => p.Tickets).ThenInclude(p => p.TicketType)
                .Include(p => p.Tickets).ThenInclude(p => p.TicketPriority)
                .Include(p => p.Tickets).ThenInclude(p => p.TicketStatus)
                .Include(p => p.Tickets).ThenInclude(p => p.OwnerUser)
                .Include(p => p.Tickets).ThenInclude(p => p.DeveloperUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImagePath,ImageData")] Project project)
        {
            if (User.IsInRole("Demo"))
            {
                TempData["DemoLockout"] = "Demo users can't submit data.";
                return RedirectToAction("Index", "Projects", new { id = project.Id });
            }
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImagePath,ImageData")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }
            if (User.IsInRole("Demo"))
            {
                TempData["DemoLockout"] = "Demo users can't submit data.";
                return RedirectToAction("Details", "Projects", new { id = project.Id });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }
        
        [Authorize(Roles = "Administrator")]
        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole("Demo"))
            {
                TempData["DemoLockout"] = "Demo users can't submit data.";
                return RedirectToAction("Index", "Projects", new { id = id });
            }
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Get: AssignUsers
        [HttpGet]
        public async Task<IActionResult> AssignUsers(int id)
        {
            var model = new ProjectUsersViewModel();
            var project = _context.Projects.Find(id);

            model.Project = project;
            List<PSUser> members = (List<PSUser>)await _PSProjectService.UsersOnProject(id);

            var usersOnProj = await _PSProjectService.UsersOnProject(project.Id);
            model.UsersOnProject = new MultiSelectList(usersOnProj.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "Id", "FullName", members);
            var usersOffProj = await _PSProjectService.UsersNotOnProject(project.Id);
            model.UsersOffProject = new MultiSelectList(usersOffProj.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "Id", "FullName", members);

            return View(model);
        }
        //Post: AssignUsers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignUsers(ProjectUsersViewModel model)
        {
            if (User.IsInRole("Demo"))
            {
                TempData["DemoLockout"] = "Demo users can't submit data.";
                return RedirectToAction("AssignUsers", "Projects", new { id = model.Project.Id });
            }
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    var currentMembers = await _context.Projects.Include(p => p.ProjectUsers).FirstOrDefaultAsync(p => p.Id == model.Project.Id);
                    List<string> memberIds = currentMembers.ProjectUsers.Select(u => u.UserId).ToList();
                    foreach (string id in model.SelectedUsers)
                    {
                        await _PSProjectService.AddUserToProject(id, model.Project.Id);
                    }
                    return RedirectToAction("AssignUsers", "Projects", new { id = model.Project.Id });
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUsers(ProjectUsersViewModel model)
        {
            if (User.IsInRole("Demo"))
            {
                TempData["DemoLockout"] = "Demo users can't submit data.";
                return RedirectToAction("AssignUsers", "Projects", new { id = model.Project.Id });
            }
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    var currentMembers = await _context.Projects.Include(p => p.ProjectUsers).FirstOrDefaultAsync(p => p.Id == model.Project.Id);
                    List<string> memberIds = currentMembers.ProjectUsers.Select(u => u.UserId).ToList();
                    foreach (string id in model.SelectedUsers)
                    {
                        await _PSProjectService.RemoveUserFromProject(id, model.Project.Id);
                    }
                    return RedirectToAction("AssignUsers", "Projects", new { id = model.Project.Id });
                }
            }
            return View(model);
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
//The Logic to create an instance of an Object : Project
//Friday