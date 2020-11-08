using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Models.ViewModels;
using MVCPrbSol.Services;
using System.Diagnostics;

namespace MVCPrbSol.Controllers
{
    public class ProjectsController : Controller
    {

        private readonly ApplicationDbContext _context;                 //Reference to be injected
        private readonly IPSProjectService _PSProjectService;

        public ProjectsController(ApplicationDbContext context, IPSProjectService PSProjectService)        //Constructor //(App... context) is injected
        {
            _context = context;
            _PSProjectService = PSProjectService;
        }



        // GET: Projects Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }


        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImagePath,ImageData")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.ProjectUsers)           //in Addition to project, bring the reference to projectuser
                .ThenInclude(p => p.User)               //also bring the user reference
                .FirstOrDefaultAsync(m => m.Id == id);      //go into db, go intoo projects table, find the first project with this id, grab that and only that item with that id
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }


        // GET: Projects/Edit
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImagePath,ImageData")] Project project)
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


        // GET: Projects/Delete
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
            return _context.Projects.Any(e => e.Id == id);
        }


        // GET: Projects/ManageProjectUsers
        public async Task<IActionResult> AssignUsers(int id)          //By default, this is a get method//
        {
            var model = new ManageProjectUsersViewModel();      //Newing up an instance of ManageProjectUsersViewModel
            var project = _context.Projects.Find(id);

            model.Project = project;
            List<PSUser> users = await _context.Users.ToListAsync();
            List<PSUser> members = (List<PSUser>)await _PSProjectService.UsersOnProject(id);
            model.Users = new MultiSelectList(users, "Id", "FullName", members);
            return View(model);
        }



        //POST: Projects/Assign Users To Project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignUsers(ManageProjectUsersViewModel model)
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
                    //return RedirectToAction(nameof(BlogPosts), new { id = post.BlogId }); Default statement that returns to all projects: return RedirectToAction("Index", "Projects");
                }
                else
                {
                    Debug.WriteLine("****ERROR****");
                    //Send an error message back
                }
            }
            return View(model);

        }




        //[HttpGet]
        //public async Task<IActionResult> RemoveUsersFromProject(int id)
        //{


        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RemoveUsersFromProject(int id)
        //{
        //    var project = await _context.Projects.FindAsync(id);
        //    _context.Projects.Remove(project);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

    }
}