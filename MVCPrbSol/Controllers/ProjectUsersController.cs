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

        //AddUser - GET
        public async Task<IActionResult> AddUserToProject(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Projects");
            }
            var model = new ManageProjectUsersViewModel();
            model.Project = await _context.Projects.FindAsync((int)id);
            //var users = await _context.Users.Where(u => _projectService.IsUserOnProject(u.Id, (int)id).Result == false).ToListAsync();
            //model.Users = new MultiSelectList(users, "Id", "FullName");
            model.Users = new MultiSelectList(await _projectService.UsersNotOnProject((int)id), "Id", "FullName");

            return View(model);
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