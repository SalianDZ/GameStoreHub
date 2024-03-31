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
    public class CartService : ICartService
	{
		private readonly GameStoreDbContext dbContext;

		private readonly IGameService gameService;
		private readonly IUserService userService;

        public CartService(GameStoreDbContext dbContext, IGameService gameService, IUserService userService)
        {
            this.dbContext = dbContext;
			this.gameService = gameService;
			this.userService = userService;
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

		public async Task<OperationResult> AddItemToCart(string userId, string gameId)
		{
			OperationResult result = new();
			try
			{
                Order cart = await GetOrCreateCartForUserByUserIdAsync(userId);

                // Check if the game is already in the cart
                OrderGame? existingItem = cart.OrderGames.FirstOrDefault(og => og.GameId == Guid.Parse(gameId));

				if (existingItem != null) 
				{
					result.AddError("The game is already added!");
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
				result.SetSuccess();
            }
			catch (Exception)
			{
				result.AddError("An error occured while attempting to procced with the data!");
			}

			return result;
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

		public async Task<OrderResult> CreateOrderAsync(string userId, CheckoutViewModel model)
		{
			try
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
				OrderResult orderResult = new(currrentOrder.Id);
				return orderResult;	
			}
			catch (Exception ex)
			{
				OrderResult orderResult = new(new List<string>()
				{ 
					ex.Message
				});
				return orderResult;
			}

		}

		//Validation method below this comment!
		//                |
		//                V

		public async Task<ValidationResult> ValidateCartByUserIdAsync(string userId, IEnumerable<CheckoutItemViewModel> cartItems)
		{
			var validationResult = new ValidationResult();

			// Validate each cart item
			foreach (var item in cartItems)
			{
				bool doesGameExist = await gameService.DoesGameExistByIdAsync(item.GameId.ToString());
				if (!doesGameExist)
				{
					validationResult.Errors.Add($"Game {item.GameId} is no longer available.");
					continue;
				}

				Game game = await gameService.GetGameByIdAsync(item.GameId.ToString());

				if (!game.IsActive)
				{
					validationResult.Errors.Add($"Game {item.GameId} is no longer available.");
					continue;
				}

				//if (item.PriceAtPurchase != game.Price)
				//{
				//	validationResult.Errors.Add($"Price for {game.Title} has changed.");
				//	// Optionally adjust the cart item price here or mark it for review
				//}
			}

			validationResult.IsValid = !validationResult.Errors.Any();
			return validationResult;
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

        public async Task<OperationResult> RemoveItemFromCart(string userId, string gameId)
        {
			OperationResult result = new();
			try
			{
                Order cart = await GetOrCreateCartForUserByUserIdAsync(userId);

                OrderGame existingItem = cart.OrderGames.First(og => og.GameId == Guid.Parse(gameId));

                dbContext.OrderGames.Remove(existingItem);

                await dbContext.SaveChangesAsync();
;
				result.SetSuccess();
            }
			catch (Exception)
			{
				result.AddError("An error occured while attempting to procced with the data!");
			}  
			
			return result;
        }
    }
}
