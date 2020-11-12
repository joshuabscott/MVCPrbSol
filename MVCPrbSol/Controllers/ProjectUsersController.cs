using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCPrbSol.Data;
using MVCPrbSol.Models.ViewModels;
using MVCPrbSol.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace MVCPrbSol.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProjectUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPSProjectService _projectService;

        public ProjectUsersController(ApplicationDbContext context, IPSProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public async Task<IActionResult> AddProjectUsers(int projectId)
        {
            ManageProjectUsersViewModel model = new ManageProjectUsersViewModel();
            model.Project = _context.Projects.FirstOrDefault(p => p.Id == projectId);
            model.UsersOnProject = (List<Models.PSUser>)await _projectService.UsersOnProject(projectId);
            model.UsersOffProject = (List<Models.PSUser>)await _projectService.UsersNotOnProject(projectId);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   Trouble with await
            return View(model);
        }
    }
}