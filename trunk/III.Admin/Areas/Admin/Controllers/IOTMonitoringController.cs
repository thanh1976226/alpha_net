using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using System.Data;
using System.Globalization;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class IOTMonitoringController : BaseController
    {
        public class IOTMonitoringJtableModel
        {
            public int Id { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public string DeviceName { get; set; }
            public string StripDue { get; set; }
            public string Accuracy { get; set; }
            public string SpleepDue { get; set; }
            public string Screen { get; set; }
            public string Pin { get; set; }
            public string Size { get; set; }
            public string Mass { get; set; }
            public string Accessories { get; set; }
            public DateTime? MeasurementTime { get; set; }
            public string LocationGps { get; set; }
            public string LocationText { get; set; }
            public string Status { get; set; }
        }
        private readonly EIMDBContext _context;
        public IOTMonitoringController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelMN : JTableModel
        {
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string DeviceName { get; set; }
            public string StripDue { get; set; }
            public string Accuracy { get; set; }
            public string SpleepDue { get; set; }
            public string Screen { get; set; }
            public string Pin { get; set; }
            public string Size { get; set; }
            public string Mass { get; set; }
            public string Accessories { get; set; }
            public DateTime? MeasurementTime { get; set; }
            public string LocationGps { get; set; }
            public string LocationText { get; set; }
            public string Status { get; set; }
        }
        [HttpPost]
        public object JTable([FromBody]JTableModelMN jTablePara)
        {
            var fromDate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toDate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.IotSensors
                        where (string.IsNullOrEmpty(jTablePara.DeviceName) || a.DeviceName.ToLower().Contains(jTablePara.DeviceName.ToLower()))
                                  && ((fromDate == null) || (a.CreatedTime.HasValue && a.CreatedTime.Value.Date >= fromDate))
                                  && ((toDate == null) || (a.CreatedTime.HasValue && a.CreatedTime.Value.Date <= toDate))
                        select new IOTMonitoringJtableModel
                        {
                            Id = a.Id,
                            DeviceName = a.DeviceName,
                            Accuracy = a.Accuracy,
                            Pin = a.Pin,
                            Size = a.Size,
                            StripDue = a.StripDue,
                            SpleepDue = a.SpleepDue,
                            Screen = a.Screen,
                            Mass = a.Mass,
                            Accessories = a.Accessories,
                            MeasurementTime = a.MeasurementTime,
                            LocationGps = a.LocationGps,
                            LocationText = a.LocationText,
                            Status = a.Status
                        };
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "DeviceName", "Accuracy", "Pin", "Size", "StripDue", "SpleepDue", "Screen", "Mass", "Accessories", "MeasurementTime", "Location", "Status", "LocationText", "LocationGps");
            return Json(jdata);
        }
        [HttpPost]
        public object GetItem(int id)
        {
            var data = _context.IotWarningSettings.FirstOrDefault(x => x.Id == id);
            return data;
        }
    }
}