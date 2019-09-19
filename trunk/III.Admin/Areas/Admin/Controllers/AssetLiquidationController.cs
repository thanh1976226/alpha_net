



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
    public class AssetLiquidationController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;

        public AssetLiquidationController(EIMDBContext context, IUploadService upload)
        {
            _context = context;
            _upload = upload;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Index
        public class JTableModelAssetLiquidation : JTableModel
        {
            public string AssetCode { get; set; }
            public string AssetName { get; set; }
            public string Status { get; set; }
        }

        [HttpPost]
        public object JTableAssetLiquidation([FromBody]JTableModelAssetLiquidation jTablePara)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("draw", 1);
            dictionary.Add("recordsFiltered", 10);
            dictionary.Add("recordsTotal", 10);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<object> datas = new List<object>();
            data.Add("Id", "1");
            data.Add("Code", "R_001");
            data.Add("Title", "Mất tài sản");
            data.Add("Branch", "ACB Quang Trung");
            data.Add("Date", "02/10/2018");
            data.Add("Person", "Nguyễn Văn Hiếu");
            data.Add("Note", "Mất ô tô");
            data.Add("Status", "Mất");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "2");
            data.Add("Code", "R_002");
            data.Add("Title", "Mất tài sản");
            data.Add("Branch", "ACB Bình Dương");
            data.Add("Date", "02/12/2018");
            data.Add("Person", "Nguyễn Văn Đạt");
            data.Add("Note", "Mất Chứng Từ");
            data.Add("Status", "Mất");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "3");
            data.Add("Code", "R_003");
            data.Add("Title", "Hỏng tài sản");
            data.Add("Branch", "ACB Quang Trung");
            data.Add("Date", "02/02/2019");
            data.Add("Person", "Nguyễn Đình Kiên");
            data.Add("Note", "Hỏng máy tính");
            data.Add("Status", "Hỏng");
            datas.Add(data);

            dictionary.Add("data", datas);
            return Json(dictionary);
        }
    }
}

        //    [HttpPost]
        //    public object DeleteAsset(int id)
        //    {
        //        var msg = new JMessage() { Error = false };
        //        try
        //        {
        //            var data = _context.Assets.FirstOrDefault(x => x.AssetID == id);
        //            data.DeletedBy = ESEIM.AppContext.UserName;
        //            data.DeletedTime = DateTime.Now;
        //            data.IsDeleted = true;
        //            _context.Assets.Update(data);
        //            _context.SaveChanges();

        //            msg.Title = "Xóa thành công";
        //            return Json(msg);
        //        }
        //        catch (Exception ex)
        //        {
        //            msg.Error = true;
        //            msg.Title = "Có lỗi xảy ra khi xóa!";
        //            msg.Object = ex;
        //            return Json(msg);
        //        }
        //    }

        //    [HttpPost]
        //    public object GetAsset(int id)
        //    {
        //        var data = _context.Assets.FirstOrDefault(x => x.AssetID == id);
        //        return Json(data);
        //    }

        //    [HttpPost]
        //    public object InsertAsset([FromBody]Asset data)
        //    {
        //        var msg = new JMessage() { Error = false };
        //        try
        //        {
        //            if (_context.Assets.FirstOrDefault(x => x.AssetCode == data.AssetCode) == null)
        //            {
        //                data.CreatedBy = ESEIM.AppContext.UserName;
        //                data.CreatedTime = DateTime.Now;
        //                _context.Assets.Add(data);
        //                _context.SaveChanges();
        //                msg.Title = "Thêm tài sản thành công!";
        //            }
        //            else
        //            {
        //                msg.Error = true;
        //                msg.Title = "Mã tài sản đã tồn tại!";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            msg.Error = true;
        //            msg.Title = "Có lỗi xảy ra khi thêm mới tài sản!";
        //            msg.Object = ex;
        //        }
        //        return Json(msg);
        //    }

        //    [HttpPost]
        //    public object UpdateAsset([FromBody]Asset data)
        //    {
        //        var msg = new JMessage() { Error = false };
        //        try
        //        {
        //            data.UpdatedBy = ESEIM.AppContext.UserName;
        //            data.UpdatedTime = DateTime.Now;
        //            _context.Assets.Update(data);
        //            _context.SaveChanges();
        //            msg.Title = "Cập nhật thông tin thành công";
        //            return Json(msg);
        //        }
        //        catch (Exception ex)
        //        {
        //            msg.Error = true;
        //            msg.Title = "Có lỗi xảy ra khi cập nhật!";
        //            return Json(msg);
        //        }
        //    }

        //    [HttpPost]
        //    public object UploadImage(IFormFile fileUpload)
        //    {
        //        var msg = new JMessage();
        //        try
        //        {
        //            var upload = _upload.UploadImage(fileUpload);
        //            msg.Error = false;
        //            msg.Title = "";
        //            msg.Object = upload.Object;
        //            return Json(msg);
        //        }
        //        catch (Exception ex)
        //        {
        //            msg.Error = true;
        //            msg.Title = "Có lỗi xảy ra khi upload file!";
        //            msg.Object = ex;
        //            return Json(msg);
        //        }
        //    }

        //    [HttpPost]
        //    public object GetAllSupplier()
        //    {
        //        var data = _context.Suppliers.Select(x => new { x.SupCode, x.SupName }).AsNoTracking();
        //        return Json(data);
        //    }
        //    [HttpPost]
        //    public object GetAssetType()
        //    {
        //        var data = _context.CommonSettings.Where(x => x.Group == "ASSET_TYPE").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
        //        return data;
        //    }
        //    [HttpPost]
        //    public object GetAssetGroup()
        //    {
        //        var data = _context.CommonSettings.Where(x => x.Group == "ASSET_GROUP").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
        //        return data;
        //    }
        //    [HttpPost]
        //    public object GetCurrency()
        //    {
        //        return GetCurrencyBase();
        //    }
        //    [HttpPost]
        //    public object GetStatus()
        //    {
        //        var data = _context.CommonSettings.Where(x => x.Group == "STATUS").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
        //        return data;
        //    }
            #endregion


    
