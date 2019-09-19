using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class IOTParameterConfigurationController : BaseController
    {
        public class IOTParameterConfigurationJtableModel
        {
            public int Id { get; set; }
            public string ObjType { get; set; }
            public string DeviceCode { get; set; }
            public decimal? CloseBelow { get; set; }
            public decimal? CloseOn { get; set; }
            public string Location { get; set; }
        }
        private readonly EIMDBContext _context;
        public IOTParameterConfigurationController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelPC : JTableModel
        {
            public string ObjType { get; set; }
            public string DeviceCode { get; set; }
            public decimal? CloseBelow { get; set; }
            public decimal?  CloseOn { get; set; }
            public string Location { get; set; }
        }

        #region combobox
        [HttpPost]
        public object GetUser()
        {
            var query = _context.Users.Where(x => x.Active == true && x.UserType != 10).Select(x => new { x.Id, x.GivenName }).AsNoTracking().ToList();
            return query;
        }
        #endregion

        #region action

        [HttpPost]
        public object JTable([FromBody]JTableModelPC jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.IotWarningSettings
                        where a.IsDeleted == false
                         && (string.IsNullOrEmpty(jTablePara.DeviceCode) || a.DeviceCode.ToLower().Contains(jTablePara.DeviceCode.ToLower()))
                        && ((jTablePara.CloseOn == null) || (jTablePara.CloseOn <= a.CloseOn))
                        && ((jTablePara.CloseBelow == null) || (jTablePara.CloseBelow <= a.CloseBelow))
                        
                        select new IOTParameterConfigurationJtableModel
                        {
                            Id = a.Id,
                            ObjType = a.ObjType,
                            DeviceCode = a.DeviceCode,
                            CloseBelow = a.CloseBelow,
                            CloseOn = a.CloseOn,
                            Location=a.Location
                        };
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "ObjType", "DeviceCode", "CloseBelow", "CloseOn");
            return Json(jdata);
        }

        [HttpPost]
        public object GetItem( int id)
        {
            var data = _context.IotWarningSettings.FirstOrDefault(x => x.Id == id);
            return data;
        }

        [HttpPost]
        public JsonResult Insert([FromBody]IotWarningSetting data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var checkExist = _context.IotWarningSettings.FirstOrDefault(x => x.ObjType.ToLower() == data.ObjType.ToLower());
                if (checkExist != null)
                {
                    msg.Error = true;
                    msg.Title ="Đã tồn tại cấu hình tham số!";
                }
                else
                {
                    data.CreatedBy = ESEIM.AppContext.UserName;
                    data.CreatedTime = DateTime.Now;
                    _context.IotWarningSettings.Add(data);
                    _context.SaveChanges();
                    msg.Title = "Thêm cấu hình tham số thành công";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi thêm?";
            }
            return Json(msg);
        }

        [HttpPost]
        public object Delete(int id)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var data = _context.IotWarningSettings.FirstOrDefault(x => x.Id == id);
                data.DeletedBy = ESEIM.AppContext.UserName;
                data.DeletedTime = DateTime.Now;
                data.IsDeleted = true;
                _context.IotWarningSettings.Update(data);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("FCRE_MSG_DELETE_DONE"));//"Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FCRE_MSG_DELETE_ERROR"));//"Có lỗi xảy ra khi xóa!";
                msg.Object = ex;
                return Json(msg);
            }
        }
        #endregion
    }
}