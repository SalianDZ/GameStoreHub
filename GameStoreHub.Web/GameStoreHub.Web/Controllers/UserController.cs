using GameStoreHub.Data.Models;
using GameStoreHub.Web.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Controllers
{
	public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;
        public UserController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userStore = userStore;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
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
            IdentityResult result = await userManager.CreateAsync(user);

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
    }
}
