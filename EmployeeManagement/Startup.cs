using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>();
           

            services.AddMvc(options => {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                  .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

			services.AddAuthentication()
				.AddGoogle(options =>
				{
					options.ClientId = "847188634534-f2s6k1b2ttngl7sd14vba53ije6ttbr1.apps.googleusercontent.com";
					options.ClientSecret = "4JybpBtS06HkSQKXXiHiC5Vj";
				})
				.AddFacebook(options =>
			   {
				   options.AppId = "893413661210935";
				   options.AppSecret = "55322fa768d4a0dd3581fffc328d3dfa";
			   });

			services.ConfigureApplicationCookie(optiions =>
			{
				optiions.AccessDeniedPath = new PathString("/Administration/AccessDenied");
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("DeleteRolePolicy",
					policy => policy.RequireClaim("Delete Role"));

				options.AddPolicy("EditRolePolicy",
					policy => policy.AddRequirements(new ManageAdminsRolesAndClaimsRequirement()));


				options.AddPolicy("AdminRolePolicy",
					policy => policy.RequireRole("Admin"));
			});

            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

			services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
			services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
               
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
   
            app.UseStaticFiles();
            app.UseAuthentication();
            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{Id?}");
            });



        }
    }
}
