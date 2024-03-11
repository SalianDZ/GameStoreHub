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
	}
}
