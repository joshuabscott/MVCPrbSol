using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Models.ViewModels;
using MVCPrbSol.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BugTracker.Controllers
{
    [Authorize]
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPSRolesService _rolesService;
        private readonly UserManager<PSUser> _usermanager;

        public UserRolesController(ApplicationDbContext context, IPSRolesService rolesService, UserManager<PSUser> userManager)     //Constructor, need to read/learn more about this
        {
            _context = context;
            _rolesService = rolesService;
            _usermanager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()          //By default, this is a get method
        {
            List<ManageUserRolesViewModel> model = new List<ManageUserRolesViewModel>();        //List so we can list all of our users
            List<PSUser> users = _context.Users.ToList();

            foreach (var user in users)
            {
                ManageUserRolesViewModel vm = new ManageUserRolesViewModel();           //Creating dropdown
                vm.User = user;
                var selected = await _rolesService.ListUserRoles(user);
                vm.Roles = new MultiSelectList(_context.Roles, "Name", "Name", selected);      //Overload example
                model.Add(vm);
            }

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel btuser)
        {
            PSUser user = _context.Users.Find(btuser.User.Id);

            IEnumerable<string> roles = await _rolesService.ListUserRoles(user);
            await _usermanager.RemoveFromRolesAsync(user, roles);
            string userRole = btuser.SelectedRoles.FirstOrDefault();

            if (Enum.TryParse(userRole, out Roles roleValue))
            {
                await _rolesService.AddUserToRole(user, userRole);
                return RedirectToAction("ManageUserRoles");
            }

            return RedirectToAction("ManageUserRoles");
        }

    }
}
