using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.Infrastructure.Extensions;
using GameStoreHub.Web.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Controllers
{
	public class OrderController : Controller
	{
		private readonly ICartService cartService;

        public OrderController(ICartService cartService)
        {
			this.cartService = cartService;
        }

		[HttpGet]
        public async Task<IActionResult> Checkout()
		{
			string userId = User.GetId();
			CheckoutViewModel model = await cartService.GetCartViewModelByUserIdAsync(userId);
			return View(model);
		}
	}
}
