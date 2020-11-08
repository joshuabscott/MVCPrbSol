using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MVCPrbSol        //Like the Russian Nesting Doll, Namespace is the outermost Doll, Inside is a class, then a method, then the logic 
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();            //Old line of code, we replaced this
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<PSUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await ContextSeed.SeedRolesAsync(roleManager);
                    await ContextSeed.SeedDefaultUsersAsync(userManager);
                    await ContextSeed.SeedDefaultTicketPrioritiesAsync(context); //Need to seed all 3 Ticket controllers default selections in ContextSeed
                    await ContextSeed.SeedDefaultTicketStatusesAsync(context);
                    await ContextSeed.SeedDefaultTicketTypesAsync(context);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occured seeding the DB.");
                }
            }
            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}