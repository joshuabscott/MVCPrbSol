using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Services
{
    public interface IPSProjectService
    {
        public async Task<bool> IsUserOnProject(string userId, int projectId);
        public Task<ICollection> ListUserProjects( string userId);

        public Task AddUserToProject(string userId, int projectId);
        public Task RemoveUserFromProject(string userId, int projectId);

        public Task<ICollection<PSUser>> UsersOnProject(int projectId);
        public Task<ICollection<PSUser>> UsersOnProject(int projectId);
    }
}
