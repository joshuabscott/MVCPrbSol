using MVCPrbSol.Data;
using MVCPrbSol.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Services
{
    public class PSProjectService : IPSProjectService
    {

        private readonly ApplicationDbContext _context;         //Creating the variable to be used in Constructor; Constructor (public BTProjectService) injects reference and variable we wish to use throughout this file

        public PSProjectService(ApplicationDbContext context)
        {

            _context = context;                                 //assigning the injection to the variable
        }

        //Methods implemented from Interface(IBTProjectService)
        public async Task<bool> IsUserOnProject(string userId, int projectId)      //This method is querying the database, asking "Is the user on a particular project?"
        {
            Project project = await _context.Projects                              //userful to have an instance of project since we're asking IsUserOnPROJECT (part of the method name)
                .Include(u => u.ProjectUsers)                                      //include project users, then include users. Where is a query;
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(u => u.Id == projectId);                      //In car scenario, this statement is saying "okay, i've picked up enough passengers (include, theninclude)

            bool result = project.ProjectUsers.Any(u => u.UserId == userId);
            return result;

            //Alternate way to do this:
            //return _context.ProjectUsers.Where(pu => pu.UserId == userId && pu.ProjectId == projectId).Any();
        }

        public async Task<ICollection<Project>> ListUserProjects(string userId)
        {
            PSUser user = await _context.Users          //user table is created by default thanks to Microsoft; 
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == userId);      //firstordefault is grabbing the first instance of parameters defined, or defaults 

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



                    //ProjectUser projectUser = new ProjectUser() /*_context.ProjectUsers.Where(m => m.UserId == userId && m.ProjectId == projectId).FirstOrDefault;*/
                    //{
                    //    ProjectId = projectId,
                    //    UserId = userId
                    //};
                    //_context.ProjectUsers.Remove(projectUser);
                    //await _context.SaveChangesAsync();



                    //This also works in place of what's above
                    //ProjectUser projectUser = await _context.ProjectUsers
                    //    .Where(pu => pu.UserId == userId && pu.ProjectId == projectId)
                    //    .FirstOrDefaultAsync();
                    //_context.ProjectUsers.Remove(projectUser);
                    //await _context.SaveChangesAsync();

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

        Task<ICollection<DbContext>> IPSProjectService.UsersOnProject(int projectId)
        {
            throw new NotImplementedException();
        }

        ICollection<DbContext> IPSProjectService.UsersNotOnProject(int projectId)
        {
            throw new NotImplementedException();
        }
    }
}
