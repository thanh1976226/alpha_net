using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using III.Domain.Enums;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class CMSExtraFieldController : BaseController
    {
        public class CMSExtraFieldsJtableModel
        {
            public int id { get; set; }
            public string name { get; set; }
            public string value { get; set; }
            public string type { get; set; }
            public int? group { get; set; }
            public bool? published { get; set; }
            public int? ordering { get; set; }
        }
        private readonly EIMDBContext _context;
        public CMSExtraFieldController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelCMSExtraField : JTableModel
        {
            public string name { get; set; }
            public string value { get; set; }
            public string type { get; set; }
            public int? group { get; set; }
            public bool? published { get; set; }
            public int? ordering { get; set; }
        }

        #region combobox
        //[HttpPost]
        //public object GetActivityType()
        //{
        //    var data = _context.CommonSettings.Where(x => x.Group == EnumHelper<AssetEnum>.GetDisplayValue(AssetEnum.AssetActivityType)).Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
        //    return data;
        //}


        [HttpPost]
        public object GetUser()
        {
            var query = _context.Users.Where(x => x.Active == true && x.UserType != 10).Select(x => new { x.Id, x.GivenName }).AsNoTracking().ToList();
            return query;
        }

        [HttpPost]
        public object GetAsset()
        {
            var data = _context.Assets.Select(x => new { Code = x.AssetCode, Name = x.AssetName }).AsNoTracking().ToList();
            return data;
        }
        #endregion

        #region action

        [HttpPost]
        public object JTable([FromBody]JTableModelCMSExtraField jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.cms_extra_fields
                        select new CMSExtraFieldsJtableModel
                        {
                            id = a.id,
                            name = a.name,
                            value = a.value,
                            type = a.type,
                            published = a.published,
                            ordering = a.ordering,
                            //group1=a.@group,
                         };
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "id", "name", "value", "type", "published", "ordering");
            return Json(jdata);
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
        //        if (checkExist != null)
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
        #endregion
    }
}