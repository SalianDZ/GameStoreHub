using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Services.Data.Models.Game;
using GameStoreHub.Web.Infrastructure.Extensions;
using GameStoreHub.Web.ViewModels.Category;
using GameStoreHub.Web.ViewModels.Game;
using GameStoreHub.Web.ViewModels.OrderGame;
using GameStoreHub.Web.ViewModels.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GameStoreHub.Common.NotificationMessagesConstants;

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

		[AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllGamesQueryModel queryModel)
        {
			try
			{
                AllGamesFilteredAndPagedServiceModel serviceModel =
				await gameService.AllAsync(queryModel);

                queryModel.Games = serviceModel.Games;
                queryModel.TotalGames = serviceModel.TotalGamesCount;
                queryModel.Categories = await categoryService.AllCategoryNamesAsync();

                return View(queryModel);
            }
			catch (Exception)
			{
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
				return RedirectToAction("Index", "Home");
            }
        }

		[AllowAnonymous]
        public async Task<IActionResult> Search(string query, decimal? minPrice, decimal? maxPrice)
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

				if (minPrice.HasValue && minPrice <= 0)
				{
					TempData[ErrorMessage] = "Minimum price must be greater than or equal to 0.";
					return RedirectToAction("All", "Game");
				}

				if (maxPrice.HasValue && maxPrice > 10000)
				{
					TempData[ErrorMessage] = "Maximum price must not exceed 10,000.";
					return RedirectToAction("All", "Game");
				}

				if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
				{
					TempData[ErrorMessage] = "Minimum price cannot be greater than maximum price.";
					return RedirectToAction("All", "Game");
				}

				if (minPrice.HasValue)
				{
					searchedGames = searchedGames.Where(g => Decimal.Parse(g.Price) >= minPrice);
				}

				if (maxPrice.HasValue)
				{
					searchedGames = searchedGames.Where(g => Decimal.Parse(g.Price) <= maxPrice);
				}

				return View(searchedGames);
			}
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
                return RedirectToAction("Index", "Home");
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
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
                return RedirectToAction("Index", "Home");
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
                    TempData[ErrorMessage] = "You do not have access to this game!";
                    return RedirectToAction("OwnedGames", "Game");
				}
				string actCode = await cartService.GetActivationCodeByUserAndGameIdAsync(userId, id);
				return Ok(actCode);
			}
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
                return RedirectToAction("Index", "Home");
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
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
                return RedirectToAction("Index", "Home");
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
                    TempData[ErrorMessage] = "Please select a valid game!";
                    return RedirectToAction("All", "Game");
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
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
                return RedirectToAction("Index", "Home");
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
                    TempData[ErrorMessage] = "This game does not exist!";
                    return RedirectToAction("All", "Game");
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
                    TempData[SuccessMessage] = "You have successfully added a review!";
                    return RedirectToAction("Details", "Game", id);
				}

				detailsModel.GameDetailsPage = await gameService.GetGameViewModelForDetailsByIdAsync(id);
				detailsModel.GameDetailsPage.Reviews = await reviewService.GetAllReviewsOfGameByIdAsync(id);
				return View(detailsModel);
			}
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
                return RedirectToAction("Index", "Home");
            }
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Add()
		{
			string userId = User.GetId();

			try
			{
				IEnumerable<CategoryViewModel> categories
				= await categoryService.GetAllCategoriesAsync();

				GameFormViewModel house = new()
				{
					Categories = categories
				};

				return View(house);
			}
			catch (Exception)
			{
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
                return RedirectToAction("Index", "Home");
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
                TempData[ErrorMessage] = "You have successfully added a game!";
                return RedirectToAction("All", "Game");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occured while adding your new house. Please try again later or contact administrator");
                model.Categories = await categoryService.GetAllCategoriesAsync();
                return View(model);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool doesGameExist = await gameService.DoesGameExistByIdAsync(id);
            if (id == null || !doesGameExist)
            {
                TempData[ErrorMessage] = "This game does not exist!";
                return RedirectToAction("All", "Game");
            }

            if (!User.isAdmin())
            {
                TempData[ErrorMessage] = "You do not have access to this page!";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                GameFormViewModel model = await gameService.GetGameForEditByIdAsync(id);
                model.Categories = await categoryService.GetAllCategoriesAsync();
                return View(model);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(GameFormViewModel model, string id)
        {
            bool doesGameExist = await gameService.DoesGameExistByIdAsync(id);
            if (id == null || !doesGameExist)
            {
                TempData[ErrorMessage] = "This game does not exist!";
                return RedirectToAction("All", "Game");
            }

            if (!User.isAdmin())
            {
                TempData[ErrorMessage] = "You do not have access to this page!";
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.GetAllCategoriesAsync();
                return View(model);
            }

            try
            {
                await gameService.EditHouseByIdAsync(model, id);
                TempData[SuccessMessage] = "You have successfully edited the game!";
                return RedirectToAction("All", "Game");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "An unexpected error occurred while processing your order. Please try again.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            bool doesGameExist = await gameService.DoesGameExistByIdAsync(id);
            if (id == null || !doesGameExist)
            {
                TempData[ErrorMessage] = "This game does not exist!";
                return RedirectToAction("All", "Game");
            }

            if (!User.isAdmin())
            {
                TempData[ErrorMessage] = "You do not have access to this page!";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                GamePreDeleteViewModel viewModel = await gameService.GetGameForDeleteByIdAsync(id);
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "An unexpected error occurred while processing your order. Please try again.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string id, GamePreDeleteViewModel model)
        {
            bool doesGameExist = await gameService.DoesGameExistByIdAsync(id);
            if (id == null || !doesGameExist)
            {
                TempData[ErrorMessage] = "This game does not exist!";
                return RedirectToAction("All", "Game");
            }

            if (!User.isAdmin())
            {
                TempData[ErrorMessage] = "You do not have access to this page!";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await gameService.DeleteGameByIdAsync(id);
                TempData["WarningMessage"] = "The house was succcessfully deleted!";
                return RedirectToAction("All", "Game");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "An unexpected error occurred while processing your order. Please try again.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
