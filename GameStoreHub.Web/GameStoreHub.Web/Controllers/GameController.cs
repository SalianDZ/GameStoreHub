using GameStoreHub.Common;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.Infrastructure.Extensions;
using GameStoreHub.Web.ViewModels.Game;
using GameStoreHub.Web.ViewModels.Review;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            bool doesGameExist = await gameService.DoesGameExistByIdAsync(id);
            if (id == null || !doesGameExist)
            {
                return NotFound();
            }

            GameDetailsViewModel model = await gameService.GetGameViewModelForDetailsByIdAsync(id);
            model.Reviews = await reviewService.GetAllReviewsOfGameByIdAsync(id);

            GameDetailsAndReviewFormViewModel detailsPageModel = new();
            detailsPageModel.GameDetailsPage = model;
            return View(detailsPageModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(string id, ReviewFormModel model)
        {
			bool doesGameExist = await gameService.DoesGameExistByIdAsync(id);
			if (id == null || !doesGameExist)
			{
				return NotFound();
			}

			GameDetailsAndReviewFormViewModel detailsModel = new();
			if (model != null)
            {
                if (model.Rating <= 0 || model.Rating >= 6)
                {
                    ModelState.AddModelError(nameof(model.Rating), "The rating must be between 1 and 5!");
                }

                if (model.Comment != null && model.Comment.Length > 100)
                {
					ModelState.AddModelError(nameof(model.Comment), "The comment must be between consist of maximum 100 characters!");
				}

                if (!ModelState.IsValid)
                {
                    detailsModel.ReviewForm = model;
                    detailsModel.GameDetailsPage = await gameService.GetGameViewModelForDetailsByIdAsync(id);
                    return View(detailsModel);
                }

                string userId = User.GetId();
                OperationResult databaseResult = await reviewService.AddReviewToGameByIdAsync(id, userId, model);

				if (databaseResult.IsSuccess)
                {
                    return RedirectToAction("Details", "Game", id);
                }
            }

			detailsModel.GameDetailsPage = await gameService.GetGameViewModelForDetailsByIdAsync(id);
            detailsModel.GameDetailsPage.Reviews = await reviewService.GetAllReviewsOfGameByIdAsync(id);
			return View(detailsModel);
		}
    }
}
