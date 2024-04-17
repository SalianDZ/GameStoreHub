using GameStoreHub.Common;
using GameStoreHub.Web.ViewModels.Order;
using GameStoreHub.Web.ViewModels.OrderGame;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface IUserService
	{
		Task<string> GetFullNameByEmailAsync(string email);

		Task<decimal> GetUserBalanceByIdAsync(string userId);

		Task<bool> DeductBalanceByUserIdAsync(string userId, decimal price);

		Task IncreaseUserBalance(string userId, decimal balance);
	}
}
