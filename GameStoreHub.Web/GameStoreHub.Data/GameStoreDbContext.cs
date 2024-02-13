using GameStoreHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Order>()
				.HasMany(o => o.OrderGames)
				.WithOne(og => og.Order)
				.HasForeignKey(og => og.OrderId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<OrderGame>()
				.HasOne(og => og.Order)
				.WithMany(o => o.OrderGames)
				.HasForeignKey(og => og.OrderId)
				.OnDelete(DeleteBehavior.Restrict); // or another behavior as needed

			builder.Entity<OrderGame>()
				.HasOne(og => og.Game)
				.WithMany(g => g.OrderGames) // Assuming Game doesn't have a navigation property back to OrderGame
				.HasForeignKey(og => og.GameId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<OrderGame>()
				.Property(og => og.PriceAtPurchase)
				.HasColumnType("decimal(18,2)");

			builder.Entity<Order>()
				.Property(o => o.TotalPrice)
				.HasColumnType("decimal(18,2)");

			builder.Entity<Game>()
				.Property(g => g.Price)
				.HasColumnType("decimal(18,2)");

			builder.Entity<OrderGame>()
				.HasKey(og => new { og.OrderId, og.GameId });

			base.OnModelCreating(builder);
		}
	}
}