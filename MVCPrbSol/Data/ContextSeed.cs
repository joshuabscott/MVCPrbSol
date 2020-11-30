using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Models;

namespace MVCPrbSol.Data
{
    public enum Roles
    {
        Administrator,
        ProjectManager,
        Developer,
        Submitter,
        NewUser,
        Demo
    }

    public static class ContextSeed
    {
        //Seed Roles
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.ProjectManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Developer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Submitter.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.NewUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Demo.ToString()));
        }

        //In order to have default values in place for our Ticket types, statuses, and priorities, we need to seed
        //default values in place.

        #region Seed Default Ticket Types
        public static async Task SeedDefaultTicketTypesAsync(ApplicationDbContext context)
        {
            try
            {
                if (!context.TicketTypes.Any(tt => tt.Name == "Runtime"))
                {
                    await context.TicketTypes.AddAsync(new TicketType { Name = "Runtime" });
                }

                if (!context.TicketTypes.Any(tt => tt.Name == "User-Interface"))
                {
                    await context.TicketTypes.AddAsync(new TicketType { Name = "User-Interface" });
                }

                if (!context.TicketTypes.Any(tt => tt.Name == "Front-End"))
                {
                    await context.TicketTypes.AddAsync(new TicketType { Name = "Front-End" });
                }

                if (!context.TicketTypes.Any(tt => tt.Name == "Back-End"))
                {
                    await context.TicketTypes.AddAsync(new TicketType { Name = "Back-End" });
                }

                if (!context.TicketTypes.Any(tt => tt.Name == "Miscellaneous"))
                {
                    await context.TicketTypes.AddAsync(new TicketType { Name = "Miscellaneous" });
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("**************** ERROR ****************");
                Debug.WriteLine("Error Seeding Ticket Types");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("************************************************");
                throw;
            }
        }
        #endregion

        #region Seed Default Ticket Priorities
        public static async Task SeedDefaultTicketPrioritiesAsync(ApplicationDbContext context)
        {
            try
            {
                if (!context.TicketPriorities.Any(tp => tp.Name == "Low"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "Low" });
                }

                if (!context.TicketPriorities.Any(tp => tp.Name == "High"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "High" });
                }

                if (!context.TicketPriorities.Any(tp => tp.Name == "Urgent"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "Urgent" });
                }

                if (!context.TicketPriorities.Any(tp => tp.Name == "Pending"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "Pending" });
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("**************** ERROR ****************");
                Debug.WriteLine("Error Seeding Ticket Priorities");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("************************************************");
                throw;
            }
        }
        #endregion

        #region Seed Default Ticket Statuses
        public static async Task SeedDefaultTicketStatusesAsync(ApplicationDbContext context)
        {
            try
            {
                if (!context.TicketStatuses.Any(ts => ts.Name == "New"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "New" });
                }

                if (!context.TicketStatuses.Any(ts => ts.Name == "In-Progress"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "In-Progress" });
                }

                if (!context.TicketStatuses.Any(ts => ts.Name == "Resolved"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "Resolved" });
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("**************** ERROR ****************");
                Debug.WriteLine("Error Seeding Ticket Statuses");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("************************************************");
                throw;
            }
        }
        #endregion

        public static async Task SeedProjectsAsync(ApplicationDbContext context)
        {
            List<Project> projects = new List<Project>();
            Project seedProject1 = new Project
            {
                Name = "Blog Project"
            };
            Project seedProject2 = new Project();

        }
       
        //See Default Users & Demo Users
        #region Default Users & Demo Users
        public static async Task SeedDefaultUsersAsync(UserManager<PSUser> userManager)
        {

            #region Administrator Seed
            var defaultUser = new PSUser
            {
                UserName = "JScott@mailinator.com",
                Email = "JScott@mailinator.com",
                FirstName = "Josh",
                LastName = "Scott",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "!1Qwerty");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Administrator.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Default Administrator User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Project Manager Seed
            defaultUser = new PSUser
            {
                UserName = "JOsterman@mailinator.com",
                Email = "JOsterman@mailinator.com",
                FirstName = "Jon",
                LastName = "Osterman",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "!1Qwerty");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Default Project Manager User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Developer Seed
            defaultUser = new PSUser
            {
                UserName = "WKovacs@mailinator.com",
                Email = "WKovacs@mailinator.com",
                FirstName = "Walter",
                LastName = "Kovacs",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "!1Qwerty");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Default Developer User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Submitter Seed
            defaultUser = new PSUser
            {
                UserName = "AVeidt@mailinator.com",
                Email = "AVeidt@mailinator.com",
                FirstName = "Adrian",
                LastName = "Veidt",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "!1Qwerty");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Default Submitter User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region New User Seed
            defaultUser = new PSUser
            {
                UserName = "EBlake@mailinator.com",
                Email = "EBlake@mailinator.com",
                FirstName = "Edward",
                LastName = "Blake",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "!1Qwerty");
                    await userManager.AddToRoleAsync(defaultUser, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion

            string demoPassword = "ytrewQ1!";

            //These are my seeded demo users for showing off the software
            //Each user occupies a "main" role and the new Demo role
            //We will target this Demo role to prevent demo users from changing the database

            #region Demo Administrator Seed
            defaultUser = new PSUser
            {
                UserName = "demoadmin@mailinator.com",
                Email = "demoadmin@mailinator.com",
                FirstName = "Josh",
                LastName = "Scott",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, Roles.Administrator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Demo Administrator User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Demo Project Manager Seed
            defaultUser = new PSUser
            {
                UserName = "demopm@mailinator.com",
                Email = "demopm@mailinator.com",
                FirstName = "Sally",
                LastName = "Jupiter",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Demo Project Manager User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Demo Developer Seed
            defaultUser = new PSUser
            {
                UserName = "nitetester@mailinator.com",
                Email = "nitetester@mailinator.com",
                FirstName = "Hollis",
                LastName = "Mason",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Demo Developer User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Demo Submitter Seed
            defaultUser = new PSUser
            {
                UserName = "spectretester@mailinator.com",
                Email = "spectretester@mailinator.com",
                FirstName = "Laurie",
                LastName = "Blake",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Demo Submitter User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }
            #endregion
            #region Demo New User Seed
            defaultUser = new PSUser
            {
                UserName = "hoodedtester@mailinator.com",
                Email = "hoodedtester@mailinator.com",
                FirstName = "Will",
                LastName = "Reeves",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, demoPassword);
                    await userManager.AddToRoleAsync(defaultUser, Roles.NewUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************** ERROR *************");
                Debug.WriteLine("Error Seeding Demo New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("**********************************");
                throw;
            }

            #endregion
        }
        #endregion
    }
}