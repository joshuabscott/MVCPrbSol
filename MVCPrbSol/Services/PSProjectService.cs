﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Services;

namespace MVCProbsol.Services
{
    public class PSProjectService : IPSProjectService
    {
        private readonly ApplicationDbContext _context;

        public PSProjectService(ApplicationDbContext context)
        {
            _context = context;
        }
        //Methods = Actions of Controller
        public async Task<bool> IsUserOnProject(string userId, int projectId)
        {
            var user = await _context.ProjectUsers.FirstOrDefaultAsync(pu => pu.UserId == userId && pu.ProjectId == projectId);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<ICollection<Project>> ListUserProjects(string userId)
        {
            PSUser user = await _context.Users
                .Include(p => p.ProjectUsers)
                .ThenInclude(p => p.Project)
                .FirstOrDefaultAsync(p => p.Id == userId);

            List<Project> project = user.ProjectUsers.SelectMany(p => (IEnumerable<Project>)p.Project).ToList();
            return project;
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
                    Debug.WriteLine($"*** ERROR *** - Error adding user to project. -->{ex.Message}");
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
                    ProjectUser projectUser = await _context.ProjectUsers.Where(p => p.UserId == userId && p.ProjectId == projectId)
                        .FirstOrDefaultAsync();

                    _context.ProjectUsers.Remove(projectUser);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"*** ERROR *** - Error removing user from project. -->{ex.Message}");
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

            List<PSUser> projectusers = project.ProjectUsers.Select(project => project.User).ToList();
            return projectusers;
        }

        public async Task<ICollection<PSUser>> UsersNotOnProject(int projectId)
        {
            var users = await _context.Users.ToListAsync();
            ICollection<PSUser> Users = new List<PSUser>();

            foreach (var user in users)
            {
                var result = IsUserOnProject(user.Id, projectId).Result;

                if (result == false)
                {
                    Users.Add(user);
                }
            }
            return Users;
        }

        Task<ICollection<DbContext>> IPSProjectService.UsersOnProject(int projectId)
        {
            throw new NotImplementedException();
        }

        Task<ICollection<DbContext>> IPSProjectService.UsersNotOnProject(int projectId)
        {
            throw new NotImplementedException();
        }
    }
}


//------------------------------------------------old-------------------------

//using MVCPrbSol.Data;
//using MVCPrbSol.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using MVCPrbSol.Services;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;

//namespace MVCPrbSol.Services
//{
//    public class PSProjectService : IPSProjectService
//    {
//        private readonly ApplicationDbContext _context;   

//        public PSProjectService(ApplicationDbContext context)
//        {

//            _context = context;                       
//        }
//        //IsUserOnProject
//        public async Task<bool> IsUserOnProject(string userId, int projectId)     
//        {
//            Project project = await _context.Projects                           
//                .Include(u => u.ProjectUsers)                                     
//                .ThenInclude(u => u.User)
//                .FirstOrDefaultAsync(u => u.Id == projectId);                     

//            bool result = project.ProjectUsers.Any(u => u.UserId == userId);
//            return result;

//            //return _context.ProjectUsers.Where(pu => pu.UserId == userId && pu.ProjectId == projectId).Any();
//        }

//        //ListUserProjects
//        public async Task<ICollection<Project>> ListUserProjects(string userId)
//        {
//            PSUser user = await _context.Users        
//                .Include(p => p.ProjectUsers)
//                .ThenInclude(p => p.Project)
//                .FirstOrDefaultAsync(p => p.Id == userId);      

//            List<Project> projects = user.ProjectUsers.SelectMany(p => (IEnumerable<Project>)p.Project).ToList();
//            return projects;
//        }

//        //AddUserToProject
//        public async Task AddUserToProject(string userId, int projectId)
//        {
//            if (!await IsUserOnProject(userId, projectId))
//            {
//                try
//                {
//                    await _context.ProjectUsers.AddAsync(new ProjectUser { ProjectId = projectId, UserId = userId });
//                    await _context.SaveChangesAsync();
//                }
//                catch (Exception ex)
//                {
//                    Debug.WriteLine($"*** ERROR *** - Error Adding User to Project.   --> {ex.Message}");
//                    throw;
//                }
//            }
//        }

//        //RemoveUserFromProject
//        public async Task RemoveUserFromProject(string userId, int projectId)
//        {
//            if (await IsUserOnProject(userId, projectId))
//            {
//                try
//                {

//                    ProjectUser projectUser = _context.ProjectUsers.Where(m => m.UserId == userId && m.ProjectId == projectId).FirstOrDefault();

//                    _context.ProjectUsers.Remove(projectUser);
//                    await _context.SaveChangesAsync();

//                }
//                catch (Exception ex)
//                {
//                    Debug.WriteLine($"*** ERROR *** - Error Removing user from project.   --> {ex.Message}");
//                    throw;
//                }
//            }
//        }

//        //UsersOnProject
//        //------------------------is there another way of doing this?
//        //jump here from ProjectsController .174..... look for a 1 on projectId hover while running to confirm the bind
//        public async Task<ICollection<PSUser>> UsersOnProject(int projectId)
//        {
//            Project project = await _context.Projects
//                .Include(u => u.ProjectUsers)
//                .ThenInclude(u => u.User)
//                .FirstOrDefaultAsync(u => u.Id == projectId);
//            //Pass back the users.....for the .ThenInclude(users)
//            List<PSUser> projectusers = project.ProjectUsers.Select(p => p.User).ToList();
//            return projectusers;
//        }

//        //UsersNotOnProject
//        public async Task<ICollection<PSUser>> UsersNotOnProject(int projectId)
//        {
//            return await _context.Users.Where(u => IsUserOnProject(u.Id, projectId).Result == false).ToListAsync();
//        }

//        Task<ICollection<DbContext>> IPSProjectService.UsersOnProject(int projectId)
//        {
//            throw new NotImplementedException();
//        }

//        Task<ICollection<DbContext>> IPSProjectService.UsersNotOnProject(int projectId)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
