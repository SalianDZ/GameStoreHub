using GameStoreHub.Common;
using GameStoreHub.Data.Models;
using GameStoreHub.Web.ViewModels.Wishlist;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface IWishlistService
	{
		Task<Wishlist> GetOrCreateWishlistForUserByUserIdAsync(string userId);

		Task AddItemToWishlist(string userId, string gameId);

		Task RemoveItemFromWishlist(string userId, string gameId);

		Task<IEnumerable<WishlistItemViewModel>> GetWishlistItemsByUserIdAsync(string userId);

		Task<bool> IsGameInWishlistByIdAsync(string userId, string gameId);
	}
}
