using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCPrbSol.Data;
using MVCPrbSol.Services;
using SQLitePCL;

namespace MVCPrbSol.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPSRolesService _rolesService;
        private readonly UserManager<PSUser> _userManager;

        public PSRolesController(ApplicationDbContext context, IPSRolesService rolesService, UserManager<PSUser> userManager)
        {
            _context = context;
            _rolesService = rolesService;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
