using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GameStoreHub.Common.NotificationMessagesConstants;

namespace GameStoreHub.Web.Controllers
{
    public class WishlistController : Controller
	{
		private readonly IWishlistService wishlistService;
		private readonly IUserService userService;
		private readonly IGameService gameService;

		public WishlistController(IWishlistService wishlistService, IUserService userService, IGameService gameService)
		{
			this.wishlistService = wishlistService;
			this.userService = userService;
			this.gameService = gameService;
		}

		[Authorize]
		public async Task<IActionResult> AddToWishlist(string id)
		{
			if (!await gameService.DoesGameExistByIdAsync(id))
			{
				TempData[ErrorMessage] = "Select a valid game!";
				return RedirectToAction("All", "Game");
			}

			if (await wishlistService.IsGameInWishlistByIdAsync(User.GetId(), id))
			{
                TempData[ErrorMessage] = "The game is already added to your wishlist!";
                return RedirectToAction("Index", "Home");
			}

			try
			{
				string userId = User.GetId();
				await wishlistService.AddItemToWishlist(userId, id);
                return RedirectToAction("Index", "Home");
            }
			catch (Exception)
			{

                TempData[ErrorMessage] = "An unexpected error occurred while processing your order. Please try again.";
                return RedirectToAction("Index", "Home");
            }
		}

		[Authorize]
		public async Task<IActionResult> RemoveFromWishlist(string id)
		{
			if (!await gameService.DoesGameExistByIdAsync(id))
			{
                TempData[ErrorMessage] = "Select a valid game!";
                return RedirectToAction("Index", "Home");
            }

			if (!await wishlistService.IsGameInWishlistByIdAsync(User.GetId(), id))
			{
                TempData[ErrorMessage] = "Selected game is not into your wishlist!";
                return RedirectToAction("Index", "Home");
            }

			try
			{
				string userId = User.GetId();
				await wishlistService.RemoveItemFromWishlist(userId, id);
                return RedirectToAction("Index", "Home");
            }
			catch (Exception)
			{
                TempData[ErrorMessage] = "An unexpected error occurred while processing your order. Please try again.";
                return RedirectToAction("Index", "Home");
			}
		}
	}
}
