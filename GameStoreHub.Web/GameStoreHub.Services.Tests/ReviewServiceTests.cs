using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data;
using GameStoreHub.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Services.Tests
{
	[TestFixture]
	public class ReviewServiceTests
	{
		private GameStoreDbContext dbContext;
		private IReviewService reviewService;
		private IGameService gameService;

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

			gameService = new GameService(dbContext);
			reviewService = new ReviewService(dbContext, gameService);
		}

		[TearDown]
		public void TearDown()
		{
			dbContext.Dispose(); // Clean up the context to avoid cross-test contamination
		}

		[Test]
		public async Task AddReview_SuccessfullyAddsReviewToGame()
		{
			var gameId = dbContext.Games.First().Id.ToString();
			var userId = dbContext.Users.First().Id.ToString();

			Game game = await gameService.GetGameByIdAsync(gameId);
			Review review = new Review()
			{
				GameId = Guid.Parse(gameId),
				UserId = Guid.Parse(userId),
				Rating = 5,
				Comment = "Great game!",
				DateCreated = DateTime.UtcNow
			};

			await dbContext.Reviews.AddAsync(review);
			await dbContext.SaveChangesAsync();

			var savedReview = await dbContext.Reviews.FirstOrDefaultAsync(r => r.GameId == Guid.Parse(gameId) && r.UserId == Guid.Parse(userId));
			Assert.IsNotNull(savedReview);
			Assert.AreEqual(5, savedReview.Rating);
			Assert.AreEqual("Great game!", savedReview.Comment);
		}

		[Test]
		public async Task GetAllReviewsOfGameByIdAsync_ReturnsActiveReviews()
		{
			var game = dbContext.Games.First();
			var user = dbContext.Users.First();

			Review review = new Review()
			{
				GameId = game.Id,
				UserId = user.Id,
				Rating = 5,
				Comment = "Great game!",
				DateCreated = DateTime.UtcNow
			};

			await dbContext.Reviews.AddAsync(review);

			Review review2 = new Review()
			{
				GameId = game.Id,
				UserId = user.Id,
				Rating = 5,
				Comment = "Nice!",
				DateCreated = DateTime.UtcNow,
				IsActive = false
			};

			await dbContext.Reviews.AddAsync(review2);
			await dbContext.SaveChangesAsync();

			var reviews = await reviewService.GetAllReviewsOfGameByIdAsync(game.Id.ToString());

			Assert.AreEqual(1, reviews.Count());
			var firstReview = reviews.First();
			Assert.AreEqual("Gosho", firstReview.Username);
			Assert.AreEqual(5, firstReview.Rating);
			Assert.AreEqual("Great game!", firstReview.Comment);
			Assert.AreEqual(DateTime.UtcNow.Date, DateTime.Parse(firstReview.DateCreated));
		}

		[Test]
		public async Task GetAllReviewsOfGameByIdAsync_WithNoActiveReviews_ReturnsEmpty()
		{
			var game = dbContext.Games.First();
			var user = dbContext.Users.First();
			// Adding an inactive review
			Review review2 = new Review()
			{
				GameId = game.Id,
				UserId = user.Id,
				Rating = 5,
				Comment = "Great game!",
				DateCreated = DateTime.UtcNow,
				IsActive = false
			};

			await dbContext.Reviews.AddAsync(review2);
			await dbContext.SaveChangesAsync();

			var reviews = await reviewService.GetAllReviewsOfGameByIdAsync(game.Id.ToString());
			Assert.IsEmpty(reviews);
		}
	}
}
