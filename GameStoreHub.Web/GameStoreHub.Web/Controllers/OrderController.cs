using GameStoreHub.Common;
using GameStoreHub.Services.Data;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.Infrastructure.Extensions;
using GameStoreHub.Web.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GameStoreHub.Web.Controllers
{
	public class OrderController : Controller
	{
		private readonly IOrderService cartService;
		private readonly IUserService userService;
		private readonly IGameService gameService;

        public OrderController(IOrderService cartService, IUserService userService, IGameService gameService)
        {
			this.cartService = cartService;
			this.userService = userService;
			this.gameService = gameService;
        }

		[HttpGet]
		[Authorize]
        public async Task<IActionResult> Checkout()
		{
			string userId = User.GetId();
			CheckoutViewModel model = await cartService.GetCartViewModelByUserIdAsync(userId);
			return View(model);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Checkout(CheckoutViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model); // Return the view with validation errors
			}

			string userId = User.GetId();


			// Check if the user has enough balance in the wallet
			try
			{
				var userBalance = await userService.GetUserBalanceByIdAsync(userId);

				if (userBalance < model.TotalPrice)
				{
					//Here tempdata can be added!
					ModelState.AddModelError("", "Insufficient balance in your wallet.");
					return View(model);
				}
			}
			catch (Exception)
			{
				return BadRequest("An unexpected error occurred while processing your order. Please try again.");
			}

			try
			{
				// Process the order
				await cartService.CreateOrderAsync(userId, model);

				// Deduct the total price from the user's wallet
				bool result = await userService.DeductBalanceByUserIdAsync(userId, model.TotalPrice);

				if (result)
				{
					await cartService.AssignActivationCodesToUserOrderByUserIdAsync(userId);

					//TODO!!!
					//return RedirectToAction("OrderConfirmation", new { orderId = orderResult.OrderId });
					return RedirectToAction("OwnedGames", "Game");
				}
				else
				{
					//We must add tempdata here!
					//There was a problem with the payment
					return RedirectToAction("Index", "Home");
				}
			}
			catch (Exception)
			{
				//We must add tempdata here
				//return RedirectToAction("Index", ""Home")
				// Log the exception
				ModelState.AddModelError("", "An unexpected error occurred while processing your order. Please try again.");
			}

			return View(model);
		}

        [Authorize]
        public async Task<IActionResult> AddToCart(string id)
		{
			string userId = User.GetId();
			if (!await gameService.DoesGameExistByIdAsync(id))
			{
				return BadRequest("Select a valid game");
			}

            if (await cartService.IsGameInCartByIdAsync(userId, id))
            {
                return BadRequest("Selected game is already in the cart!");
            }

			if (await cartService.IsGameAlreadyBoughtBefore(userId, id))
			{
				return BadRequest("You already have bought this game before!");
			}

			try
			{
				await cartService.AddItemToCart(userId, id);
			}
			catch (Exception)
			{
				return BadRequest("An error occured while adding item to the cart!");
			}

            return RedirectToAction("Cart", "Order");
		}

		[Authorize]
		public async Task<IActionResult> RemoveFromCart(string id)
		{
			string userId = User.GetId();

			if (!await gameService.DoesGameExistByIdAsync(id))
            {
                return BadRequest("Select a valid game!");
            }

			if (!await cartService.IsGameInCartByIdAsync(userId, id))
			{
				return BadRequest("Selected game is already removed from the cart!");
			}

			try
			{
				await cartService.RemoveItemFromCart(userId, id);
			}
			catch (Exception)
			{
				return BadRequest("An error occured while adding item to the cart!");
			}

			return RedirectToAction("Cart", "Order");
        }

		[Authorize]
		public async Task<IActionResult> RemoveFromIndexCart(string id)
		{
			string userId = User.GetId();
			if (!await gameService.DoesGameExistByIdAsync(id))
			{
				return BadRequest("Select a valid game!");
			}

			if (!await cartService.IsGameInCartByIdAsync(userId, id))
			{
				return BadRequest("Selected game is already removed from the cart!");
			}

			try
			{
				await cartService.RemoveItemFromCart(userId, id);
			}
			catch (Exception)
			{
				return BadRequest("An error occured while adding item to the cart!");
			}

			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Cart()
		{
			try
			{
				string userId = User.GetId();
				CheckoutViewModel model = await cartService.GetCartViewModelByUserIdAsync(userId);
				return View(model);
			}
			catch (Exception)
			{
				return BadRequest("An error occured while adding item to the cart!");
			}
        }
	}
}
