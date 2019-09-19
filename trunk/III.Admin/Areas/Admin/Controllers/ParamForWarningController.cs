using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;



namespace III.Admin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ParamForWarningController : Controller
    {
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        public ParamForWarningController(EIMDBContext context, IUploadService upload)
        {
            _context = context;
            _upload = upload;
        }
        public IActionResult Index()
        {
            return View();
        }
        public class ParamForWarningJtableModel
        {
            public int Id { get; set; }
            public decimal? Total { get; set; }
            public string Curency { get; set; }
            public string FromTime { get; set; }
            public string ToTime { get; set; }
            public string Acttype { get; set; }
            public string CatCode { get; set; }
        }
        public class JTableModelParamForWarning : JTableModel
        {

            public decimal? Total { get; set; }
            public string Curency { get; set; }
            public string FromTime { get; set; }
            public string ToTime { get; set; }




        }
        public class ChangeRate
        {
            public string Key { set; get; }
            public string Value { set; get; }
        }
        public class JTableModelAct : JTableModel
        {

            public int Id { get; set; }
            public string Currency { get; set; }
            public decimal Rate { get; set; }
        }
        [HttpPost]
        public object GetItem([FromBody] int Id)
        {
            var data = _context.ParamForWarnings.FirstOrDefault(x => x.id == Id);

            var obj = new ParamForWarningModel
            {
                id = data.id,
                AETType = data.aetType,
                Total = data.total.ToString(),
                CatCode = data.catCode,
                Currency = data.currency,
                FromTime = data.fromTime.HasValue ? data.fromTime.Value.ToString("dd/MM/yyyy") : "",
                ToTime = data.toTime.HasValue ? data.toTime.Value.ToString("dd/MM/yyyy") : "",


            };
            return obj;
        }

        [HttpPost]
        public object JTable([FromBody]JTableModelParamForWarning jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var fromTime = !string.IsNullOrEmpty(jTablePara.FromTime) ? DateTime.ParseExact(jTablePara.FromTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toTime = !string.IsNullOrEmpty(jTablePara.ToTime) ? DateTime.ParseExact(jTablePara.ToTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;

            var query = from a in _context.ParamForWarnings
                        join b in _context.FundCatReptExpss on a.catCode equals b.CatCode
                        where (string.IsNullOrEmpty(jTablePara.Curency) || a.currency.ToLower().Equals(jTablePara.Curency.ToLower()))
                        && ((jTablePara.Total == null) || (jTablePara.Total <= a.total))
                        && ((fromTime == null || (fromTime <= a.fromTime))) && (toTime == null || toTime <= a.toTime)
                        && a.isDeleted == false
                        select new
                        {
                            id = a.id,
                            aettype = a.aetType,
                            catCode = a.catCode,
                            total = a.total,
                            currency = a.currency,
                            fromTime = a.fromTime,
                            toTime = a.toTime,
                            catName = b.CatName

                        };

            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "id", "aettype", "catCode", "total", "currency", "fromTime", "toTime", "catName");
            return Json(jdata);
        }
        [HttpPost]
        
        public object GetCurrency()
        {
            var data = _context.FundCurrencys.Select(x => new { Currency = x.CurrencyCode}).AsNoTracking().ToList();
            return data;



        }
        [HttpPost]
        public object GetCatCode()
        {
            var data = _context.FundCatReptExpss.Where(x => x.IsDeleted == false).Select(x => new { CatCode = x.CatCode, CatName = x.CatName }).AsNoTracking().ToList();
            return data;
        }

        [HttpPost]
        public object Insert([FromBody]ParamForWarningModel data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var fromDate = !string.IsNullOrEmpty(data.FromTime) ? DateTime.ParseExact(data.FromTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                var toDate = !string.IsNullOrEmpty(data.ToTime) ? DateTime.ParseExact(data.ToTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                var model = _context.ParamForWarnings.FirstOrDefault(x => x.aetType.ToLower() == data.AETType.ToLower() && x.catCode.ToLower() == data.CatCode.ToLower() && ((fromDate >= x.fromTime && fromDate < x.toTime) || (toDate <= x.toTime && toDate > x.fromTime)) && x.isDeleted == false);
                if (model == null)
                {

                    var obj = new ParamForWarning
                    {
                        aetType = data.AETType,
                        createdTime = DateTime.Now,
                        catCode = data.CatCode,
                        currency = data.Currency,
                        total = decimal.Parse(data.Total),
                        createdBy = ESEIM.AppContext.UserName,
                        fromTime = fromDate,
                        toTime = toDate


                    };

                    //data.CreatedBy = ESEIM.AppContext.UserName;
                    //data.CreatedTime = DateTime.Now;

                    _context.ParamForWarnings.Add(obj);
                    _context.SaveChanges();

                    msg.Title = String.Format(CommonUtil.ResourceValue("PFW_MSG_ADD_WARNING_SUCCESS"));// thêm canh bao thành công
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("PFW_MSG_WARNING_EXITS"));//"Cảnh báo đã tồn tại";
                }

                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("PFW_MSG_ADD_WARNING_FAIL")); //"Có lỗi xảy ra khi thêm!";
                msg.Object = ex;
                return Json(msg);
            }
        }
        [HttpPost]
        public object Update([FromBody]ParamForWarningModel obj)
        {
            var msg = new JMessage() { Error = false };
            var fromTime = !string.IsNullOrEmpty(obj.FromTime) ? DateTime.ParseExact(obj.FromTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toTime = !string.IsNullOrEmpty(obj.ToTime) ? DateTime.ParseExact(obj.ToTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var model = _context.ParamForWarnings.
                        Where(x => (x.isDeleted == false) && (x.aetType.ToLower() == obj.AETType.ToLower()) && (x.catCode.ToLower() == obj.CatCode.ToLower()) && ((fromTime >= x.fromTime && fromTime < x.toTime) || (toTime > x.fromTime && toTime <= x.toTime)) && (x.id != obj.id)).
                        AsNoTracking().ToList();
            //var model1 = _context.ParamForWarnings.Where(x => (x.id == data.id) && (x.isDeleted == false)).AsNoTracking().ToList(); 
           
            try
            {
                
                var data = _context.ParamForWarnings.FirstOrDefault(x => x.id == obj.id && x.isDeleted == false);
                if (model.Count == 0)
                {
                    if (data != null)
                    {
                        data.updatedBy = ESEIM.AppContext.UserName;
                        data.updatedTime = DateTime.Now;
                        data.total =decimal.Parse(obj.Total);
                        data.currency = obj.Currency;
                        data.fromTime = Convert.ToDateTime(fromTime);
                        data.toTime = Convert.ToDateTime(toTime);
                        data.aetType = obj.AETType;
                        data.catCode = obj.CatCode;

                    }
                    _context.ParamForWarnings.Update(data);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("PFW_MSG_UPDATE_WARNING_SUCCESS"));//"Cập nhật cảnh báo thành công";

                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("PFW_MSG_WARNING_EXITS"));//"Cảnh báo đã tồn tại";
                }
                return Json(msg);

            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("PFW_MSG_UPDATE_WARNING_FAIL"));//"Có lỗi xảy ra khi cập nhật!";
            }
            return Json(msg);
        }

        [HttpPost]
        public object Delete(int id)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var data = _context.ParamForWarnings.FirstOrDefault(x => x.id == id);
                data.deletedBy = ESEIM.AppContext.UserName;
                data.deletedTime = DateTime.Now;
                data.isDeleted = true;

                _context.ParamForWarnings.Update(data);
                _context.SaveChanges();

                msg.Title = String.Format(CommonUtil.ResourceValue("PFW_MSG_DELETE_WARNING_SUCCESS")); //"Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("PFW_MSG_DELETE_WARNING_FAIL")); //"Có lỗi xảy ra khi xóa!";
                msg.Object = ex;
                return Json(msg);
            }
        }

    }
}