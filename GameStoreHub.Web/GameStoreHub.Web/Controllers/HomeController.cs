using GameStoreHub.Services.Data;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Game;
using GameStoreHub.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameStoreHub.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IGameService gameService;

		public HomeController(ILogger<HomeController> logger, IGameService gameService)
		{
			_logger = logger;
			this.gameService = gameService;
		}

		public async Task<IActionResult> Index()
		{
			var topSellingGames = await gameService.GetTopSellingGames(); // Fetch top-selling games
			ViewBag.TopSellingGames = topSellingGames;
			IEnumerable<GamesViewModel> latestGames = await gameService.GetLatestFiveGamesAsync();
			return View(latestGames);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}