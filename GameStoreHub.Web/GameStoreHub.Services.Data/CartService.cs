using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Data.Models.Enums;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Order;
using GameStoreHub.Web.ViewModels.OrderGame;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net;

namespace GameStoreHub.Services.Data
{
	public class CartService : ICartService
	{
		private readonly GameStoreDbContext dbContext;

        public CartService(GameStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<Order> GetOrCreateCartForUserByUserIdAsync(string userId)
		{
				Order? cart =
				await dbContext.Orders
				.Include(o => o.OrderGames)
				.FirstOrDefaultAsync(o => o.UserId == Guid.Parse(userId) && o.IsActive && o.OrderStatus == OrderStatus.InCart);

				if (cart == null)
				{
					cart = new Order
					{
						UserId = Guid.Parse(userId),
						IsActive = true,
						OrderDate = DateTime.UtcNow,
						OrderStatus = OrderStatus.InCart, // Or whatever logic you use to denote an active cart
						TotalPrice = 0,
						OrderGames = new HashSet<OrderGame>()
					};

					dbContext.Orders.Add(cart);
					await dbContext.SaveChangesAsync();

				}

			return cart;	
		}

		public async Task<bool> AddItemToCart(string userId, string gameId)
		{
			Order cart = await GetOrCreateCartForUserByUserIdAsync(userId);

			// Check if the game is already in the cart
			OrderGame? existingItem = cart.OrderGames.FirstOrDefault(og => og.GameId == Guid.Parse(gameId));

			if (existingItem != null)
			{
				// The game is already in the cart, so we prevent adding it again
				// Log this event or handle as needed, e.g., return a message indicating the item is already in the cart
				return false; // Indicate failure or that the operation is not allowed
			}

			// Since the game is not in the cart, proceed to add it
			Game game = await dbContext.Games.FirstAsync(g => g.Id == Guid.Parse(gameId));
			OrderGame newItem = new OrderGame
			{
				OrderId = cart.Id,
				GameId = Guid.Parse(gameId),
				PriceAtPurchase = game.Price,
				IsActive = true
			};

			dbContext.OrderGames.Add(newItem);
			await dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<CartViewModel> GetCartViewModelByUserIdAsync(string userId)
		{
			Order cart = await GetOrCreateCartForUserByUserIdAsync(userId);

			CartViewModel model = new CartViewModel()
			{
				Items = cart.OrderGames.Select(og => new CartItemViewModel
				{
					GameId = og.GameId,
					GameTitle = og.Game.Title,
					GameImagePath = og.Game.ImagePath,
					PriceAtPurchase = og.PriceAtPurchase
				}).ToList()
			};

			return model;
		}
	}
}
