using GameStoreHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameStoreHub.Data
{
	public class GameStoreDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{
		public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options)
			: base(options)
		{
		}
	}
}