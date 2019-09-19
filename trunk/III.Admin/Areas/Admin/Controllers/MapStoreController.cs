using Microsoft.AspNetCore.Mvc;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class MapStoreController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}