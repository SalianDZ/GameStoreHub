using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Services.Data.Interfaces;
using GameStoreHub.Web.ViewModels.User;
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

		public async Task IncreaseUserBalance(string userId, decimal balance)
		{
			ApplicationUser user = await dbContext.Users.FirstAsync(u => u.Id == Guid.Parse(userId));
			user.WalletBalance += balance;
			await dbContext.SaveChangesAsync();
		}

		public async Task<string> GetFullNameByIdAsync(string id)
		{
			ApplicationUser? user =
				await dbContext.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(id));

			if (user == null)
			{
				return string.Empty;
			}

			return $"{user.FirstName} {user.LastName}";
		}

		public async Task<IEnumerable<UserViewModel>> AllAsync()
		{
			IEnumerable<UserViewModel> allUsers =
				await dbContext.Users.Select(u => new UserViewModel
				{
					Id = u.Id.ToString(),
					FullName = u.FirstName + " " + u.LastName,
					Email = u.Email,
					Balance = u.WalletBalance.ToString("f2")
				}).ToArrayAsync();

			return allUsers;
		}
	}
}
