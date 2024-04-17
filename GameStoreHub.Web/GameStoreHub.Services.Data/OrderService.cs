using GameStoreHub.Common;
using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Data.Models.Enums;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Order;
using GameStoreHub.Web.ViewModels.OrderGame;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Services.Data
{
    public class OrderService : IOrderService
	{
		private readonly GameStoreDbContext dbContext;

		private readonly IGameService gameService;

        public OrderService(GameStoreDbContext dbContext, IGameService gameService, IUserService userService)
        {
            this.dbContext = dbContext;
			this.gameService = gameService;
        }

		public async Task<Order> GetOrCreateCartForUserByUserIdAsync(string userId)
		{
				Order? cart =
				await dbContext.Orders
				.Include(o => o.OrderGames)
				.ThenInclude(o => o.Game)
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
						Address = "",
						City = "",
						Country = "",
						ZipCode = "",
						PhoneNumber= "",
						OrderGames = new HashSet<OrderGame>()
					};

					dbContext.Orders.Add(cart);
					await dbContext.SaveChangesAsync();

				}

			return cart;	
		}

		public async Task AddItemToCart(string userId, string gameId)
		{
			Order cart = await GetOrCreateCartForUserByUserIdAsync(userId);

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
		}

		public async Task RemoveItemFromCart(string userId, string gameId)
		{
			Order cart = await GetOrCreateCartForUserByUserIdAsync(userId);

			OrderGame existingItem = cart.OrderGames.First(og => og.GameId == Guid.Parse(gameId));

			dbContext.OrderGames.Remove(existingItem!);

			await dbContext.SaveChangesAsync();
		}

		public async Task<string> GetActivationCodeByUserAndGameIdAsync(string userId, string gameId)
		{
			Order order = await dbContext.Orders.FirstAsync(o => o.OrderGames.Any(og => og.GameId == Guid.Parse(gameId) && o.UserId == Guid.Parse(userId)));
			OrderGame game = order.OrderGames.First(og => og.GameId == Guid.Parse(gameId));
			return game.GameKey!;
		}

		public async Task<bool> IsGameAlreadyBoughtBefore(string userId, string gameId)
		{
			IEnumerable<Order> allCarts = await dbContext.Orders
				.Where(o => o.UserId == Guid.Parse(userId) && o.IsActive)
				.Include(o => o.OrderGames)
				.ThenInclude(og => og.Game)
				.ToArrayAsync();

			bool isGameAlreadyAdded = false;

			foreach (var currentCart in allCarts)
			{
				foreach (var game in currentCart.OrderGames.ToList())
				{
					if (game.GameId == Guid.Parse(gameId))
					{
						isGameAlreadyAdded = true;
					}
				}
			}

			return isGameAlreadyAdded;
		}

		public async Task<CheckoutViewModel> GetCartViewModelByUserIdAsync(string userId)
		{
			Order cart = await GetOrCreateCartForUserByUserIdAsync(userId);

			CheckoutViewModel model = new()
			{
				Items = cart.OrderGames
				.Select(og => new CheckoutItemViewModel
				{
					GameId = og.GameId,
					GameTitle = og.Game.Title,
					GameImagePath = og.Game.ImagePath,
					PriceAtPurchase = og.PriceAtPurchase
				})
				.ToList()
			};

			return model;
		}

		public async Task<IEnumerable<CheckoutItemViewModel>> GetItemsForCheckoutByUserIdAsync(string userId)
		{
			Order cart = await GetOrCreateCartForUserByUserIdAsync(userId);

			IEnumerable<CheckoutItemViewModel> items = cart.OrderGames.Select(og => new CheckoutItemViewModel
			{
				GameId = og.GameId,
				GameImagePath = og.Game.ImagePath,
				GameTitle = og.Game.Title,
				PriceAtPurchase = og.PriceAtPurchase
			}).ToHashSet();

			return items;
		}

		public async Task<IEnumerable<CheckoutItemViewModel>> GetPurchasedItemsByUserIdAsync(string userId)
		{
			IEnumerable<Order> allCarts = await dbContext.Orders
				.Where(o => o.OrderStatus == OrderStatus.Completed && o.UserId == Guid.Parse(userId))
				.Include(o => o.OrderGames)
				.ThenInclude(og => og.Game)
				.ToArrayAsync();

			List<CheckoutItemViewModel> allPurchasedGames = new();
			foreach (var currentCart in allCarts)
			{
				foreach (var game in currentCart.OrderGames.ToList())
				{
					CheckoutItemViewModel currentGame = new()
					{
						GameId = game.GameId,
						GameImagePath = game.Game.ImagePath,
						GameTitle = game.Game.Title,
						PriceAtPurchase = game.PriceAtPurchase
					};
					allPurchasedGames.Add(currentGame);
				}
			}

			return allPurchasedGames;
		}

		public async Task CreateOrderAsync(string userId, CheckoutViewModel model)
		{
			Order currrentOrder = await GetOrCreateCartForUserByUserIdAsync(userId);
			currrentOrder.TotalPrice = currrentOrder.OrderGames.Sum(og => og.PriceAtPurchase);
			currrentOrder.OrderStatus = OrderStatus.Completed;
			currrentOrder.OrderDate = DateTime.Now;
			currrentOrder.Address = model.BillingData.Address;
			currrentOrder.PhoneNumber = model.BillingData.PhoneNumber;
			currrentOrder.City = model.BillingData.City;
			currrentOrder.Country = model.BillingData.Country;
			currrentOrder.ZipCode = model.BillingData.ZipCode;
			currrentOrder.OrderNotes = model.BillingData.OrderNotes;
		}

		public async Task AssignActivationCodesToUserOrderByUserIdAsync(string userId)
		{
			List<Order> carts = await dbContext.Orders.Where(o => o.UserId == Guid.Parse(userId) && o.OrderStatus == OrderStatus.Completed).ToListAsync();

			foreach (var cart in carts)
			{
				foreach (var orderGame in cart.OrderGames)
				{
					if (orderGame.GameKey == null)
					{
						orderGame.GameKey = GenerateActivationKeyForGame();
					}
				}
			}

			await dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<CheckoutItemViewModel>> GetCartItemsByUserIdAsync(string userId)
		{
			Order order = await GetOrCreateCartForUserByUserIdAsync(userId);

			IEnumerable<CheckoutItemViewModel> items =
				order.OrderGames
				.Select(og => new CheckoutItemViewModel
				{
					GameId = og.GameId,
					GameImagePath = og.Game.ImagePath,
					GameTitle = og.Game.Title,
					PriceAtPurchase = og.PriceAtPurchase
				}).ToList();

			return items;
		}

        public async Task<bool> IsGameInCartByIdAsync(string userId, string gameId)
        {
			Order cart = await GetOrCreateCartForUserByUserIdAsync(userId);

			OrderGame? existingItem = cart.OrderGames.FirstOrDefault(og => og.GameId == Guid.Parse(gameId));

			if (existingItem != null)
			{
				return true;
			}

			return false;
		}

		private static string GenerateActivationKeyForGame()
		{
			return Guid.NewGuid().ToString();
		}
    }
}
