using GameStoreHub.Web.ViewModels.OrderGame;

namespace GameStoreHub.Web.ViewModels.Order
{
	public class CartViewModel
	{
		public IEnumerable<CartItemViewModel> Items { get; set; } = new HashSet<CartItemViewModel>();
		public decimal TotalPrice => Items.Sum(item => item.PriceAtPurchase);
	}
}
