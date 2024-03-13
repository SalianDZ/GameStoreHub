using GameStoreHub.Data.Models;
using GameStoreHub.Web.ViewModels.Order;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface ICartService
	{
		Task<Order> GetOrCreateCartForUserByUserIdAsync(string userId);

		Task<CartViewModel> GetCartViewModelByUserIdAsync(string userId);
	}
}
