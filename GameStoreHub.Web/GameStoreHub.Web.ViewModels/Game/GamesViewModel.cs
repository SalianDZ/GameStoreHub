namespace GameStoreHub.Web.ViewModels.Game
{
    public class GamesViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Price { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string ImagePath { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }
    }
}
