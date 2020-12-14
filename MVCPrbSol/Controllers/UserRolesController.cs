using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Models.ViewModels;
using MVCPrbSol.Services;

namespace MVCPrbSol.Controllers    //Namespace is the outermost , Inside is a class, than a method, than the logic
{
    [Authorize(Roles = "Administrator, ProjectManager")]
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPSRolesService _rolesService;
        private readonly UserManager<PSUser> _userManager;

        public UserRolesController(ApplicationDbContext context, IPSRolesService rolesService, UserManager<PSUser> userManager)     
        {
            _context = context;
            _rolesService = rolesService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            List<ManageUserRolesViewModel> model = new List<ManageUserRolesViewModel>();
            List<PSUser> users = _context.Users.ToList();

            foreach (var user in users)
            {
                ManageUserRolesViewModel vm = new ManageUserRolesViewModel
                {
                    User = user
                };
                var selected = await _rolesService.ListUserRoles(user);
                vm.Roles = new MultiSelectList(_context.Roles, "Name", "Name", selected);      //Overload example, maybe just a select list instead?
                model.Add(vm);
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel psuser)
        {
            if (!User.IsInRole("Demo"))
            {

                PSUser user = _context.Users.Find(psuser.User.Id);

                IEnumerable<string> roles = await _rolesService.ListUserRoles(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                var userRoles = psuser.SelectedRoles;
                string userRole = psuser.SelectedRoles.FirstOrDefault();

                foreach (var role in userRoles)
                {
                    if (Enum.TryParse(role, out Roles roleValue))
                    {
                        await _rolesService.AddUserToRole(user, role);

                    }

                }
                return RedirectToAction("ManageUserRoles");

            }
            else
            {
                TempData["DemoLockout"] = "Your changes have not been saved. To make changes to the database, please log in as a full user.";
                return RedirectToAction("Index", "Home");

            }
        }
    }
}