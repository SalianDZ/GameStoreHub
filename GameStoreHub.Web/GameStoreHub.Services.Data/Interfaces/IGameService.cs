using GameStoreHub.Data.Models;
using GameStoreHub.Web.ViewModels.Game;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface IGameService
	{
		Task<IEnumerable<GamesViewModel>> GetAllGamesFromCategoryByCategoryIdAsync(int categoryId);

		Task<IEnumerable<GamesViewModel>> GetLatestFiveGamesAsync();

		Task<bool> DoesGameExistByIdAsync(string id);

		Task<GameDetailsViewModel> GetGameViewModelForDetailsByIdAsync(string gameId);

		Task<Game> GetGameByIdAsync(string id);

		Task<List<TopSellingGameViewModel>> GetTopSellingGames(int count = 5);

		Task<IEnumerable<GamesViewModel>> GetSearchedGames(string query);
	}
}
