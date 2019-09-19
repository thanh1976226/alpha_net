﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace III.Admin.Controllers
{
    public class Candidate
    {
        public CandidateInfo CandidatesInfo { get; set; }
        public CandidateInfoMore CandidatesInfoMore { get; set; }
    }
    public class CandidateInfo
    {
        public string CandidateCode { get; set; }
        public string Fullname { get; set; }
        public string Sex { get; set; }
        public string Married { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }
        public string Skype { get; set; }
        public string FileCv_1 { get; set; }
        public string Targeting { get; set; }
    }
    public class CandidateInfoMore
    {
        public string CandidateCode { get; set; }
        public string Ability { get; set; }
        public string CanJoinDate { get; set; }
        public string MainSkill { get; set; }
        public string MainPracticeTime { get; set; }
        public string SalaryHope { get; set; }
        public string SubSkill { get; set; }
        public string SubPracticeTime { get; set; }
        public string LanguageUse { get; set; }
        public string Targeting { get; set; }
        public string FileCv_1 { get; set; }
        public string LaptopInfo { get; set; }
        public string SmartphoneInfo { get; set; }
        public string WorkPlace { get; set; }
    }
    public class EventCat
    {
        public string CandidateCode { get; set; }
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

    public class EventCatCalender
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime? start { get; set; }
    }
    public class SearchModel : JTableModel
    {
        public string CandidateCode { get; set; }
        public int? Membertype { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int Worktype { get; set; }
    }
    public class CalendarRegistrationController : Controller
    {
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        private readonly IHostingEnvironment _hostingEnvironment;
        public CalendarRegistrationController(EIMDBContext context, IUploadService upload, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _upload = upload;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }


        #region Candidate_Info , Candidate_Info_More
        [HttpGet]
        public JsonResult CreateCandiateCode()
        {
            var msg = new JMessage() { Error = true };
            try
            {
                string code = "CAN_" + DateTime.Now.ToString("ddMMyyyy", CultureInfo.InvariantCulture) +
                              "_" + (_context.CandiateBasic.Count() > 0 ? (_context.CandiateBasic.OrderByDescending(x => x.Id).First().Id + 1).ToString() : 0.ToString());
                msg.Title = code;
                msg.Error = false;
                var candidate = new CandidateBasic()
                {
                    CandidateCode = code,
                    CreatedTime = DateTime.Now,
                    //CreatedBy = ESEIM.AppContext.UserName
                };
                _context.CandiateBasic.Add(candidate);
                _context.SaveChanges();
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi khi tạo mã!";
                return Json(msg);
            }
        }

        [HttpGet]
        public JsonResult SearchCandiateCode(string candidateCode)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var data = _context.CandiateBasic.FirstOrDefault(x => x.CandidateCode == candidateCode);
                if (data != null)
                {
                    var model = new Candidate();
                    model.CandidatesInfo = new CandidateInfo
                    {
                        CandidateCode = data.CandidateCode,
                        Address = data.Address,
                        Birthday = data.Birthday.HasValue ? data.Birthday.Value.ToString("dd/MM/yyyy") : null,
                        Email = data.Email,
                        Sex = data.Sex.ToString(),
                        Fullname = data.Fullname,
                        Skype = data.Skype,
                        Phone = data.Phone,
                        Married = data.Married ? "1" : "0",
                        FileCv_1 = data.FileCv_1,
                        Targeting = data.Targeting,
                    };
                    model.CandidatesInfoMore = new CandidateInfoMore
                    {
                        CandidateCode = data.CandidateCode,
                        MainSkill = data.MainSkill,
                        SubSkill = data.SubSkill,
                        WorkPlace = data.WorkPlace,
                        LaptopInfo = data.LaptopInfo,
                        SmartphoneInfo = data.SmartphoneInfo,
                        Ability = data.Ability,
                        SalaryHope = data.SalaryHope.ToString(),
                        CanJoinDate = data.CanJoinDate.ToString()
                    };
                    msg.Object = model;
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Ứng viên không tồn tại!";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi khi tìm ứng viên!";
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult UpdateCandidateInfo([FromBody]CandidateInfo data)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                #region check validate
                if (string.IsNullOrEmpty(data.CandidateCode))
                {
                    msg.Error = true;
                    msg.Title = "Bạn chưa lấy mã đăng ký trước!";
                    return Json(msg);
                }

                if (string.IsNullOrEmpty(data.Fullname))
                {
                    msg.Error = true;
                    msg.Title = "Nhập họ tên!";
                    return Json(msg);
                }
                else
                {
                    if (data.Fullname.Length > 255)
                    {
                        msg.Error = true;
                        msg.Title = "Tên tối đa 255 ký tự!";
                        return Json(msg);
                    }
                }

                if (string.IsNullOrEmpty(data.Phone))
                {
                    msg.Error = true;
                    msg.Title = "Nhập số điện thoại!";
                    return Json(msg);
                }
                else
                {
                    if (data.Phone.Length > 50)
                    {
                        msg.Error = true;
                        msg.Title = "Số điện thoại chỉ tối đa 50 ký tự!";
                        return Json(msg);
                    }
                }

                if (string.IsNullOrEmpty(data.Email))
                {
                    msg.Error = true;
                    msg.Title = "Nhập email!";
                    return Json(msg);
                }
                else
                {
                    if (data.Email.Length > 100)
                    {
                        msg.Error = true;
                        msg.Title = "Email chỉ tối đa 100 ký tự!";
                        return Json(msg);
                    }
                }

                if (!string.IsNullOrEmpty(data.Address))
                {
                    if (data.Address.Length > 255)
                    {
                        msg.Error = true;
                        msg.Title = "Địa chỉ tối đa 255 ký tự!";
                        return Json(msg);
                    }
                }

                if (!string.IsNullOrEmpty(data.Skype))
                {
                    if (data.Skype.Length > 255)
                    {
                        msg.Error = true;
                        msg.Title = "Skype chỉ tối đa 255 ký tự!";
                        return Json(msg);
                    }
                }

                //if (!string.IsNullOrEmpty(data.MainSkill))
                //{
                //    if (data.MainSkill.Length > 255)
                //    {
                //        msg.Title = "MainSkill chỉ tối đa 255 ký tự!";
                //        return Json(msg);
                //    }
                //}

                //if (!string.IsNullOrEmpty(data.SubSkill))
                //{
                //    if (data.SubSkill.Length > 255)
                //    {
                //        msg.Title = "SubSkill chỉ tối đa 255 ký tự!";
                //        return Json(msg);
                //    }
                //}

                //if (!string.IsNullOrEmpty(data.LanguageUse))
                //{
                //    if (data.LanguageUse.Length > 255)
                //    {
                //        msg.Title = "Ngôn ngữ chỉ tối đa 255 ký tự!";
                //        return Json(msg);
                //    }
                //}

                //if (!string.IsNullOrEmpty(data.Targeting))
                //{
                //    if (data.Targeting.Length > 500)
                //    {
                //        msg.Title = "Mục tiêu chỉ tối đa 500 ký tự!";
                //        return Json(msg);
                //    }
                //}

                if (!string.IsNullOrEmpty(data.FileCv_1))
                {
                    if (data.FileCv_1.Length > 255)
                    {
                        msg.Error = true;
                        msg.Title = "Tên file quá dài!";
                        return Json(msg);
                    }
                }

                Regex regex1 = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                Match match = regex1.Match(data.Email);
                if (!match.Success)
                {
                    msg.Error = true;
                    msg.Title = "Email không hợp lệ!";
                    return Json(msg);
                }
                #endregion

                var can = _context.CandiateBasic.FirstOrDefault(x => x.CandidateCode.Equals(data.CandidateCode));

                can.Fullname = data.Fullname;
                //can.Ability = string.IsNullOrEmpty(data.Ability) ? "" : data.Ability.Remove(data.Ability.Length - 1, 1);
                can.Address = data.Address;
                can.Birthday = string.IsNullOrEmpty(data.Birthday) ? (DateTime?)null : DateTime.ParseExact(data.Birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (can.Birthday != null)
                {
                    decimal age = (decimal)(DateTime.Now - can.Birthday).Value.TotalDays / 365.25m;
                    if (age < 18)
                    {
                        msg.Error = true;
                        msg.Title = "Bạn chưa đủ 18 tuổi!";
                        return Json(msg);
                    }
                }

                //can.CanJoinDate = string.IsNullOrEmpty(data.CanJoinDate) ? (DateTime?)null : DateTime.ParseExact(data.CanJoinDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                can.Email = data.Email;
                //can.MainPracticeTime = data.MainPracticeTime;
                can.Married = data.Married == "0" ? false : true;
                can.Phone = data.Phone;
                //can.SalaryHope = string.IsNullOrEmpty(data.SalaryHope) ? 0 : decimal.Parse(data.SalaryHope);
                can.Sex = int.Parse(data.Sex);
                can.Skype = data.Skype;
                //can.MainSkill = data.MainSkill;
                //can.SubSkill = data.SubSkill;
                //can.LanguageUse = data.LanguageUse;
                //can.SubPracticeTime = data.SubPracticeTime;
                can.Targeting = data.Targeting;
                can.FileCv_1 = data.FileCv_1;
                can.UpdatedTime = DateTime.Now;
                //can.LaptopInfo = data.LaptopInfo;
                //can.SmartphoneInfo = data.SmartphoneInfo;
                //can.WorkPlace = data.WorkPlace;
                _context.CandiateBasic.Update(can);
                _context.SaveChanges();
                msg.Title = "Lưu lại thông tin thành công!";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra!";
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult UpdateCandidateInfoMore([FromBody]CandidateInfoMore data)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                #region check validate
                if (string.IsNullOrEmpty(data.CandidateCode))
                {
                    msg.Error = true;
                    msg.Title = "Bạn chưa lấy mã đăng ký!";
                    return Json(msg);
                }

                //if (string.IsNullOrEmpty(data.Fullname))
                //{
                //    msg.Title = "Nhập họ tên!";
                //    return Json(msg);
                //}
                //else
                //{
                //    if (data.Fullname.Length > 255)
                //    {
                //        msg.Title = "Tên tối đa 255 ký tự!";
                //        return Json(msg);
                //    }
                //}

                //if (string.IsNullOrEmpty(data.Phone))
                //{
                //    msg.Title = "Nhập số điện thoại!";
                //    return Json(msg);
                //}
                //else
                //{
                //    if (data.Phone.Length > 50)
                //    {
                //        msg.Title = "Số điện thoại chỉ tối đa 50 ký tự!";
                //        return Json(msg);
                //    }
                //}

                //if (string.IsNullOrEmpty(data.Email))
                //{
                //    msg.Title = "Nhập email!";
                //    return Json(msg);
                //}
                //else
                //{
                //    if (data.Email.Length > 100)
                //    {
                //        msg.Title = "Email chỉ tối đa 100 ký tự!";
                //        return Json(msg);
                //    }
                //}

                //if (!string.IsNullOrEmpty(data.Address))
                //{
                //    if (data.Address.Length > 255)
                //    {
                //        msg.Title = "Địa chỉ tối đa 255 ký tự!";
                //        return Json(msg);
                //    }
                //}

                //if (!string.IsNullOrEmpty(data.Skype))
                //{
                //    if (data.Skype.Length > 255)
                //    {
                //        msg.Title = "Skype chỉ tối đa 255 ký tự!";
                //        return Json(msg);
                //    }
                //}

                if (!string.IsNullOrEmpty(data.MainSkill))
                {
                    if (data.MainSkill.Length > 255)
                    {
                        msg.Title = "MainSkill chỉ tối đa 255 ký tự!";
                        return Json(msg);
                    }
                }

                if (!string.IsNullOrEmpty(data.SubSkill))
                {
                    if (data.SubSkill.Length > 255)
                    {
                        msg.Error = true;
                        msg.Title = "SubSkill chỉ tối đa 255 ký tự!";
                        return Json(msg);
                    }
                }

                if (!string.IsNullOrEmpty(data.LanguageUse))
                {
                    if (data.LanguageUse.Length > 255)
                    {
                        msg.Error = true;
                        msg.Title = "Ngôn ngữ chỉ tối đa 255 ký tự!";
                        return Json(msg);
                    }
                }

                if (!string.IsNullOrEmpty(data.Targeting))
                {
                    if (data.Targeting.Length > 500)
                    {
                        msg.Error = true;
                        msg.Title = "Mục tiêu chỉ tối đa 500 ký tự!";
                        return Json(msg);
                    }
                }

                if (!string.IsNullOrEmpty(data.FileCv_1))
                {
                    if (data.FileCv_1.Length > 255)
                    {
                        msg.Title = "Tên file quá dài!";
                        return Json(msg);
                    }
                }
                #endregion

                var can = _context.CandiateBasic.FirstOrDefault(x => x.CandidateCode.Equals(data.CandidateCode));
                can.Ability = string.IsNullOrEmpty(data.Ability) ? "" : data.Ability.Remove(data.Ability.Length - 1, 1);
                can.CanJoinDate = string.IsNullOrEmpty(data.CanJoinDate) ? (DateTime?)null : DateTime.ParseExact(data.CanJoinDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                can.MainPracticeTime = data.MainPracticeTime;
                can.SalaryHope = string.IsNullOrEmpty(data.SalaryHope) ? 0 : decimal.Parse(data.SalaryHope);
                can.MainSkill = data.MainSkill;
                can.SubSkill = data.SubSkill;
                can.LanguageUse = data.LanguageUse;
                can.SubPracticeTime = data.SubPracticeTime;
                can.Targeting = data.Targeting;
                can.FileCv_1 = data.FileCv_1;
                can.UpdatedTime = DateTime.Now;
                can.LaptopInfo = data.LaptopInfo;
                can.SmartphoneInfo = data.SmartphoneInfo;
                can.WorkPlace = data.WorkPlace;
                _context.CandiateBasic.Update(can);
                _context.SaveChanges();
                msg.Title = "Lưu lại thông tin thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra!" + ex;
            }
            return Json(msg);
        }

        [HttpPost]
        public object UploadFile(IFormFile fileUpload)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var upload = _upload.UploadFile(fileUpload, Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\files"));
                if (upload.Error)
                {
                    msg.Error = true;
                    msg.Title = upload.Title;
                }
                else
                {
                    msg.Object = upload.Object;
                }

            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi upload file!";
            }
            return Json(msg);
        }
        #endregion

        #region Calendar
        [HttpPost]
        public JsonResult InsertEventCat([FromBody]EventCat data)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var fromDate = DateTime.ParseExact(data.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var toDate = DateTime.ParseExact(data.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var appointmentTime = string.IsNullOrEmpty(data.AppointmentTime) ? DateTime.ParseExact(data.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;

                var candidate = _context.CandiateBasic.FirstOrDefault(x => x.CandidateCode == data.CandidateCode);
                if (candidate != null)
                {
                    //update candidate
                    candidate.WorkFrom = fromDate;
                    candidate.WorkTo = toDate;
                    candidate.AppointmentTime = appointmentTime;
                    _context.CandiateBasic.Update(candidate);


                    //insert,update event
                    var listEvent = _context.CandidateWorkEvents.Where(x => x.CandidateCode == data.CandidateCode);
                    if (listEvent.Any())
                    {
                        _context.CandidateWorkEvents.RemoveRange(listEvent);
                    }

                    for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
                    {

                        string frameTime = data.Morning.ToString() + ";" +
                                data.Afternoon.ToString() + ";" +
                                data.Evening.ToString();
                        var evt = new CandidateWorkEvent
                        {
                            CandidateCode = data.CandidateCode,
                            FrameTime = frameTime,
                            CreatedTime = DateTime.Now,
                            DatetimeEvent = date,
                        };
                        _context.CandidateWorkEvents.Add(evt);
                    }
                    _context.SaveChanges();
                    msg.Title = "Đăng ký thành công!";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Ứng viên không tồn tại!";
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
        public object GetEventCat(string candidateCode)
        {
            var query = (from a in _context.CandidateWorkEvents
                         where a.CandidateCode == candidateCode
                         select new
                         {
                             a.Id,
                             a.DatetimeEvent,
                             a.FrameTime,
                         }).AsNoTracking();
            return query;
        }

        [HttpPost]
        public object GetEventCatGrid([FromBody]SearchModel searchData)
        {
            int intBegin = (searchData.CurrentPage - 1) * searchData.Length;
            var fromDate = string.IsNullOrEmpty(searchData.FromDate) ? (DateTime?)null : DateTime.ParseExact(searchData.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var toDate = string.IsNullOrEmpty(searchData.ToDate) ? (DateTime?)null : DateTime.ParseExact(searchData.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(searchData.CandidateCode))
            {
                var query = from a in _context.CandiateBasic
                            join b in _context.CandidateWorkEvents on a.CandidateCode equals b.CandidateCode
                            where a.CandidateCode == searchData.CandidateCode
                            select new
                            {

                                a.CandidateCode,
                                a.Fullname,
                                b.Id,
                                b.DatetimeEvent,
                                //Morning = b.FrameTime.Split(';')[0], //Chua tuong thich
                                //Afternoon = b.FrameTime.Split(';')[1],
                                //Evening = b.FrameTime.Split(';')[2]
                            };
                int count = query.Count();
                var data = query.OrderUsingSortExpression(searchData.QueryOrderBy).Skip(intBegin).Take(searchData.Length).AsNoTracking().ToList();
                var jdata = JTableHelper.JObjectTable(data, searchData.Draw, count, "CandidateCode", "Fullname", "Id", "DatetimeEvent", "Morning", "Afternoon", "Evening");
                return Json(jdata);
            }
            else
            {
                var list = new List<object>();
                var jdata = JTableHelper.JObjectTable(list, searchData.Draw, 0, "CandidateCode", "Fullname", "Id", "DatetimeEvent", "Morning", "Afternoon", "Evening");
                return Json(jdata);
            }
            //string memberType = "";
            //switch (searchData.Membertype)
            //{
            //    case 0:
            //        memberType = "CAN";
            //        break;
            //    case 1:
            //        memberType = "USR";
            //        break;
            //    case 2:
            //        memberType = "EPL";
            //        break;
            //}

            //try
            //{
            //var query = from a in _context.WORKUserEvent
            //            join b in _context.WORKEventCat on a.EventCatCode equals b.EventCatCode
            //            where a.MemberId.Equals(memberType + "_" + searchData.MemberCode) &&
            //                  ((fromDate == null || (b.DatetimeEvent >= fromDate)) &&
            //                   (toDate == null || (b.DatetimeEvent <= toDate))) &&
            //                  (searchData.Worktype == -1 ? true : (searchData.Worktype == 0 ? (b.FrameTime.Split(';')[0].Equals("True") &&
            //                                                                                  b.FrameTime.Split(';')[1].Equals("True")) : b.FrameTime.Split(';')[searchData.Worktype - 1].Equals("True"))
            //                  )
            //            select new
            //            {
            //                b.Id,
            //                b.EventCatCode,
            //                b.DatetimeEvent,
            //                a.MemberId,
            //                Fullname = GetMemberName(searchData.Membertype, searchData.MemberCode),
            //                MorningPresent = a.Present.Split(';')[0],
            //                AfternoonPresent = a.Present.Split(';')[1],
            //                EveningPresent = a.Present.Split(';')[2],
            //                Morning = b.FrameTime.Split(';')[0],
            //                Afternoon = b.FrameTime.Split(';')[1],
            //                Evening = b.FrameTime.Split(';')[2]
            //            };
            //int count = query.Count();
            //var data = query.OrderUsingSortExpression(searchData.QueryOrderBy).Skip(intBegin).Take(searchData.Length).AsNoTracking().ToList();
            //var jdata = JTableHelper.JObjectTable(data, searchData.Draw, count, "Id", "Fullname", "MorningPresent", "AfternoonPresent", "EveningPresent", "EventCatCode", "DatetimeEvent", "MemberId", "Morning", "Afternoon", "Evening");
            //return Json(jdata);



            //}
            //catch (Exception ex)
            //{
            //    return Json(ex);
            //}
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
                var data = _context.CandidateWorkEvents.FirstOrDefault(x => x.Id == id);
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
                _context.CandidateWorkEvents.Update(data);
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
        #endregion

        #region SetLanguage
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        #endregion
    }
}