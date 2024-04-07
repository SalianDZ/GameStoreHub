using GameStoreHub.Data.Models.Enums;
using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using GameStoreHub.Data;
using GameStoreHub.Common;
using GameStoreHub.Web.ViewModels.OrderGame;
using GameStoreHub.Web.ViewModels.Wishlist;

namespace GameStoreHub.Services.Data
{
	public class WishlistService : IWishlistService
	{
		private readonly GameStoreDbContext dbContext;

        public WishlistService(GameStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Wishlist> GetOrCreateWishlistForUserByUserIdAsync(string userId)
		{
			Wishlist? wishlist =
			await dbContext.Wishlists
			.Include(o => o.WishlistItems)
			.ThenInclude(o => o.Game)
			.FirstOrDefaultAsync(o => o.UserId == Guid.Parse(userId));

			if (wishlist == null)
			{
				wishlist = new Wishlist
				{
					UserId = Guid.Parse(userId),
					WishlistItems = new HashSet<WishlistItem>()
				};

				dbContext.Wishlists.Add(wishlist);
				await dbContext.SaveChangesAsync();

			}

			return wishlist;
		}

		public async Task<OperationResult> AddItemToWishlist(string userId, string gameId)
		{
			OperationResult result = new();

			try
			{
				Wishlist wishlist = await GetOrCreateWishlistForUserByUserIdAsync(userId);

				// Since the game is not in the wishlist, proceed to add it
				Game game = await dbContext.Games.FirstAsync(g => g.Id == Guid.Parse(gameId));

				WishlistItem newItem = new()
				{
					WishlistId = wishlist.Id,
					Game = game
				};

				dbContext.WishlistItems.Add(newItem);
				await dbContext.SaveChangesAsync();
				result.SetSuccess();
			}
			catch (Exception)
			{
				result.AddError("An error occured while attempting to procced with the data!");
			}

			return result;
		}

		public async Task<OperationResult> RemoveItemFromWishlist(string userId, string gameId)
		{
			OperationResult result = new();
			try
			{
				Wishlist wishlist = await GetOrCreateWishlistForUserByUserIdAsync(userId);

				WishlistItem existingItem = wishlist.WishlistItems.First(og => og.GameId == Guid.Parse(gameId));

				dbContext.WishlistItems.Remove(existingItem!);

				await dbContext.SaveChangesAsync();
				result.SetSuccess();
			}
			catch (Exception)
			{
				result.AddError("An error occured while attempting to procced with the data!");
			}

			return result;
		}

		public async Task<IEnumerable<WishlistItemViewModel>> GetWishlistItemsByUserIdAsync(string userId)
		{
			Wishlist wishlist = await GetOrCreateWishlistForUserByUserIdAsync(userId);

			IEnumerable<WishlistItemViewModel> items =
				wishlist.WishlistItems
				.Select(og => new WishlistItemViewModel
				{
					GameId = og.GameId,
					GameImagePath = og.Game.ImagePath,
					GameTitle = og.Game.Title,
					PriceAtPurchase = og.Game.Price
				}).ToList();

			return items;
		}

		public async Task<bool> IsGameInWishlistByIdAsync(string userId, string gameId)
		{
			Wishlist wishlist = await GetOrCreateWishlistForUserByUserIdAsync(userId);

			WishlistItem? existingItem = wishlist.WishlistItems.FirstOrDefault(og => og.GameId == Guid.Parse(gameId));

			if (existingItem != null)
			{
				return true;
			}

			return false;
		}
	}
}
