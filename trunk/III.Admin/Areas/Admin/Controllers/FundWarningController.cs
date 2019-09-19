using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using System.Collections.Generic;

namespace III.Admin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FundWarningController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly EIMDBContext _context;
        public FundWarningController(EIMDBContext context)
        {
            _context = context;
        }
        public class JTableModelWarning : JTableModel
        {
            public string CatCode { get; set; }
        }
            [HttpPost]
        public object JTableExpense(JTableModelWarning jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var data = (from a in _context.FundAccEntrys.Where(x => x.IsDeleted == false && x.IsPlan == false && x.AetType == "Expense")
                        join b in _context.FundAccEntryTrackings.Where(x => x.IsDeleted == false && x.Action == "APPROVED")
                        on a.AetCode equals b.AetCode
                        select new
                        {
                            a.CatCode,
                            a.AetType,
                            a.AetCode,
                            a.DeadLine,
                            a.Payer,
                            a.Receiptter,
                            a.Total,
                        }).ToList();
            var data1 = (from a in _context.FundCatReptExpss.Where(x => x.IsDeleted == false)
                         join b in data on a.CatCode equals b.CatCode
                         select new
                         {
                             CatName = a.CatName,
                             CatCode = b.CatCode,
                             AetType = b.AetType,
                             AetCode = b.AetCode,
                             DeadLine = b.DeadLine,
                             Payer = b.Payer,
                             Receiptter = b.Receiptter,
                             Total = b.Total
                         }).ToList();
            var data2 = (from a in data1
                        join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Expense")
                        on a.CatCode equals b.catCode
                        where a.DeadLine >= b.fromTime && a.DeadLine <= b.toTime
                        group a by new {a.CatCode,a.AetType}
                        into list
                        orderby list.Key.CatCode
                        select new
                        {
                            list,
                            catName = list.Select(x=> x.CatName).FirstOrDefault(),
                            total = list.Sum(x => x.Total),
                            catCode = list.Key.CatCode,
                            aetType = list.Key.AetType,
                            maxDate = list.Max(x => x.DeadLine),
                            minDate = list.Min(x => x.DeadLine),

                        }).ToList();
            var query = (from a in data2.Where(x => x.aetType == "Expense")
                         join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Expense")
                         on a.catCode equals b.catCode
                         where a.maxDate <= b.toTime && a.minDate >= b.fromTime && a.total >= b.total
                         select new
                         {
                             id = b.id,
                             maxTotal = b.total,
                             fromDate = b.fromTime,
                             toDate = b.toTime,
                             aetType = b.aetType,
                             catCode = b.catCode,
                             currency = b.currency,
                             total = a.total,
                             catName = a.catName,
                         }).ToList();
            int count = query.Count();
            //var data1 = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(query, jTablePara.Draw, count, "id", "maxTotal", "fromDate", "toDate", "currency", "aetType", "catCode", "total","catName");
            return Json(jdata);
        }
        [HttpPost]
        public int CountExpense()
        {
            var data = (from a in _context.FundAccEntrys.Where(x => x.IsDeleted == false && x.IsPlan == false && x.AetType == "Expense")
                        join b in _context.FundAccEntryTrackings.Where(x => x.IsDeleted == false && x.Action == "APPROVED")
                        on a.AetCode equals b.AetCode
                        select new
                        {
                            a.CatCode,
                            a.AetType,
                            a.AetCode,
                            a.DeadLine,
                            a.Payer,
                            a.Receiptter,
                            a.Total,
                        }).ToList();
            var data1 = (from a in data
                         join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Expense")
                         on a.CatCode equals b.catCode
                         where a.DeadLine >= b.fromTime && a.DeadLine <= b.toTime
                         group a by new { a.CatCode, a.AetType }
                        into list
                         orderby list.Key.CatCode
                         select new
                         {
                             list,
                             total = list.Sum(x => x.Total),
                             catCode = list.Key.CatCode,
                             aetType = list.Key.AetType,
                             maxDate = list.Max(x => x.DeadLine),
                             minDate = list.Min(x => x.DeadLine),

                         }).ToList();
            var query = (from a in data1.Where(x => x.aetType == "Expense")
                         join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Expense")
                         on a.catCode equals b.catCode
                         where a.maxDate <= b.toTime && a.minDate >= b.fromTime && a.total >= b.total
                         select new
                         {
                             id = b.id,
                             maxTotal = b.total,
                             fromDate = b.fromTime,
                             toDate = b.toTime,
                             aetType = b.aetType,
                             catCode = b.catCode,
                             currency = b.currency,
                             total = a.total,
                         }).ToList();
            int count = query.Count();
            return count;
        }
        [HttpPost]
        public int CountReceipt()
        {
            var data = (from a in _context.FundAccEntrys.Where(x => x.IsDeleted == false && x.IsPlan == false && x.AetType == "Receipt" && x.Status == "APPROVED")
                        join b in _context.FundAccEntryTrackings.Where(x => x.IsDeleted == false && x.Action == "APPROVED")
                        on a.AetCode equals b.AetCode
                        select new
                        {
                            a.CatCode,
                            a.AetType,
                            a.AetCode,
                            a.DeadLine,
                            a.Payer,
                            a.Receiptter,
                            a.Total,
                        }).ToList();
            var data1 = (from a in data
                         join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Receipt")
                         on a.CatCode equals b.catCode
                         where a.DeadLine >= b.fromTime && a.DeadLine <= b.toTime
                         group a by new { a.CatCode, a.AetType }
                        into list
                         orderby list.Key.CatCode
                         select new
                         {
                             list,
                             total = list.Sum(x => x.Total),
                             catCode = list.Key.CatCode,
                             aetType = list.Key.AetType,
                             maxDate = list.Max(x => x.DeadLine),
                             minDate = list.Min(x => x.DeadLine),

                         }).ToList();
            var query = (from a in data1.Where(x => x.aetType == "Receipt")
                         join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Receipt")
                         on a.catCode equals b.catCode
                         where a.maxDate <= b.toTime && a.minDate >= b.fromTime && a.total >= b.total
                         select new
                         {
                             id = b.id,
                             maxTotal = b.total,
                             fromDate = b.fromTime,
                             toDate = b.toTime,
                             aetType = b.aetType,
                             catCode = b.catCode,
                             currency = b.currency,
                             total = a.total,
                         }).ToList();
            int count = query.Count();
            return count;
                
        }
        [HttpPost]
        public int GetCountWarning()
        {
            var dataExpense = (from a in _context.FundAccEntrys.Where(x => x.IsDeleted == false && x.IsPlan == false && x.AetType == "Expense" && x.Status == "APPROVED")
                        join b in _context.FundAccEntryTrackings.Where(x => x.IsDeleted == false && x.Action == "APPROVED")
                        on a.AetCode equals b.AetCode
                        select new
                        {
                            a.CatCode,
                            a.AetType,
                            a.AetCode,
                            a.DeadLine,
                            a.Payer,
                            a.Receiptter,
                            a.Total,
                        }).ToList();
            var dataExpense1 = (from a in dataExpense
                               join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Expense")
                         on a.CatCode equals b.catCode
                         where a.DeadLine >= b.fromTime && a.DeadLine <= b.toTime
                         group a by new { a.CatCode, a.AetType }
                        into list
                         orderby list.Key.CatCode
                         select new
                         {
                             list,
                             total = list.Sum(x => x.Total),
                             catCode = list.Key.CatCode,
                             aetType = list.Key.AetType,
                             maxDate = list.Max(x => x.DeadLine),
                             minDate = list.Min(x => x.DeadLine),

                         }).ToList();
            var queryExpense = (from a in dataExpense1.Where(x => x.aetType == "Expense")
                         join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Expense")
                         on a.catCode equals b.catCode
                         where a.maxDate <= b.toTime && a.minDate >= b.fromTime && a.total >= b.total
                         select new
                         {
                             id = b.id,
                             maxTotal = b.total,
                             fromDate = b.fromTime,
                             toDate = b.toTime,
                             aetType = b.aetType,
                             catCode = b.catCode,
                             currency = b.currency,
                             total = a.total,
                         }).ToList();
            var dataReceipt = (from a in _context.FundAccEntrys.Where(x => x.IsDeleted == false && x.IsPlan == false && x.AetType == "Receipt")
                        join b in _context.FundAccEntryTrackings.Where(x => x.IsDeleted == false && x.Action == "APPROVED")
                        on a.AetCode equals b.AetCode
                        select new
                        {
                            a.CatCode,
                            a.AetType,
                            a.AetCode,
                            a.DeadLine,
                            a.Payer,
                            a.Receiptter,
                            a.Total,
                        }).ToList();
            var dataReceipt1 = (from a in dataReceipt
                                join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Receipt")
                         on a.CatCode equals b.catCode
                         where a.DeadLine >= b.fromTime && a.DeadLine <= b.toTime
                         group a by new { a.CatCode, a.AetType }
                        into list
                         orderby list.Key.CatCode
                         select new
                         {
                             list,
                           
                             total = list.Sum(x => x.Total),
                             catCode = list.Key.CatCode,
                             aetType = list.Key.AetType,
                             maxDate = list.Max(x => x.DeadLine),
                             minDate = list.Min(x => x.DeadLine),

                         }).ToList();
            var queryReceipt = (from a in dataReceipt1.Where(x => x.aetType == "Receipt")
                         join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Receipt")
                         on a.catCode equals b.catCode
                         where a.maxDate <= b.toTime && a.minDate >= b.fromTime && a.total >= b.total
                         select new
                         {
                             id = b.id,
                             maxTotal = b.total,
                             fromDate = b.fromTime,
                             toDate = b.toTime,
                             aetType = b.aetType,
                             catCode = b.catCode,
                             currency = b.currency,
                             total = a.total,
                           
                         }).ToList();

            return queryReceipt.Count() + queryExpense.Count();
            
      
        }
        [HttpPost]       
        public object JTableReceipt(JTableModelWarning jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var data = (from a in _context.FundAccEntrys.Where(x => x.IsDeleted == false && x.IsPlan == false && x.AetType == "Receipt")
                        join b in _context.FundAccEntryTrackings.Where(x => x.IsDeleted == false && x.Action == "APPROVED")
                        on a.AetCode equals b.AetCode
                        select new
                        {
                            a.CatCode,
                            a.AetType,
                            a.AetCode,
                            a.DeadLine,
                            a.Payer,
                            a.Receiptter,
                            a.Total,
                        }).ToList();
            var data1 = (from a in _context.FundCatReptExpss.Where(x => x.IsDeleted == false)
                         join b in data on a.CatCode equals b.CatCode
                         select new
                         {
                             CatName = a.CatName,
                             CatCode = b.CatCode,
                             AetType = b.AetType,
                             AetCode = b.AetCode,
                             DeadLine = b.DeadLine,
                             Payer = b.Payer,
                             Receiptter = b.Receiptter,
                             Total = b.Total
                         }).ToList();
            var data2 = (from a in data1
                         join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Receipt")
                         on a.CatCode equals b.catCode
                         where a.DeadLine >= b.fromTime && a.DeadLine <= b.toTime
                         group a by new { a.CatCode, a.AetType }
                        into list
                         orderby list.Key.CatCode
                         select new
                         {
                             list,
                             catName = list.Select(x => x.CatName).FirstOrDefault(),
                             total = list.Sum(x => x.Total),
                             catCode = list.Key.CatCode,
                             aetType = list.Key.AetType,
                             maxDate = list.Max(x => x.DeadLine),
                             minDate = list.Min(x => x.DeadLine),

                         }).ToList();
            var query = (from a in data2.Where(x => x.aetType == "Receipt")
                         join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == "Receipt")
                         on a.catCode equals b.catCode
                         where a.maxDate <= b.toTime && a.minDate >= b.fromTime && a.total >= b.total
                         select new
                         {
                             id = b.id,
                             maxTotal = b.total,
                             fromDate = b.fromTime,
                             toDate = b.toTime,
                             aetType = b.aetType,
                             catCode = b.catCode,
                             currency = b.currency,
                             total = a.total,
                             catName = a.catName
                         }).ToList();
            int count = query.Count();
            //var data1 = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(query, jTablePara.Draw, count, "id", "maxTotal", "fromDate", "toDate", "currency", "aetType", "catCode", "total","catName");
            return Json(jdata);

        }

    }
}