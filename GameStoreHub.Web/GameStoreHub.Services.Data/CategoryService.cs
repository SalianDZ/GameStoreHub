using GameStoreHub.Data;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Services.Data
{
	public class CategoryService : ICategoryService
	{
		private readonly GameStoreDbContext dbContext;

        public CategoryService(GameStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
		{
			IEnumerable<CategoryViewModel> allCategories = await dbContext.Categories.Where(c => c.IsActive).Select(c => new CategoryViewModel
			{
				Id = c.Id,
				Name = c.Name,
				ImagePath = c.ImagePath
			}).ToArrayAsync();

			return allCategories;
		}
	}
}
