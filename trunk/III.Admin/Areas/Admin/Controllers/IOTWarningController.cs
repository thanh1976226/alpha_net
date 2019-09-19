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
    public class IotWarningController : BaseController
    {
        public class IotWarningJtableModel
        {
            public int Id { get; set; }

            public DateTime? FromDate { get; set; }

            public DateTime? ToDate { get; set; }

            public string Device { get; set; }

            public string Unit { get; set; }
            public string Picture1 { get; set; }
            public string Picture2 { get; set; }
            public string Picture3 { get; set; }
        }
        private readonly EIMDBContext _context;
        public IotWarningController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelSUA : JTableModel
        {
            public int Id { get; set; }

            public string FromDate { get; set; }

            public string ToDate { get; set; }

            public string Device { get; set; }

            public string Unit { get; set; }
            public string Picture1 { get; set; }
            public string Picture2 { get; set; }
            public string Picture3 { get; set; }
        }
        [HttpPost]
        public object JTable([FromBody]JTableModelSUA jTablePara)
        {
            var fromDate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toDate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.IotSetUpAlerts
                        where (string.IsNullOrEmpty(jTablePara.Device) || a.Device.ToLower().Contains(jTablePara.Device.ToLower()))
                                  && ((fromDate == null) || (a.CreatedTime.HasValue && a.CreatedTime.Value.Date >= fromDate))
                                  && ((toDate == null) || (a.CreatedTime.HasValue && a.CreatedTime.Value.Date <= toDate))
                        select new IotWarningJtableModel
                        {
                            Id = a.Id,
                            Device = a.Device,
                            Unit = a.Unit,
                            FromDate = a.FromDate,
                            ToDate = a.ToDate,
                        };
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "Device", "Unit", "FromDate", "ToDate");
            return Json(jdata);
        }
        [HttpPost]
        public object GetItem([FromBody] int id)
        {
            var data = _context.IotSetUpAlerts.FirstOrDefault(x => x.Id == id);
            return data;
        }
    }
}