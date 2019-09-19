using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace III.Admin.Controllers
{
    public class EventCatJtableModel : JTableModel
    {
        public string MemberId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    [Area("Admin")]
    public class StaffRegistrationController : BaseController
    {
        public class EventCatUser
        {
            public string MemberId { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string AppointmentTime { get; set; }
            public bool FullTime { get; set; }
            public bool Morning { get; set; }
            public bool Afternoon { get; set; }
            public bool Evening { get; set; }
            public bool Sunday { get; set; }
            public bool Saturday { get; set; }
        }
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        public StaffRegistrationController(EIMDBContext context, IUploadService upload)
        {
            _context = context;
            _upload = upload;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region Calendar
        [HttpPost]
        public JsonResult InsertEventCat([FromBody]EventCatUser data)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var fromDate = DateTime.ParseExact(data.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var toDate = DateTime.ParseExact(data.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var appointmentTime = string.IsNullOrEmpty(data.AppointmentTime) ? DateTime.ParseExact(data.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;

                var employess = _context.HREmployees.FirstOrDefault(x => x.Id.ToString() == data.MemberId);
                if (employess != null)
                {
                    //insert,update event
                    var listEvent = _context.StaffScheduleWorks.Where(x => x.MemberId == data.MemberId);
                    if (listEvent.Any())
                    {
                        _context.StaffScheduleWorks.RemoveRange(listEvent);
                    }

                    for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
                    {

                        string frameTime = data.Morning.ToString() + ";" +
                                data.Afternoon.ToString() + ";" +
                                data.Evening.ToString();
                        var evt = new StaffScheduleWork
                        {
                            MemberId = data.MemberId,
                            FrameTime = frameTime,
                            CreatedBy = ESEIM.AppContext.UserName,
                            CreatedTime = DateTime.Now,
                            DatetimeEvent = date,
                        };
                        _context.StaffScheduleWorks.Add(evt);
                    }
                    _context.SaveChanges();
                    msg.Title = "Đăng ký thành công!";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Nhân viên không tồn tại!";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra đăng ký!";
            }
            return Json(msg);
        }

        [HttpGet]
        public object GetEventCat(string memberId)
        {
            var data = (from a in _context.StaffScheduleWorks
                        where a.MemberId == memberId
                        select new
                        {
                            a.Id,
                            a.DatetimeEvent,
                            //Morning = a.FrameTime.Split(';')[0],//Chua tuong thich
                            //Afternoon = a.FrameTime.Split(';')[1],
                            //Evening = a.FrameTime.Split(';')[2],
                        }).AsNoTracking();
            return data;
        }

        [HttpPost]
        public object GetEventCatGrid([FromBody]EventCatJtableModel searchData)
        {
            int intBegin = (searchData.CurrentPage - 1) * searchData.Length;
            var fromDate = string.IsNullOrEmpty(searchData.FromDate) ? (DateTime?)null : DateTime.ParseExact(searchData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var toDate = string.IsNullOrEmpty(searchData.ToDate) ? (DateTime?)null : DateTime.ParseExact(searchData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(searchData.MemberId))
            {
                var query = from a in _context.HREmployees
                            join b in _context.StaffScheduleWorks on a.Id.ToString() equals b.MemberId
                            where b.MemberId == searchData.MemberId
                            select new
                            {
                                a.fullname,
                                b.Id,
                                b.DatetimeEvent,
                                //Morning = b.FrameTime.Split(';')[0],//Chua tuong thich
                                //Afternoon = b.FrameTime.Split(';')[1],
                                //Evening = b.FrameTime.Split(';')[2]
                            };
                int count = query.Count();
                var data = query.OrderUsingSortExpression(searchData.QueryOrderBy).Skip(intBegin).Take(searchData.Length).AsNoTracking().ToList();
                var jdata = JTableHelper.JObjectTable(data, searchData.Draw, count, "fullname", "Id", "DatetimeEvent", "Morning", "Afternoon", "Evening");
                return Json(jdata);
            }
            else
            {
                var list = new List<object>();
                var jdata = JTableHelper.JObjectTable(list, searchData.Draw, 0, "GivenName", "Id", "DatetimeEvent", "Morning", "Afternoon", "Evening");
                return Json(jdata);
            }
        }

        [HttpPost]
        public JsonResult DeleteFrametime(int? id, int FRAME)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var data = _context.CandidateWorkEvents.FirstOrDefault(x => x.Id == id);
                if (data.DatetimeEvent.AddDays(1) < DateTime.Now)
                {
                    msg.Title = "Không thể thay đổi ngày đã qua!";
                    return Json(msg);
                }
                string[] frameTime = data.FrameTime.Split(';');
                frameTime[FRAME] = "False";
                data.FrameTime = frameTime[0] + ";" + frameTime[1] + ";" + frameTime[2]; /*+ ";" + frameTime[3];*/
                _context.CandidateWorkEvents.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Cập nhập thành công!";
                return Json(msg);

            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
            }
        }

        [HttpPost]
        public JsonResult ChangeFrametimeStatus(int id, int frame)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                frame = frame - 1;
                var data = _context.StaffScheduleWorks.FirstOrDefault(x => x.Id == id);
                if (data.DatetimeEvent.AddDays(1) < DateTime.Now)
                {
                    msg.Title = "Không thể thay đổi ngày đã qua!";
                    return Json(msg);
                }
                string[] frameTime = data.FrameTime.Split(';');
                if (frameTime[frame].Equals("True"))
                {
                    frameTime[frame] = "False";
                }
                else
                {
                    frameTime[frame] = "True";
                }
                data.FrameTime = frameTime[0].ToString() + ";" + frameTime[1].ToString() + ";" + frameTime[2];
                data.UpdatedTime = DateTime.Now;
                data.UpdatedBy = ESEIM.AppContext.UserName;
                _context.StaffScheduleWorks.Update(data);
                _context.SaveChanges();
                msg.Title = "Cập nhật thành công!";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
            }
            return Json(msg);
        }

        //[HttpPost]
        //public JsonResult GetGridToday([FromBody]JTableModel jtablePara)
        //{
        //    int intBegin = (jtablePara.CurrentPage - 1) * jtablePara.Length;
        //    var query = from a in _context.WORKUserEvent
        //                join b in _context.WORKEventCat
        //                on a.EventCatCode equals b.EventCatCode
        //                where b.DatetimeEvent == DateTime.Now.Date
        //                select new
        //                {
        //                    b.Id,
        //                    b.EventCatCode,
        //                    b.DatetimeEvent,
        //                    a.MemberId,
        //                    MorningPresent = a.Present.Split(';')[0],
        //                    AfternoonPresent = a.Present.Split(';')[1],
        //                    EveningPresent = a.Present.Split(';')[2],
        //                    Morning = b.FrameTime.Split(';')[0],
        //                    Afternoon = b.FrameTime.Split(';')[1],
        //                    Evening = b.FrameTime.Split(';')[2]
        //                };
        //    int count = query.Count();
        //    var data = query.OrderUsingSortExpression(jtablePara.QueryOrderBy).Skip(intBegin).Take(jtablePara.Length).AsNoTracking().ToList();
        //    var jdata = JTableHelper.JObjectTable(data, jtablePara.Draw, count, "Id", "Fullname", "MorningPresent", "AfternoonPresent", "EveningPresent", "EventCatCode", "DatetimeEvent", "MemberId", "Morning", "Afternoon", "Evening");
        //    return Json(jdata);

        //}

        [HttpPost]
        public object GetListEmployee()
        {
            var query = _context.HREmployees.Where(x => x.flag == 1).Select(x => new { x.Id, x.fullname }).AsNoTracking().ToList();
            return query;
        }
        #endregion
    }
}