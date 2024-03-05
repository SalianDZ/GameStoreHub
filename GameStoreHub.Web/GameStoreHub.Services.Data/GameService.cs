using GameStoreHub.Data;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Game;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Services.Data
{
	public class GameService : IGameService
	{
		private readonly GameStoreDbContext dbContext;

        public GameService(GameStoreDbContext dbContext)
        {
			    this.dbContext = dbContext;
        }

		public async Task<bool> DoesGameExistByIdAsync(string id)
		{
			bool result = await dbContext.Games.AnyAsync(g => g.IsActive && g.Id == Guid.Parse(id));
			return result;
		}

		public async Task<IEnumerable<GamesViewModel>> GetAllGamesFromCategoryByCategoryIdAsync(int categoryId)
		{
			IEnumerable<GamesViewModel> allGamesFromCategory =
				await dbContext.Games
				.Include(g => g.Category)
				.Where(g => g.IsActive && g.CategoryId == categoryId)
				.Select(g => new GamesViewModel
				{
					Id = g.Id,
					Title = g.Title,
					Category = g.Category.Name,
					Price = g.Price.ToString(),
					ImagePath = g.ImagePath
				}).ToArrayAsync();

			return allGamesFromCategory;
		}

		public async Task<GameDetailsViewModel> GetGameForDetailsByIdAsync(string gameId)
		{
			GameDetailsViewModel model = await dbContext.Games.Where(g => g.IsActive && g.Id == Guid.Parse(gameId)).Select(g => new GameDetailsViewModel
			{ 
				Id= g.Id,
				Title = g.Title,
				Description = g.Description,
				Developer = g.Developer,
				Category = g.Category.Name,
				ImagePath = g.ImagePath,
				ReleaseDate = g.ReleaseDate.ToString("d"),
				CategoryId = g.CategoryId,
				Price = g.Price
			}).FirstAsync();

			return model;
		}

		public async Task<IEnumerable<GamesViewModel>> GetLatestFiveGamesAsync()
		{
			IEnumerable<GamesViewModel> model = 
				await dbContext.Games
				.Include(g => g.Category)
				.OrderByDescending(g => g.ReleaseDate)
				.Take(5)
				.Select(g => new GamesViewModel
			{
				Id = g.Id,
				Title = g.Title,
				Category = g.Category.Name,
				Price = g.Price.ToString(),
				ImagePath = g.ImagePath
			}).ToArrayAsync();

			return model;
		}
	}
}
