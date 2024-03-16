using GameStoreHub.Web.ViewModels.OrderGame;

namespace GameStoreHub.Web.ViewModels.Order
{
	public class CheckoutViewModel
	{
		public IEnumerable<CheckoutItemViewModel> Items { get; set; } = new HashSet<CheckoutItemViewModel>();
		public decimal TotalPrice => Items.Sum(item => item.PriceAtPurchase);

		public CheckoutBillingFormViewModel BillingData { get; set; } = new();
    }
}
