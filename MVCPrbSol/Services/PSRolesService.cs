using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Services;

namespace MVCPrbSol.Services
{
    public class PSRolesService : IPSRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<PSUser> _userManager;

        public PSRolesService(RoleManager<IdentityRole> roleManager, UserManager<PSUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> AddUserToRole(PSUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<bool> IsUserInRole(PSUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IEnumerable<string>> ListUserRoles(PSUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> RemoveUserFromRole(PSUser user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<ICollection<PSUser>> UsersInRole(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users;
        }

        public async Task<IEnumerable<PSUser>> UsersNotInRole(string roleName)
        {
            var inRole = await _userManager.GetUsersInRoleAsync(roleName);
            var users = await _userManager.Users.ToListAsync();
            return users.Except(inRole);
        }
    }
}