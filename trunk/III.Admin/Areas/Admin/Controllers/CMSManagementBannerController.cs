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
    public class CMSManagementBannerController : BaseController
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
        public CMSManagementBannerController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelAct : JTableModel
        {
            public string ActCode { get; set; }
            public string ActTitle { get; set; }
            public string ActType { get; set; }
            public string ActNote { get; set; }
        }

        #region combobox
        //[HttpPost]
        //public object GetActivityType()
        //{
        //    var data = _context.CommonSettings.Where(x => x.Group == EnumHelper<AssetEnum>.GetDisplayValue(AssetEnum.CMSManagementSlideType)).Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
       //     return data;
       // }


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
        public object JTable([FromBody]JTableModelAct jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            var query = (from a in _context.AssetAtivitys
                         join b in listCommon on a.ActType equals b.CodeSet into b1
                         where (!a.IsDeleted
                               && (string.IsNullOrEmpty(jTablePara.ActCode) || (a.ActCode.ToLower().Contains(jTablePara.ActCode.ToLower())))
                               && (string.IsNullOrEmpty(jTablePara.ActTitle) || (a.ActTitle.ToLower().Contains(jTablePara.ActTitle.ToLower())))
                                && (string.IsNullOrEmpty(jTablePara.ActType) || (a.ActType.Equals(jTablePara.ActType)))
                               && (string.IsNullOrEmpty(jTablePara.ActNote) || (a.ActNote.ToLower().Contains(jTablePara.ActNote.ToLower()))))
                         select new AssetAtivitysJtableModel
                         {
                             ActivityId = a.ActivityId,
                             ActCode = a.ActCode,
                             ActTitle = a.ActTitle,
                             ActType = a.ActType,
                             ActNote = a.ActNote,
                             ActMember = a.ActMember
                         }).AsParallel();
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "ActivityId", "ActCode", "ActTitle", "ActType", "ActNote", "ActMember");
            return Json(jdata);
        }

        [HttpPost]
        public object GetItem([FromBody] int id)
        {
            var data = _context.AssetAtivitys.FirstOrDefault(x => x.ActivityId == id);
            return data;
        }

        //[HttpPost]
        //public JsonResult Insert([FromBody]CMSManagementSlide data)
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
        //public object Update([FromBody]CMSManagementSlide data)
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

        [HttpPost]
        public object Delete(int id)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var data = _context.AssetAtivitys.FirstOrDefault(x => x.ActivityId == id);
                data.DeletedBy = ESEIM.AppContext.UserName;
                data.DeletedTime = DateTime.Now;
                data.IsDeleted = true;

                _context.AssetAtivitys.Update(data);
                _context.SaveChanges();
                msg.Title = "Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi xóa!";
                msg.Object = ex;
                return Json(msg);
            }
        }
        #endregion
    }
}