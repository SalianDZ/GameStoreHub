using GameStoreHub.Data;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Review;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Services.Data
{
	public class ReviewService : IReviewService
	{
		private readonly GameStoreDbContext dbContext;

        public ReviewService(GameStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
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
