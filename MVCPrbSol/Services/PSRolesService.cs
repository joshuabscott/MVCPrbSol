using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MVCPrbSol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MVCPrbSol.Services
{
    public class PSRolesService : IPSRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<PSUser> _userManager;

        public PSRolesService(RoleManager<IdentityRole> roleManager, RoleManager<PSUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> AddUserToRole(PSUser user, string roleName)
        {
            var result = await _userManger.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

    public async Task<bool> IsUserInRole(PSUser user, string roleName)
        {          
            throw new NotImplementedException();
        }
    }
}