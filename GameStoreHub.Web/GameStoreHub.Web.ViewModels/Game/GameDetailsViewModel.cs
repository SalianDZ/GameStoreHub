using GameStoreHub.Web.ViewModels.Review;

namespace GameStoreHub.Web.ViewModels.Game
{
	public class GameDetailsViewModel
	{

        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string Developer { get; set; } = null!;

        public string ImagePath { get; set; } = null!;

        public string ReleaseDate { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int CategoryId { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; } = new HashSet<ReviewViewModel>();
    }
}
