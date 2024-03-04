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
		private readonly IGameService gamerService;

		public HomeController(ILogger<HomeController> logger, IGameService gamerService)
		{
			_logger = logger;
			this.gamerService = gamerService;
		}

		public async Task<IActionResult> Index()
		{
			IEnumerable<GamesViewModel> latestGames = await gamerService.GetLatestFiveGamesAsync();
			return View(latestGames);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}