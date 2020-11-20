using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MVCPrbSol.Models;
using NuGet.Frameworks;

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

    //public enum TicketTypes
    //{
    //    UI,
    //    Calculation,
    //    Logic,
    //    Security
    //}

    //public enum TicketPriorities
    //{
    //    Low,
    //    Moderate,
    //    Major,
    //    Critical
    //}

    //public enum TicketStatuses
    //{
    //    Opened,
    //    Testing,
    //    Development,
    //    QA,
    //    FinalPass,
    //    Closed
    //}
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



        //Seed Users
        public static async Task SeedDefaultUsersAsync(UserManager<PSUser> userManager)
        {
            //SeedDefault Administrator
            #region SeedAdministrator
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
                Debug.WriteLine("Error Seeding Default Administrator.");
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

            //SeedDefault Developer
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

            #region New
            // New User
            var defaultNew = new PSUser
            {
                UserName = "a@b.com",
                Email = "a@b.com",
                FirstName = "Adam",
                LastName = "Brooks",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Abrooks1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "a@d.com",
                Email = "a@d.com",
                FirstName = "Anthony",
                LastName = "Duval",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Aduval1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "j@h.com",
                Email = "j@h.com",
                FirstName = "Jackson",
                LastName = "Holliday",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jholliday1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "j@s.com",
                Email = "j@s.com",
                FirstName = "Josh",
                LastName = "Scott",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jscott1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "k@b.com",
                Email = "k@b.com",
                FirstName = "Kenan",
                LastName = "Bjelosevic",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Kbjelosevic1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "m@n.com",
                Email = "m@n.com",
                FirstName = "MacColl",
                LastName = "Nicolson",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Mnicolson1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "a@e.com",
                Email = "a@e.com",
                FirstName = "Adrian",
                LastName = "Edelen",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Aedelen1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "b@c.com",
                Email = "b@c.com",
                FirstName = "Beau",
                LastName = "Cunningham",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Bcunningham1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "j@h.com",
                Email = "j@h.com",
                FirstName = "Jessica",
                LastName = "Hedenskog",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jhedenskog1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "k@c.com",
                Email = "k@c.com",
                FirstName = "Kit",
                LastName = "Chau",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Kchau1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "n@w.com",
                Email = "n@w.com",
                FirstName = "Nick",
                LastName = "Webb",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Nwebb1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "t@b.com",
                Email = "t@b.com",
                FirstName = "Tony",
                LastName = "Beavers",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Tbeavers1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "andy@rivera.com",
                Email = "andy@rivera.com",
                FirstName = "Andres",
                LastName = "Rivera",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Arivera1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "c@t.com",
                Email = "c@t.com",
                FirstName = "Charles",
                LastName = "Tincher",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Ctincher1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "e@j.com",
                Email = "e@j.com",
                FirstName = "Ethan",
                LastName = "Jones",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Ejones1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "eli@jones.com",
                Email = "eli@jones.com",
                FirstName = "Eli",
                LastName = "Jones",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Ejones1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "j@g.com",
                Email = "j@g.com",
                FirstName = "Jonathan",
                LastName = "Green",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jgreen1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "joseph@green.com",
                Email = "joseph@green.com",
                FirstName = "Joseph",
                LastName = "Green",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jgreen1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "j@s.com",
                Email = "j@s.com",
                FirstName = "Julio",
                LastName = "Segarra",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jsegarra1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "l@a.com",
                Email = "l@a.com",
                FirstName = "Larry",
                LastName = "Ashton",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Lashton1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "o@o.com",
                Email = "o@o.com",
                FirstName = "Orlando",
                LastName = "Olmo",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Oolmo1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "a@a.com",
                Email = "a@a.com",
                FirstName = "Andrew",
                LastName = "Albanese",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Aalbanese1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "d@j.com",
                Email = "d@j.com",
                FirstName = "Denis",
                LastName = "Jojot",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Djojot1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "f@s.com",
                Email = "f@s.com",
                FirstName = "Fred",
                LastName = "Smith",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Fsmith1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "d@c.com",
                Email = "d@c.com",
                FirstName = "Danny",
                LastName = "Carroll",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Dcarroll1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "m@j.com",
                Email = "m@j.com",
                FirstName = "Mark",
                LastName = "Janicki",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Mjanicki1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            defaultNew = new PSUser
            {
                UserName = "s@j.com",
                Email = "s@j.com",
                FirstName = "Shyann",
                LastName = "Jobe",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Sjobe1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            #endregion



            //Seed Demo Administrator
            #region Demo Administrator
            var demoAdmin = new PSUser
            {
                UserName = "demoAdmin@mailinator.com",
                Email = "demoAdmin@mailinator.com",
                FirstName = "DemoA",
                LastName = "Admin",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(demoAdmin.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoAdmin, "g9N88.se!");
                    await userManager.AddToRoleAsync(demoAdmin, Roles.Administrator.ToString());
                    await userManager.AddToRoleAsync(demoAdmin, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("************ ERROR  ************");
                Debug.WriteLine("Error Seeding Default Administrator.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("********************************");
                throw;
            }
            #endregion

            //Seed Demo PM User
            #region Demo SeedPM        

            var demoPM = new PSUser
            {
                UserName = "demoPM@mailinator.com",
                Email = "demoaPM@mailinator.com",
                FirstName = "DemoPM",
                LastName = "PM",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(demoPM.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoPM, "g9N88.se!");
                    await userManager.AddToRoleAsync(demoPM, Roles.ProjectManager.ToString());
                    await userManager.AddToRoleAsync(demoPM, Roles.Demo.ToString());
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

            //Seed Demo Developer User
            #region Demo Developer        

            var demoDeveloper = new PSUser
            {
                UserName = "demoaDev@mailinator.com",
                Email = "demoDev@mailinator.com",
                FirstName = "DemoDev",
                LastName = "Dev",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(demoDeveloper.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoDeveloper, "g9N88.se!");
                    await userManager.AddToRoleAsync(demoDeveloper, Roles.Developer.ToString());
                    await userManager.AddToRoleAsync(demoDeveloper, Roles.Demo.ToString());
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

            //Seed Demo Submitter User
            #region Demo Submitter

            var demoSubmitter = new PSUser
            {
                UserName = "demoSub@mailinator.com",
                Email = "demoSub@mailinator.com",
                FirstName = "DemoSub",
                LastName = "Sub",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(demoSubmitter.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoSubmitter, "g9N88.se!");
                    await userManager.AddToRoleAsync(demoSubmitter, Roles.Submitter.ToString());
                    await userManager.AddToRoleAsync(demoSubmitter, Roles.Demo.ToString());
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

            //Seed Demo NewUser User
            #region Demo NewUser

            var demoNewUser = new PSUser
            {
                UserName = "demoUser@mailinator.com",
                Email = "demoUser@mailinator.com",
                FirstName = "DemoNU",
                LastName = "NewUser",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(demoNewUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoNewUser, "g9N88.se!");
                    await userManager.AddToRoleAsync(demoNewUser, Roles.NewUser.ToString());
                    await userManager.AddToRoleAsync(demoNewUser, Roles.Demo.ToString());
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