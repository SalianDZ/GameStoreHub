using GameStoreHub.Web.ViewModels.User;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface IUserService
	{
		Task<string> GetFullNameByEmailAsync(string email);

		Task<decimal> GetUserBalanceByIdAsync(string userId);

		Task<bool> DeductBalanceByUserIdAsync(string userId, decimal price);

		Task IncreaseUserBalance(string userId, decimal balance);

		Task<string> GetFullNameByIdAsync(string userId);

		Task<IEnumerable<UserViewModel>> AllAsync();
	}
}
