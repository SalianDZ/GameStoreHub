using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> All()
		{
			IEnumerable<CategoryViewModel> allCategories = await categoryService.GetAllCategoriesAsync();
			return View(allCategories);
		}
	}
}
