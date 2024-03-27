using GameStoreHub.Common;
using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Review;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Services.Data
{
	public class ReviewService : IReviewService
	{
		private readonly GameStoreDbContext dbContext;

		private readonly IGameService gameService;

        public ReviewService(GameStoreDbContext dbContext, IGameService gameService)
        {
            this.dbContext = dbContext;
			this.gameService = gameService;
        }

		public async Task<OperationResult> AddReviewToGameByIdAsync(string gameId,string userId, ReviewFormModel model)
		{
			OperationResult result = new();
			try
			{
				Game game = await gameService.GetGameByIdAsync(gameId);
				Review review = new Review()
				{
					GameId = Guid.Parse(gameId),
					UserId = Guid.Parse(userId),
					Rating = model.Rating,
					Comment = model.Comment,
					DateCreated = DateTime.UtcNow
				};

				await dbContext.Reviews.AddAsync(review);
				await dbContext.SaveChangesAsync();
				result.SetSuccess();
				return result;
			}
			catch (Exception)
			{
				result.AddError("An error occured while attempting to procced with the data!");
				return result;
			}
		}

		public async Task<IEnumerable<ReviewViewModel>> GetAllReviewsOfGameByIdAsync(string gameId)
		{
			IEnumerable<ReviewViewModel> reviews =
				await dbContext.Reviews
				.Where(r => r.IsActive && r.GameId == Guid.Parse(gameId))
				.Select(r => new ReviewViewModel
				{
					Username = r.User.UserName,
					Rating = r.Rating,
					Comment = r.Comment,
					DateCreated = r.DateCreated.ToString("d"),
				}).ToArrayAsync();

			return reviews;
		}
	}
}
