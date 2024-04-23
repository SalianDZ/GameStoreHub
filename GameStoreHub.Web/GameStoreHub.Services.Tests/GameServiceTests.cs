using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Game;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Services.Tests
{
	public class GameServiceTests
	{
		private GameStoreDbContext dbContext;
		private IGameService gameService;

		[SetUp]
		public void SetUp()
		{
			var options = new DbContextOptionsBuilder<GameStoreDbContext>()
				.UseInMemoryDatabase(databaseName: "TestGameDatabase") // Ensure a unique name to avoid conflicts between tests
				.Options;

			dbContext = new GameStoreDbContext(options);

			var categories = new List<Category>
			{
				new Category { Id = 1, Name = "Action", ImagePath="Test/Path" },
				new Category { Id = 2, Name = "Adventure", ImagePath= "Test/Path"}
			};

			dbContext.Categories.AddRange(categories);

			// Populate the database with test data
			dbContext.Games.AddRange(
				new Game
				{ 
					Title = "TestTitle1",
					Description = "TestDescription1",
					Developer="TestDeveloper1",
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

				},
				new Game
				{
					Title = "TestTitle5",
					Description = "TestDescription5",
					Developer = "TestDeveloper5",
					ReleaseDate = DateTime.Now,
					CategoryId = 2,
					ImagePath = "TestImagePath5",
					IsActive = true,
					Price = 80

				},
				new Game
				{
					Title = "TestTitle6",
					Description = "TestDescription6",
					Developer = "TestDeveloper6",
					ReleaseDate = DateTime.Now,
					CategoryId = 2,
					ImagePath = "TestImagePath6",
					IsActive = true,
					Price = 70

				}
			);

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

            var order1 = new Order()
            {
                UserId = user1.Id,
                OrderDate = DateTime.UtcNow,
                Address = "TestAdress1",
                City = "TestCity1",
                Country = "TestCountry1",
                ZipCode = "TestZipCode1",
                PhoneNumber = "TestPhoneNumber1",
                IsActive = true
            };

            dbContext.Orders.Add(order1);

            var order2 = new Order()
            {
                UserId = user2.Id,
                OrderDate = DateTime.UtcNow,
                Address = "TestAdress2",
                City = "TestCity2",
                Country = "TestCountry2",
                ZipCode = "TestZipCode2",
                PhoneNumber = "TestPhoneNumber2",
                IsActive = true
            };

            dbContext.Orders.Add(order2);

            dbContext.SaveChanges();

			gameService = new GameService(dbContext);
		}

		[TearDown]
		public void TearDown()
		{
			dbContext.Dispose(); // Clean up the context to avoid cross-test contamination
		}


		[Test]
		public async Task DoesGameExistByIdAsync_WithValidAndActiveGuid_ReturnsTrue()
		{
			var validActiveGuid = dbContext.Games.First(g => g.IsActive).Id.ToString();

			bool result = await gameService.DoesGameExistByIdAsync(validActiveGuid);
			Assert.IsTrue(result);
		}

		[Test]
		public async Task DoesGameExistByIdAsync_WithValidButInactiveGuid_ReturnsFalse()
		{
			var validInactiveGuid = dbContext.Games.First(g => !g.IsActive).Id.ToString();

			bool result = await gameService.DoesGameExistByIdAsync(validInactiveGuid);
			Assert.IsFalse(result);
		}

		[Test]
		public async Task GetAllGamesFromCategoryByCategoryIdAsync_ReturnsOnlyActiveGames()
		{
			var games = await gameService.GetAllGamesFromCategoryByCategoryIdAsync(1);
			Assert.AreEqual(1, games.Count()); // Only 1 active game in category 1
			Assert.IsTrue(games.All(g => g.Title == "TestTitle1"));
		}


		[Test]
		public async Task GetAllGamesFromCategoryByCategoryIdAsync_WithNoActiveGames_ReturnsEmpty()
		{
			var games = await gameService.GetAllGamesFromCategoryByCategoryIdAsync(2);
			Assert.AreEqual(4, games.Count()); // Category 2 has 1 active game
		}

		[Test]
		public async Task GetAllGamesFromCategoryByCategoryIdAsync_WithNonExistentCategory_ReturnsEmpty()
		{
			var games = await gameService.GetAllGamesFromCategoryByCategoryIdAsync(3); // Non-existent category
			Assert.IsEmpty(games);
		}

		[Test]
		public async Task GetRelatedGamesByCategoryIdAsync_ExcludesSpecifiedGameAndReturnsActiveGames()
		{
			var excludedGame = dbContext.Games.First(g => g.CategoryId == 2);
			var games = await gameService.GetRelatedGamesByCategoryIdAsync(2, excludedGame.Id.ToString());

			Assert.AreEqual(3, games.Count()); // Only one other active game in category 1 should be returned
			Assert.IsFalse(games.Any(g => g.Id == excludedGame.Id)); // The specified game should be excluded
		}

		[Test]
		public async Task GetRelatedGamesByCategoryIdAsync_WithNoActiveGames_ReturnsEmpty()
		{
			// Assuming category 3 does not exist or has no active games
			var games = await gameService.GetRelatedGamesByCategoryIdAsync(3, Guid.NewGuid().ToString());
			Assert.IsEmpty(games);
		}

		[Test]
		public async Task GetGameByIdAsync_ReturnsCorrectGame()
		{
			var expectedGame = dbContext.Games.First();
			var game = await gameService.GetGameByIdAsync(expectedGame.Id.ToString());

			Assert.AreEqual(expectedGame.Id, game.Id);
			Assert.AreEqual(expectedGame.Title, game.Title);
		}

		[Test]
		public void GetGameByIdAsync_WithNonExistentId_ThrowsException()
		{
			var nonExistentId = Guid.NewGuid().ToString();

			// Expect an InvalidOperationException because FirstAsync throws when no elements are found
			Assert.ThrowsAsync<InvalidOperationException>(async () => await gameService.GetGameByIdAsync(nonExistentId));
		}

		[Test]
		public async Task GetGameViewModelForDetailsByIdAsync_ReturnsCorrectDetails()
		{
			var game = await dbContext.Games.FirstAsync();
			var result = await gameService.GetGameViewModelForDetailsByIdAsync(game.Id.ToString());

			Assert.AreEqual(game.Id, result.Id);
			Assert.AreEqual(game.Title, result.Title);
			Assert.AreEqual(game.Description, result.Description);
			Assert.AreEqual(game.Developer, result.Developer);
			Assert.AreEqual(game.Category.Name, result.Category);
			Assert.AreEqual(game.ImagePath, result.ImagePath);
			Assert.AreEqual(game.ReleaseDate.ToString("d"), result.ReleaseDate);
			Assert.AreEqual(game.Price, result.Price);
		}

		[Test]
		public void GetGameViewModelForDetailsByIdAsync_WithNonExistentId_ThrowsException()
		{
			var nonExistentId = Guid.NewGuid().ToString();
			Assert.ThrowsAsync<InvalidOperationException>(() => gameService.GetGameViewModelForDetailsByIdAsync(nonExistentId));
		}

		[Test]
		public async Task GetLatestFiveGamesAsync_ReturnsExactlyFiveGamesOrderedByReleaseDate()
		{
			var games = await gameService.GetLatestFiveGamesAsync();
			Assert.AreEqual(5, games.Count()); // Should always return exactly five games
			Assert.IsTrue(games.First().Title == "TestTitle6"); // Latest game based on ReleaseDate
			Assert.IsTrue(games.Last().Title == "TestTitle1"); // Fifth latest game
		}

		[Test]
		public async Task GetLatestFiveGamesAsync_WithLessThanFiveGames_ReturnsAllGames()
		{
			dbContext.Games.RemoveRange(dbContext.Games);
			dbContext.Games.Add(new Game {Title = "Game 7", CategoryId = 1, ReleaseDate = DateTime.Now, Price = 70M, ImagePath = "/images/game7.jpg", Description = "TestDescription", Developer = "TestDeveloper", IsActive = true});
			dbContext.SaveChanges();

			var games = await gameService.GetLatestFiveGamesAsync();
			Assert.AreEqual(1, games.Count()); // Should return only the available game
			Assert.IsTrue(games.First().Title == "Game 7");
		}

		[Test]
		public async Task GetSearchedGames_WithQueryInTitle_ReturnsMatchingGames()
		{
			var games = await gameService.GetSearchedGames("1");
			Assert.AreEqual(1, games.Count());
			Assert.IsTrue(games.Any(g => g.Title == "TestTitle1"));
		}

		[Test]
		public async Task GetSearchedGames_WithInactiveGame_DoesNotReturnInactiveGames()
		{
			var games = await gameService.GetSearchedGames("TestTitle2");
			Assert.IsEmpty(games); // Should not return inactive games
		}

		[Test]
		public async Task GetSearchedGames_WithNoMatchingQuery_ReturnsEmpty()
		{
			var games = await gameService.GetSearchedGames("Nonexistent");
			Assert.IsEmpty(games);
		}

        [Test]
        public async Task AddGameAsync_SuccessfullyAddsGame()
        {
            var model = new GameFormViewModel
            {
                Title = "New Game",
                Description = "A great game",
                Price = 59.99M,
                Developer = "Famous Dev",
                ReleaseDate = DateTime.UtcNow,
                CategoryId = 1,
                ImagePath = "/images/new-game.jpg"
            };

            await gameService.AddGameAsync(model);

            var game = await dbContext.Games.FirstOrDefaultAsync(g => g.Title == model.Title);
            Assert.IsNotNull(game);
            Assert.AreEqual(model.Title, game.Title);
            Assert.AreEqual(model.Description, game.Description);
            Assert.AreEqual(model.Price, game.Price);
            Assert.AreEqual(model.Developer, game.Developer);
            Assert.AreEqual(model.ReleaseDate, game.ReleaseDate);
            Assert.AreEqual(model.CategoryId, game.CategoryId);
            Assert.AreEqual(model.ImagePath, game.ImagePath);
            Assert.IsTrue(game.IsActive);
        }

        [Test]
        public async Task AddGameAsync_WhenTitleIsMissing_ThrowsException()
        {
            var model = new GameFormViewModel
            {
                // Title is omitted to simulate a missing required field
                Description = "A great game",
                Price = 59.99M,
                Developer = "Famous Dev",
                ReleaseDate = DateTime.UtcNow,
                CategoryId = 1,
                ImagePath = "/images/new-game.jpg"
            };

            // Expect an exception due to missing title
            Assert.ThrowsAsync<DbUpdateException>(() => gameService.AddGameAsync(model));
        }

        [Test]
        public async Task GetGameForEditByIdAsync_RetrievesActiveGame_ReturnsCorrectViewModel()
        {
            var game = dbContext.Games.First();
            var viewModel = await gameService.GetGameForEditByIdAsync(game.Id.ToString());

            Assert.AreEqual(game.Title, viewModel.Title);
            Assert.AreEqual(game.Description, viewModel.Description);
            Assert.AreEqual(game.Developer, viewModel.Developer);
            Assert.AreEqual(game.ImagePath, viewModel.ImagePath);
            Assert.AreEqual(game.Price, viewModel.Price);
            Assert.AreEqual(game.ReleaseDate, viewModel.ReleaseDate);
            Assert.AreEqual(game.CategoryId, viewModel.CategoryId);
        }

        [Test]
        public void GetGameForEditByIdAsync_GameNotFound_ThrowsException()
        {
            var nonExistentGameId = Guid.NewGuid().ToString();
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                gameService.GetGameForEditByIdAsync(nonExistentGameId));
        }

        [Test]
        public async Task EditGameByIdAsync_UpdatesGameCorrectly()
        {
            var game = await dbContext.Games.FirstAsync();
            var model = new GameFormViewModel
            {
                Title = "Updated Title",
                Description = "Updated Description",
                Developer = "Updated Developer",
                ImagePath = "/images/updated.jpg",
                Price = 25.00M,
                ReleaseDate = DateTime.UtcNow,
                CategoryId = game.CategoryId
            };

            await gameService.EditGameByIdAsync(model, game.Id.ToString());

            var updatedGame = await dbContext.Games.FirstAsync(g => g.Id == game.Id);
            Assert.AreEqual(model.Title, updatedGame.Title);
            Assert.AreEqual(model.Description, updatedGame.Description);
            Assert.AreEqual(model.Developer, updatedGame.Developer);
            Assert.AreEqual(model.ImagePath, updatedGame.ImagePath);
            Assert.AreEqual(model.Price, updatedGame.Price);
            Assert.AreEqual(model.ReleaseDate, updatedGame.ReleaseDate);
            Assert.AreEqual(model.CategoryId, updatedGame.CategoryId);
        }

        [Test]
        public void EditGameByIdAsync_GameNotFound_ThrowsException()
        {
            var model = new GameFormViewModel
            {
                Title = "New Title",
                Description = "New Description",
                Developer = "New Developer",
                ImagePath = "/images/new.jpg",
                Price = 30.00M,
                ReleaseDate = DateTime.UtcNow,
                CategoryId = 1
            };

            var nonExistentGameId = Guid.NewGuid().ToString();
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                gameService.EditGameByIdAsync(model, nonExistentGameId));
        }

        [Test]
        public async Task GetGameForDeleteByIdAsync_RetrievesActiveGame_ReturnsCorrectViewModel()
        {
            var game = await dbContext.Games.FirstAsync();
            var viewModel = await gameService.GetGameForDeleteByIdAsync(game.Id.ToString());

            Assert.AreEqual(game.Title, viewModel.Title);
            Assert.AreEqual(game.Description, viewModel.Description);
            Assert.AreEqual(game.ImagePath, viewModel.ImagePath);
        }

        [Test]
        public void GetGameForDeleteByIdAsync_GameNotFound_ThrowsException()
        {
            var nonExistentGameId = Guid.NewGuid().ToString();
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                gameService.GetGameForDeleteByIdAsync(nonExistentGameId));
        }

        [Test]
        public async Task DeleteGameByIdAsync_DeletesGameSuccessfully()
        {
            var game = await dbContext.Games.FirstAsync();
            await gameService.DeleteGameByIdAsync(game.Id.ToString());

            var deletedGame = await dbContext.Games.FirstOrDefaultAsync(g => g.Id == game.Id);
            Assert.IsNull(deletedGame);
        }

        [Test]
        public void DeleteGameByIdAsync_GameNotFound_ThrowsException()
        {
            var nonExistentGameId = Guid.NewGuid().ToString();
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                gameService.DeleteGameByIdAsync(nonExistentGameId));
        }

        [Test]
        public async Task GetTopSellingGames_ReturnsCorrectGamesAndOrder()
        {
			Game[] games = await dbContext.Games.Where(g => g.IsActive).ToArrayAsync();

			Order order1 = await dbContext.Orders.FirstAsync();
            Order order2 = await dbContext.Orders.LastAsync();

			order1.OrderGames.Add(new OrderGame()
			{ 
				GameId = games[0].Id,
				OrderId = order1.Id,
				PriceAtPurchase = games[0].Price
			});

            order1.OrderGames.Add(new OrderGame()
            {
                GameId = games[1].Id,
                OrderId = order1.Id,
                PriceAtPurchase = games[1].Price
            });

            order2.OrderGames.Add(new OrderGame()
            {
                GameId = games[0].Id,
                OrderId = order2.Id,
                PriceAtPurchase = games[2].Price
            });

            order1.OrderStatus = GameStoreHub.Data.Models.Enums.OrderStatus.Completed;
            order2.OrderStatus = GameStoreHub.Data.Models.Enums.OrderStatus.Completed;
            await dbContext.SaveChangesAsync();

            var topSellingGames = await gameService.GetTopSellingGames(5);

            Assert.AreEqual(2, topSellingGames.Count);
            Assert.IsTrue(topSellingGames[0].Title == "TestTitle1" && topSellingGames[1].Title == "TestTitle3");
            Assert.IsTrue(topSellingGames[0].SalesCount > topSellingGames[1].SalesCount);
        }

        [Test]
        public async Task GetTopSellingGames_NoSales_ReturnsEmptyList()
        {
            var topSellingGames = await gameService.GetTopSellingGames(5);
            Assert.IsEmpty(topSellingGames);
        }
    }
}
