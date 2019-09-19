using System;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class CandidatesManagementController : BaseController
    {
        public class EDMSCandidatesManagementJtableModel : JTableModel
        {
            public string CandidateCode { get; set; }
        }
        public class EDMSCandidatesManagementJtable : JTableModel
        {
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CandidatesManagementController(EIMDBContext context, IUploadService upload, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _upload = upload;
            _hostingEnvironment = hostingEnvironment;
        }

        #region index
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public object JTable([FromBody]EDMSCandidatesManagementJtable jTablePara)
        {
            var intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var fromDate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toDate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.CandiateBasic
                        where ((fromDate == null) || (a.CreatedTime.Date >= fromDate.Value.Date))
                            && ((toDate == null) || (a.CreatedTime.Date <= toDate.Value.Date))
                        select new
                        {
                            a.Id,
                            a.CandidateCode,
                            a.Fullname,
                            a.Sex,
                            a.Birthday,
                            a.Phone,
                            a.CreatedTime,
                        };

            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "Id", "CandidateCode", "Fullname", "Sex", "Birthday", "Phone", "CreatedTime");
            return Json(jdata);
        }


        [HttpPost]
        public JsonResult GetItem([FromBody] int id)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var data = _context.CandiateBasic.FirstOrDefault(x => x.Id == id);
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
                    };
                    model.CandidatesInfoMore = new CandidateInfoMore
                    {
                        CandidateCode = data.CandidateCode,
                        MainSkill = data.MainSkill,
                        SubSkill = data.SubSkill,
                        MainPracticeTime = data.MainPracticeTime,
                        WorkPlace = data.WorkPlace,
                        LaptopInfo = data.LaptopInfo,
                        SmartphoneInfo = data.SmartphoneInfo,
                        Ability = data.Ability,
                        SalaryHope = data.SalaryHope.ToString(),
                        CanJoinDate = data.CanJoinDate != null ? data.CanJoinDate.Value.ToString("dd/MM/yyyy") : null,
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
                msg.Title = "Có lỗi xảy ra khi xóa!";
            }
            return Json(msg);
        }

        public object GetCountCandidateToday()
        {
            var today = DateTime.Today;
            var count = _context.CandiateBasic.Where(x => x.CreatedTime.Date == today).Count();
            return count;
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var data = _context.CandiateBasic.FirstOrDefault(x => x.Id == id);
                if (data != null)
                {
                    _context.CandiateBasic.Remove(data);
                    _context.SaveChanges();
                    msg.Title = "Xóa ứng viên thành công!";
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
                msg.Title = "Có lỗi xảy ra khi xóa!";
            }
            return Json(msg);
        }
        #endregion

        #region Candidate_Info
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
                    CreatedBy = ESEIM.AppContext.UserName
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
                        Married = data.Married ? "0" : "1",
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

        //public void InsertCandiateCode(string Code)
        //{
        //    var msg = new JMessage() { Error = true };
        //    try
        //    {
        //        CandidateBasic candidate = new CandidateBasic()
        //        {
        //            CandidateCode = Code,
        //            CreatedTime = DateTime.Now,
        //            CreatedBy = ESEIM.AppContext.UserName
        //        };
        //        _context.CandiateBasic.Add(candidate);
        //        _context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

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
                //can.Targeting = data.Targeting;
                can.FileCv_1 = data.FileCv_1;
                can.UpdatedBy = ESEIM.AppContext.UserName;
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

        #region Candidate_Info_More
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
                can.UpdatedBy = ESEIM.AppContext.UserName;
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
        #endregion

        #region Cadidate_event
        [HttpGet]
        public object GetEventCat(string candidateCode)
        {
            var data = (from a in _context.CandidateWorkEvents
                        where a.CandidateCode == candidateCode
                        select new
                        {
                            a.Id,
                            a.DatetimeEvent,
                            a.FrameTime,
                        }).AsNoTracking();
            return data;
        }

        [HttpPost]
        public object GetEventCatGrid([FromBody]EDMSCandidatesManagementJtableModel searchData)
        {
            int intBegin = (searchData.CurrentPage - 1) * searchData.Length;
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
                                b.FrameTime
                            };
                int count = query.Count();
                var data = query.OrderUsingSortExpression(searchData.QueryOrderBy).Skip(intBegin).Take(searchData.Length).AsNoTracking().ToList();
                var jdata = JTableHelper.JObjectTable(data, searchData.Draw, count, "CandidateCode", "Fullname", "Id", "DatetimeEvent", "FrameTime");
                return Json(jdata);
            }
            else
            {
                var list = new List<object>();
                var jdata = JTableHelper.JObjectTable(list, searchData.Draw, 0, "CandidateCode", "Fullname", "Id", "DatetimeEvent", "FrameTime");
                return Json(jdata);
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
                data.UpdatedBy = ESEIM.AppContext.UserName;
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
        #endregion
    }
}