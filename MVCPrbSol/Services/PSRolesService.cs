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
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> IsUserInRole(PSUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IEnumerable<string>> LIstUserRoles(PSUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveUserFromRole(PSUser user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<ICollection<PSUser>> UsersInRole(string roleName)
        {
            var users = await _userManager.GetUserAsync(roleName);
            return users;
        }

        //  //
        public async Task<ICollection<PSUser>> UsersNotInRole(IdentityRole role)
        {
            //var roleId = await _roleManager.GetRoleIdAsync(role);
            return await _userManager.Users.Where(u => IsUserInRole(u, role.Name).Result == false).ToList();
        }
    }
}