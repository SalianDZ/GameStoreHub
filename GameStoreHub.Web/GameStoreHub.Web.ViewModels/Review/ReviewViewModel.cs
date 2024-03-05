namespace GameStoreHub.Web.ViewModels.Review
{
	public class ReviewViewModel
	{
		public string Username { get; set; } = null!;

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public string DateCreated { get; set; } = null!;
    }
}
