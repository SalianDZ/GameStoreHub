using GameStoreHub.Data.Models;
using GameStoreHub.Web.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static GameStoreHub.Common.EntityValidationConstants.GeneralApplicationConstants;

namespace GameStoreHub.Web.Infrastructure.Extensions
{
	public static class WebApplicationBuilderExtensions
	{
        /// <summary>
        /// This method seeds admin role if it does not exist.
        /// Passed email should be valid email of existing user in the application.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email)
        {
            using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

            IServiceProvider serviceProvider = scopedServices.ServiceProvider;

            UserManager<ApplicationUser> userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole<Guid>> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdminRoleName))
                {
                    return;
                }

                IdentityRole<Guid> role =
                    new IdentityRole<Guid>(AdminRoleName);

                await roleManager.CreateAsync(role);

                ApplicationUser adminUser =
                    await userManager.FindByEmailAsync(email);

                await userManager.AddToRoleAsync(adminUser, AdminRoleName);
            })
            .GetAwaiter()
            .GetResult();

            return app;
        }

        //public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email)
        //{
        //	using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

        //	IServiceProvider serviceProvider = scopedServices.ServiceProvider;

        //	UserManager<ApplicationUser> userManager = 
        //		serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        //	RoleManager<IdentityRole<Guid>> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        //	Task.Run(async () => 
        //	{
        //		if (await roleManager.RoleExistsAsync(AdminRoleName))
        //		{
        //			return;
        //		}

        //		IdentityRole<Guid> role = new IdentityRole<Guid>(AdminRoleName);
        //		await roleManager.CreateAsync(role);

        //		ApplicationUser adminUser = 
        //		await userManager.FindByEmailAsync(email);

        //		await userManager.AddToRoleAsync(adminUser, AdminRoleName);
        //	})
        //	.GetAwaiter()
        //	.GetResult();

        //	return app;
        //}

        public static IApplicationBuilder EnableOnlineUsersCheck(this IApplicationBuilder app)
		{
			return app.UseMiddleware<OnlineUsersMiddleware>();
		}
	}
}
