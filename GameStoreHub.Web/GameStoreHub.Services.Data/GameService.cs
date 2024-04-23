using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Game;
using Microsoft.EntityFrameworkCore;
using GameStoreHub.Data.Models.Enums;
using GameStoreHub.Services.Data.Models.Game;
using GameStoreHub.Web.ViewModels.Game.Enums;

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
			Guid idAsGuid;

			if (!Guid.TryParse(id, out idAsGuid))
			{
				return false;
			}

			bool result = await dbContext.Games.AnyAsync(g => g.IsActive && g.Id == idAsGuid);
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
					ImagePath = g.ImagePath,
					ReleaseDate = g.ReleaseDate
				}).ToArrayAsync();

			return allGamesFromCategory;
		}

		public async Task<IEnumerable<GamesViewModel>> GetRelatedGamesByCategoryIdAsync(int categoryId, string gameId)
		{
			IEnumerable<GamesViewModel> allGamesFromCategory =
				await dbContext.Games
				.Include(g => g.Category)
				.Where(g => g.IsActive && g.CategoryId == categoryId && g.Id != Guid.Parse(gameId))
				.Select(g => new GamesViewModel
				{
					Id = g.Id,
					Title = g.Title,
					Category = g.Category.Name,
					Price = g.Price.ToString(),
					ImagePath = g.ImagePath,
					ReleaseDate = g.ReleaseDate
				}).ToArrayAsync();

			return allGamesFromCategory;
		}


		public async Task<List<TopSellingGameViewModel>> GetTopSellingGames(int count = 5)
		{ 
			List<TopSellingGameViewModel> topSellingGames = await dbContext.OrderGames
				.Where(og => og.Order.OrderStatus == OrderStatus.Completed)
				.GroupBy(og => og.GameId)
				.Select(og => new TopSellingGameViewModel
				{
					Id = og.First().Game.Id,
					Title = og.First().Game.Title,
					ImagePath = og.First().Game.ImagePath,
					Price = og.First().Game.Price,
					// Use the count of OrderGame records as a proxy for sales volume
					SalesCount = og.Count() // This counts the occurrences of each game in OrderGames
				})
				.OrderByDescending(vm => vm.SalesCount)
				.Take(count)
				.ToListAsync();

			return topSellingGames;
		}

		public async Task<Game> GetGameByIdAsync(string id)
		{
			Game game = await dbContext.Games.FirstAsync(g => g.Id == Guid.Parse(id));
			return game;
		}

		public async Task<GameDetailsViewModel> GetGameViewModelForDetailsByIdAsync(string gameId)
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
				.Where(g => g.IsActive)
				.Include(g => g.Category)
				.OrderByDescending(g => g.ReleaseDate)
				.Take(5)
				.Select(g => new GamesViewModel
			{
				Id = g.Id,
				Title = g.Title,
				Category = g.Category.Name,
				Price = g.Price.ToString(),
				ImagePath = g.ImagePath,
				ReleaseDate = g.ReleaseDate
			}).ToArrayAsync();

			return model;
		}

		public async Task<IEnumerable<GamesViewModel>> GetSearchedGames(string query)
		{
			IEnumerable<GamesViewModel> games = await dbContext.Games
				.Include(g => g.Category)
				.Where(g => (g.Title.Contains(query) || g.Description.Contains(query)) && g.IsActive)
				.Select(g => new GamesViewModel
				{
					Id = g.Id,
					Price = g.Price.ToString(),
					ImagePath = g.ImagePath,
					Category = g.Category.Name,
					Title = g.Title,
					ReleaseDate= g.ReleaseDate
				}).ToListAsync();

			return games;
		}

        public async Task AddGameAsync(GameFormViewModel model)
        {
			Game game = new()
			{
				Title = model.Title,
				Description = model.Description,
				Price = model.Price,
				Developer = model.Developer,
				ReleaseDate = model.ReleaseDate,
				CategoryId = model.CategoryId,
				ImagePath = model.ImagePath,
				IsActive = true,
			};

			await dbContext.Games.AddAsync(game);
			await dbContext.SaveChangesAsync();
        }

		public async Task<AllGamesFilteredAndPagedServiceModel> AllAsync(AllGamesQueryModel queryModel)
		{
			IQueryable<Game> gamesQuery = dbContext.Games
				.Include(g => g.Category)
				.Where(g => g.IsActive).AsQueryable();

			if (!string.IsNullOrWhiteSpace(queryModel.Category))
			{
				gamesQuery = gamesQuery.Where(g => g.Category.Name == queryModel.Category);
			}

			gamesQuery = queryModel.GameSorting switch
			{ 
				GameSorting.Newest => gamesQuery.OrderBy(g => g.ReleaseDate),
				GameSorting.Oldest => gamesQuery.OrderByDescending(g => g.ReleaseDate),
				GameSorting.PriceAscending => gamesQuery.OrderBy(g => g.Price),
				GameSorting.PriceDescending => gamesQuery.OrderByDescending(g => g.Price),
				_ => gamesQuery
			};

			IEnumerable<GamesViewModel> allGames = await gamesQuery.Skip((queryModel.CurrentPage - 1) * queryModel.GamesPerPage)
				.Take(queryModel.GamesPerPage)
				.Select(g => new GamesViewModel()
				{
					Id = g.Id,
					Title = g.Title,
					ReleaseDate = g.ReleaseDate,
					ImagePath = g.ImagePath,
					Price = g.Price.ToString(),
					Category = g.Category.Name
				}).ToArrayAsync();

			int totalGames = gamesQuery.Count();

			return new AllGamesFilteredAndPagedServiceModel()
			{
				TotalGamesCount = totalGames,
				Games = allGames
			};
		}

        public async Task<GameFormViewModel> GetGameForEditByIdAsync(string id)
        {
			Game game = await dbContext.Games.Include(g => g.Category)
				.Where(g => g.IsActive && g.Id == Guid.Parse(id))
				.FirstAsync();

			return new GameFormViewModel()
			{ 
				Title = game.Title,
				Description = game.Description,
				Developer = game.Developer,
				ImagePath= game.ImagePath,
				Price = game.Price,
				ReleaseDate= game.ReleaseDate,
				CategoryId = game.CategoryId
			};
        }

        public async Task EditGameByIdAsync(GameFormViewModel model, string id)
        {
			Game game = await dbContext.Games.Where(g => g.IsActive).FirstAsync(g => g.Id == Guid.Parse(id));

			game.Title = model.Title;
			game.Description = model.Description;
			game.Developer = model.Developer;
			game.ImagePath = model.ImagePath;
			game.Price = model.Price;
			game.ReleaseDate = model.ReleaseDate;
			game.CategoryId = model.CategoryId;

			await dbContext.SaveChangesAsync();
        }


        public async Task<GamePreDeleteViewModel> GetGameForDeleteByIdAsync(string id)
        {
            Game game = await dbContext.Games.Where(g => g.IsActive).FirstAsync(g => g.Id == Guid.Parse(id));

			return new GamePreDeleteViewModel()
			{ 
				Title = game.Title,
				Description = game.Description,
				ImagePath = game.ImagePath
			};
        }

        public async Task DeleteGameByIdAsync(string gameId)
        {
            Game game = await dbContext.Games.Where(g => g.IsActive).FirstAsync(g => g.Id == Guid.Parse(gameId));
			dbContext.Games.Remove(game);
			await dbContext.SaveChangesAsync();
        }
    }
}
