using System;
using System.Globalization;
using System.Linq;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace III.Admin.Controllers
{
    public class SStaffTimeKeepingModel
    {
        public int? Id { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string ActionTime { get; set; }
        public string ActionTo { get; set; }
        public string LocationGps { get; set; }
        public string Ip { get; set; }
        public string Note { get; set; }
    }
    [Area("Admin")]
    public class StaffTimeKeepingController : BaseController
    {
        public class SStaffTimeKeepingJtableModel : JTableModel
        {
            public string Name { get; set; }
        }
        private readonly EIMDBContext _context;
        public StaffTimeKeepingController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public object JTable([FromBody]SStaffTimeKeepingJtableModel jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var today = DateTime.Today;
            var query = (from a in _context.StaffTimetableWorkings
                         join b in _context.Users on a.UserId equals b.Id
                         where (a.Action == "CHECKIN" || a.Action == "CHECKOUT")
                         && (string.IsNullOrEmpty(jTablePara.Name) || b.GivenName.ToLower().Contains(jTablePara.Name.ToLower()))
                         select new
                         {
                             a.Id,
                             FullName = b.GivenName,
                             b.Gender,
                             a.Action,
                             a.ActionTime,
                             a.ActionTo,
                             a.Note,
                         }).AsNoTracking().Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var count = (from a in _context.StaffTimetableWorkings
                         join b in _context.Users on a.UserId equals b.Id
                         where (a.Action == "CHECKIN" || a.Action == "CHECKOUT")
                         && (string.IsNullOrEmpty(jTablePara.Name) || b.GivenName.ToLower().Contains(jTablePara.Name.ToLower()))
                         select new
                         {
                             a
                         }).AsNoTracking().Count();
            var jdata = JTableHelper.JObjectTable(query, jTablePara.Draw, count, "Id", "FullName", "Gender", "Action", "ActionTime", "ActionTo", "Note");
            return Json(jdata);
        }



        [HttpPost]
        public object GetListEmployee()
        {
            var query = _context.Users.Where(x => x.Active).Select(x => new { x.Id, x.GivenName }).AsNoTracking().ToList();
            return query;
        }
        //[HttpPost]
        //public object GetItem([FromBody]int id)
        //{
        //    var msg = new JMessage() { Error = false, Title = "" };
        //    var data = _context.StaffTimetableWorkings.FirstOrDefault(x => x.Id == id);
        //    if (data != null)
        //    {
        //        var model = new SStaffTimeKeepingModel
        //        {
        //            Id = id,
        //            UserId = data.UserId,
        //            Action = data.Action,
        //            ActionTime = data.ActionTime.ToString("dd/MM/yyyy"),
        //            ActionTo = data.ActionTo.HasValue ? data.ActionTo.Value.ToString("dd/MM/yyyy") : null,
        //            Note = data.Note,
        //        };
        //        msg.Object = model;
        //    }
        //    else
        //    {
        //        msg.Error = true;
        //        msg.Title = "Không tồn tại dữ liệu";
        //    }
        //    return Json(msg);
        //}

        [HttpPost]
        public JsonResult CheckIn([FromBody]SStaffTimeKeepingModel obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var model = new StaffTimetableWorking
                {
                    UserId = obj.UserId,
                    Action = "CHECKIN",
                    ActionTime = DateTime.Now,
                    Note = obj.Note,
                };
                _context.StaffTimetableWorkings.Add(model);
                _context.SaveChanges();
                msg.Title = "Checkin thành công!";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Checkin lỗi!";
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult CheckOut([FromBody]SStaffTimeKeepingModel obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var model = new StaffTimetableWorking
                {
                    UserId = obj.UserId,
                    Action = "CHECKOUT",
                    ActionTime = DateTime.Now,
                    Note = obj.Note,
                };
                _context.StaffTimetableWorkings.Add(model);
                _context.SaveChanges();
                msg.Title = "Checkout thành công!";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Checkout lỗi!";
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Delete([FromBody]int id)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var data = _context.StaffTimetableWorkings.FirstOrDefault(x => x.Id == id);
                if (data != null)
                {
                    _context.StaffTimetableWorkings.Remove(data);
                }
                _context.SaveChanges();
                msg.Title = "Xóa chấm công thành công!";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Xóa chấm công lỗi!";
            }
            return Json(msg);
        }

    }
}