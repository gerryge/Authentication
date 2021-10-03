using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Basics.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Basics
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Cookie")
                .AddCookie("Cookie", options =>
                {
                    options.Cookie.Name = "Gerry.Cookie";
                    options.LoginPath = "/Home/Authenticate";
                });

            services.AddAuthorization(options =>
            {
                // var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                // var defaultAuthPolicy = defaultAuthBuilder
                //     .RequireAuthenticatedUser()
                //     .RequireClaim(ClaimTypes.DateOfBirth)
                //     .Build();
                // options.DefaultPolicy = defaultAuthPolicy;

                // options.AddPolicy("Claim.DoB", 
                //     policyBuilder => { policyBuilder.RequireClaim(ClaimTypes.DateOfBirth); });

                // options.AddPolicy("Claim.DoB",
                //     policyBuilder =>
                //     {
                //         policyBuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                //     });
                options.AddPolicy("Claim.DoB",
                    policyBuilder => { policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth); });

                // //Use Policy instead of Role
                // options.AddPolicy("Admin",
                //     policyBuilder => { policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"); });
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}