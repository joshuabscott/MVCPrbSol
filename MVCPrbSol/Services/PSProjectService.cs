using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Services
{
    public class PSProjectService : IPSProjectService
    {
        private readonly ApplicationDbContext _context;

        //The Constructor - make usre to only use one
        public PSProjectService(roleManager<IdentityRole> roleManager, roleManager<> userManager, ApplicationDbContext<context>)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        //Methods
        public async Task<bool> IsUserOnProject(string userId, int projectId)
        {
            Project project = await _context.Projects
                .Include(u => u.ProjectUsers.Where(u => u.UserId == userId)).ThenInclude(u => u.User)
                theninclude
                .FirstOrDefaultAsync(u => u.Id == projectId);

            bool result = project.ProjectUsers.Any(u => u.UserId == userId);
            return result;

        }


        public async Task<ICollection<Project>> ListUserProjects(string userId)
        {
            PSUser user = await _context.Users
                .Include(p => p.ProjectUsers).ThenInclude(p => p.Project)
                .ThenInclude
                .FirstOrDefaultAsync(p => p.Id == userId);
            List<Project> projects = user.ProjectUsers.SelectMany(p => (IEnumerable<Project>)p.Project).ToList();
            return projects;
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




        public async Task<>


        public async Task<ICollection<PSUser>> UsersNotOnProject(int userId)
        {
            return await _context.Users.Where(u => IsUserOnProject(u.Id, projectId).Result == false).ToListAsync(); ;
        }
    }
}