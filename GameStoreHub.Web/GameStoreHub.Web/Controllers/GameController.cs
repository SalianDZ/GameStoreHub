using Microsoft.AspNetCore.Mvc;

namespace GameStoreHub.Web.Controllers
{
    public class GameController : Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
