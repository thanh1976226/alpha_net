using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ESEIM.Models;
using ESEIM.Utils;
using III.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class StaffSalaryController : Controller
    {

        private readonly EIMDBContext _context;
        public StaffSalaryController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult CalculateSalary(string fromDate, string toDate)
        {
            var msg = new JMessage { Error = false, Title = "" };
            var timeWorking = new TimeSpan(8, 30, 0);
            var freeTime = new TimeSpan(1, 30, 0);
            var listSalary = new  List<SalaryUserModel>();
            var timeWork = 0.0;
            var from = !string.IsNullOrEmpty(fromDate) ? DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var to = !string.IsNullOrEmpty(toDate) ? DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            if (from != null && to != null)
            {
                var list = _context.StaffTimetableWorkings.Where(x => x.ActionTime >= from && x.ActionTime <= to);
                if (list.Any())
                {
                    var listUser = list.GroupBy(x => x.UserId).Select(x => x.Key);
                    foreach (var user in listUser)
                    {
                        var listForUser = list.Where(x => x.UserId == user);

                        var listGoLateForUser = listForUser.Where(x => x.Action == StaffStauts.CheckIn.DescriptionAttr() && x.Session == 1 && x.ActionTime.TimeOfDay < timeWorking);
                        var listNoWorking = listForUser.Where(x => x.Action == StaffStauts.NoWork.DescriptionAttr());
                        var dayNoWork = Convert.ToInt32(listNoWorking.Where(x => x.ActionTo.HasValue).Select(x => x.ActionTo.Value.Subtract(x.ActionTime).Days).First());
                        var listCheckInCheckOut = listForUser.Where(x => x.Action != StaffStauts.NoWork.DescriptionAttr());
                        if (listCheckInCheckOut.Any())
                        {
                            var listSession = listCheckInCheckOut.GroupBy(x => x.Session).Select(x => x.Key);
                            foreach (var item in listSession)
                            {
                                var session = listCheckInCheckOut.Where(x => x.Session == item);
                                if (session.Any())
                                {
                                    var checkOut = session.FirstOrDefault(x => x.Action == StaffStauts.CheckOut.DescriptionAttr()).ActionTime;
                                    var checkIn = session.FirstOrDefault(x => x.Action == StaffStauts.CheckIn.DescriptionAttr()).ActionTime;
                                    timeWork = timeWork + ((checkOut.Subtract(checkIn)).TotalMinutes);
                                }
                            }
                        }
                        var model = new SalaryUserModel
                        {
                            UserName = _context.Users.FirstOrDefault(x => x.Id == listForUser.First().UserId)?.GivenName,
                            NumberLate = listGoLateForUser.Count(),
                            TotalTimeLate = listGoLateForUser.Select(x => new
                            {
                                TimeLate = x.ActionTime.Subtract(timeWorking).TimeOfDay.TotalMinutes,
                            }).Sum(x => x.TimeLate),
                            NumberNoWork = dayNoWork,
                            TimeNoWork = (dayNoWork * 7.5),
                            NumberMinutesWork = timeWork - freeTime.TotalMinutes,
                            NumberHourseWork = Math.Round((timeWork - freeTime.TotalMinutes) / 60, 2),
                            NumberDayWork = Math.Round(((timeWork - freeTime.TotalMinutes) / 60) / 7.5, 2),
                        };
                        listSalary.Add(model);
                    }
                    msg.Object = listSalary;
                }
            }
            return Json(msg);
        }


        public class SalaryUserModel
        {
            public string UserName { get; set; }
            public int NumberLate { get; set; }
            public double TotalTimeLate { get; set; }
            public int NumberNoWork { get; set; }
            public double TimeNoWork { get; set; }
            public double NumberMinutesWork { get; set; }
            public double NumberHourseWork { get; set; }
            public double NumberDayWork { get; set; }
        }
    }
}