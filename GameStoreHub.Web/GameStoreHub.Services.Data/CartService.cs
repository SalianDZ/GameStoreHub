using GameStoreHub.Data;
using GameStoreHub.Data.Models;
using GameStoreHub.Data.Models.Enums;
using GameStoreHub.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GameStoreHub.Services.Data
{
	public class CartService : ICartService
	{
		private readonly GameStoreDbContext dbContext;

        public CartService(GameStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<Order?> GetOrCreateCartForUserByUserIdAsync(string userId)
		{
			try
			{
				Order? cart =
				await dbContext.Orders
				.Include(o => o.OrderGames)
				.FirstOrDefaultAsync(o => o.UserId == Guid.Parse(userId) && o.IsActive && o.OrderStatus == OrderStatus.InCart);

				if (cart == null)
				{
					cart = new Order
					{
						UserId = Guid.Parse(userId),
						IsActive = true,
						OrderDate = DateTime.UtcNow,
						OrderStatus = OrderStatus.InCart, // Or whatever logic you use to denote an active cart
						TotalPrice = 0
					};

					dbContext.Orders.Add(cart);
					await dbContext.SaveChangesAsync();
				}

				return cart;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
