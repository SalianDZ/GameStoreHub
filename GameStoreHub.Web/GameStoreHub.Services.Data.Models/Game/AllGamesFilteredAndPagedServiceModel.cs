using GameStoreHub.Web.ViewModels.Game;

namespace GameStoreHub.Services.Data.Models.Game
{
	public class AllGamesFilteredAndPagedServiceModel
	{
        public AllGamesFilteredAndPagedServiceModel()
        {
            Games = new HashSet<GamesViewModel>();     
        }
        
        public int TotalGamesCount { get; set; }

        public IEnumerable<GamesViewModel> Games { get; set; }
    }
}
