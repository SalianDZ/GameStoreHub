using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Game;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }
            
        public IActionResult All()
        {
            return View();
        }

        public async Task<IActionResult> GamesByCategory(int categoryId)
        {
            IEnumerable<GamesViewModel> model = await gameService.GetAllGamesFromCategoryByCategoryIdAsync(categoryId);
            return View(model);
        }
    }
}
