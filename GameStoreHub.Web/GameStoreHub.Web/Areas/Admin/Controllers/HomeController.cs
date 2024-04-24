using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error(int statusCode)
        //{

        //    if (statusCode == 401 || statusCode == 403)
        //    {
        //        return View("Error403");
        //    }

        //    return View();
        //}
    }
} 
