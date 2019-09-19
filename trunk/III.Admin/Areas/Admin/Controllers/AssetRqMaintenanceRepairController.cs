using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using System.Collections.Generic;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class AssetRqMaintenanceRepairController : BaseController
    {
        private readonly EIMDBContext _context;
        public AssetRqMaintenanceRepairController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public object JTable([FromBody]JTableModel jTablePara)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("draw", 1);
            dictionary.Add("recordsFiltered", 10);
            dictionary.Add("recordsTotal", 10);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<object> datas = new List<object>();
            data.Add("Id", "1");
            data.Add("RqCode", "RQ_001");
            data.Add("RqName", "Yêu cầu bão dưỡng xe máy");
            data.Add("Branch", "Hà Nội");
            data.Add("RqDate", "02/10/2019");
            data.Add("AssetType", "Tài sản cá nhân");
            data.Add("Description", "Xe wave màu đỏ, mang biển số 29T8 - 7178");
            data.Add("Status", "Đang chờ");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "2");
            data.Add("RqCode", "RQ_002");
            data.Add("RqName", "Yêu cầu sửa chữa bảo dưỡng và đường ống");
            data.Add("Branch", "Hà Nội");
            data.Add("RqDate", "02/10/2019");
            data.Add("AssetType", "Tài sản công ty");
            data.Add("Description", "Các loại đường ống được yêu cầu sửa chữa 06/08/2019");
            data.Add("Status", "Đang chờ");

            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "3");
            data.Add("RqCode", "RQ_003");
            data.Add("RqName", "Yêu cầu sửa chữa các thùng, bể chứa");
            data.Add("Branch", "Hà Nội");
            data.Add("RqDate", "09/10/2019");
            data.Add("AssetType", "Tài sản công ty");
            data.Add("Description", "Các loại thùng được yêu cầu sửa chữa 09/08/2019");
            data.Add("Status", "Đang chờ");
            datas.Add(data);


            data = new Dictionary<string, string>();
            data.Add("Id", "4");
            data.Add("RqCode", "RQ_004");
            data.Add("RqName", "Yêu cầu sửa chữa và bảo dưỡng các máy phát chạy hơi nước và khí khác");
            data.Add("Branch", "Hà Đông");
            data.Add("RqDate", "15/10/2019");
            data.Add("AssetType", "Tài sản công ty");
            data.Add("Description", "Các loại máy móc được yêu cầu sửa chữa 15/08/2019");
            data.Add("Status", "Đang chờ");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "5");
            data.Add("RqCode", "RQ_005");
            data.Add("RqName", "Yêu cầu sửa chữa và bảo dưỡng nồi hơi điện hoặc dùng cho ngành hàng hải");
            data.Add("Branch", "Hưng Yên");
            data.Add("RqDate", "15/10/2019");
            data.Add("AssetType", "Tài sản công ty");
            data.Add("Description", "Các loại nồi hơi điện được yêu cầu sửa chữa 20/08/2019");
            data.Add("Status", "Đang chờ");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "6");
            data.Add("RqCode", "RQ_006");
            data.Add("RqName", "Yêu cầu sửa chữa và bảo dưỡng các xe chở hàng, các thiết bị bốc dỡ nguyên, vật liệu, v.v");
            data.Add("Branch", "Hà Nội");
            data.Add("RqDate", "14/10/2019");
            data.Add("AssetType", "Khác");
            data.Add("Description", "Các loại xe chở hàng được yêu cầu sửa chữa 13/08/2019");
            data.Add("Status", "Đang chờ");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "7");
            data.Add("RqCode", "RQ_007");
            data.Add("RqName", "Yêu cầu sửa chữa và bảo dưỡng vũ khí và quân nhu (bao gồm sửa chữa súng thể thao và giải trí)");
            data.Add("Branch", "Hà Nội");
            data.Add("RqDate", "20/10/2019");
            data.Add("AssetType", "Khác");
            data.Add("Description", "Các loại vũ khí quân sự được yêu cầu sửa chữa 13/08/2019");
            data.Add("Status", "Đã duyệt");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "8");
            data.Add("RqCode", "RQ_008");
            data.Add("RqName", "Yêu cầu sửa chữa các nồi hơi trung tâm và bộ tản nhiệt");
            data.Add("Branch", "Hà Nội");
            data.Add("RqDate", "15/10/2019");
            data.Add("AssetType", "Khác");
            data.Add("Description", "Các loại nồi hơi trung tâm được yêu cầu sửa chữa 13/08/2019");
            data.Add("Status", "Đang chờ");
            datas.Add(data);

            dictionary.Add("data", datas);
            return Json(dictionary);
        }
    }
}