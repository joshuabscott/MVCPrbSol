using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using MVCPrbSol.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCPrbSol.Controllers
{
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


        public async Task<IActionResult>  ManageUserRoles()
        {
            List<ManageUserRolesViewModel> model = new List<ManageUserRolesViewModel>();
            List<PSUser> users = _context.Users.ToList();

            foreach (var user in users)
            {
                ManageUserRolesViewModel vm = new ManageUserRolesViewModel();
                vm.User = user;
                var selected = await _rolesService.LIstUserRoles(user);
                vm.Roles = new MultiSelectList(_context.Roles, "Name", "Name", selected);
                model.Add(vm);
            }

            return View(model);
        }
    }
}


//UserRolesController Index.cshtml
