using GameStoreHub.Common;
using GameStoreHub.Web.ViewModels.Review;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface IReviewService
	{
		Task<IEnumerable<ReviewViewModel>> GetAllReviewsOfGameByIdAsync(string gameId);

		Task<OperationResult> AddReviewToGameByIdAsync(string gameId,string userId, ReviewFormModel model);
	}
}
