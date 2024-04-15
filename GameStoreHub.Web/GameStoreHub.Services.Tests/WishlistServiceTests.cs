using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data;
using GameStoreHub.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Services.Tests
{
	public class WishlistServiceTests
	{
		private GameStoreDbContext dbContext;
		private WishlistService wishlistService;

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

			var wishlists = new List<Wishlist>
			{
				new Wishlist()
				{
					UserId = user1.Id
				}
			};

			dbContext.Wishlists.AddRange(wishlists);

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

			wishlistService = new WishlistService(dbContext);
		}

		[TearDown]
		public void TearDown()
		{
			dbContext.Dispose(); // Clean up the context to avoid cross-test contamination
		}

		[Test]
		public async Task GetOrCreateWishlistForUserByUserIdAsync_ExistingWishlist_ReturnsWishlist()
		{
			var user = dbContext.Users.First();
			var existingWishlist = dbContext.Wishlists.First(w => w.UserId == user.Id);

			var wishlist = await wishlistService.GetOrCreateWishlistForUserByUserIdAsync(user.Id.ToString());

			Assert.IsNotNull(wishlist);
			Assert.AreEqual(existingWishlist.Id, wishlist.Id);
		}

		[Test]
		public async Task GetOrCreateWishlistForUserByUserIdAsync_NoExistingWishlist_CreatesNewWishlist()
		{
			var user = dbContext.Users.Last();
			bool result = await dbContext.Wishlists.AnyAsync(w => w.UserId == user.Id);
			Assert.IsFalse(result);

			var wishlist = await wishlistService.GetOrCreateWishlistForUserByUserIdAsync(user.Id.ToString());

			Assert.IsNotNull(wishlist);
			Assert.AreEqual(user.Id, wishlist.UserId);
			Assert.IsEmpty(wishlist.WishlistItems); // Ensure new wishlist is empty
		}

		[Test]
		public async Task AddItemToWishlist_AddsItemToExistingWishlist()
		{
			var user = dbContext.Users.First();
			var game = dbContext.Games.First();
			var wishlist = user.Wishlist;

			await wishlistService.AddItemToWishlist(user.Id.ToString(), game.Id.ToString());

			var items = dbContext.WishlistItems.Where(wi => wi.WishlistId == wishlist.Id);
			Assert.AreEqual(1, items.Count());
			Assert.AreEqual(game.Id, items.First().GameId);
		}

		[Test]
		public async Task AddItemToWishlist_AddsItemToNewWishlist()
		{
			var user = new ApplicationUser { Id = Guid.NewGuid() };
			var game = new Game { Id = Guid.NewGuid(), Title = "New Game" };
			dbContext.Users.Add(user);
			dbContext.Games.Add(game);
			dbContext.SaveChanges();

			await wishlistService.AddItemToWishlist(user.Id.ToString(), game.Id.ToString());

			var wishlist = dbContext.Wishlists.FirstOrDefault(w => w.UserId == user.Id);
			Assert.IsNotNull(wishlist);
			Assert.AreEqual(1, wishlist.WishlistItems.Count);
			Assert.AreEqual(game.Id, wishlist.WishlistItems.First().GameId);
		}

		[Test]
		public void AddItemToWishlist_WithNonexistentGameId_ThrowsException()
		{
			var user = dbContext.Users.First();
			var nonexistentGameId = Guid.NewGuid().ToString();

			// Expect an InvalidOperationException because FirstAsync throws when no elements are found
			Assert.ThrowsAsync<InvalidOperationException>(() => wishlistService.AddItemToWishlist(user.Id.ToString(), nonexistentGameId));
		}
	}
}
