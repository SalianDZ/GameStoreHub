using GameStoreHub.Web.ViewModels.Category;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface ICategoryService
	{
		Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
	}
}
