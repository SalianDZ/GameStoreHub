using GameStoreHub.Common;
using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.Order;
using GameStoreHub.Web.ViewModels.OrderGame;
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

		public async Task<bool> DeductBalanceByUserIdAsync(string userId, decimal price)
		{
			try
			{
				ApplicationUser user = await dbContext.Users.FirstAsync(u => u.Id == Guid.Parse(userId));
				user.WalletBalance -= price;
				await dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
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
