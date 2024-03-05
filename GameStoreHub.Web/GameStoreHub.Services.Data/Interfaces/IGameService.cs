using GameStoreHub.Web.ViewModels.Game;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface IGameService
	{
		Task<IEnumerable<GamesViewModel>> GetAllGamesFromCategoryByCategoryIdAsync(int categoryId);

		Task<IEnumerable<GamesViewModel>> GetLatestFiveGamesAsync();

		Task<bool> DoesGameExistByIdAsync(string id);

		Task<GameDetailsViewModel> GetGameForDetailsByIdAsync(string gameId);
	}
}
