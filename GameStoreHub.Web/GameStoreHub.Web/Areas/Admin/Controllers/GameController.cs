using GameStoreHub.Services.Data.Models.Game;
using GameStoreHub.Web.ViewModels.Game;
using Microsoft.AspNetCore.Mvc;
using GameStoreHub.Services.Data.Interfaces;
using static GameStoreHub.Common.NotificationMessagesConstants;
using GameStoreHub.Web.Infrastructure.Extensions;
using GameStoreHub.Web.ViewModels.Category;

namespace GameStoreHub.Web.Areas.Admin.Controllers
{
    public class GameController : BaseAdminController
	{
		private readonly IGameService gameService;
		private readonly ICategoryService categoryService;

        public GameController(IGameService gameService, ICategoryService categoryService)
        {
            this.gameService = gameService;
			this.categoryService = categoryService;
        }

        public async Task<IActionResult> All([FromQuery] AllGamesQueryModel queryModel)
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
				await gameService.EditGameByIdAsync(model, id);
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
