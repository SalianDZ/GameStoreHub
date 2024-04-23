using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Data.Models.Enums;
using GameStoreHub.Services.Data;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Order;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Services.Tests
{
	[TestFixture]
	public class OrderServiceTests
	{
		private GameStoreDbContext dbContext;
		private IOrderService orderService;
		private IUserService userService;

		[SetUp]
		public void SetUp()
		{
			var options = new DbContextOptionsBuilder<GameStoreDbContext>()
				.UseInMemoryDatabase(databaseName: "TestGameDatabase") // Ensure a unique name to avoid conflicts between tests
				.Options;

			dbContext = new GameStoreDbContext(options);

			ApplicationUser user1 = new ApplicationUser()
			{
				UserName = "Gosho",
				NormalizedEmail = "GOSHO",
				Email = "gosho@abv.bg",
				EmailConfirmed = true,
				PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
				ConcurrencyStamp = "8b51706e-f6e8-4dae-b240-54f856fb3004",
				TwoFactorEnabled = false,
				FirstName = "Gosho",
				LastName = "Goshov"
			};
			dbContext.Users.Add(user1);

			ApplicationUser user2 = new ApplicationUser()
			{
				UserName = "Pesho",
				NormalizedEmail = "PESHO",
				Email = "pesho@agents.com",
				EmailConfirmed = true,
				PasswordHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
				ConcurrencyStamp = "caf271d7-0ba7-4ab1-8d8d-6d0e3711c27d",
				TwoFactorEnabled = false,
				FirstName = "Pesho",
				LastName = "Peshov"
			};
			dbContext.Users.Add(user2);

			// Populate the database with test data
			dbContext.Games.AddRange(
				new Game
				{
					Title = "TestTitle1",
					Description = "TestDescription1",
					Developer = "TestDeveloper1",
					ReleaseDate = DateTime.Now,
					CategoryId = 1,
					ImagePath = "TestImagePath1",
					IsActive = true,
					Price = 50

				},
				new Game
				{
					Title = "TestTitle2",
					Description = "TestDescription2",
					Developer = "TestDeveloper2",
					ReleaseDate = DateTime.Now,
					CategoryId = 1,
					ImagePath = "TestImagePath2",
					IsActive = false,
					Price = 60

				},
				new Game
				{
					Title = "TestTitle3",
					Description = "TestDescription3",
					Developer = "TestDeveloper3",
					ReleaseDate = DateTime.Now,
					CategoryId = 2,
					ImagePath = "TestImagePath3",
					IsActive = true,
					Price = 20

				},
				new Game
				{
					Title = "TestTitle4",
					Description = "TestDescription4",
					Developer = "TestDeveloper4",
					ReleaseDate = DateTime.Now,
					CategoryId = 2,
					ImagePath = "TestImagePath4",
					IsActive = true,
					Price = 25
				}
			);
			dbContext.SaveChanges();

			var order = new Order()
			{
				UserId = user1.Id,
				OrderDate = DateTime.UtcNow,
				Address = "TestAdress1",
				City = "TestCity1",
				Country = "TestCountry1",
				ZipCode = "TestZipCode",
				PhoneNumber = "TestPhoneNumber",
				IsActive = true
			};

			dbContext.Orders.Add(order);
			dbContext.SaveChanges();

			userService = new UserService(dbContext);
			orderService = new OrderService(dbContext);
		}

		[TearDown]
		public void TearDown()
		{
			dbContext.Dispose(); // Clean up the context to avoid cross-test contamination
		}

		[Test]
		public async Task IsGameInCartByIdAsync_WhenGameIsInCart_ReturnsTrue()
		{
			var user = dbContext.Users.First();
			var game = dbContext.Games.First();
			var order = dbContext.Orders.First();

			// Add the game to the cart
			await dbContext.OrderGames.AddAsync(new OrderGame { OrderId = order.Id, GameId = game.Id});
			await dbContext.SaveChangesAsync();

			bool isInCart = await orderService.IsGameInCartByIdAsync(user.Id.ToString(), game.Id.ToString());

			Assert.IsTrue(isInCart);
		}

		[Test]
		public async Task IsGameInCartByIdAsync_WhenGameIsNotInCart_ReturnsFalse()
		{
			var user = dbContext.Users.First();
			var game = dbContext.Games.Last();	

			bool isInCart = await orderService.IsGameInCartByIdAsync(user.Id.ToString(), game.Id.ToString());

			Assert.IsFalse(isInCart);
		}

		[Test]
		public async Task GetCartItemsByUserIdAsync_ReturnsCorrectCartItems()
		{
			var user = dbContext.Users.First();
			var game = dbContext.Games.First();
			var order = dbContext.Orders.First();

			order.OrderGames.Add(new OrderGame()
			{
				GameId = game.Id,
				Game = game,
				PriceAtPurchase = 0
			});
			await dbContext.SaveChangesAsync();

			var items = await orderService.GetCartItemsByUserIdAsync(user.Id.ToString());

			Assert.AreEqual(1, items.Count());
			var item = items.First();
			Assert.AreEqual(dbContext.Games.First().Id, item.GameId);
			Assert.AreEqual(dbContext.Games.First().ImagePath, item.GameImagePath);
			Assert.AreEqual(dbContext.Games.First().Title, item.GameTitle);
		}

		[Test]
		public async Task GetCartItemsByUserIdAsync_WithNoCart_ReturnsEmpty()
		{
			var newUser = dbContext.Users.Last();
			var items = await orderService.GetCartItemsByUserIdAsync(newUser.Id.ToString());
			Assert.IsEmpty(items);
		}

		[Test]
		public async Task AssignActivationCodesToUserOrderByUserIdAsync_AssignsKeysToAllCompletedOrders()
		{
			var user = await dbContext.Users.FirstAsync();
			var game1 = await dbContext.Games.FirstAsync();
			var game2 = await dbContext.Games.LastAsync();
			var order = await dbContext.Orders.FirstAsync();

			order.OrderGames.Add(new OrderGame() 
			{
				OrderId = order.Id,
				Game = game1
			});

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game2
			});

			order.OrderStatus = OrderStatus.Completed;
			await dbContext.SaveChangesAsync();

			await orderService.AssignActivationCodesToUserOrderByUserIdAsync(user.Id.ToString());

			var orderGames = await dbContext.OrderGames.ToListAsync();
			Assert.IsTrue(orderGames.TrueForAll(og => !string.IsNullOrEmpty(og.GameKey)), "All order games should have a game key assigned.");
		}

		[Test]
		public async Task AssignActivationCodesToUserOrderByUserIdAsync_NoCompletedOrders_NoKeysAssigned()
		{
			var user = await dbContext.Users.FirstAsync();
			var game1 = await dbContext.Games.FirstAsync();
			var game2 = await dbContext.Games.LastAsync();
			var order = await dbContext.Orders.FirstAsync();

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game1
			});

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game2
			});

			await orderService.AssignActivationCodesToUserOrderByUserIdAsync(user.Id.ToString());

			var orderGame = await dbContext.OrderGames.FirstOrDefaultAsync(og => og.Order.UserId == user.Id);
			Assert.IsNull(orderGame.GameKey, "No game key should be assigned as the order is not completed.");
		}

		[Test]
		public async Task CreateOrderAsync_FinalizesOrderCorrectly()
		{
			var user = dbContext.Users.First();
			var order = await dbContext.Orders.FirstAsync(o => o.UserId == user.Id);
			var game1 = await dbContext.Games.FirstAsync();
			var game2 = await dbContext.Games.LastAsync();

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game1,
				PriceAtPurchase = 30
			});

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game2,
				PriceAtPurchase = 40
			});

			CheckoutViewModel model = new()
			{
				BillingData = new CheckoutBillingFormViewModel() 
				{
					Address = "TestAdress1",
					City = "TestCity1",
					Country = "TestCountry1",
					PhoneNumber = "TestPhoneNumber1",
					ZipCode = "TestZipCode!",
				}
			};

			await orderService.CreateOrderAsync(user.Id.ToString(), model);

			Assert.IsNotNull(order);
			Assert.AreEqual(70, order.TotalPrice); // Total price should be sum of game prices
			Assert.AreEqual(OrderStatus.Completed, order.OrderStatus);
			Assert.AreEqual(model.BillingData.Address, order.Address);
			Assert.AreEqual(model.BillingData.PhoneNumber, order.PhoneNumber);
			Assert.AreEqual(model.BillingData.City, order.City);
			Assert.AreEqual(model.BillingData.Country, order.Country);
			Assert.AreEqual(model.BillingData.ZipCode, order.ZipCode);
			Assert.AreEqual(model.BillingData.OrderNotes, order.OrderNotes);
			Assert.AreEqual(DateTime.Now.Date, order.OrderDate.Date);
		}

		[Test]
		public async Task GetPurchasedItemsByUserIdAsync_ReturnsPurchasedItemsCorrectly()
		{
			var user = dbContext.Users.First();
			var order = await dbContext.Orders.FirstAsync(o => o.UserId == user.Id);
			var game1 = await dbContext.Games.FirstAsync();
			var game2 = await dbContext.Games.LastAsync();

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game1,
				PriceAtPurchase = 30
			});

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game2,
				PriceAtPurchase = 40
			});

			order.OrderStatus = OrderStatus.Completed;
			await dbContext.SaveChangesAsync();

			var items = await orderService.GetPurchasedItemsByUserIdAsync(user.Id.ToString());

			Assert.AreEqual(2, items.Count());
			var item = items.First();
			Assert.AreEqual("TestTitle1", item.GameTitle);
			Assert.AreEqual("TestImagePath1", item.GameImagePath);
		}

		[Test]
		public async Task GetPurchasedItemsByUserIdAsync_WithNoPurchases_ReturnsEmptyList()
		{
			var user = dbContext.Users.First();
			var items = await orderService.GetPurchasedItemsByUserIdAsync(user.Id.ToString());
			Assert.IsEmpty(items);
		}

		[Test]
		public async Task GetCartViewModelByUserIdAsync_ReturnsCartItems()
		{
			var user = dbContext.Users.First();
			var order = await dbContext.Orders.FirstAsync(o => o.UserId == user.Id);
			var game1 = await dbContext.Games.FirstAsync();
			var game2 = await dbContext.Games.LastAsync();

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game1,
				PriceAtPurchase = 30
			});

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game2,
				PriceAtPurchase = 40
			});

			await dbContext.SaveChangesAsync();

			var model = await orderService.GetCartViewModelByUserIdAsync(user.Id.ToString());

			Assert.AreEqual(2, model.Items.Count());
			var item = model.Items.First();
			Assert.AreEqual("TestTitle1", item.GameTitle);
			Assert.AreEqual("TestImagePath1", item.GameImagePath);
		}

		[Test]
		public async Task GetCartViewModelByUserIdAsync_WithEmptyCart_ReturnsEmptyViewModel()
		{
			var user = dbContext.Users.First();

			var model = await orderService.GetCartViewModelByUserIdAsync(user.Id.ToString());
			Assert.IsEmpty(model.Items);
		}

		[Test]
		public async Task IsGameAlreadyBoughtBefore_WhenGameIsPurchased_ReturnsTrue()
		{
			var user = await dbContext.Users.FirstAsync();
			var game = await dbContext.Games.FirstAsync();
			var order = await dbContext.Orders.FirstAsync();

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game,
				PriceAtPurchase = 30
			});

			order.OrderStatus = OrderStatus.Completed;
			await dbContext.SaveChangesAsync();	


			bool result = await orderService.IsGameAlreadyBoughtBefore(user.Id.ToString(), game.Id.ToString());
			Assert.IsTrue(result);
		}

		[Test]
		public async Task IsGameAlreadyBoughtBefore_WhenGameIsNotPurchased_ReturnsFalse()
		{
			var user = await dbContext.Users.FirstAsync();
			var game = await dbContext.Games.FirstAsync();
			var order = await dbContext.Orders.FirstAsync();


			bool result = await orderService.IsGameAlreadyBoughtBefore(user.Id.ToString(), game.Id.ToString());
			Assert.IsFalse(result);
		}

		[Test]
		public async Task GetActivationCodeByUserAndGameIdAsync_ReturnsCorrectGameKey()
		{
			var user = dbContext.Users.First();
			var order = await dbContext.Orders.FirstAsync(o => o.UserId == user.Id);
			var game = await dbContext.Games.FirstAsync();

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game,
				PriceAtPurchase = 30,
				GameKey = "Testkey"
			});

			order.OrderStatus = OrderStatus.Completed;
			await dbContext.SaveChangesAsync();

			var gameKey = await orderService.GetActivationCodeByUserAndGameIdAsync(user.Id.ToString(), game.Id.ToString());

			Assert.AreEqual("Testkey", gameKey);
		}

		[Test]
		public async Task GetActivationCodeByUserAndGameIdAsync_WhenGameNotPurchased_ThrowsException()
		{
			var game = await dbContext.Games.FirstAsync();
			var user = dbContext.Users.First();


			Assert.ThrowsAsync<InvalidOperationException>(() =>
				orderService.GetActivationCodeByUserAndGameIdAsync(user.Id.ToString(), game.Id.ToString()));
		}

		[Test]
		public void GetActivationCodeByUserAndGameIdAsync_WhenUserDoesNotExist_ThrowsException()
		{
			var nonExistentUserId = Guid.NewGuid().ToString();
			var game = dbContext.Games.First();

			Assert.ThrowsAsync<InvalidOperationException>(() =>
				orderService.GetActivationCodeByUserAndGameIdAsync(nonExistentUserId, game.Id.ToString()));
		}

		[Test]
		public async Task RemoveItemFromCart_RemovesItemSuccessfully()
		{
			var user = await dbContext.Users.FirstAsync();
			var game = await dbContext.Games.FirstAsync();
			var order = await dbContext.Orders.FirstAsync();

			order.OrderGames.Add(new OrderGame()
			{
				OrderId = order.Id,
				Game = game,
				PriceAtPurchase = 30
			});
			await dbContext.SaveChangesAsync();

			Assert.IsTrue(order.OrderGames.Any(og => og.GameId == game.Id));
			await orderService.RemoveItemFromCart(user.Id.ToString(), game.Id.ToString());
			Assert.IsFalse(order.OrderGames.Any(og => og.GameId == game.Id));
		}

		[Test]
		public async Task RemoveItemFromCart_WhenGameNotInCart_ThrowsException()
		{
			var user = await dbContext.Users.FirstAsync();
			var gameNotInCart = await dbContext.Games.LastAsync(); // This game was not added to the cart

			Assert.ThrowsAsync<InvalidOperationException>(() =>
				orderService.RemoveItemFromCart(user.Id.ToString(), gameNotInCart.Id.ToString()));
		}

		[Test]
		public async Task AddItemToCart_AddsItemSuccessfully()
		{
			var user = await dbContext.Users.FirstAsync();
			var game = await dbContext.Games.FirstAsync();

			await orderService.AddItemToCart(user.Id.ToString(), game.Id.ToString());

			var orderGame = await dbContext.OrderGames.FirstOrDefaultAsync(og => og.GameId == game.Id && og.Order.UserId == user.Id);

			Assert.IsNotNull(orderGame);
			Assert.AreEqual(game.Price, orderGame.PriceAtPurchase);
		}

		[Test]
		public async Task AddItemToCart_WhenGameDoesNotExist_ThrowsException()
		{
			var user = await dbContext.Users.FirstAsync();
			var nonExistentGameId = Guid.NewGuid().ToString();

			Assert.ThrowsAsync<InvalidOperationException>(() =>
				orderService.AddItemToCart(user.Id.ToString(), nonExistentGameId));
		}

		[Test]
		public async Task GetOrCreateCartForUserByUserIdAsync_ExistingCart_ReturnsCart()
		{
			var user = dbContext.Users.First();
			var cart = await orderService.GetOrCreateCartForUserByUserIdAsync(user.Id.ToString());

			Assert.IsNotNull(cart);
			Assert.AreEqual(OrderStatus.InCart, cart.OrderStatus);
		}

		[Test]
		public async Task GetOrCreateCartForUserByUserIdAsync_NoCart_CreatesNewCart()
		{
			var userWithoutACart = await dbContext.Users.LastAsync();

			var newCart = await orderService.GetOrCreateCartForUserByUserIdAsync(userWithoutACart.Id.ToString());

			Assert.IsNotNull(newCart);
			Assert.AreEqual(OrderStatus.InCart, newCart.OrderStatus);
			Assert.AreEqual(0, newCart.TotalPrice); // Checking that the new cart is initialized correctly
		}
	}
}
