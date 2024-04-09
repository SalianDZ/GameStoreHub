namespace GameStoreHub.Web.ViewModels.Game
{
	public class TopSellingGameViewModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = null!;
		public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
        public int SalesCount { get; set; }
    }
}
