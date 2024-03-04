using GameStoreHub.Web.ViewModels.Game;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface IGameService
	{
		Task<IEnumerable<GamesViewModel>> GetAllGamesFromCategoryByCategoryIdAsync(int categoryId);

		Task<IEnumerable<GamesViewModel>> GetLatestFiveGamesAsync();
	}
}
