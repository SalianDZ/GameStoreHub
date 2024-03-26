using GameStoreHub.Web.ViewModels.Review;

namespace GameStoreHub.Web.ViewModels.Game
{
	public class GameDetailsAndReviewFormViewModel
	{
        public GameDetailsViewModel GameDetailsPage { get; set; } = null!;

        public ReviewFormModel? ReviewForm { get; set; }
    }
}
