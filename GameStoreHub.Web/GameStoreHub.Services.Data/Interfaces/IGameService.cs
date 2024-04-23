using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data.Models.Game;
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

		Task<IEnumerable<GamesViewModel>> GetRelatedGamesByCategoryIdAsync(int categoryId, string gameId);

		Task AddGameAsync(GameFormViewModel model);

		Task<IEnumerable<GamesViewModel>> GetAllGames();

		Task<AllGamesFilteredAndPagedServiceModel> AllAsync(AllGamesQueryModel queryModel);

		Task<GameFormViewModel> GetGameForEditByIdAsync(string id);

		Task EditHouseByIdAsync(GameFormViewModel model, string id);

		Task<GamePreDeleteViewModel> GetGameForDeleteByIdAsync(string id);

		Task DeleteGameByIdAsync(string gameId);

    }
}
