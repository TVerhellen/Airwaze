using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
