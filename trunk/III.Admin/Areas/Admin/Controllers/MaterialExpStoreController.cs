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

namespace III.Admin.Controllers
{
    public class MaterialStoreExpModel
    {
        public string ExpCode { get; set; }
        public int? SupplierId { get; set; }
        public string UserId { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPayment { get; set; }
        public int? Vat { get; set; }
        public decimal? TotalPayed { get; set; }
        public string ShipperId { get; set; }
        public int? StoreId { get; set; }
        public string TimeImpCreate { get; set; }
        public string InsurantTime { get; set; }
        public bool? Debt { get; set; }
        public string Note { get; set; }
    }
    [Area("Admin")]
    public class MaterialExpStoreController : Controller
    {
        private readonly EIMDBContext _context;
        public MaterialExpStoreController(EIMDBContext context)
        {
            _context = context;
        }
        public class JTableModelMaterialImpStoreDetail : JTableModel
        {
            public string ExpCode { get; set; }
            public string StoreName { get; set; }
            public string FromTo { get; set; }
            public string DateTo { get; set; }

        }
        public class JTableModelMaterialStoreExpGoodsHeaders : JTableModel
        {
            public string ExpCode { get; set; }
            public string StoreName { get; set; }
            public string FromTo { get; set; }
            public string DateTo { get; set; }

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public object JTable([FromBody]JTableModelMaterialStoreExpGoodsHeaders jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            DateTime? fromDate = !string.IsNullOrEmpty(jTablePara.FromTo) ? DateTime.ParseExact(jTablePara.FromTo, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? toDate = !string.IsNullOrEmpty(jTablePara.DateTo) ? DateTime.ParseExact(jTablePara.DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.MaterialStoreExpGoodsHeaders
                        join c in _context.MaterialStores on a.StoreId equals c.StoreId into c1
                        from c in c1.DefaultIfEmpty()
                        where
                            ((fromDate == null || (a.TimeImpCreate >= fromDate)) && (toDate == null || (a.TimeImpCreate <= toDate)))
                            && (string.IsNullOrEmpty(jTablePara.ExpCode) || a.ExpCode.ToLower().Contains(jTablePara.ExpCode.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.StoreName) || c.StoreName.ToLower().Contains(jTablePara.StoreName.ToLower()))
                        select new
                        {
                            a.Id,
                            a.ExpCode,
                            StoreName = c != null ? c.StoreName : null,
                            a.Total,
                            a.TotalPayment,
                            a.Vat,
                            a.TotalPayed,
                            a.Debt,
                            a.TimeImpCreate
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "ExpCode", "StoreName", "Total", "TotalPayment", "Vat", "TotalPayed", "Debt", "TimeImpCreate");
            return Json(jdata);
        }
        [HttpPost]
        public object JTableDetail([FromBody]JTableModelMaterialImpStoreDetail jTablePara)
        {
            if (string.IsNullOrEmpty(jTablePara.ExpCode))
            {
                var data = new List<object>();
                var jquery = JTableHelper.JObjectTable(data, jTablePara.Draw, 0, "Id", "ProductName", "Quantity", "Unit", "Cose", "Saleoff", "BarCode", "ExpireDate");
                return Json(jquery);
            }
            else
            {
                int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
                var query = from a in _context.MaterialStoreImpGoodsDetails
                            join b in _context.MaterialProducts on a.GoodsCode equals b.ProductCode into b1
                            from b in b1.DefaultIfEmpty()
                            join c in _context.CommonSettings on a.Unit equals c.CodeSet into c1
                            from c in c1.DefaultIfEmpty()
                            where a.ImpCode == jTablePara.ExpCode
                            select new
                            {
                                a.Id,
                                ProductName = b != null ? b.ProductName : null,
                                a.Quantity,
                                Unit = c != null ? c.ValueSet : null,
                                a.Cose,
                                a.Saleoff,
                                a.Barcode,
                                a.ExpireDate
                            };
                int count = query.Count();
                var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length).AsNoTracking().ToList();
                var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "ProductName", "Quantity", "Unit", "Cose", "Saleoff", "Barcode", "ExpireDate");
                return Json(jdata);
            }
        }
        [HttpPost]
        public JsonResult GetItem([FromBody]int id)
        {
            var mess = new JMessage { Error = false, Title = "" };
            try
            {
                var query = _context.MaterialStoreExpGoodsHeaders.AsParallel().Where(x => x.Id == id)
                                                                                  .Select(x => new MaterialStoreExpModel
                                                                                  {
                                                                                      ExpCode = x.ExpCode,
                                                                                      SupplierId = x.SupplierId,
                                                                                      UserId = x.UserId,
                                                                                      Total = x.Total,
                                                                                      TotalPayment = x.TotalPayment,
                                                                                      Vat = (int?)x.Vat,
                                                                                      TotalPayed = x.TotalPayed,
                                                                                      ShipperId = x.ShipperId,
                                                                                      StoreId = x.StoreId,
                                                                                      TimeImpCreate = x.TimeImpCreate?.ToString("dd/MM/yyyy"),
                                                                                      InsurantTime = x.InsurantTime?.ToString("dd/MM/yyyy"),
                                                                                      Debt = x.Debt,
                                                                                      Note = x.Note,
                                                                                  }).FirstOrDefault();
                if (query == null)
                {
                    mess.Error = true;
                    mess.Title = "Không tồn tại phiếu xuất kho!";
                }
                mess.Object = query;
            }
            catch (Exception ex)
            {
                mess.Error = true;
                mess.Title = "Không tồn tại phiếu xuất kho!";
            }
            return Json(mess);
        }
        [HttpPost]
        public JsonResult Insert([FromBody]MaterialStoreExpModel obj)
        {
            var msg = new JMessage() { Error = false, Title = "" };

            try
            {
                var query = _context.MaterialStoreExpGoodsHeaders.FirstOrDefault(x => x.ExpCode.Equals(obj.ExpCode));
                if (query == null)
                {
                    var temp = new MaterialStoreExpGoodsHeader
                    {
                        ExpCode = obj.ExpCode,
                        SupplierId = obj.SupplierId,
                        UserId = obj.UserId,
                        Total = obj.Total,
                        TotalPayment = obj.TotalPayment,
                        Vat = obj.Vat,
                        TotalPayed = obj.TotalPayed,
                        ShipperId = obj.ShipperId,
                        StoreId = obj.StoreId,
                        TimeImpCreate = !string.IsNullOrEmpty(obj.TimeImpCreate) ? DateTime.ParseExact(obj.TimeImpCreate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                        InsurantTime = !string.IsNullOrEmpty(obj.InsurantTime) ? DateTime.ParseExact(obj.InsurantTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                        Debt = obj.Debt,
                        Note = obj.Note,
                    };
                    _context.MaterialStoreExpGoodsHeaders.Add(temp);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("MES_TITLE_INFORMATION_EXPSTORE"));
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_EXITS"), CommonUtil.ResourceValue("MES_TITLE_INFORMATION_EXPSTORE"));
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAILED"), CommonUtil.ResourceValue("MES_TITLE_INFORMATION_EXPSTORE"));
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult Update([FromBody]MaterialStoreExpModel obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var item = _context.MaterialStoreExpGoodsHeaders.FirstOrDefault(x => x.ExpCode == obj.ExpCode);
                if (item != null)
                {
                    item.ExpCode = obj.ExpCode;
                    item.SupplierId = obj.SupplierId;
                    item.UserId = obj.UserId;
                    item.Total = obj.Total;
                    item.TotalPayment = obj.TotalPayment;
                    item.Vat = obj.Vat;
                    item.TotalPayed = obj.TotalPayed;
                    item.ShipperId = obj.ShipperId;
                    item.StoreId = obj.StoreId;
                    item.TimeImpCreate = !string.IsNullOrEmpty(obj.TimeImpCreate) ? DateTime.ParseExact(obj.TimeImpCreate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                    item.InsurantTime = !string.IsNullOrEmpty(obj.InsurantTime) ? DateTime.ParseExact(obj.InsurantTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                    item.Debt = obj.Debt;
                    item.Note = obj.Note;
                    _context.MaterialStoreExpGoodsHeaders.Update(item);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("MES_TITLE_INFORMATION_EXPSTORE"));
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_EXITS"), CommonUtil.ResourceValue("MES_TITLE_INFORMATION_EXPSTORE"));
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_FAILED"), CommonUtil.ResourceValue("MES_TITLE_INFORMATION_EXPSTORE"));
                msg.Object = ex;
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult Delete([FromBody]int id)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var data = _context.MaterialStoreExpGoodsHeaders.FirstOrDefault(x => x.Id == id);
                if (data == null)
                {
                    msg.Error = true;

                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_NOT_EXITS"), CommonUtil.ResourceValue("MES_TITLE_INFORMATION_SHIPMENT"));
                }
                else
                {
                    //data.IsDeleted = true;
                    //data.DeletedBy = ESEIM.AppContext.UserName;
                    //data.DeletedTime = DateTime.Now;
                    //_context.MaterialStoreExpGoodsHeaders.Update(data);
                    _context.MaterialStoreExpGoodsHeaders.Remove(data);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("MES_TITLE_INFORMATION_SHIPMENT"));
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("MES_TITLE_INFORMATION_SHIPMENT"));
                msg.Object = ex;
            }
            return Json(msg);
        }
        [HttpPost]
        public object GetStore()
        {
            var store = _context.MaterialStores.AsParallel().Where(x => !x.IsDeleted).Select(x => new { Id = x.StoreId, Name = x.StoreName });
            return store;
        }
        [HttpPost]
        public object Getsupplier()
        {
            var supplier = _context.Suppliers.AsParallel().Where(x => !x.IsDeleted).Select(x => new { Id = x.SupID, Name = x.SupName });
            return supplier;
        }
        [HttpPost]
        public object GetUser()
        {
            var query = _context.Users.Where(x => x.Active && x.UserName != "admin").AsParallel().Select(x => new { Id = x.Id, Name = x.GivenName });
            return query;
        }
        [HttpGet]
        public object GetUnit(string impCode)
        {
            var unit = _context.MaterialStoreImpGoodsDetails.Where(x => x.ImpCode == impCode).Select(x => x.Unit);
            var list = _context.CommonSettings.Where(x => unit.Any(y => x.CodeSet == y)).Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return list;
        }
    }
}