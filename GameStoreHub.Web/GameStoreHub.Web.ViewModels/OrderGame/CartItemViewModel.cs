namespace GameStoreHub.Web.ViewModels.OrderGame
{
	public class CartItemViewModel
	{
		public Guid GameId { get; set; }
		public string GameTitle { get; set; } = null!;
        public string GameImagePath { get; set; } = null!;
        public decimal PriceAtPurchase { get; set; }
	}
}
