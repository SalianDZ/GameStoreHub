namespace GameStoreHub.Web.ViewModels.Wishlist
{
	public class WishlistItemViewModel
	{
		public Guid GameId { get; set; }
		public string GameTitle { get; set; } = null!;
		public string GameImagePath { get; set; } = null!;
	}
}
