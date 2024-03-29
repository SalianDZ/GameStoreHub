namespace GameStoreHub.Data.Models
{
	public class Wishlist
	{
        public Wishlist()
        {
            Id = Guid.NewGuid();
            WishlistItems = new HashSet<WishlistItem>();    
        }

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;

        public ICollection<WishlistItem> WishlistItems { get; set; }
    }
}
