using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data;
using GameStoreHub.Services.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Web
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<GameStoreDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
			{
				options.SignIn.RequireConfirmedAccount =
					builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
				options.Password.RequireNonAlphanumeric =
					builder.Configuration.GetValue<bool>("Password:SignIn:RequireNonAlphanumeric");
				options.Password.RequireUppercase =
					builder.Configuration.GetValue<bool>("Password:SignIn:RequireUppercase");
				options.Password.RequireLowercase = 
					builder.Configuration.GetValue<bool>("Password:SignIn:RequireLowercase");
			}).AddEntityFrameworkStores<GameStoreDbContext>();

			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<IGameService, GameService>();
			builder.Services.AddScoped<IReviewService, ReviewService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IOrderService, OrderService>();
			builder.Services.AddScoped<IWishlistService, WishlistService>();

			builder.Services.ConfigureApplicationCookie(cfg =>
			{
				cfg.LoginPath = "/User/Login";
			});

			builder.Services.AddControllersWithViews()
				.AddMvcOptions(options =>
				{
					options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
				});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			app.Run();
		}
	}
}