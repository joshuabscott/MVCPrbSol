using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCPrbSol.Models;
using MVCPrbSol.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MVCPrbSol.Services
{
    public class PSRolesService : IPSRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;        //Like empty shells, need contructor to fill these containers with "the goods" -Denis Jojot
        private readonly UserManager<PSUser> _userManager;

        public PSRolesService(RoleManager<IdentityRole> roleManager, UserManager<PSUser> userManager)  //Contructor. When these method reference variables _roleManager and _userManager, 
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


        public async Task<IEnumerable<string>> ListUserRoles(PSUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public Task<IEnumerable<string>> LIstUserRoles(PSUser user)
        {
            throw new NotImplementedException();
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

        public async Task<ICollection<PSUser>> UsersNotInRole(IdentityRole role)
        {
            //var roleId = await _roleManager.GetRoleIdAsync(role);
            return await _userManager.Users.Where(u => IsUserInRole(u, role.Name).Result == false).ToListAsync();

        }
    }
}