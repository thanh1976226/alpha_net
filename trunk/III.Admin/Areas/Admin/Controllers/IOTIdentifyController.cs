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
    public class IotIdentifyController : BaseController
    {
        public class IotIdentifyJtableModel
        {
            public int Id { get; set; }

            public DateTime? FromDate { get; set; }

            public DateTime? ToDate { get; set; }

            public string Address { get; set; }

            public string FaceId { get; set; }

            public string NameFace { get; set; }

            public bool ObjType { get; set; }

            public decimal Total { get; set; }
            public string Action { get; set; }
            public string Image1 { get; set; }
            public string Image2 { get; set; }
            public string Image3 { get; set; }
            public string Image4 { get; set; }
            public string LocationGps { get; set; }
            public string LocationText { get; set; }
        }
        private readonly EIMDBContext _context;
        public IotIdentifyController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelAA : JTableModel
        {

            public string FromDate { get; set; }

            public string ToDate { get; set; }

            public string Address { get; set; }

            public string FaceId { get; set; }

            public string NameFace { get; set; }

            public bool ObjType { get; set; }

            public decimal Total { get; set; }
            public string Action { get; set; }
            public string Image1 { get; set; }
            public string Image2 { get; set; }
            public string Image3 { get; set; }
            public string Image4 { get; set; }
            public string LocationGps { get; set; }
            public string LocationText { get; set; }
        }
        [HttpPost]
        public object JTable([FromBody]JTableModelAA jTablePara)
        {
            var fromDate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toDate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.IotAnalysis_Actions
                            where ((fromDate == null) || (a.CreatedTime.HasValue && a.CreatedTime.Value.Date >= fromDate))
                            && ((toDate == null) || (a.CreatedTime.HasValue && a.CreatedTime.Value.Date <= toDate))
                        select new 
                        {
                            Id = a.Id,
                            Address = a.Address,
                            FaceId = a.FaceId,
                            NameFace = a.NameFace,
                            ObjType = a.ObjType,
                            Total = a.Total,
                            Action = a.Action,
                            Image1=a.Image1,
                            Image2 =a.Image2,
                            Image3 =a.Image3,
                            Image4 =a.Image4
                        };
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "Address", "FaceId", "NameFace", "ObjType", "Total", "Action", "Image1", "Image2", "Image3", "Image4");
            return Json(jdata);
        }
    }
}