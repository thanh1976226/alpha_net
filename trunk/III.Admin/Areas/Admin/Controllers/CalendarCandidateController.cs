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
    public class CalendarCandidateController : BaseController
    {
        public class EDMSCalendarCandidateSearchModel
        {
            public string MemberId { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        public CalendarCandidateController(EIMDBContext context, IUploadService upload)
        {
            _context = context;
            _upload = upload;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public object GetListCandidate()
        {
            var query = _context.CandiateBasic.Select(x => new { x.CandidateCode, x.Fullname }).AsNoTracking().ToList();
            return query;
        }

        [HttpPost]
        public object GetEventCat([FromBody]EDMSCalendarCandidateSearchModel obj)
        {
            var fromDate = string.IsNullOrEmpty(obj.FromDate) ? (DateTime?)null : DateTime.ParseExact(obj.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var toDate = string.IsNullOrEmpty(obj.ToDate) ? (DateTime?)null : DateTime.ParseExact(obj.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = (from a in _context.CandidateWorkEvents
                        join b in _context.CandiateBasic on a.CandidateCode equals b.CandidateCode
                        where a.DatetimeEvent.Date >= fromDate && a.DatetimeEvent.Date <= toDate
                        && (string.IsNullOrEmpty(obj.MemberId) || b.CandidateCode == obj.MemberId)
                        select new
                        {
                            a.Id,
                            a.DatetimeEvent,
                            b.Fullname,
                        }).AsNoTracking();
            return data;
        }
    }

}