namespace GameStoreHub.Data.Models
{
	public class WishlistItem
	{
        public WishlistItem()
        {
            Id = Guid.NewGuid();    
        }

        public Guid Id { get; set; }

        public Guid WishlistId { get; set; }

        public Wishlist Wishlist { get; set; } = null!;

        public Guid GameId { get; set; }

        public Game Game { get; set; } = null!;
    }
}
