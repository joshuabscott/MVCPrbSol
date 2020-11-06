using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Models.ViewModels;
using MVCPrbSol.Services;

namespace MVCPrbSol.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;                 
        //Reference to be injected
        private readonly IPSProjectService _BTProjectService;

        public ProjectsController(ApplicationDbContext context, IPSProjectService BTProjectService)        
        //Constructor //(App... context) is injected
        {
            _context = context;
            _BTProjectService = BTProjectService;
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
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> AssignUsers(int id)          
        //By default, this is a get method//
        {
            var model = new ManageProjectUsersViewModel();      
            //Newing up an instance of ManageProjectUsersViewModel
            var project = _context.Projects.Find(id);

            model.Project = project;
            List<PSUser> users = await _context.Users.ToListAsync();
            List<PSUser> members = (List<PSUser>)await _BTProjectService.UsersOnProject(id);
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
                if (model.SelectedProjects != null)
                {
                    var currentMembers = await _context.Projects.Include(p => p.ProjectUsers)           
                        //Error points to this linq expression, but why?!  11/5/2020
                        .FirstOrDefaultAsync(p => p.Id == model.Project.Id);
                    List<string> memberIds = currentMembers.ProjectUsers.Select(u => u.UserId).ToList();

                    foreach (string id in memberIds)
                    {
                        await _BTProjectService.RemoveUserFromProject(id, model.Project.Id);
                    }

                    foreach (string id in model.SelectedProjects)
                    {
                        await _BTProjectService.AddUserToProject(id, model.Project.Id);
                    }
                    return RedirectToAction("Index", "Projects");
                }
                else
                {
                    Debug.WriteLine("****ERROR****");
                    //Send an error message back
                }
            }
            return View(model);

        }

    }
}