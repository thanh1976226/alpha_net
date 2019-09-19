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
    [Area("Admin")]
    public class CalendarInterviewController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        public CalendarInterviewController(EIMDBContext context, IUploadService upload)
        {
            _context = context;
            _upload = upload;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetInterviewDate([FromBody]JtableModelInterview searchData)
        {
            var fromDate = string.IsNullOrEmpty(searchData.FromDate) ? (DateTime?)null : DateTime.ParseExact(searchData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var toDate = string.IsNullOrEmpty(searchData.ToDate) ? (DateTime?)null : DateTime.ParseExact(searchData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            try
            {
                var data = from a in _context.CandidateInterviews
                           join b in _context.CandiateBasic
                           on a.CandidateCode equals b.CandidateCode
                           where (string.IsNullOrEmpty(searchData.CandidateCode) || searchData.CandidateCode.Equals(a.CandidateCode))
                           && (fromDate == null || (a.InterviewDate.Date >= fromDate))
                           && (toDate == null || (a.InterviewDate.Date <= toDate))
                           select new
                           {
                               a.Id,
                               a.InterviewDate,
                               b.Fullname
                           };

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpPost]
        public JsonResult GetMemberCode(int MemberType)
        {
            if (MemberType == 0)
            {
                var data = _context.CandiateBasic.Where(x => x.Fullname != null).Select(x => new { code = x.CandidateCode, name = x.Fullname }).ToList();
                return Json(data);
            }
            if (MemberType == 1)
            {
                var data = _context.Users.Where(x => x.Active && x.UserName != "admin").Select(x => new { code = x.UserName, name = x.GivenName }).ToList();
                return Json(data);
            }
            if (MemberType == 2)
            {
                var data = _context.HREmployees.Select(x => new { code = x.Id, name = x.fullname }).ToList();
                return Json(data);
            }

            var msg = new JMessage()
            {
                Error = true,
                Title = "Member type error!"
            };
            return Json(msg);
        }

        [HttpPost]
        public JsonResult SetInterviewDate([FromBody]Interview data)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                if (data == null)
                {
                    msg.Title = "Chọn ngày phỏng vấn!";
                    return Json(msg);
                }

                if (string.IsNullOrEmpty(data.CandidateCode))
                {
                    msg.Title = "Chọn mã candidate!";
                    return Json(msg);
                }


                var query = _context.CandidateInterviews.Where(x => x.CandidateCode.Equals(data.CandidateCode));
                if (query.Count() > 0)
                {
                    var a = _context.CandidateInterviews.FirstOrDefault(x => x.CandidateCode.Equals(data.CandidateCode));
                    a.InterviewDate = data.InterviewDate;
                    _context.CandidateInterviews.Update(a);
                    _context.SaveChanges();

                    msg.Error = false;
                    msg.Title = "Cập nhật lịch thành công";

                    return Json(msg);
                }

                CandidateInterview obj = new CandidateInterview()
                {
                    CandidateCode = data.CandidateCode,
                    //InterviewDate = DateTime.ParseExact(data.InterviewDate.Replace(' ', 'T'), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                    InterviewDate = data.InterviewDate
                };

                _context.CandidateInterviews.Add(obj);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Cập nhật thành công!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi xảy ra!";
                return Json(msg);
                throw;
            }
        }
    }
    public class Interview
    {
        public string CandidateCode { get; set; }
        public DateTime InterviewDate { get; set; }
    }
    public class JtableModelInterview : JTableModel
    {
        public string CandidateCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}