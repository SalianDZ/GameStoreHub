using GameStoreHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStoreHub.Data.Configurations
{
    public class OrderGameEntityConfiguration : IEntityTypeConfiguration<OrderGame>
    {
        public void Configure(EntityTypeBuilder<OrderGame> builder)
        {
            builder
                .HasOne(og => og.Order)
                .WithMany(o => o.OrderGames)
                .HasForeignKey(og => og.OrderId)
				.OnDelete(DeleteBehavior.Cascade);

            
            builder
                .HasOne(og => og.Game)
                .WithMany(g => g.OrderGames)
                .HasForeignKey(og => og.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(og => og.PriceAtPurchase)
                .HasColumnType("decimal(18,2)");

            builder
                .HasKey(og => new { og.OrderId, og.GameId });
        }
    }
}
