using Microsoft.AspNetCore.Mvc;
using MVCPrbSol.Data;
using MVCPrbSol.Models.ViewModels;
using MVCPrbSol.Services;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Controllers
{
    public class ProjectUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPSProjectService _projectService;

        public ManageProjectUsersViewModel model = new ManageProjectUsersViewModel)
        {
            _context = context;
            _projectService = IPSProjectService;
        }

        public async Task<IActionResult> ManageProjectUsers(int projectId)
        {
            ManageProjectUsersViewModel model = new ManageProjectUsersViewModel();
            model.Project = _context.Projects.FristOrDefault(p => p.Id == projectId);
            model.UsersOnProject = (List<Models.PSUser>)await _projectService.UsersOnProject(projectId);
            model.UsersNotOnProject = (List<Models.PSUser>)await _projectService.UsersNotOnProject(projectId);
            return View(model);
        }
    }
}
