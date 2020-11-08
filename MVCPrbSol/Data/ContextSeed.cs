using Microsoft.AspNetCore.Identity;
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
        ProjectManager,
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
            await roleManager.CreateAsync(new IdentityRole(Roles.ProjectManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Developer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Submitter.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.NewUser.ToString()));
        }



        //In order to have default values in place for our Ticket types, statuses, and priorities, we need to seed
        //default values in place.
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
                if (!context.TicketPriorities.Any(tp => tp.Name == "Blocker"))
                {
                    await context.TicketPriorities.AddAsync(new TicketPriority { Name = "Blocker" });

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
                if (!context.TicketStatuses.Any(ts => ts.Name == "Pending"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "Pending" });

                }
                if (!context.TicketStatuses.Any(ts => ts.Name == "In-Progress"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "In-Progress" });

                }
                if (!context.TicketStatuses.Any(ts => ts.Name == "Completed"))
                {
                    await context.TicketStatuses.AddAsync(new TicketStatus { Name = "Completed" });

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

        #region Seed Default Ticket Types
        public static async Task SeedDefaultTicketTypesAsync(ApplicationDbContext context)
        {
            try
            {
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



        //Seed Users
        public static async Task SeedDefaultUsersAsync(UserManager<PSUser> userManager)
        {
            #region SeedAdmin
            //SeedDefault Admin User
            var defaultAdmin = new PSUser
            {
                UserName = "TaliaalGhul@email.com",
                Email = "TaliaalGhul@email.com",
                FirstName = "Talia",
                LastName = "al Ghul",
                EmailConfirmed = true
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
                Debug.WriteLine("************ ERROR  ************");
                Debug.WriteLine("Error Seeding Default Admin User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("********************************");
                throw;
            }
            #endregion

            //SeedDefault PM User
            #region SeedPM        

            var defaultPM = new PSUser
            {
                UserName = "PamelaIsley@email.com",
                Email = "PamelaIsley@email.com",
                FirstName = "Pamela",
                LastName = "Isley",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultPM.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultPM, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultPM, Roles.ProjectManager.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************ ERROR  ************");
                Debug.WriteLine("Error Seeding Default ProjectManager User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("********************************");
                throw;
            }
            #endregion

            //SeedDefault Developer User
            #region SeedDeveloper        

            var defaultDeveloper = new PSUser
            {
                UserName = "RomanSion@email.com",
                Email = "RomanSionis@email.com",
                FirstName = "Roman",
                LastName = "Sionis",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultDeveloper.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultDeveloper, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultDeveloper, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************ ERROR  ************");
                Debug.WriteLine("Error Seeding Default Developer User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("********************************");
                throw;
            }
            #endregion

            //SeedDefault Submitter User
            #region SeedSubmitter

            var defaultSubmitter = new PSUser
            {
                UserName = "ThomasElliot@email.com",
                Email = "ThomasElliot@email.com",
                FirstName = "Thomas",
                LastName = "Elliot",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultSubmitter.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultSubmitter, "g9N88.se!");
                    await userManager.AddToRoleAsync(defaultSubmitter, Roles.Submitter.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************ ERROR  ************");
                Debug.WriteLine("Error Seeding Default Submitter User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("********************************");
                throw;
            }
            #endregion

            //SeedDefault NewUser User
            #region SeedNewUser

            var defaultNewUser = new PSUser
            {
                UserName = "BruceWayne@email.com",
                Email = "BruceWayne@email.com",
                FirstName = "Bruce",
                LastName = "Wayne",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNewUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNewUser, "d(b9L>?D_Bg");
                    await userManager.AddToRoleAsync(defaultNewUser, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************ ERROR  ************");
                Debug.WriteLine("Error Seeding Default NewUser User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("********************************");
                throw;
            }
            #endregion
        }
    }
}