using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class AssetReceiptFailController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;

        public AssetReceiptFailController(EIMDBContext context, IUploadService upload)
        {
            _context = context;
            _upload = upload;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelAsset : JTableModel
        {
            public string AssetCode { get; set; }
            public string AssetName { get; set; }
            public string Status { get; set; }
        }

        [HttpPost]
        public object JTableAssetReceiptFail([FromBody]JTableModelAsset jTablePara)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("draw", 1);
            dictionary.Add("recordsFiltered", 8);
            dictionary.Add("recordsTotal", 8);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<object> datas = new List<object>();
            data.Add("Id", "1");
            data.Add("Code", "R_001");
            data.Add("Title", "Mất ô tô ngày 06/09/2019");
            data.Add("Branch", "Hà Nội");
            data.Add("Date", "02/10/2018");
            data.Add("Person", "Nguyễn Văn Hiếu");
            data.Add("Note", "Ô tô màu xanh, biển số 30E 777.79");
            data.Add("Status", "Mất");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "2");
            data.Add("Code", "R_002");
            data.Add("Title", "Mất xe máy");
            data.Add("Branch", "Hà Nội");
            data.Add("Date", "02/12/2018");
            data.Add("Person", "Nguyễn Văn Đạt");
            data.Add("Note", "Xe máy wave mang biển số 29K 999.79");
            data.Add("Status", "Mất");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "3");
            data.Add("Code", "R_003");
            data.Add("Title", "Mất điện thoại");
            data.Add("Branch", "Hà Nội");
            data.Add("Date", "02/02/2019");
            data.Add("Person", "Nguyễn Đình Kiên");
            data.Add("Note", "Điện thoại Iphone 6 màu vàng");
            data.Add("Status", "Hỏng");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "4");
            data.Add("Code", "R_004");
            data.Add("Title", "Hỏng máy chiếu");
            data.Add("Branch", "Hà Nội");
            data.Add("Date", "02/02/2019");
            data.Add("Person", "Nguyễn Đình Sáu");
            data.Add("Note", "Máy chập chờn không lên");
            data.Add("Status", "Hỏng");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "5");
            data.Add("Code", "R_004");
            data.Add("Title", "Hỏng điều hòa ngày 08/06/2019");
            data.Add("Branch", "Hà Nội");
            data.Add("Date", "08/06/2019");
            data.Add("Person", "Nguyễn Đình Bảy");
            data.Add("Note", "Điều hòa hỏng không lên");
            data.Add("Status", "Hỏng");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "6");
            data.Add("Code", "R_006");
            data.Add("Title", "Hỏng máy in ngày 20/06/2019");
            data.Add("Branch", "Hà Nội");
            data.Add("Date", "20/06/2019");
            data.Add("Person", "Nguyễn Đình Hiệp");
            data.Add("Note", "Máy in hỏng không lên ");
            data.Add("Status", "Hỏng");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "7");
            data.Add("Code", "R_007");
            data.Add("Title", "Mất chứng minh thư ngày 25/06/2019");
            data.Add("Branch", "Hà Nội");
            data.Add("Date", "25/06/2019");
            data.Add("Person", "Nguyễn Đình Hiệp");
            data.Add("Note", "Tôi bị mất chứng minh thư vào ngày 25/06/2019 ");
            data.Add("Status", "Mất");
            datas.Add(data);


            data = new Dictionary<string, string>();
            data.Add("Id", "8");
            data.Add("Code", "R_008");
            data.Add("Title", "Mất giấy tờ xe");
            data.Add("Branch", "Hà Nội");
            data.Add("Date", "25/06/2019");
            data.Add("Person", "Trần Văn Phát");
            data.Add("Note", "Tôi bị giấy tờ xe thư vào ngày 25/06/2019 ");
            data.Add("Status", "Mất");
            datas.Add(data);

            dictionary.Add("data", datas);
            return Json(dictionary);
        }
    }
}