using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Services;

namespace MVCPrbSol  //Namespace is the outermost , Inside is a class, then a method, then the logic
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           var host = CreateHostBuilder(args).Build();
            await DataHelper.ManageDataAsync(host);//Async?
            ////using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            //    try
            //    {
            //        var context = services.GetRequiredService<ApplicationDbContext>();
            //        var userManager = services.GetRequiredService<UserManager<PSUser>>();
            //        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            //        //This is where we seed Roles, Users, and Ticket items
            //        //await ContextSeed.RunSeedMethods(roleManager, userManager, context);     this is the defualt way
            //        //await ContextSeed.SeedRolesAsync(roleManager);
            //        //await ContextSeed.SeedDefaultUsersAsync(userManager);
            //        // projects, users, tickets
            //        //await Data.Enums.TicketPrioritiesAsync(context);
            //        //await ContextSeed.SeedDefaultTicketStatusesAsync(context);
            //        //await ContextSeed.SeedDefaultTicketTypesAsync(context);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Exception while running Manage Data => {ex}");
            //        var logger = loggerFactory.CreateLogger<Program>();
            //        logger.LogError(ex, "An error occurred seeding the Database.");
            //    }
            //}
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
                    webBuilder.UseStartup<Startup>();
                });
    }
}