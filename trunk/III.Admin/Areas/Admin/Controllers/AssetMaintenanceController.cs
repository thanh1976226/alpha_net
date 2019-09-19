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
    public class AssetMaintenanceController : BaseController
    {
        public class AssetAtivitysJtableModel
        {
            public int ActivityId { get; set; }
            public string ActCode { get; set; }
            public string ActTitle { get; set; }
            public string ActType { get; set; }
            public string ActNote { get; set; }
            public string ActMember { get; set; }
        }
        private readonly EIMDBContext _context;
        public AssetMaintenanceController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelMain : JTableModel
        {
            public string Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Branch { get; set; }
            public string UnitSCBD { get; set; }
            public string Content { get; set; }
           
        }

        [HttpPost]
        public object JTable([FromBody]JTableModelMain jTablePara)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("draw", 1);
            dictionary.Add("recordsFiltered", 10);
            dictionary.Add("recordsTotal", 10);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<object> datas = new List<object>();
            data.Add("Id", "1");
            data.Add("Code", "R_001");
            data.Add("Name", "P_001");
            data.Add("Branch", "ACB Quang Trung");
            data.Add("Date", "02/10/2018");
            data.Add("UnitSCBD", "Nguyễn Văn A");
            data.Add("Content", "Hỏng phanh");
            
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "2");
            data.Add("Code", "R_002");
            data.Add("Name", "P_002");
            data.Add("Branch", "ACB Nguyễn Trãi");
            data.Add("Date", "02/10/2018");
            data.Add("UnitSCBD", "Nguyễn Văn B");
            data.Add("Content", "Mất gương");

            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "3");
            data.Add("Code", "R_003");
            data.Add("Name", "P_003");
            data.Add("Branch", "ACB Thanh Xuất");
            data.Add("Date", "02/10/2018");
            data.Add("UnitSCBD", "Nguyễn Văn C");
            data.Add("Content", "Vỡ kính");
            datas.Add(data);

            dictionary.Add("data", datas);
            return Json(dictionary);
        }
        [HttpPost]
        public object JTableAsset([FromBody]JTableModelMain jTablePara)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("draw", 1);
            dictionary.Add("recordsFiltered", 10);
            dictionary.Add("recordsTotal", 10);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<object> datas = new List<object>();
            data.Add("Id", "1");
            data.Add("Code_Asset", "A_001");
            data.Add("Name_Asset", "B_001");
            data.Add("Branch_Asset", "ACB Quang Trung");
            data.Add("Date_Asset", "02/10/2018");
            data.Add("Unit_Asset", "Nguyễn Văn A");
            data.Add("Content_Asset", "Hỏng phanh");

            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "2");
            data.Add("Code_Asset", "A_002");
            data.Add("Name_Asset", "B_002");
            data.Add("Branch_Asset", "ACB Quang Trung");
            data.Add("Date_Asset", "02/10/2018");
            data.Add("Unit_Asset", "Nguyễn Văn B");
            data.Add("Content_Asset", "Mất gương");

            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "3");
            data.Add("Code_Asset", "A_003");
            data.Add("Name_Asset", "B_003");
            data.Add("Branch_Asset", "ACB Quang Trung");
            data.Add("Date_Asset", "02/10/2018");
            data.Add("Unit_Asset", "Nguyễn Văn Hiếu");
            data.Add("Content_Asset", "Vỡ kinh");
            datas.Add(data);

            dictionary.Add("data", datas);
            return Json(dictionary);
        }

        //[HttpPost]
        //public object GetItem([FromBody] int id)
        //{
        //    var data = _context.AssetAtivitys.FirstOrDefault(x => x.ActivityId == id);
        //    return data;
        //}

        //[HttpPost]
        //public JsonResult Insert([FromBody]AssetActivity data)
        //{
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {
        //        var checkExist = _context.AssetAtivitys.FirstOrDefault(x => x.ActCode.ToLower() == data.ActCode.ToLower());
        //        if (checkExist!=null)
        //        {
        //            msg.Error = true;
        //            msg.Title = "Đã tồn tại mã hoạt động!";
        //        }
        //        else
        //        {
        //            data.CreatedBy = ESEIM.AppContext.UserName;
        //            data.CreatedTime = DateTime.Now;
        //            _context.AssetAtivitys.Add(data);
        //            _context.SaveChanges();
        //            msg.Title = "Thêm hoạt động thành công";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra khi thêm?";
        //    }
        //    return Json(msg);
        //}

        //[HttpPost]
        //public object Update([FromBody]AssetActivity data)
        //{
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {
        //        data.UpdatedBy = ESEIM.AppContext.UserName;
        //        data.UpdatedTime = DateTime.Now;
        //        _context.AssetAtivitys.Update(data);
        //        _context.SaveChanges();
        //        msg.Title = "Cập nhật hoạt động thành công";
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra khi cập nhật!";
        //    }
        //    return Json(msg);
        //}

        //[HttpPost]
        //public object Delete(int id)
        //{
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {
        //        var data = _context.AssetAtivitys.FirstOrDefault(x => x.ActivityId == id);
        //        data.DeletedBy = ESEIM.AppContext.UserName;
        //        data.DeletedTime = DateTime.Now;
        //        data.IsDeleted = true;

        //        _context.AssetAtivitys.Update(data);
        //        _context.SaveChanges();
        //        msg.Title = "Xóa thành công";
        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra khi xóa!";
        //        msg.Object = ex;
        //        return Json(msg);
        //    }
        //}
        //#endregion
    }
}