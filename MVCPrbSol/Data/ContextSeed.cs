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

    public enum TicketTypes
    {
        UI,
        Calculation,
        Logic,
        Security
    }

    public enum TicketPriorities
    {
        Critical,
        Important,
        Normal,
        Low
    }

    public enum TicketStatuses
    {
        Opened,
        Testing,
        Development,
        QA,
        FinalPass,
        Resolved
    }

    public static class ContextSeed
    { 
    public static async Task RunSeedMethodsAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<PSUser> userManager,
            ApplicationDbContext context)
    {
        await SeedRolesAsync(roleManager);
        await SeedDefaultUsersAsync(userManager);
        //await SeedTicketTypesAsync(context);
        //await SeedTicketStatusesAsync(context);
        //await SeedTicketPrioritiesAsync(context);
        //await SeedProjectsAsync(context);
        await SeedProjectUsersAsync(context, userManager);
        await SeedTicketsAsync(context, userManager);
    }
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

            //These are my seeded demo users for showing off the software
            //Each user occupies a "main" role and the new Demo role
            //We will target this Demo role to prevent demo users from changing the database
            string demoPassword = "ytrewQ1!";

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


        //In order to have default values in place for our Ticket types, statuses, and priorities, we need to seed
        //default values in place.

        #region Seed Default Ticket Types / Priorities / Statuses
        public static async Task SeedTicketListsAsync(ApplicationDbContext context)
        {
            try
            {
                // Types
                var types = context.TicketTypes.Any();
                if (types == false)
                {
                    var type = new TicketType { Name = TicketTypes.UI.ToString() };
                    context.TicketTypes.Add(type);
                    type = new TicketType { Name = TicketTypes.Calculation.ToString() };
                    context.TicketTypes.Add(type);
                    type = new TicketType { Name = TicketTypes.Logic.ToString() };
                    context.TicketTypes.Add(type);
                    type = new TicketType { Name = TicketTypes.Security.ToString() };
                    context.TicketTypes.Add(type);
                    await context.SaveChangesAsync();
                }

                // Priority
                var priorites = context.TicketPriorities.Any();
                if (priorites == false)
                {
                    var priority = new TicketPriority { Name = TicketPriorities.Critical.ToString() };
                    context.TicketPriorities.Add(priority);
                    priority = new TicketPriority { Name = TicketPriorities.Important.ToString() };
                    context.TicketPriorities.Add(priority);
                    priority = new TicketPriority { Name = TicketPriorities.Normal.ToString() };
                    context.TicketPriorities.Add(priority);
                    priority = new TicketPriority { Name = TicketPriorities.Low.ToString() };
                    context.TicketPriorities.Add(priority);
                    await context.SaveChangesAsync();
                }

                // Status
                var statuses = context.TicketStatuses.Any();
                if (statuses == false)
                {
                    var status = new TicketStatus { Name = TicketStatuses.Opened.ToString() };
                    context.TicketStatuses.Add(status);
                    status = new TicketStatus { Name = TicketStatuses.Testing.ToString() };
                    context.TicketStatuses.Add(status);
                    status = new TicketStatus { Name = TicketStatuses.Development.ToString() };
                    context.TicketStatuses.Add(status);
                    status = new TicketStatus { Name = TicketStatuses.QA.ToString() };
                    context.TicketStatuses.Add(status);
                    status = new TicketStatus { Name = TicketStatuses.FinalPass.ToString() };
                    context.TicketStatuses.Add(status);
                    status = new TicketStatus { Name = TicketStatuses.Resolved.ToString() };
                    context.TicketStatuses.Add(status);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Ticket Priorities.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
        }
        #endregion

        #region Seed Default Projects
        public static async Task SeedProjectAsync(ApplicationDbContext context)
        {
            List<string> projectNames = new List<string> { "Blog", "Bug Tracker", "Financial Portal" };
            foreach (var projectName in projectNames)
            {
                Project project = new Project
                {
                    Name = projectName
                };
                try
                {
                    if (context.Projects.FirstOrDefault(p => p.Name == projectName) == null)
                    {
                        await context.Projects.AddAsync(project);
                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("*********** ERROR **********");
                    Debug.WriteLine($"Error Seeding Project: {projectName}");
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine("****************************");
                    throw;
                }
            }
        }
        #endregion

        #region Seed Default Project Users
        public static async Task SeedProjectUsersAsync(ApplicationDbContext context, UserManager<PSUser> userManager)
        {
            string adminId = (await userManager.FindByEmailAsync("garretreynolds@mailinator.com")).Id;
            string pmId = (await userManager.FindByEmailAsync("alexheim@mailinator.com")).Id;
            string devId = (await userManager.FindByEmailAsync("dennisenerson@mailinator.com")).Id;
            string subId = (await userManager.FindByEmailAsync("larryedwards@mailinator.com")).Id;
            int project1Id = context.Projects.FirstOrDefault(p => p.Name == "Blog").Id;
            int project2Id = context.Projects.FirstOrDefault(p => p.Name == "Bug Tracker").Id;
            int project3Id = context.Projects.FirstOrDefault(p => p.Name == "Financial Portal").Id;

            List<string> userIds = new List<string> { adminId, pmId, devId, subId };
            List<int> projectIds = new List<int> { project1Id, project2Id, project3Id };

            ProjectUser projectUser = new ProjectUser();
            foreach (var userId in userIds)
            {
                foreach (var projectId in projectIds)
                {
                    projectUser.UserId = userId;
                    projectUser.ProjectId = projectId;
                    try
                    {
                        var record = context.ProjectUsers.FirstOrDefault(p => p.UserId == userId && p.ProjectId == projectId);
                        if (record == null)
                        {
                            await context.ProjectUsers.AddAsync(projectUser);
                            await context.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("*********** ERROR **********");
                        Debug.WriteLine($"Error Seeding {userId} for {projectId}");
                        Debug.WriteLine(ex.Message);
                        Debug.WriteLine("****************************");
                        throw;
                    }
                }
            }
        }
        #endregion

        #region Seed Default Tickets
        public static async Task SeedTicketsAsync(ApplicationDbContext context, UserManager<PSUser> userManager)
        {
            string devId = (await userManager.FindByEmailAsync("dennisenerson@mailinator.com")).Id;
            string subId = (await userManager.FindByEmailAsync("larryedwards@mailinator.com")).Id;
            int project1Id = context.Projects.FirstOrDefault(p => p.Name == "Blog").Id;
            int project2Id = context.Projects.FirstOrDefault(p => p.Name == "Bug Tracker").Id;
            int project3Id = context.Projects.FirstOrDefault(p => p.Name == "Financial Portal").Id;
            List<int> projects = new List<int> { project1Id, project2Id, project3Id };
            int numTickets = 10;
            var statuses = context.TicketStatuses.ToList();
            var types = context.TicketTypes.ToList();
            var priorities = context.TicketPriorities.ToList();
            for (var i = 0; i < numTickets; i++)
            {
                int random = new Random().Next(numTickets);
                bool hasDeveloper = random > (numTickets / 2) ? true : false;
                Ticket ticket = new Ticket
                {
                    Title = $"Random: {random}",
                    Description = $"{random} uniquely identifies this ticket",
                    Created = DateTimeOffset.Now.AddDays(-(random * random)),
                    Updated = DateTimeOffset.Now.AddHours(-(random * random)),
                    ProjectId = projects[new Random().Next(projects.Count)],
                    TicketPriorityId = priorities[new Random().Next(priorities.Count)].Id,
                    TicketStatusId = statuses[new Random().Next(statuses.Count)].Id,
                    TicketTypeId = types[new Random().Next(types.Count)].Id,
                    DeveloperUserId = hasDeveloper ? devId : null,
                    OwnerUserId = subId
                };
                try
                {
                    var myticket = context.Tickets.FirstOrDefault(t => t.Title == ticket.Title);
                    if (myticket == null)
                    {
                        await context.Tickets.AddAsync(ticket);
                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("*********** ERROR **********");
                    Debug.WriteLine($"Error Seeding ticket {random}");
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine("****************************");
                    throw;
                }
            }
            #endregion
        }
    }
}