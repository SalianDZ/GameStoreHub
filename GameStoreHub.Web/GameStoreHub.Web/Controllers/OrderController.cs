using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.Infrastructure.Extensions;
using GameStoreHub.Web.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GameStoreHub.Common.NotificationMessagesConstants;

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
			try
			{
                string userId = User.GetId();
                CheckoutViewModel model = await cartService.GetCartViewModelByUserIdAsync(userId);
                return View(model);
            }
			catch (Exception)
			{
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
                return RedirectToAction("Index", "Home");
            }
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
				model.Items = await cartService.GetCartItemsByUserIdAsync(userId);
				var userBalance = await userService.GetUserBalanceByIdAsync(userId);

				if (userBalance < model.TotalPrice)
				{
                    TempData[ErrorMessage] = "Insufficient balance in your wallet!";
                    return RedirectToAction("Checkout", "Order");
                }
			}
			catch (Exception)
			{
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
                return RedirectToAction("Index", "Home");
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
                    TempData[SuccessMessage] = "You have succesfully purchased this game!";
                    return RedirectToAction("OwnedGames", "Game");
				}
				else
				{
                    TempData[ErrorMessage] = "An unexpected error occurred while processing with your transaction! Please try again.";
                    return RedirectToAction("OwnedGames", "Game");
                }
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "An unexpected error occurred while processing your order. Please try again.";
				return RedirectToAction("Index", "Home");
			}
		}

        [Authorize]
        public async Task<IActionResult> AddToCart(string id)
		{
			string userId = User.GetId();
			if (!await gameService.DoesGameExistByIdAsync(id))
			{
				TempData[ErrorMessage] = "Select a valid game!";
				return RedirectToAction("Index", "Home");
			}

            if (await cartService.IsGameInCartByIdAsync(userId, id))
            {
                TempData[ErrorMessage] = "Selected game is already in the cart!";
                return RedirectToAction("Index", "Home");
            }

			if (await cartService.IsGameAlreadyBoughtBefore(userId, id))
			{
                TempData[ErrorMessage] = "You have already bought this game before!";
                return RedirectToAction("Index", "Home");
            }

			try
			{
				await cartService.AddItemToCart(userId, id);
                return RedirectToAction("Cart", "Order");
            }
			catch (Exception)
			{
                TempData[ErrorMessage] = "An unexpected error occurred while processing your order. Please try again.";
                return RedirectToAction("Index", "Home");
            }
		}

		[Authorize]
		public async Task<IActionResult> RemoveFromCart(string id)
		{
			string userId = User.GetId();

			if (!await gameService.DoesGameExistByIdAsync(id))
            {
                TempData[ErrorMessage] = "Select a valid game!";
                return RedirectToAction("Cart", "Order");
            }

			if (!await cartService.IsGameInCartByIdAsync(userId, id))
			{
                TempData[ErrorMessage] = "Selected game is not into your cart!";
                return RedirectToAction("Cart", "Order");
			}

			try
			{
				await cartService.RemoveItemFromCart(userId, id);
                return RedirectToAction("Cart", "Order");
            }
			catch (Exception)
			{
                TempData[ErrorMessage] = "An unexpected error occurred while processing your order. Please try again.";
                return RedirectToAction("Index", "Home");
            }

        }

		[Authorize]
		public async Task<IActionResult> RemoveFromIndexCart(string id)
		{
			string userId = User.GetId();
			if (!await gameService.DoesGameExistByIdAsync(id))
			{
                TempData[ErrorMessage] = "Select a valid game!";
                return RedirectToAction("Cart", "Order");
            }

			if (!await cartService.IsGameInCartByIdAsync(userId, id))
			{
                TempData[ErrorMessage] = "Selected game is not into your cart!";
                return RedirectToAction("Cart", "Order");
            }

			try
			{
				await cartService.RemoveItemFromCart(userId, id);
                return RedirectToAction("Index", "Home");
            }
			catch (Exception)
			{
                TempData[ErrorMessage] = "An unexpected error occurred while processing your order. Please try again.";
                return RedirectToAction("Index", "Home");
            }

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
                TempData[ErrorMessage] = "An unexpected error occurred while processing your order. Please try again.";
                return RedirectToAction("Index", "Home");
            }
        }
	}
}
