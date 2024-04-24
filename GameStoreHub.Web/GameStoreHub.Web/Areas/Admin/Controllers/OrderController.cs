using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Areas.Admin.Controllers
{
	public class OrderController : BaseAdminController
	{
		private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }


        public async Task<IActionResult> All()
		{
			IEnumerable<OrderViewModel> orders = await orderService.AllOrdersAsync();
			return View(orders);
		}
	}
}
