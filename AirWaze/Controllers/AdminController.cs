using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
