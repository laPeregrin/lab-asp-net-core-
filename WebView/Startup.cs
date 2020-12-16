using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using BL_BusinessLogic_;
using WebView.Roles;
using System.Security.Claims;

namespace WebView
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
            services
                .AddAuthentication("Cookie")
                .AddCookie("Cookie", options =>
                 {
                     options.LoginPath = "/Account/GetRegistrationView";
                     options.AccessDeniedPath = "/Main/Index";
                 });
            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy("Manager", builder =>
                    {
                        builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "Manager")
                        || x.User.HasClaim(ClaimTypes.Role, "Contacts"));
                    });
                    options.AddPolicy("Contact", builder =>
                    {
                        builder.RequireClaim(ClaimTypes.Role, "Contact");
                    });
                    options.AddPolicy("Administrator", builder =>
                    {
                        builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "Administrator"));
                    });
                });


            services
                .AddControllersWithViews();
            services
                .AddSingleton<ContactBLL>();
            services
                .AddSingleton<ManagerBLL>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //who are you
            app.UseAuthentication();
            //are you alloved?
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
