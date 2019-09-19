using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using System.Globalization;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class IotCarInAndOutController : BaseController
    {
        public class IotCarInAndOutJtableModel
        {
            public int Id { get; set; }
            public string LicensePlate { get; set; }
            public DateTime? DateTime { get; set; }
            public DateTime? ConfirmTime { get; set; }
            public string Active { get; set; }
            public string Driver { get; set; }
            public string Img1 { get; set; }
            public string Img2 { get; set; }
            public string Img3 { get; set; }
        }
        private readonly EIMDBContext _context;
        public IotCarInAndOutController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelCIO : JTableModel
        {
            public int Id { get; set; }
            public string LicensePlate { get; set; }
            public string Active { get; set; }
            public string DateTime { get; set; }
            public string ConfirmTime { get; set; }
            public string Driver { get; set; }
            public string Img1 { get; set; }
            public string Img2 { get; set; }
            public string Img3 { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }
        [HttpPost]
        public object JTable([FromBody]JTableModelCIO jTablePara)
        {
            var fromDate = !string.IsNullOrEmpty(jTablePara.DateTime) ? DateTime.ParseExact(jTablePara.DateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toDate = !string.IsNullOrEmpty(jTablePara.ConfirmTime) ? DateTime.ParseExact(jTablePara.ConfirmTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.IotCarInOuts
                        where (string.IsNullOrEmpty(jTablePara.Active) || a.Active.ToLower().Contains(jTablePara.Active.ToLower()))
                            &&(string.IsNullOrEmpty(jTablePara.LicensePlate) || a.LicensePlate.ToLower().Contains(jTablePara.LicensePlate.ToLower()))
                            &&((fromDate == null) || (a.DateTime.HasValue && a.DateTime >= fromDate))
                            && ((toDate == null) || (a.DateTime.HasValue && a.DateTime <= toDate))
                        select new IotCarInAndOutJtableModel
                        {
                            Id = a.Id,
                            LicensePlate = a.LicensePlate,
                            Active = a.Active,
                            Driver = a.Driver,
                            DateTime = a.DateTime,
                            ConfirmTime = a.ConfirmTime
                        };
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "LicensePlate", "Active", "Driver", "DateTime", "ConfirmTime");
            return Json(jdata);
        }
        [HttpPost]
        public object GetItem([FromBody] int id)
        {
            var data = _context.IotCarInOuts.FirstOrDefault(x => x.Id == id);
            return data;
        }
    }
}