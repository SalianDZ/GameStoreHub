﻿using GameStoreHub.Common;
using GameStoreHub.Web.ViewModels.Order;
using GameStoreHub.Web.ViewModels.OrderGame;

namespace GameStoreHub.Services.Data.Interfaces
{
	public interface IUserService
	{
		Task<string> GetFullNameByEmailAsync(string email);

		Task<decimal> GetUserBalanceByIdAsync(string userId);

		Task<OperationResult> DeductBalanceByUserIdAsync(string userId, decimal price);
	}
}
