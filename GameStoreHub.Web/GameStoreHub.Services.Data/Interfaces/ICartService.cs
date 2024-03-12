using GameStoreHub.Data.Models;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface ICartService
	{
		Task<Order?> GetOrCreateCartForUserByUserIdAsync(string userId);
	}
}
