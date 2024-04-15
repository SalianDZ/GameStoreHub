using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data;
using GameStoreHub.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Services.Tests
{
	public class GameServiceTests
	{
		private GameStoreDbContext dbContext;
		private GameService gameService;

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

			dbContext.SaveChanges();

			gameService = new GameService(dbContext);
		}

		[TearDown]
		public void TearDown()
		{
			dbContext.Dispose(); // Clean up the context to avoid cross-test contamination
		}

		//[TestCase("invalid-guid")]
        [TestCase("12345")]
		[Test]
		public async Task DoesGameExistByIdAsync_WithInvalidGuid_ReturnsFalse(string id)
		{
			bool result = await gameService.DoesGameExistByIdAsync(id);
			Assert.IsFalse(result);
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
			Assert.AreEqual(1, games.Count()); // Category 2 has 1 active game
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

			Assert.AreEqual(1, games.Count()); // Only one other active game in category 1 should be returned
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
	}
}
