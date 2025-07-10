using Microsoft.AspNetCore.Mvc;

namespace OmenHotel.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
