using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class ObeAiRecognitionTrackingController : BaseController
    {
        public class ObeAiRecognitionTrackingJtableModel
        {
            public int Id { get; set; }
            public string FaceId { get; set; }
            public string ObjName { get; set; }
            public string ImgPath { get; set; }
            public string DeviceId { get; set; }
            public bool CheckPeople { get; set; }
            public DateTime CreatedTime { get; set; }
            public string LocationGps { get; set; }
        }
        private readonly EIMDBContext _context;
        public ObeAiRecognitionTrackingController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelRT : JTableModel
        {
            public string FaceId { get; set; }
            public string ObjName { get; set; }
            public string ImgPath { get; set; }
            public string DeviceId { get; set; }
            public DateTime CreatedTime { get; set; }
            public string LocationGps { get; set; }
        }
        [HttpPost]
        public object JTable([FromBody]JTableModelRT jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.ObeAiRecognitionTrackings
                            //where a.IsDeleted == false
                            //&& (string.IsNullOrEmpty(jTablePara.Driver) || a.Driver.ToLower().Contains(jTablePara.Driver.ToLower()))
                        select new ObeAiRecognitionTrackingJtableModel
                        {
                            Id = a.Id,
                            FaceId = a.FaceId,
                            ObjName = a.ObjName,
                            ImgPath = a.ImgPath,
                            DeviceId = a.DeviceId,
                            CheckPeople = a.CheckPeople,
                            CreatedTime = a.CreatedTime,
                            LocationGps = a.LocationGps
                        };
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "FaceId", "ObjName", "ImgPath", "DeviceId", "CreatedTime" ,"CheckPeople","LocationGps");
            return Json(jdata);
        }
        [HttpPost]
        public object GetItem([FromBody] int id)
        {
            var data = _context.ObeAiRecognitionTrackings.FirstOrDefault(x => x.Id == id);
            return data;
        }
    }
}