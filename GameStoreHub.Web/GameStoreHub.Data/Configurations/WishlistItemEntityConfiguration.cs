using GameStoreHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStoreHub.Data.Configurations
{
	public class WishlistItemEntityConfiguration : IEntityTypeConfiguration<WishlistItem>
	{
		public void Configure(EntityTypeBuilder<WishlistItem> builder)
		{
			builder
				.HasKey(og => new { og.WishlistId, og.GameId });

			builder
				.HasOne(og => og.Wishlist)
				.WithMany(o => o.WishlistItems)
				.HasForeignKey(og => og.WishlistId)
				.OnDelete(DeleteBehavior.Restrict);

			builder
				.HasOne(og => og.Game)
				.WithMany(g => g.)
				.HasForeignKey(og => og.GameId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
