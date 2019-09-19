using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class AssetCommonSettingController : BaseController
    {
        private readonly EIMDBContext _context;

        public AssetCommonSettingController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}