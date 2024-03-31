using GameStoreHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GameStoreHub.Data
{
	public class GameStoreDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{
		public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options)
			: base(options)
		{
		}

		public DbSet<Game> Games { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<OrderGame> OrderGames { get; set; } = null!;

        public DbSet<Review> Reviews { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

		public DbSet<Wishlist> Wishlists { get; set; } = null!;

        public DbSet<WishlistItem> WishlistItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
		{
			Assembly configAssembly = Assembly.GetAssembly(typeof(GameStoreDbContext)) ??
				Assembly.GetExecutingAssembly();	

            builder.ApplyConfigurationsFromAssembly(configAssembly);

            builder.Entity<ApplicationUser>()
			.Property(g => g.WalletBalance)
			.HasColumnType("decimal(18,2)");

            base.OnModelCreating(builder);
		}
	}
}