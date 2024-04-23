using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 
