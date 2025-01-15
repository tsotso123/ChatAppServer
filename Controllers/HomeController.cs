using Microsoft.AspNetCore.Mvc;

namespace ChatAppServer.Controllers
{
    [Route("/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
