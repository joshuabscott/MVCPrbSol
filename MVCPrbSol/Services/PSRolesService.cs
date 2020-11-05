using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MVCPrbSol.Data;
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
        //private readonly ApplicationDbContext _context;

        //The Constructor - make usre to only use one
        public PSRolesService(RoleManager<IdentityRole> roleManager, UserManager<PSUser> userManager/*, ApplicationDbContext<context>*/)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            //_context = context;
        }


        //public async Task AddUserToProject(PSUser user, string roleName)
        //{
        //    var result = await _userManager.AddToRoleAsync(user, roleName);
        //    return result.Succeeded;
        //}

        //public async Task<bool> IsUserOnProject(PSUser user, string roleName)
        //{
        //    return await _userManager.IsUserOnProject(user, roleName);
        //}

        //public async Task<bool> IsUserInRole(PSUser user, string roleName)
        //{
        //    return await _userManager.IsUserOnAsync(user, roleName);
        //}

        //public async Task<IEnumerable<string>> ListUserRoles(PSUser user)
        //{
        //   return await _userManager.GetRolesAsync(user);
        //}

        //public async Task<bool> RemoveUserFromRole(PSUser user, string roleName)
        //{
        //    var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        //    return result.Succeeded;
        //}

        //public async Task<ICollection<PSUser>> UsersInRole(string roleName)
        //{
        //    return await _userManager.GetUsersInRoleAsync(roleName);
        //}

        //public async Task<ICollectionPSUser>> IsUserOnProject(IdentityRole role)
        //{
        //    return await _userManager.Users.Where(u => IsUserOnProject(u, role.Name).Result == false).ToListAsync();
        //}

//        Project project = await _context.Projects
//            .Include(u => u.ProjectUsers.Where(u => u.UserId == userId)).ThenInclude(u => u.User)
//            .FirstOrDefaultAsync(u => u.Id == projectId);
//        bool result = project.ProjectUsers.Any(u => u.UserId == userId);
//            return result;
//        }
//    public async Task<ICollection<Project>> ListUserProjects(string userId)
//    {
//        PSUser user = await _context.Users
//            .Include(p => p.ProjectUsers).ThenInclude(p => p.Project)
//            .FirstOrDefaultAsync(p => p.Id == userId);
//        List<Project> projects = user.ProjectUsers.SelectMany(p => (IEnumerable<Project>)p.Project).ToList();
//        return projects;
//    }

//    public async Task<IEnumerable<string>> LIstUserRoles(PSUser user)
//    {
//        throw new NotImplementedException();
//    }

//    public Task<bool> RemoveUserFromRole(PSUser user, string roleName)
//    {
//        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
//        return result.Succeeded;
//    }

//    public async Task<ICollection<PSUser>> UsersInRole(string roleName)
//    {
//        var users = await _userManager.GetUserAsync(roleName);
//        return users;
//    }

//    //  //
//    public async Task<ICollection<PSUser>> UsersNotInRole(IdentityRole role)
//    {
//        //var roleId = await _roleManager.GetRoleIdAsync(role);
//        return await _userManager.Users.Where(u => IsUserInRole(u, role.Name).Result == false).ToList();
//    }
//}
//}