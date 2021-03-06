﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Models;

namespace MVCPrbSol.Services
{
    public interface IPSProjectService
    {
        public Task <bool> IsUserOnProject(string userId, int projectId);
        //public Task<ICollection<Project>> ListUserProjects( string userId);
        public Task<List<Project>> ListUserProjects(string userId);

        public Task AddUserToProject(string userId, int projectId);
        public Task RemoveUserFromProject(string userId, int projectId);

        public Task<ICollection<PSUser>> UsersOnProject(int projectId);
        public Task<ICollection<PSUser>> UsersNotOnProject(int projectId);
        //public Task<ICollection<DbContext>> UsersOnProject(int projectId);
        //public Task<ICollection<DbContext>> UsersNotOnProject(int projectId);
    }
}//How we search the list of projects to determine what user is already on a project team or not
//Saturday
