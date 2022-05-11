using Microsoft.AspNetCore.Mvc;

namespace AirWaze.Controllers
{
    public class PlaneController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
