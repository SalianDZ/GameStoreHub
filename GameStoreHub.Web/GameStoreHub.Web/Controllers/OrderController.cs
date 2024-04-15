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

				if (result.IsSuccess)
				{
					await cartService.AssignActivationCodesToUserOrderByUserIdAsync(userId);

					//TODO!!!
					//return RedirectToAction("OrderConfirmation", new { orderId = orderResult.OrderId });
					return Ok();
				}
			}
			catch (Exception)
			{
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

            OperationResult result = await cartService.AddItemToCart(userId, id);

            if (!result.IsSuccess)
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest(error);
                }
            }

            return RedirectToAction("Cart", "Order");
		}

		[Authorize]
		public async Task<IActionResult> RemoveFromCart(string id)
		{
            if (!await gameService.DoesGameExistByIdAsync(id))
            {
                return BadRequest("Select a valid game!");
            }

			if (!await cartService.IsGameInCartByIdAsync(User.GetId(), id))
			{
				return BadRequest("Selected game is already removed from the cart!");
			}

            string userId = User.GetId();
            OperationResult result = await cartService.RemoveItemFromCart(userId, id);

			if (!result.IsSuccess)
			{
				foreach (var error in result.Errors)
				{
					return BadRequest(error);
				}
			}

			return RedirectToAction("Cart", "Order");
        }

		[Authorize]
		public async Task<IActionResult> RemoveFromIndexCart(string id)
		{
			if (!await gameService.DoesGameExistByIdAsync(id))
			{
				return BadRequest("Select a valid game!");
			}

			if (!await cartService.IsGameInCartByIdAsync(User.GetId(), id))
			{
				return BadRequest("Selected game is already removed from the cart!");
			}

			string userId = User.GetId();
			OperationResult result = await cartService.RemoveItemFromCart(userId, id);

			if (!result.IsSuccess)
			{
				foreach (var error in result.Errors)
				{
					return BadRequest(error);
				}
			}

			return RedirectToAction("Index", "Home");
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
