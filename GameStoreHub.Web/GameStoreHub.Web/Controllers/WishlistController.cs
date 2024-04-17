using GameStoreHub.Common;
using GameStoreHub.Services.Data;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

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
				return BadRequest("Select a valid game");
			}

			if (await wishlistService.IsGameInWishlistByIdAsync(User.GetId(), id))
			{
				return BadRequest("Selected game is already in the wishlist!");
			}

			try
			{
				string userId = User.GetId();
				await wishlistService.AddItemToWishlist(userId, id);
			}
			catch (Exception)
			{

				return BadRequest("Something happen while accessing the database! Please try again later.");
			}

			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		public async Task<IActionResult> RemoveFromWishlist(string id)
		{
			if (!await gameService.DoesGameExistByIdAsync(id))
			{
				return BadRequest("Select a valid game!");
			}

			if (!await wishlistService.IsGameInWishlistByIdAsync(User.GetId(), id))
			{
				return BadRequest("Selected game is not in the wishlist!");
			}

			try
			{
				string userId = User.GetId();
				await wishlistService.RemoveItemFromWishlist(userId, id);
			}
			catch (Exception)
			{

				return BadRequest("Something happen while accessing the database! Please try again later.");
			}

			return RedirectToAction("Index", "Home");
		}
	}
}
