using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
//using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Data;
using MVCPrbSol.Models;

namespace MVCPrbSol.Services
{
    public class PSProjectService : IPSProjectService
    {
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly UserManager<PSUser> _userManger;
        private readonly ApplicationDbContext _context;

        //The Constructor - make user to only use one
        public PSProjectService(RoleManager<IdentityRole> roleManager, UserManager<PSUser> userManager, ApplicationDbContext context)
        {
            //_roleManager = roleManager;
            //_userManager = userManager;
            _context = context;
        }

        //Methods
        public async Task<bool> IsUserOnProject(string userId, int projectId)
        {
            Project project = await _context.Projects
                .Include(u => u.ProjectUsers.Where(u => u.UserId == userId))
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == projectId);

            bool result = project.ProjectUsers.Any(u => u.UserId == userId);
            return result;

        }

        public async Task<ICollection<Project>> ListUserProjects(string userId)
        {
            var user = await _context.Users
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.Project)
                .FirstOrDefaultAsync(matchingUser => matchingUser.Id == userId);

            List<Project> projects = user.ProjectUsers.SelectMany(p => (IEnumerable<Project>)p.Project).ToList();
            return projects;
        }

        public async Task AddUserToProject(string userId, int projectId)
        {
            if (!await IsUserOnProject(userId, projectId))
            {
                try
                {
                    await _context.ProjectUsers.AddAsync(new ProjectUser { ProjectId = projectId, UserId = userId });
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"*** Error *** - Error Adding user to project. --> {ex.Message}");
                    throw;
                }
            }
        }

        public async Task RemoveUserFromProject(string userId, int projectId)
        {
            if (await IsUserOnProject(userId, projectId))
            {
                try
                {
                    ProjectUser projectUser = new ProjectUser()
                    {
                        ProjectId = projectId,
                        UserId = userId
                    };
                    _context.ProjectUsers.Remove(projectUser);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"*** An ERROR Occurred When Removing the User from the Project ***");
                    throw;
                }
            }
        }

        
        public async Task<ICollection<PSUser>> UsersOnProject(int projectId)
        {
            Project project = await _context.Projects
                .Include(u => u.ProjectUsers).ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == projectId);
            List<PSUser> projectusers = project.ProjectUsers.SelectMany(project => (IEnumerable<PSUser>)project.User).ToList();
            return projectusers;
        }

        public async Task<ICollection<PSUser>> UsersNotOnProject(int projectId)
        {
            return await _context.Users.Where(u => IsUserNotOnProject(u.Id, projectId).Result == false).ToListAsync();
        }
    }
}