using GameStoreHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStoreHub.Data.Configurations
{
	public class WishlistEntityConfiguration : IEntityTypeConfiguration<Wishlist>
	{
		public void Configure(EntityTypeBuilder<Wishlist> builder)
		{
			builder
			.HasMany(o => o.WishlistItems)
			.WithOne(og => og.Wishlist)
			.HasForeignKey(og => og.WishlistId)
			.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
