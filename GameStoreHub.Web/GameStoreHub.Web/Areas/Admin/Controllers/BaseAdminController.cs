using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GameStoreHub.Common.EntityValidationConstants.GeneralApplicationConstants;

namespace GameStoreHub.Web.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class BaseAdminController : Controller
    {

    }
}
