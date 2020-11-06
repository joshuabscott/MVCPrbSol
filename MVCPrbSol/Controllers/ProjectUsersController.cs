using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
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
        private readonly IPSRolesService _rolesService;
        private readonly ProjectUser<PSUser> _projectUser;

        public ProjectUsersController(ApplicationDbContext context, IPSRolesService rolesService, ProjectUser<PSUser> projectManager)
        {
            _context = context;
            _rolesService = rolesService;
            _projectUser = projectUser;
        }


        public IActionResult Index()
        {
            return View();
        }

        //Get
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            List<ManageProjectUsersViewModel> model = new List<ManageProjectUsersViewModel>();
            List<ProjectUser> projectusers = _context.ProjectUsers.ToList();

            foreach (var projectuser in projectusers)
            {
                ManageProjectUsersViewModel vm = new ManageProjectUsersViewModel();
                vm.ProjectUser = projectuser;
                var selected = await _rolesService.LIstUserRoles(user);
                vm.ProjectUsers = new MultiSelectList(_context.Roles, "Name", "Name", selected);
                model.Add(vm);
            }

            return View(model);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageProjectUsersViewModel psuser)
        {
            PSUser user = await _context.Users.FindAsync(psuser.User.Id);

            IEnumerable<string> roles = await _rolesService.LIstUserRoles(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            string userRole = psuser.SelectedProjects.FirstOrDefault();

            if (Enum.TryParse(userRole, out Roles roleValue))
            {
                await _rolesService.AddUserToRole(user, userRole);
                return RedirectToAction("ManageUserRoles");
            }
            return RedirectToAction("ManageUserRoles");
        }
    }
}