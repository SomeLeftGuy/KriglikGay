using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesMain.Controllers;
using CoursesMain.Filters;
using CoursesMain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoursesMain
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication()
               .AddFacebook(facebookOptions =>
               {
                facebookOptions.AppId = "383083749274356";
                   facebookOptions.AppSecret = "0336b92886a9c424ba8be4ee6107373c";
               })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = "75f77783-3fd8-4ee7-a778-6dff6a86a966";
                    microsoftOptions.ClientSecret = "V5UxIPjpJZPbs9A2Ytt0FRmi.DXvX@:@";
                });

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
               //.AddDefaultUI(UIFramework.Bootstrap4);

            services.AddSignalR();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseSignalR(routes =>
            {
                routes.MapHub<CommentsHub>("/Comments");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
