using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Game;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService gameService;
        private readonly IReviewService reviewService;

        public GameController(IGameService gameService, IReviewService reviewService)
        {
            this.gameService = gameService;
            this.reviewService = reviewService;
        }
            
        public IActionResult All()
        {
            return View();
        }

        public async Task<IActionResult> GamesByCategory(int id)
        {
            IEnumerable<GamesViewModel> model = await gameService.GetAllGamesFromCategoryByCategoryIdAsync(id);
            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            bool doesGameExist = await gameService.DoesGameExistByIdAsync(id);
            if (id == null || !doesGameExist)
            {
                return NotFound();
            }

            GameDetailsViewModel model = await gameService.GetGameViewModelForDetailsByIdAsync(id);
            model.Reviews = await reviewService.GetAllReviewsOfGameByIdAsync(id);

            return View(model);
        }
    }
}
