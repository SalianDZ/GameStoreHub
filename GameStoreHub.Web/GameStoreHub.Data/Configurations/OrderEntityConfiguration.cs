using GameStoreHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStoreHub.Data.Configurations
{
	public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
               .HasMany(o => o.OrderGames) // Assuming Order has a collection of OrderGames
               .WithOne(og => og.Order)    // Assuming OrderGame has a navigation property back to Order
               .HasForeignKey(og => og.OrderId)
               .OnDelete(DeleteBehavior.Cascade); // Assuming OrderGame has a foreign key property to Order
			builder
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}
