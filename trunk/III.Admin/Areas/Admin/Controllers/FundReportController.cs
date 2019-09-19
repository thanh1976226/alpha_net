using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ESEIM;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using III.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class FundReportController : BaseController
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<AspNetRole> _roleManager;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly EIMDBContext _context;

        public FundReportController(IOptions<AppSettings> appSettings, EIMDBContext context, UserManager<AspNetUser> userManager, RoleManager<AspNetRole> roleManager, IHostingEnvironment hostingEnvironment)
        {
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class SearchChartModel
        {
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string AetType { get; set; }
            public string CatParent { get; set; }
            public string CatCodeExpense { get; set; }
            public string CatCodeReceipte { get; set; }
        }
        public class SearchChartResponse
        {
            public decimal Total { get; set; }
            public string Date { get; set; }
  
        }
        [HttpPost]
        public (object totalReceipt, object totalExpense) SearchChart([FromBody]SearchChartModel obj)
        {
            List<SearchChartResponse> totalReceipt = new List<SearchChartResponse>();
            List<SearchChartResponse> totalExpense = new List<SearchChartResponse>();
            try
            {
                DateTime? fromDate = !string.IsNullOrEmpty(obj.FromDate) ? DateTime.ParseExact(obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                DateTime? toDate = !string.IsNullOrEmpty(obj.ToDate) ? DateTime.ParseExact(obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                if (string.IsNullOrEmpty(obj.CatCodeExpense))
                {
                    totalExpense = (from a in _context.FundAccEntrys.Where(x => x.IsDeleted == false && x.IsPlan == false && x.Status == "APPROVED")
                                    join b in _context.FundCatReptExpss.Where(x => x.IsDeleted == false) on a.CatCode equals b.CatCode
                                    let date = a.DeadLine.Value.Date
                                    where (fromDate == null || (a.CreatedTime >= fromDate))
                                    && (toDate == null || (a.CreatedTime <= toDate))
                                    && ((string.IsNullOrEmpty(obj.AetType) && a.AetType == "Expense") || (!string.IsNullOrEmpty(a.AetType) && a.AetType == "Expense" && a.AetType == obj.AetType))
                                    && (string.IsNullOrEmpty(obj.CatParent) || (!string.IsNullOrEmpty(b.CatParent) && b.CatParent == obj.CatParent))
                                    group new { a } by new { date }
                                    into grp
                                    orderby grp.Key.date
                                    select new SearchChartResponse
                                    {
                                        Total = grp.Sum(x => x.a.Total),
                                        Date = grp.Key.date.ToString("dd/MM/yyyy"),
                                    })
                                    .ToList();
                }
               else {
                    totalExpense = (from a in _context.FundAccEntrys.Where(x => x.IsDeleted == false && x.IsPlan == false && x.Status== "APPROVED")
                                        //join b in _context.FundCatReptExpss.Where(x => x.IsDeleted == false) on a.CatCode equals b.CatCode
                                    let date = a.DeadLine.Value.Date
                                    where (fromDate == null || (a.CreatedTime >= fromDate))
                                    && (toDate == null || (a.CreatedTime <= toDate))
                                    && ((string.IsNullOrEmpty(obj.AetType) && a.AetType == "Expense") || (!string.IsNullOrEmpty(a.AetType) && a.AetType == "Expense" && a.AetType == obj.AetType))
                                    && (!string.IsNullOrEmpty(a.CatCode) && a.CatCode == obj.CatCodeExpense)
                                    group new { a } by new { date }
                                     into grp
                                    orderby grp.Key.date
                                    select new SearchChartResponse
                                    {
                                        Total = grp.Sum(x => x.a.Total),
                                        Date = grp.Key.date.ToString("dd/MM/yyyy"),
                                    })
                                .ToList();
                }
                if (string.IsNullOrEmpty(obj.CatCodeReceipte))
                {
                    totalReceipt = (from a in _context.FundAccEntrys.Where(x => x.IsDeleted == false && x.IsPlan == false && x.Status == "APPROVED")
                                    join b in _context.FundCatReptExpss.Where(x => x.IsDeleted == false) on a.CatCode equals b.CatCode
                                    let date = a.DeadLine.Value.Date
                                    where (fromDate == null || (a.CreatedTime >= fromDate))
                                    && (toDate == null || (a.CreatedTime <= toDate))
                                    && ((string.IsNullOrEmpty(obj.AetType) && a.AetType == "Receipt") || (!string.IsNullOrEmpty(a.AetType) && a.AetType == "Receipt" && a.AetType == obj.AetType))
                                    && (string.IsNullOrEmpty(obj.CatParent) || (!string.IsNullOrEmpty(b.CatParent) && b.CatParent == obj.CatParent))
                                    group new { a } by new { date }
                                into grp
                                    orderby grp.Key.date
                                    select new SearchChartResponse
                                    {
                                        Total = grp.Sum(x => x.a.Total),
                                        Date = grp.Key.date.ToString("dd/MM/yyyy"),
                                    })
                                .ToList();
                }
                else
                {

                    totalReceipt = (from a in _context.FundAccEntrys.Where(x => x.IsDeleted == false && x.IsPlan == false && x.Status == "APPROVED")
                                        //join b in _context.FundCatReptExpss.Where(x => x.IsDeleted == false) on a.CatCode equals b.CatCode
                                    let date = a.DeadLine.Value.Date
                                    where (fromDate == null || (a.CreatedTime >= fromDate))
                                    && (toDate == null || (a.CreatedTime <= toDate))
                                    && ((string.IsNullOrEmpty(obj.AetType) && a.AetType == "Receipt") || (!string.IsNullOrEmpty(a.AetType) && a.AetType == "Receipt" && a.AetType == obj.AetType))
                                    && (string.IsNullOrEmpty(obj.CatParent) || (!string.IsNullOrEmpty(a.CatCode) && a.CatCode == obj.CatCodeReceipte))
                                    group new { a } by new { date }
                                    into grp
                                    orderby grp.Key.date
                                    select new SearchChartResponse
                                    {
                                        Total = grp.Sum(x => x.a.Total),
                                        Date = grp.Key.date.ToString("dd/MM/yyyy"),
                                    })
                                    .ToList();
                }

            }
            catch
            {

            }
            return (totalReceipt, totalExpense);
        }

        [HttpPost]
        public object GetCatParent()
        {
            var data = _context.FundCatReptExpss.Where(x => x.CatParent != null && x.IsDeleted ==false).Select(x => new { x.CatParent}).DistinctBy(x=>x.CatParent).ToList();
            var query = (from a in _context.FundCatReptExpss
                         join b in data on a.CatCode equals b.CatParent
                         select new
                         {
                             CatName = a.CatName,
                             CatCode = a.CatCode,
                         }).ToList();
            return query;
        }
        [HttpPost]
        public object GetCatChildrentExpense(string obj)
        {

               var  data = _context.FundCatReptExpss.Where(x => x.CatParent == obj && x.IsDeleted == false).Select(x => new { CatNameExpense = x.CatName, CatCodeExpense = x.CatCode }).ToList();

            return data;
        }
        [HttpPost]
        public object GetCatChildrentReceipte(string obj)
        {

            var data = _context.FundCatReptExpss.Where(x => x.CatParent == obj && x.IsDeleted == false).Select(x => new { CatNameReceipte = x.CatName, CatCodeReceipte = x.CatCode }).ToList();

            return data;
        }

    }
 
}