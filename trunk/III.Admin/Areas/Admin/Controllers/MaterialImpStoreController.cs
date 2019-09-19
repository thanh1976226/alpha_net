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
    public class MaterialStoreModel
    {
        public string ImpCode { get; set; }
        public int? SupplierId { get; set; }
        public string UserId { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPayment { get; set; }
        public int? Vat { get; set; }
        public decimal? TotalPayed { get; set; }
        public string ShipperId { get; set; }
        public int? StoreId { get; set; }
        public string CreatedTime { get; set; }
        public string TimeImpCreate { get; set; }
        public string InsurantTime { get; set; }
        public bool? Debt { get; set; }
        public string Note { get; set; }
    }
    [Area("Admin")]
    public class MaterialImpStoreController : BaseController
    {
        private readonly EIMDBContext _context;
        public MaterialImpStoreController(EIMDBContext context)
        {
            _context = context;
        }
        public class JTableModelMaterialImpStoreDetail : JTableModel
        {
            public string ImpCode { get; set; }
        }
        public class JTableModelMaterialImpStore : JTableModel
        {
            public string ImpCode { get; set; }
            public string StoreName { get; set; }
            public string FromTo { get; set; }
            public string DateTo { get; set; }
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public object JTable([FromBody]JTableModelMaterialImpStore jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            DateTime? fromDate = !string.IsNullOrEmpty(jTablePara.FromTo) ? DateTime.ParseExact(jTablePara.FromTo, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? toDate = !string.IsNullOrEmpty(jTablePara.DateTo) ? DateTime.ParseExact(jTablePara.DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.MaterialStoreImpGoodsHeaders
                        join c in _context.MaterialStores on a.StoreId equals c.StoreId into c1
                        from c in c1.DefaultIfEmpty()
                        where !a.IsDeleted
                        &&((fromDate == null || (a.CreatedTime >= fromDate)) && (toDate == null || (a.CreatedTime <= toDate)))
                        && (string.IsNullOrEmpty(jTablePara.ImpCode) || a.ImpCode.ToLower().Contains(jTablePara.ImpCode.ToLower()))
                        && (string.IsNullOrEmpty(jTablePara.StoreName) || c.StoreName.ToLower().Contains(jTablePara.StoreName.ToLower()))
                        select new
                        {
                            a.Id,
                            a.ImpCode,
                            StoreName = c != null ? c.StoreName : null,
                            a.Total,
                            a.TotalPayment,
                            a.Vat,
                            a.TotalPayed,
                            a.Debt,
                            a.CreatedTime
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "ImpCode", "StoreName", "Total", "TotalPayment", "Vat", "TotalPayed", "Debt", "CreatedTime");
            return Json(jdata);
        }
        [HttpPost]
        public object JTableDetail([FromBody]JTableModelMaterialImpStoreDetail jTablePara)
        {
            if (string.IsNullOrEmpty(jTablePara.ImpCode))
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
                            where a.ImpCode == jTablePara.ImpCode
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
                var query = _context.MaterialStoreImpGoodsHeaders.AsParallel().Where(x => x.Id == id)
              .Select(x => new MaterialStoreModel
              {
                  ImpCode = x.ImpCode,
                  SupplierId = x.SupplierId,
                  UserId = x.UserId,
                  Total = x.Total,
                  TotalPayment = x.TotalPayment,
                  Vat = x.Vat,
                  TotalPayed = x.TotalPayed,
                  ShipperId = x.ShipperId,
                  StoreId = x.StoreId,
                  CreatedTime = x.CreatedTime?.ToString("dd/MM/yyyy"),
                  Debt = x.Debt,
                  Note = x.Note,
              }).FirstOrDefault();
                if (query == null)
                {
                    mess.Error = true;
                    mess.Title = "Không tồn tại lô hàng";
                }
                mess.Object = query;
            }
            catch (Exception ex)
            {
                mess.Error = true;
                mess.Title = "Không tồn tại lô hàng";
            }
            return Json(mess);
        }
        [HttpPost]
        public JsonResult Insert([FromBody]MaterialStoreModel obj)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
               
                if (_context.MaterialStoreImpGoodsHeaders.FirstOrDefault(x=>x.ImpCode==obj.ImpCode)==null)
                {
                    var temp = new MaterialStoreImpGoodsHeader
                    {
                        ImpCode = obj.ImpCode,
                        SupplierId = obj.SupplierId,
                        UserId = obj.UserId,
                        Total = obj.Total,
                        TotalPayment = obj.TotalPayment,
                        Vat = obj.Vat,
                        TotalPayed = obj.TotalPayed,
                        ShipperId = obj.ShipperId,
                        StoreId = obj.StoreId,
                        CreatedTime = !string.IsNullOrEmpty(obj.CreatedTime) ? DateTime.ParseExact(obj.CreatedTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                        Debt = obj.Debt,
                        Note = obj.Note,
                        CreatedBy = ESEIM.AppContext.UserName,
                    };
                    _context.MaterialStoreImpGoodsHeaders.Add(temp);
                    _context.SaveChanges();
                    msg.Title = "Thêm lô hàng thành công";
                    return Json(msg);
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Mã lô hàng đã tồn tại";
                    return Json(msg);
                }
               
                
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Thêm lô hàng lỗi!";
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult Update([FromBody]MaterialStoreModel obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var item = _context.MaterialStoreImpGoodsHeaders.FirstOrDefault(x => x.ImpCode == obj.ImpCode);
                item.ImpCode = obj.ImpCode;
                item.SupplierId = obj.SupplierId;
                item.UserId = obj.UserId;
                item.Total = obj.Total;
                item.TotalPayment = obj.TotalPayment;
                item.Vat = obj.Vat;
                item.TotalPayed = obj.TotalPayed;
                item.ShipperId = obj.ShipperId;
                item.StoreId = obj.StoreId;
                item.CreatedTime = !string.IsNullOrEmpty(obj.TimeImpCreate) ? DateTime.ParseExact(obj.TimeImpCreate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                item.Debt = obj.Debt;
                item.Note = obj.Note;
                item.UpdatedBy = ESEIM.AppContext.UserName;
                item.UpdatedTime = DateTime.Now;
                _context.MaterialStoreImpGoodsHeaders.Update(item);
                _context.SaveChanges();
                msg.Title = "Cập nhật lô hàng thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Cập nhật lô hàng lỗi!";
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
                var data = _context.MaterialStoreImpGoodsHeaders.FirstOrDefault(x => x.Id == id);
                if (data == null)
                {
                    msg.Error = true;
                    msg.Title = "Không tồn tại lô hàng";
                }
                else
                {
                    data.IsDeleted = true;
                    data.DeletedBy = ESEIM.AppContext.UserName;
                    data.DeletedTime = DateTime.Now;
                    _context.MaterialStoreImpGoodsHeaders.Update(data);
                    _context.SaveChanges();
                    msg.Title = "Xóa lô hàng thành công";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Xóa lô hàng lỗi!";
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