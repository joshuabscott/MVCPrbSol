using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Enums
{
    public enum Roles
    {
        Administrator,
        ProgjectManager,
        Developer,
        Submitter,
        NewUser
    }

    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        
    }
}
