using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.AspNetCore.Mvc;
namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class MapSupplierController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        public MapSupplierController(EIMDBContext context, IUploadService upload)
        {
            _context = context;
            _upload = upload;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}