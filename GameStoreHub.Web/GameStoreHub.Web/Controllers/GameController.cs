using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.Infrastructure.Extensions;
using GameStoreHub.Web.ViewModels.Category;
using GameStoreHub.Web.ViewModels.Game;
using GameStoreHub.Web.ViewModels.OrderGame;
using GameStoreHub.Web.ViewModels.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Controllers
{
	public class GameController : Controller
    {
        private readonly IGameService gameService;
        private readonly IReviewService reviewService;
        private readonly IOrderService cartService;
		private readonly ICategoryService categoryService;

        public GameController(IGameService gameService, IReviewService reviewService, IOrderService cartService, ICategoryService categoryService)
        {
            this.gameService = gameService;
            this.reviewService = reviewService;
            this.cartService = cartService;
			this.categoryService = categoryService;
        }
            
        public async Task<IActionResult> All(decimal? minPrice, decimal? maxPrice)
        {
            IEnumerable<GamesViewModel> allGames = await gameService.GetAllGames();

			if (minPrice.HasValue && minPrice <= 0)
			{
				ModelState.AddModelError("minPrice", "Minimum price must be greater than or equal to 0.");
			}

			if (maxPrice.HasValue && maxPrice > 10000)
			{
				ModelState.AddModelError("maxPrice", "Maximum price must not exceed 10,000.");
			}

			if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
			{
				ModelState.AddModelError("minPrice", "Minimum price cannot be greater than maximum price.");
			}

			if (!ModelState.IsValid)
			{
				// Optionally, you can pass the form back with previously entered values
				return View(allGames);
			}

			if (minPrice.HasValue)
            {
				allGames = allGames.Where(g => Decimal.Parse(g.Price) >= minPrice);
			}

            if (maxPrice.HasValue)
            {
				allGames = allGames.Where(g => Decimal.Parse(g.Price) <= maxPrice);
			}

			return View(allGames);
        }

        public async Task<IActionResult> Search(string query)
        {
            try
            {
				if (string.IsNullOrWhiteSpace(query))
				{
					return RedirectToAction("Index", "Home");
				}

				IEnumerable<GamesViewModel> searchedGames = await gameService.GetSearchedGames(query);
				if (!searchedGames.Any())
				{
					return View("NoResultsFound");
				}
				return View(searchedGames);
			}
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> OwnedGames()
        {
            try
            {
				string userId = User.GetId();
				IEnumerable<CheckoutItemViewModel> purchasedItems = await cartService.GetPurchasedItemsByUserIdAsync(userId);
				return View(purchasedItems);
			}
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        public async Task<IActionResult> GetActivationCode(string id)
        {
            try
            {
				string userId = User.GetId();
				if (!await cartService.IsGameAlreadyBoughtBefore(userId, id))
				{
					return BadRequest("You do not have access to this game!");
				}
				string actCode = await cartService.GetActivationCodeByUserAndGameIdAsync(userId, id);
				return Ok(actCode);
			}
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> GamesByCategory(int id)
        {
            try
            {
				IEnumerable<GamesViewModel> model = await gameService.GetAllGamesFromCategoryByCategoryIdAsync(id);
				return View(model);
			}
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
				bool doesGameExist = await gameService.DoesGameExistByIdAsync(id);
				if (id == null || !doesGameExist)
				{
					return StatusCode(404);
				}

				GameDetailsViewModel model = await gameService.GetGameViewModelForDetailsByIdAsync(id);
				model.Reviews = await reviewService.GetAllReviewsOfGameByIdAsync(id);

                //This is for the partial view in the details
				IEnumerable<GamesViewModel> relatedGames = await gameService.GetRelatedGamesByCategoryIdAsync(model.CategoryId, model.Id.ToString());
                ViewBag.RelatedGames = relatedGames;

				GameDetailsAndReviewFormViewModel detailsPageModel = new();
				detailsPageModel.GameDetailsPage = model;
				return View(detailsPageModel);
			}
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(string id, ReviewFormModel model)
        {
            try
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
					await reviewService.AddReviewToGameByIdAsync(id, userId, model);
					return RedirectToAction("Details", "Game", id);
				}

				detailsModel.GameDetailsPage = await gameService.GetGameViewModelForDetailsByIdAsync(id);
				detailsModel.GameDetailsPage.Reviews = await reviewService.GetAllReviewsOfGameByIdAsync(id);
				return View(detailsModel);
			}
            catch (Exception)
            {
                return StatusCode(500);
            }
		}

		[Authorize]
		public async Task<IActionResult> Add()
		{
			string userId = User.GetId();

			try
			{
				IEnumerable<CategoryViewModel> categories
				= await categoryService.GetAllCategoriesAsync();

				GameFormViewModel house = new()
				{
					Categories = categories,
				};

				return View(house);
			}
			catch (Exception)
			{
				return BadRequest("Something happened while trying to add a game! Please try again later!");
			}

		}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(GameFormViewModel model)
        {
            string userId = User.GetId();

            if (model.CategoryId < 1 || model.CategoryId > 5)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Selected category doesn't exists");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.GetAllCategoriesAsync();

                return View(model);
            }

            try
            {
                await gameService.AddGameAsync(model);
                return RedirectToAction("All", "Game");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occured while adding your new house. Please try again later or contact administrator");
                model.Categories = await categoryService.GetAllCategoriesAsync();

                return View(model);
            }
        }
    }
}
