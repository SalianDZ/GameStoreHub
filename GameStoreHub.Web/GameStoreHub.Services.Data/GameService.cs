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
	}
}
