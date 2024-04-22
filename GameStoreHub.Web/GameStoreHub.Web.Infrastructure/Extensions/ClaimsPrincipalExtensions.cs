using System.Security.Claims;
using static GameStoreHub.Common.EntityValidationConstants.GeneralApplicationConstants;

namespace GameStoreHub.Web.Infrastructure.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		public static string GetId(this ClaimsPrincipal user)
		{
			return user.FindFirstValue(ClaimTypes.NameIdentifier);
		}

		public static bool isAdmin(this ClaimsPrincipal user)
		{
			return user.IsInRole(AdminRoleName); 
		}
	}
}
