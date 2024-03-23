using GameStoreHub.Common;
using GameStoreHub.Data.Models;
using GameStoreHub.Web.ViewModels.Order;
using GameStoreHub.Web.ViewModels.OrderGame;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface ICartService
	{
		Task<Order> GetOrCreateCartForUserByUserIdAsync(string userId);

		Task<CheckoutViewModel> GetCartViewModelByUserIdAsync(string userId);

		Task<ValidationResult> ValidateCartByUserIdAsync(string userId, IEnumerable<CheckoutItemViewModel> cartItems);

		Task<OrderResult> CreateOrderAsync(string userId, CheckoutViewModel model);

		Task<IEnumerable<CheckoutItemViewModel>> GetItemsForCheckoutByUserIdAsync(string userId);

		Task<bool> AddItemToCart(string userId, string gameId);

		Task<IEnumerable<CheckoutItemViewModel>> GetCartItemsByUserIdAsync(string userId);
	}
}
