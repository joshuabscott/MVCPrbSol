using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVCPrbSol.Data;
using Npgsql;

namespace MVCPrbSol.Services
{
    public class DataHelper
    {
        //The default connection string will come from app settings like usual
        public static string GetConnectionString(IConfiguration configuration)
        {
            //It will be automatically overwritten if we are running on Heroku
            var connnectionString = configuration.GetConnectionString("DefaultConnection");

            var herokuDatabaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(herokuDatabaseUrl) ? connnectionString : BuildConnectionString(herokuDatabaseUrl);
        }

        public static string BuildConnectionString(string herokuDataBaseUrl)
        {
            //Provide an object representation of a uniform resource identifier URI
            var herokuDatabaseUri = new Uri(herokuDataBaseUrl);
            var userInfo = herokuDatabaseUri.UserInfo.Split(":");
            //Provides a simple way to create and manage the contents of connection strings used by the NpgsqlConnection class.
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = herokuDatabaseUri.Host,
                Port = herokuDatabaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = herokuDatabaseUri.LocalPath.TrimStart('/')
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
            catch (PostgresException ex)
            {
                Console.WriteLine($"Something went wrong: {ex}");
            }
        }
    }
}
