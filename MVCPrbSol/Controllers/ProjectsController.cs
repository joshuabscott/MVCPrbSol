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

namespace MVCPrbSol.Controllers   //Namespace is the outermost , Inside is a class, than a method, than the logic
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

        // GET: Projects Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }

        //GET: Projects/ MyProjects 
        public async Task<IActionResult> MyProjects(int? id)
        {   // Should function similarly to MyTickets

            var model = new List<Project>();
            var userId = _userManager.GetUserId(User);

            if (User.IsInRole("Administrator"))
            {
                //create or use a service (BTProjectService?) that will filter a user's projects on the MyProjects View
                model = _context.Projects.ToList();
                return View("MyProjects", model);
            }
            //My Projects for the different roles
            if (User.IsInRole("ProjectManager") || User.IsInRole("Developer") || User.IsInRole("Submitter") || User.IsInRole("NewUser"))
            {

                model = await _projectService.ListUserProjects(userId);
                return View("MyProjects", model);

            }

            return NotFound();

        }

        //GET: Projects/Create
        [Authorize(Roles = "Administrator, ProjectManager")]
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
            if (!User.IsInRole("Demo"))
            {

                if (ModelState.IsValid)
                {
                    _context.Add(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(project);
            }
            else
            {
                TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database, please log in as a full user.";
                return RedirectToAction("MyProjects", "Projects");

            }
        }

        // GET: Projects/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.ProjectUsers)           //in Addition to project, bring the reference to project user
                .ThenInclude(p => p.User)               //also bring the user reference
                .FirstOrDefaultAsync(m => m.Id == id);      //go into db, go into projects table, find the first project with this id, grab that and only that item with that id

            project.Tickets = await _context.Tickets
                .Where(t => t.ProjectId == id)
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .ToListAsync();


            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Edit
        [Authorize(Roles = "Administrator, ProjectManager")]
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


        // POST: Projects/Edit
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImagePath,ImageData")] Project project)
        {
            if (!User.IsInRole("Demo"))
            {

                if (id != project.Id)
                {
                    return NotFound();
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
            else
            {
                TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database, please log in as a full user.";
                return RedirectToAction("MyProjects", "Projects");

            }
        }


        // GET: Projects/Delete
        [Authorize(Roles = "Administrator")]
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


        // POST: Projects/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(pe => pe.Id == id);
        }

        // GET: Projects/ManageProjectUsers
        [Authorize(Roles = "Administrator, ProjectManager")]
        public async Task<IActionResult> AssignUsers(int id)          //By default, this is a get method//
        {
            var model = new ManageProjectUsersViewModel();      //Newing up an instance of ManageProjectUsersViewModel
            var project = _context.Projects.Find(id);

            model.Project = project;
            List<PSUser> users = await _context.Users.ToListAsync();
            List<PSUser> members = (List<PSUser>)await _PSProjectService.UsersOnProject(id); //
            model.Users = new MultiSelectList(users, "Id", "FullName", members);
            return View(model);
        }



        //POST: Projects/Assign Users To Project
        [Authorize(Roles = "Administrator, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignUsers(ManageProjectUsersViewModel model)
        {
            if (!User.IsInRole("Demo")) //Prevent Demo User from 
            {

                if (ModelState.IsValid)
                {
                    if (model.SelectedUsers != null)
                    {
                        var currentMembers = await _context.Projects.Include(p => p.ProjectUsers)
                            .FirstOrDefaultAsync(p => p.Id == model.Project.Id);
                        List<string> memberIds = currentMembers.ProjectUsers.Select(u => u.UserId).ToList();

                        foreach (string id in memberIds)
                        {
                            await _PSProjectService.RemoveUserFromProject(id, model.Project.Id);
                        }

                        foreach (string id in model.SelectedUsers)
                        {
                            await _PSProjectService.AddUserToProject(id, model.Project.Id);
                        }
                        return RedirectToAction("Details", "Projects", new { id = model.Project.Id });
                        //return RedirectToAction(name of(BlogPosts), new { id = post.BlogId }); Default statement that returns to all projects: return RedirectToAction("Index", "Projects");
                    }
                    else
                    {
                        Debug.WriteLine("****ERROR****");
                        //Send an error message back
                    }
                }
                return View(model);
            }
            else
            {
                TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database, please log in as a full user.";
                return RedirectToAction("MyProjects", "Projects");

            }
        }
    }
}
