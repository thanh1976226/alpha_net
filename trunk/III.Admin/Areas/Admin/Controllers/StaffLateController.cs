using System;
using System.Globalization;
using System.Linq;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class StaffLateController : BaseController
    {
        public class SStaffTimeKeepingJtableModel : JTableModel
        {
            public string Name { get; set; }
        }
        private readonly EIMDBContext _context;
        public StaffLateController(EIMDBContext context)
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
                         where (a.Action != "CHECKIN" && a.Action != "CHECKOUT") &&
                         (((a.Action == "NOTWORK" && a.ActionTime.Date <= today) && (a.ActionTo.HasValue && a.ActionTo.Value.Date >= today)) || (a.ActionTime.Date == today))
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
                         join b in _context.HREmployees on a.UserId equals b.Id.ToString()
                         where ((a.Action == "NOTWORK" && a.ActionTime.Date <= today && (a.ActionTo.HasValue && a.ActionTo.Value.Date >= today)) || (a.ActionTime.Date == today))
                         select new
                         {
                             a
                         }).AsNoTracking().Count();
            var jdata = JTableHelper.JObjectTable(query, jTablePara.Draw, count, "Id", "FullName", "Gender", "Action", "ActionTime", "ActionTo", "Note");
            return Json(jdata);
        }

        [HttpPost]
        public object GetItem([FromBody]int id)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            var data = _context.StaffTimetableWorkings.FirstOrDefault(x => x.Id == id);
            if (data != null)
            {
                var model = new SStaffTimeKeepingModel
                {
                    Id = id,
                    UserId = data.UserId,
                    Action = data.Action,
                    ActionTime = data.ActionTime.ToString("dd/MM/yyyy"),
                    ActionTo = data.ActionTo.HasValue ? data.ActionTo.Value.ToString("dd/MM/yyyy") : null,
                    Note = data.Note,
                };
                msg.Object = model;
            }
            else
            {
                msg.Error = true;
                msg.Title = "Không tồn tại dữ liệu";
            }
            return Json(msg);
        }

        [HttpPost]
        public object GetListEmployee()
        {
            var query = _context.Users.Where(x => x.Active).Select(x => new { x.Id, x.GivenName }).AsNoTracking().ToList();
            return query;
        }

        [HttpPost]
        public JsonResult Add(SStaffTimeKeepingModel obj, IFormFile picture)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                if (obj.Action != "NOTWORK")
                {
                    var model = new StaffTimetableWorking
                    {
                        UserId = obj.UserId,
                        Action = obj.Action,
                        ActionTime = !string.IsNullOrEmpty(obj.ActionTime) ? DateTime.ParseExact(obj.ActionTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now,
                        Note = obj.Note,
                    };
                    _context.StaffTimetableWorkings.Add(model);
                }
                else
                {
                    var model = new StaffTimetableWorking
                    {
                        UserId = obj.UserId,
                        Action = obj.Action,
                        ActionTime = !string.IsNullOrEmpty(obj.ActionTime) ? DateTime.ParseExact(obj.ActionTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now,
                        ActionTo = !string.IsNullOrEmpty(obj.ActionTo) ? DateTime.ParseExact(obj.ActionTo, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                        Note = obj.Note,
                    };
                    _context.StaffTimetableWorkings.Add(model);
                }
                _context.SaveChanges();
                msg.Title = "Chấm công thành công!";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Chấm công lỗi!";
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Update(SStaffTimeKeepingModel obj, IFormFile picture)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                if (obj.Action != "NOTWORK")
                {
                    var data = _context.StaffTimetableWorkings.FirstOrDefault(x => x.Id == obj.Id);
                    if (data != null)
                    {
                        data.UserId = obj.UserId;
                        data.Action = obj.Action;
                        data.ActionTime = !string.IsNullOrEmpty(obj.ActionTime) ? DateTime.ParseExact(obj.ActionTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;
                        data.Note = obj.Note;
                    }
                    _context.StaffTimetableWorkings.Update(data);
                }
                else
                {
                    var data = _context.StaffTimetableWorkings.FirstOrDefault(x => x.Id == obj.Id);
                    if (data != null)
                    {
                        data.UserId = obj.UserId;
                        data.Action = obj.Action;
                        data.ActionTime = !string.IsNullOrEmpty(obj.ActionTime) ? DateTime.ParseExact(obj.ActionTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;
                        data.ActionTo = !string.IsNullOrEmpty(obj.ActionTo) ? DateTime.ParseExact(obj.ActionTo, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;
                        data.Note = obj.Note;
                    }
                    _context.StaffTimetableWorkings.Update(data);
                }
                _context.SaveChanges();
                msg.Title = "Cập nhập chấm công thành công!";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Cập nhập chấm công lỗi!";
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