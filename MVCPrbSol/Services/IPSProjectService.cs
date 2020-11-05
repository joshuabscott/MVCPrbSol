//using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Services
{
    public interface IPSProjectService
    {
        public Task <bool> IsUserOnProject(string userId, int projectId);
        public Task<ICollection<Project>> ListUserProjects( string userId);

        public Task AddUserToProject(string userId, int projectId);
        public Task <bool> RemoveUserFromProject(string userId, int projectId);

        public Task<ICollection<DbContext>> UsersOnProject(int projectId);
        public ICollection<DbContext> UsersNotOnProject(int projectId);
    }
}
