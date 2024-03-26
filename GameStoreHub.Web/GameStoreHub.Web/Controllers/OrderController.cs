using GameStoreHub.Common;
using GameStoreHub.Services.Data;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.Infrastructure.Extensions;
using GameStoreHub.Web.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Controllers
{
	public class OrderController : Controller
	{
		private readonly ICartService cartService;
		private readonly IUserService userService;
		private readonly IGameService gameService;

        public OrderController(ICartService cartService, IUserService userService, IGameService gameService)
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

			model.Items = await cartService.GetItemsForCheckoutByUserIdAsync(userId);

			ValidationResult validationResult = await cartService.ValidateCartByUserIdAsync(userId, model.Items);

			if (!validationResult.IsValid)
			{
				// Handle validation errors, e.g., by displaying them to the user
				foreach (var error in validationResult.Errors)
				{
					ModelState.AddModelError(string.Empty, error);
				}
				return View(model);
			}

			// Check if the user has enough balance in the wallet
			var userBalance = await userService.GetUserBalanceByIdAsync(userId);

			if (userBalance < model.TotalPrice)
			{
				ModelState.AddModelError("", "Insufficient balance in your wallet.");
				return View(model);
			}

			try
			{
				// Process the order
				var orderResult = await cartService.CreateOrderAsync(userId, model);
				if (!orderResult.Success)
				{
					foreach (var errorMessage in orderResult.Errors)
					{
						ModelState.AddModelError("", errorMessage);
					}

					return View(model);
				}

				// Deduct the total price from the user's wallet
				OperationResult result = await userService.DeductBalanceByUserIdAsync(userId, model.TotalPrice);

				// Generate and assign game activation codes for each purchased game
				// This can be part of the orderService.CreateOrderAsync logic or a separate step

				if (result.IsSuccess)
				{
					//TODO!!!!
					//await gameService.AssignActivationCodesToUserByUserIdAsync(userId, orderResult.OrderId);

					// Redirect to an order confirmation page or order details page

					//TODO!!!
					//return RedirectToAction("OrderConfirmation", new { orderId = orderResult.OrderId });
					return Ok();
				}
			}
			catch (Exception ex)
			{
				// Log the exception
				ModelState.AddModelError("", "An unexpected error occurred while processing your order. Please try again.");
			}

			return View(model);
		}

        [Authorize]
        public async Task<IActionResult> AddToCart(string id)
		{
			if (!await gameService.DoesGameExistByIdAsync(id))
			{
				return BadRequest("Select a valid game");
			}

			var userId = User.GetId();
			// Ensure you have a method to get the current user's ID
			var success = await cartService. AddItemToCart(userId, id);

			if (!success)
			{
				return BadRequest("Could not add the game to the cart, it might already be there.");
			}

			return RedirectToAction("Checkout", "Order");
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Cart()
		{
            string userId = User.GetId();
            CheckoutViewModel model = await cartService.GetCartViewModelByUserIdAsync(userId);
            return View(model);
        }
	}
}
