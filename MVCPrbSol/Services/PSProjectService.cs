using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Services;

namespace MVCPrbSol.Services
{
    public class PSProjectService : IPSProjectService
    {
        private readonly ApplicationDbContext _context;

        public PSProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsUserOnProject(string userId, int projectId)                           
        {
            Project project = await _context.Projects                                                  
                .Include(u => u.ProjectUsers)                           
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == projectId);      

            bool result = project.ProjectUsers.Any(u => u.UserId == userId);
            return result;
        }

        public async Task<List<Project>> ListUserProjects(string userId)
        {
            PSUser user = await _context.Users         
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == userId);    

            List<Project> projects = user.ProjectUsers.Select(p => p.Project).ToList();
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
                    Debug.WriteLine($"*** ERROR *** - Error Adding User to Project.   --> {ex.Message}");
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

                    ProjectUser projectUser = _context.ProjectUsers.Where(m => m.UserId == userId && m.ProjectId == projectId).FirstOrDefault();

                    _context.ProjectUsers.Remove(projectUser);
                    await _context.SaveChangesAsync();


                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"*** ERROR *** - Error Removing user from project.   --> {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<ICollection<PSUser>> UsersOnProject(int projectId)
        {
            Project project = await _context.Projects
                .Include(u => u.ProjectUsers)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == projectId);

            List<PSUser> projectusers = project.ProjectUsers.Select(p => p.User).ToList();
            return projectusers;
        }

        public async Task<ICollection<PSUser>> UsersNotOnProject(int projectId)
        {
            return await _context.Users.Where(u => IsUserOnProject(u.Id, projectId).Result == false).ToListAsync();
        }
    }
}//Sat