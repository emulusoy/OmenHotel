using Microsoft.AspNetCore.Mvc;

namespace OmenHotel.Controllers
{
    public class RoomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
