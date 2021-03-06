﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MVCPrbSol.Models;

namespace MVCPrbSol.Services
{
    public interface IPSRolesService
    {
        public Task<bool> AddUserToRole(PSUser user, string roleName);
        public Task<bool> IsUserInRole(PSUser user, string roleName);

        public Task<IEnumerable<string>> ListUserRoles(PSUser user);
        public Task<bool> RemoveUserFromRole(PSUser user, string roleName);

        public Task<ICollection<PSUser>> UsersInRole(string roleName);
        //public Task<ICollection<PSUser>> UsersNotInRole(IdentityRole role);
        public Task<IEnumerable<PSUser>> UsersNotInRole(string roleName);
    }
}// the methods allow for roles to be added and removed
//Sat
