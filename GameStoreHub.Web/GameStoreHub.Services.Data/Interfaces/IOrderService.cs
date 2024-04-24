using GameStoreHub.Data.Models;
using GameStoreHub.Web.ViewModels.Order;
using GameStoreHub.Web.ViewModels.OrderGame;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface IOrderService
	{
		Task<Order> GetOrCreateCartForUserByUserIdAsync(string userId);

		Task<CheckoutViewModel> GetCartViewModelByUserIdAsync(string userId);

		Task CreateOrderAsync(string userId, CheckoutViewModel model);

		Task AddItemToCart(string userId, string gameId);

        Task RemoveItemFromCart(string userId, string gameId);

        Task<IEnumerable<CheckoutItemViewModel>> GetCartItemsByUserIdAsync(string userId);

		Task<bool> IsGameInCartByIdAsync(string userId, string gameId);

		Task AssignActivationCodesToUserOrderByUserIdAsync(string userId);

		Task<IEnumerable<CheckoutItemViewModel>> GetPurchasedItemsByUserIdAsync(string userId);

		Task<bool> IsGameAlreadyBoughtBefore(string userId, string gameId);

		Task<string> GetActivationCodeByUserAndGameIdAsync(string userId, string gameId);

		Task<IEnumerable<OrderViewModel>> AllOrdersAsync();
	}
}
