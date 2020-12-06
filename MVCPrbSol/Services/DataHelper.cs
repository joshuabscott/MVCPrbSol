using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVCPrbSol.Data;
using MVCPrbSol.Models;

namespace MVCPrbSol.Services
{
    public class DataHelper
    {//working with the Heroku Database
        public static string GetConnectionString(IConfiguration configuration)
        {
            var connnectionString = configuration.GetConnectionString("DefaultConnection");
            var HerokuDatabaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(HerokuDatabaseUrl) ? connnectionString : BuildConnectionString(HerokuDatabaseUrl);
        }

        public static string BuildConnectionString(string HerokuDataBaseUrl)
        {
            var HerokuDatabaseUri = new Uri(HerokuDataBaseUrl);
            var userInfo = HerokuDatabaseUri.UserInfo.Split(":");

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = HerokuDatabaseUri.Host,
                Port = HerokuDatabaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = HerokuDatabaseUri.LocalPath.TrimStart('/')
            };
            return builder.ToString();
        }

        public static async Task ManageDataAsync(IHost host)
        {
            try
            {
                using var svcScope = host.Services.CreateScope();
                var svcProvider = svcScope.ServiceProvider;

                var context = svcProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong => {ex}");
            }
        }
    }
}
