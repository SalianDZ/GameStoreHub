using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.Infrastructure.Extensions;
using GameStoreHub.Web.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace GameStoreHub.Web.Controllers
{
	public class CartController : Controller
	{
		private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
			this.cartService = cartService;
        }

        public async Task<IActionResult> Cart()
		{
			string userId = User.GetId();
			CartViewModel model = await cartService.GetCartViewModelByUserIdAsync(userId);
			return View(model);
		}
	}
}
