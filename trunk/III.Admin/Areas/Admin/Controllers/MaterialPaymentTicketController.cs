﻿using System;
using System.Globalization;
using System.Linq;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class MaterialPaymentTicketController : BaseController
    {
        public class JTableModelTicket : JTableModel
        {
            public string PayTitle { get; set; }
            public string ContractName { get; set; }
            public bool? PayType { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }

        private readonly EIMDBContext _context;
        public MaterialPaymentTicketController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public object JTable([FromBody]JTableModelTicket jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            DateTime? fromDate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? toDate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.MaterialPaymentTickets
                        join b in _context.ContractHeaders on a.PayObjId equals b.ContractCode into b1
                        from b in b1.DefaultIfEmpty()
                        join c in listCommon on a.Currency equals c.CodeSet into c1
                        from c in c1.DefaultIfEmpty()
                        where (string.IsNullOrEmpty(jTablePara.ContractName) || b.Title.ToLower().Contains(jTablePara.ContractName.ToLower()))
                           && (string.IsNullOrEmpty(jTablePara.PayTitle) || a.PayTitle.ToLower().Contains(jTablePara.PayTitle.ToLower()))
                           && ((fromDate == null || (a.CreatedTime >= fromDate)) && (toDate == null || (a.CreatedTime <= toDate)))
                           && (jTablePara.PayType == null || a.PayType == jTablePara.PayType)
                        select new
                        {
                            Id = a.PaymentTickitId,
                            Title = a.PayTitle,
                            ContractName = b != null ? b.Title : "Không xác định",
                            Type = a.PayType == true ? "Phiếu thu" : "Phiếu chi",
                            TotalPay = a.MoneyTotal,
                            Unit = c != null ? c.CodeSet : "Không xác định",
                            a.CreatedTime,
                        };
            int count = query.Count();
            //var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length).AsNoTracking().ToList();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "Id", "Title", "ContractName", "TotalPay", "Unit", "Type", "CreatedTime");
            return Json(jdata);
        }
        [HttpGet]
        public JsonResult GetItem(int id)
        {
            var mess = new JMessage { Error = false, Title = "" };
            try
            {
                var query = _context.MaterialPaymentTickets.AsParallel().Where(x => x.PaymentTickitId.Equals(id)).FirstOrDefault();
                if (query == null)
                {
                    mess.Error = true;
                    mess.Title = "Không tồn tại lô hàng";
                }
                else
                {
                    mess.Object = query;
                }
            }
            catch (Exception ex)
            {
                mess.Error = true;
                mess.Title = "Không tồn tại lô hàng";
            }
            return Json(mess);
        }
        [HttpPost]
        public JsonResult Insert([FromBody]MaterialPaymentTicket obj)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var query = _context.MaterialPaymentTickets.Where(x => x.PayCode.Equals(obj.PayCode)).FirstOrDefault();
                if (query == null)
                {
                    obj.CreatedTime = DateTime.Now;
                    obj.CreatedBy = ESEIM.AppContext.UserName;
                    _context.MaterialPaymentTickets.Add(obj);
                    _context.SaveChanges();
                    msg.Title = "Thêm phiếu thu - chi thành công";
                    return Json(msg);
                }
                else
                { 
                    msg.Error = true;
                    msg.Title = "Mã phiếu thu - chi đã tồn tại";
                }
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Thêm phiếu thu - chi lỗi!";
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult Delete([FromBody]int id)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var data = _context.MaterialPaymentTickets.FirstOrDefault(x => x.PaymentTickitId == id);
                if (data == null)
                {
                    msg.Error = true;
                    msg.Title = "Không tồn tại phiếu thu chi";
                }
                else
                {
                    _context.MaterialPaymentTickets.Remove(data);
                    _context.SaveChanges();
                    msg.Title = "Xóa phiếu thu chi thành công";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Xóa phiếu thu chi lỗi!";
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult Update([FromBody]MaterialPaymentTicket obj)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                obj.UpdatedTime = DateTime.Now.Date;
                _context.MaterialPaymentTickets.Update(obj);
                _context.SaveChanges();
                msg.Title = "Sửa phiếu thu - chi thành công";

                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Sửa phiếu thu - chi lỗi!";
            }
            return Json(msg);
        }
        //[HttpPost]
        //public JsonResult Update([FromBody]MaterialSBatchModel obj)
        //{
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {
        //        var item = _context.MaterialStoreBatchGoodss.FirstOrDefault(x => x.Code == obj.Code);
        //        item.Name = obj.Name;
        //        item.ProductCode = obj.ProductCode;
        //        item.StoreId = obj.StoreId;
        //        item.Quantity = obj.Quantity;
        //        item.Unit = obj.Unit;
        //        item.Vat = obj.Vat;
        //        item.Cost = obj.Cost;
        //        item.Currency = obj.Currency;
        //        item.Barcode = obj.Barcode;
        //        item.SupplierId = obj.SupplierId;
        //        item.Madein = obj.Madein;
        //        item.Packing = obj.Packing;
        //        item.Sale = obj.Sale;
        //        item.Description = obj.Description;
        //        item.DateProduce = !string.IsNullOrEmpty(obj.DateProduce) ? DateTime.ParseExact(obj.DateProduce, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
        //        item.DateExpire = !string.IsNullOrEmpty(obj.DateExpire) ? DateTime.ParseExact(obj.DateExpire, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
        //        item.DateReiceive = !string.IsNullOrEmpty(obj.DateReiceive) ? DateTime.ParseExact(obj.DateReiceive, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
        //        item.UpdatedBy = ESEIM.AppContext.UserName;
        //        item.UpdatedTime = DateTime.Now;
        //        _context.MaterialStoreBatchGoodss.Update(item);
        //        _context.SaveChanges();
        //        msg.Title = "Cập nhật lô hàng thành công";
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Cập nhật lô hàng lỗi!";
        //        msg.Object = ex;
        //    }
        //    return Json(msg);
        //}


        #region comboxbox
        [HttpPost]
        public object GetPaymentObjType()
        {
            var query = _context.CommonSettings.AsParallel().Where(x => x.CodeSet == "PAYMENT_CONTRACT").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return query;
        }
        [HttpPost]
        public object GetPaymentObjTypeProject()
        {
            var query = _context.CommonSettings.AsParallel().Where(x => x.CodeSet == "PAYMENT_PROJECT").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return query;
        }
        [HttpPost]
        public object GetUnit()
        {
            return GetCurrencyBase();
        }

        [HttpPost]
        public object GetContract()
        {
            var query = _context.ContractHeaders.AsParallel().Where(x => !x.IsDeleted).Select(x => new { Code = x.ContractCode, Name = x.Title });
            return query;
        }
        [HttpPost]
        public object GetProject()
        {
            var query = _context.Projects.AsParallel().Where(x => !x.FlagDeleted).Select(x => new { Code = x.ProjectCode, Name = x.ProjectTitle });
            return query;
        }
        #endregion

        #region Kiểm tra hợp đồng đã thanh toán hết hay chưa
        //Kiểm tra hợp đồng đã thanh toán hết hay chưa
        [HttpGet]
        public JsonResult CheckContract(string contractCode)
        {
            var msg = new JMessage { Error = false, Title = "" };
            decimal? total = 0;
            var query = _context.ContractHeaders.FirstOrDefault(x => !x.IsDeleted && x.ContractCode == contractCode);
            if (query == null)
            {
                msg.Title = "Không tồn tại hợp đồng";
                msg.Error = true;
            }
            else
            {
                var getListPayment = _context.MaterialPaymentTickets.Where(x => x.PayObjId == contractCode).Sum(x => x.MoneyTotal);
                total = (query.Budget - getListPayment);
            }
            msg.Object = total;
            return Json(msg);
        }
        [HttpGet]
        public JsonResult CheckProject(string contractCode)
        {
            var msg = new JMessage { Error = false, Title = "" };
            //var da = _context.Projects.FirstOrDefault(x => x.ProjectCode == contractCode);
            decimal? total = 0;
            var query = _context.Projects.FirstOrDefault(x => !x.FlagDeleted && x.ProjectCode == contractCode);
            if (query == null)
            {
                msg.Title = "Không tồn tại dự án";
                msg.Error = true;
            }
            else
            {
                var getListPayment = _context.MaterialPaymentTickets.Where(x => x.PayObjId == contractCode).Sum(x => x.MoneyTotal);
                total = (Convert.ToDecimal(query.Budget) - getListPayment);
            }
            msg.Object = total;
            return Json(msg);
        }

        #endregion
    }
}