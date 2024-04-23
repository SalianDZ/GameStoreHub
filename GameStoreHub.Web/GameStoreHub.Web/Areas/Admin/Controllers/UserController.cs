using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Areas.Admin.Controllers
{
	public class UserController : BaseAdminController
	{
		private readonly IUserService userService;

        public UserController(IUserService userService)
        {
			this.userService = userService;   
        }

		[Route("User/All")]
        public async Task<IActionResult> All()
		{
			IEnumerable<UserViewModel> viewModel = await userService.AllAsync();
			return View(viewModel);
		}
	}
}
