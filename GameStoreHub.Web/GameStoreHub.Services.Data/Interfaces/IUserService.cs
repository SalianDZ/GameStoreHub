using GameStoreHub.Common;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface IUserService
	{
		Task<string> GetFullNameByEmailAsync(string email);

		Task<decimal> GetUserBalanceByIdAsync(string userId);

		Task<OperationResult> DeductBalanceByUserIdAsync(string userId, decimal price);
	}
}
