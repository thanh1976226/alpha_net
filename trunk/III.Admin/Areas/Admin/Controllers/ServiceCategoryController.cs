using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceCategoryController : BaseController
    {
        //private readonly IHostingEnvironment _hostingEnvironment;
        private readonly EIMDBContext _context;
        //private readonly ILogger _logger;
        //private readonly IActionLogService _actionLog;
        //private readonly AppSettings _appSettings;


        public class JTableModelCustom : JTableModel
        {
            public string servicecode { get; set; }
            public string servicename { get; set; }
            public string unit { get; set; }
            public string servicegroup { get; set; }
        }
        public ServiceCategoryController(EIMDBContext context)
        {

            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public object JTable([FromBody]JTableModelCustom jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            var count = (from a in _context.ServiceCategorys
                         where (string.IsNullOrEmpty(jTablePara.servicecode) || a.ServiceCode.ToLower().Contains(jTablePara.servicecode.ToLower()))
                         && (string.IsNullOrEmpty(jTablePara.servicename) || a.ServiceName.ToLower().Contains(jTablePara.servicename.ToLower()))
                         && (string.IsNullOrEmpty(jTablePara.unit) || a.Unit == jTablePara.unit)
                         && (string.IsNullOrEmpty(jTablePara.servicegroup) || a.ServiceGroup == jTablePara.servicegroup)
                         select a).AsNoTracking().Count();
            var query = (from a in _context.ServiceCategorys
                         where (string.IsNullOrEmpty(jTablePara.servicecode) || a.ServiceCode.ToLower().Contains(jTablePara.servicecode.ToLower()))
                         && (string.IsNullOrEmpty(jTablePara.servicename) || a.ServiceName.ToLower().Contains(jTablePara.servicename.ToLower()))
                         && (string.IsNullOrEmpty(jTablePara.unit) || a.Unit == jTablePara.unit)
                         && (string.IsNullOrEmpty(jTablePara.servicegroup) || a.ServiceGroup == jTablePara.servicegroup)
                         select a).AsNoTracking().Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var data = query.Select(x => new
            {
                x.ServiceCatID,
                x.ServiceCode,
                x.ServiceName,
                Unit = listCommon.FirstOrDefault(y => y.CodeSet == x.Unit)?.ValueSet,
                ServiceGroup = listCommon.FirstOrDefault(y => y.CodeSet == x.ServiceGroup)?.ValueSet,
                x.Note,
            }).ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "ServiceCatID", "ServiceCode", "ServiceName", "Unit", "ServiceGroup", "Note");
            return Json(jdata);
        }
        [HttpPost]
        public JsonResult Insert([FromBody]ServiceCategory obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var checkExist = _context.ServiceCategorys.FirstOrDefault(x => x.ServiceCode == obj.ServiceCode);
                if (checkExist == null)
                {
                    obj.CreatedBy = ESEIM.AppContext.UserName;
                    obj.CreatedTime = DateTime.Now;
                    _context.ServiceCategorys.Add(obj);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("SVC_TITLE_SERVICE"));
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_EXITS"), CommonUtil.ResourceValue("SVC_CURE_LBL_CODE"));
                }
            }
            catch
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAILED"), CommonUtil.ResourceValue("SVC_TITLE_SERVICE"));
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Update([FromBody]ServiceCategory obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                obj.UpdatedBy = ESEIM.AppContext.UserName;
                obj.UpdatedTime = DateTime.Now;
                _context.ServiceCategorys.Update(obj);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("SVC_TITLE_SERVICE"));
            }
            catch
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_ADD"));
            }
            return Json(msg);
        }
        [HttpPost]
        public object Delete(int Id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.ServiceCategorys.FirstOrDefault(x => x.ServiceCatID == Id);
                _context.ServiceCategorys.Remove(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("SVC_TITLE_SERVICE"));
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_ADD"));
                return Json(msg);
            }
        }

        public object GetItem(int id)
        {

            //if (id == null || id < 0)
            //{
            //    return Json("");
            //}
            var a = _context.ServiceCategorys.AsNoTracking().Single(m => m.ServiceCatID == id);
            return Json(a);
        }
        public object GetItemDetail(int id)
        {
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            var query = from ad in _context.ServiceCategorys

                        join b in listCommon on ad.Unit equals b.CodeSet into b1
                        from b in b1.DefaultIfEmpty()
                        join c in listCommon on ad.ServiceGroup equals c.CodeSet into c1
                        from c in c1.DefaultIfEmpty()
                        where ad.ServiceCatID == id
                        select new
                        {
                            ServiceCode = ad.ServiceCode,
                            ServiceName = ad.ServiceName,
                            Note = ad.Note,
                            Unit = b != null ? b.ValueSet : "Không xác định",
                            ServiceGroup = c != null ? c.ValueSet : "Không xác định",
                        };
            return Json(query);
        }


        [HttpPost]
        public object GetServiceGroup()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "SERVICE_GROUP").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return data;
        }
        [HttpPost]
        public object GetServiceUnit()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "SERVICE_UNIT").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return data;
        }
    }
}