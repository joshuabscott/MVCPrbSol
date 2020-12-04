using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using MVCPrbSol.Data;
using MVCPrbSol.Models;
using MVCPrbSol.Services;

namespace MVCPrbSol
{
    public class Startup    //At Startup the tools/services are declared and confirmed for use  Remember Dependences Injection is set up her in the configureServices
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) // Remember Dependences Injection is set up her in the configureServices
        {//services are configured for using DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseNpgsql(//for using DbContext
                  DataHelper.GetConnectionString(Configuration)));
            //Configuration.GetConnectionString("DefaultConnection")));

            //using directive for injection using IdentityRole with PSUser
            services.AddIdentity<PSUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddScoped<IPSRolesService, PSRolesService>();
            services.AddScoped<IPSProjectService, PSProjectService>();
            services.AddScoped<IPSHistoriesService, PSHistoriesService>();
            services.AddScoped<IPSAccessService, PSAccessService>();
            services.AddScoped<IPSFileService, PSFileService>();
            //new service addded Mon 23
            services.AddScoped<IPSFileService, PSFileService>();

            //These services are for the Email
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSender, EmailService>();

            //adding controllers with views and RazorPages
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                pattern: "{controller=Home}/{action=LandingPage}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}