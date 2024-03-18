using GameStoreHub.Common;
using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Services.Data
{
	public class UserService : IUserService
	{
		private readonly GameStoreDbContext dbContext;

        public UserService(GameStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<OperationResult> DeductBalanceByUserIdAsync(string userId, decimal price)
		{
			OperationResult result = new OperationResult();
			try
			{
				ApplicationUser user = await dbContext.Users.FirstAsync(u => u.Id == Guid.Parse(userId));
				user.WalletBalance -= price;
				await dbContext.SaveChangesAsync();

				result.SetSuccess("The deduction has been successfull");
				return result;
			}
			catch (Exception ex)
			{
				result.AddError(ex.Message);
				return result;
			}
		}

		public async Task<string> GetFullNameByEmailAsync(string email)
		{
			ApplicationUser? user =
				await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);


			if (user == null)
			{
				return string.Empty;
			}

			return $"{user.FirstName} {user.LastName}";
		}

		public async Task<decimal> GetUserBalanceByIdAsync(string userId)
		{
			ApplicationUser currentUser = await dbContext.Users.FirstAsync(u => u.Id == Guid.Parse(userId));
			decimal userBalance = currentUser.WalletBalance;
			return userBalance;
		}
	}
}
