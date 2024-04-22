using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.Infrastructure.Extensions;
using GameStoreHub.Web.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static GameStoreHub.Common.NotificationMessagesConstants;

namespace GameStoreHub.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        public UserController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
			if (User.Identity!.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}

			if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = new()
            { 
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            await userManager.SetUserNameAsync(user ,model.Email);
            await userManager.SetEmailAsync(user ,model.Email);
            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) 
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                } 

                return View(result);
            }
			await signInManager.SignInAsync(user, false);
			return RedirectToAction("Index", "Home");
		}

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
			if (User.Identity!.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}

			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            LoginFormModel model = new()
            {
                ReturnUrl = returnUrl
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
			if (User.Identity!.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}

			if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded) 
            {
				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
			}

            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize]
		public async Task<IActionResult> Logout()
        {
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

        [Authorize]
        [HttpGet]
        public IActionResult AddFunds()
        {
            return View();
        }

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddFunds(decimal amount)
		{
            string userId = User.GetId();
            if (amount <= 0 || amount > 10000)
            {
                TempData[ErrorMessage] = "Please insert a valid amount!";
                return RedirectToAction("Index", "Home");
			}

			try
            {
                await userService.IncreaseUserBalance(userId, amount);
                TempData[SuccessMessage] = "The transaction has been successfull!";
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
