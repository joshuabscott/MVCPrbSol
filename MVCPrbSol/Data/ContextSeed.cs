using Microsoft.AspNetCore.Identity;
using MVCPrbSol.Enums;
using MVCPrbSol.Models;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Data
{
    public enum Roles
    {
        Administrator,
        ProgjectManager,
        Developer,
        Submitter,
        NewUser
    }

    public static class ContextSeed
    {
        //Seed Roles
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.ProgjectManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Developer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Submitter.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.NewUser.ToString()));
        }

        public static async Task SeedDefaultUserAsync(UserManager<PSUser> userManager)
        {
            #region Seed Administrator
            //Seed Default Administrator User
            var defaultAdmin = new PSUser
            {
                UserName = "joshuabscott@gmail.com",
                Email = "joshuabscott@gmail.com",
                FirstName = "Joshua",
                LastName = "Scott",
                EmailConfirmed = true,
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultAdmin.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultAdmin, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultAdmin, Roles.Administrator.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("**************ERROR * *****************");
                Debug.WriteLine("ERROR Seeding Default Administrator.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************************");
                throw;
            }
            #endregion

            #region Seed Project Manager
            //Seed Project Manager User
            var defaultPM = new PSUser
            {
                UserName = "RomanSionis@mailinator.com",
                Email = "RomanSionis@mailinator.com",
                FirstName = "Roman",
                LastName = "Sionis",
                EmailConfirmed = true,
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultPM.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultPM, "dzmYm9<[niRP");
                    await userManager.AddToRoleAsync(defaultPM, Roles.ProgjectManager.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("**************ERROR * *****************");
                Debug.WriteLine("ERROR Seeding Default Project Manager.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************************");
                throw;
            }
            #endregion

            #region Seed Developer
            //Seed Default Developer User
            var defaultDev = new PSUser
            {
                UserName = " SimonHurt@mailinator.com",
                Email = " SimonHurt@mailinator.com",
                FirstName = " Simon",
                LastName = "Hurt",
                EmailConfirmed = true,
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultDev.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultDev, "Abi4%)}K8C#?");
                    await userManager.AddToRoleAsync(defaultDev, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("**************ERROR * *****************");
                Debug.WriteLine("ERROR Seeding Default Developer.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************************");
                throw;
            }
            #endregion

            #region Seed Submitter
            //Seed Default Submitter User
            var defaultSub = new PSUser
            {
                UserName = "MattHagen@mailinator.com",
                Email = "MattHagen@mailinator.com",
                FirstName = "Matt",
                LastName = "Hagen",
                EmailConfirmed = true,
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultSub.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultSub, "L5bt?H;46p");
                    await userManager.AddToRoleAsync(defaultSub, Roles.Submitter.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("**************ERROR * *****************");
                Debug.WriteLine("ERROR Seeding Default Submitter.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**************************************");
                throw;
            }
            #endregion
        }
    }
}

