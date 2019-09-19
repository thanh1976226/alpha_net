
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace ESEIM.Controllers
{
    [Area("Admin")]
    public class taskNhungController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly EIMDBContext _context;
        private readonly ILogger _logger;
        private readonly IActionLogService _actionLog;
        private readonly AppSettings _appSettings;
        public taskNhungController(EIMDBContext context, ILogger<task010Controller> logger, IOptions<AppSettings> appSettings, IHostingEnvironment hostingEnvironment, IActionLogService actionLog)
        {
            _context = context;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _actionLog = actionLog;
            _appSettings = appSettings.Value;

        }
        public IActionResult Index()
        {
            return View("Index");
        }
        public class JTableModelCustom : JTableModel
        {
            public string Key { get; set; }
            public string Key4 { get; set; }

        }

        //view dữ liệu ra bảng
        public object jtable([FromBody]JTableModelCustom jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.edu_cat_ranking
                        where a.flag == 1               
                        select new
                        {
                            id = a.id,
                            name=a.name,
                            picture=a.picture,
                            note=a.note,
                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "name", "picture", "note");
            return Json(jdata);
        }

        ////insert dữ liệu

        //[HttpPost]
        //public JsonResult insert([FromBody]edu_course obj)
        //{
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {
        //        var query = from a in _context.edu_course
        //                    where a.course_code == obj.course_code
        //                    select a;
        //        if(query.Count() == 0)
        //        {
        //            edu_course obj1 = new edu_course();
        //            obj1.course_code = obj.course_code;
        //            obj1.course_name = obj.course_name;
        //            obj1.coursetype = obj.coursetype;
        //            //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
        //            obj1.note = obj.note;
        //            obj1.price = obj.price;
        //            obj1.totalday = obj.totalday;
        //            obj1.userid = obj.userid;
        //            obj1.level = obj.level;
        //            obj1.flag = 1;
        //            obj1.createtime = DateTime.Now;

        //            _context.edu_course.Add(obj1);
        //            _context.SaveChanges();
        //            msg.Title = "Thêm thành công";
        //        }
        //        else
        //        {
        //            msg.Error = true;
        //            msg.Title = "Mã đã tồn tại";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Object = ex;
        //        msg.Title = "Có lỗi khi thêm ";
        //    }
        //    return Json(msg);
        //}

        ////update dữ liệu
        //[HttpPost]
        //public JsonResult update([FromBody]edu_course obj)
        //{
        //    var msg = new JMessage() { Error = true };
        //    try
        //    {
        //        var rs = _context.edu_course.SingleOrDefault(x => x.id == obj.id);
        //        if (rs != null)
        //        {
        //            rs.id = obj.id;
        //            rs.course_name = obj.course_name;
        //            rs.note = obj.note;
        //            rs.coursetype = obj.coursetype;
        //            rs.updatetime = DateTime.Now;
        //            rs.price = obj.price;
        //            rs.userid = obj.userid;

        //            rs.course_code = obj.course_code;

        //            rs.totalday = obj.totalday;
        //            rs.level = obj.level;


        //            _context.edu_course.Update(rs);

        //            _context.SaveChanges();
        //            msg.Title = "Cập nhật thông tin thành công";
        //            msg.Error = false;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Object = ex;
        //        msg.Title = "Có lỗi khi cập nhật";

        //    }
        //    return Json(msg);
        //}




        public async Task<JsonResult> Insert(edu_cat_ranking obj, IFormFile picture)
        {

            var msg = new JMessage() { Error = false, ID = 1 };
            try
            {
            

                    var icpicture = "";

                    if (picture != null && picture.Length > 0)
                    {
                        var pathUpload = Path.Combine(_hostingEnvironment.WebRootPath, "pictures\\avatar");
                        if (!Directory.Exists(pathUpload)) Directory.CreateDirectory(pathUpload);

                        var fileName = DateTimeOffset.Now.ToUnixTimeMilliseconds() + picture.FileName;
                        var filePath = Path.Combine(pathUpload, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await picture.CopyToAsync(stream);
                        }
                        icpicture = "/pictures/avatar/" + fileName;
                    }
                    if (icpicture != "")
                    {
                        obj.picture = icpicture;
                    }


                    obj.createtime = DateTime.Now;
                    obj.flag = 1;
                    _context.edu_cat_ranking.Add(obj);
                    _context.SaveChanges();

                    msg.Title = "Thêm mới xếp hạng thành công";
                    msg.Error = false;
                    _actionLog.InsertActionLog("edu_cat_ranking", "Insert new edu_cat_ranking successfully", null, obj, "Insert");
        
            }
            catch (Exception ex)
            {
                msg.ID = 0;
                msg.Error = true;
                msg.Object = ex;
                msg.Title = "Có lỗi khi thêm khoản mục";
                _actionLog.InsertActionLog("edu_cat_ranking", "Insert new edu_cat_ranking fail", null, obj, "Insert");
            }
            return Json(msg);
        }



        public async Task<JsonResult> Update(edu_cat_ranking obj, IFormFile picture)
        {
            var msg = new JMessage() { Error = false, ID = 1 };
            try
            {

                edu_cat_ranking rs = _context.edu_cat_ranking.FirstOrDefault(x => x.id == obj.id);
                if (rs != null)
                {

                    var icpicture = "";

                    if (picture != null && picture.Length > 0)
                    {
                        var pathUpload = Path.Combine(_hostingEnvironment.WebRootPath, "pictures\\avatar");
                        if (!Directory.Exists(pathUpload)) Directory.CreateDirectory(pathUpload);

                        var fileName = DateTimeOffset.Now.ToUnixTimeMilliseconds() + picture.FileName;
                        var filePath = Path.Combine(pathUpload, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await picture.CopyToAsync(stream);
                        }
                        icpicture = "/pictures/avatar/" + fileName;
                    }
                    if (icpicture != "")
                    {
                        rs.picture = icpicture;
                    }
                    rs.name = obj.name;
                    rs.priority = obj.priority;
                    rs.note = obj.note;
                
                    rs.updatetime = DateTime.Now;
                    _context.edu_cat_ranking.Update(rs);
                    _context.SaveChanges();
                    msg.Title = "Sửa thông tin xếp hạng thành công";
                    msg.Error = false;
                    _actionLog.InsertActionLog("edu_cat_ranking", "update edu_cat_ranking successfully", rs, obj, "Update");
                }
                else
                {
                    msg.Title = "Xảy ra lỗi, vui lòng thử lại.";
                    msg.Error = true;
                }


            }
            catch (Exception ex)
            {
                msg.ID = 0;
                msg.Error = true;
                msg.Object = ex;
                msg.Title = "Có lỗi khi sửa khoản mục";
                _actionLog.InsertActionLog("edu_cat_ranking", "update edu_cat_ranking fail", null, obj, "Update");
            }
            return Json(msg);
        }







        //getItem


        [HttpGet]
        public object getitem(int id)
        {
            try
            {
          //      var booking = _context.Location.SingleOrDefault(x => x.id == id);
                var query = from a in _context.edu_cat_ranking

                            where a.id == id
                            select new
                            {
                                id=a.id,
                                name = a.name,
                                picture = a.picture,
                                priority = a.priority,
                                note = a.note,
                    
                            };
                var data = query.Where(x => x.id == id);

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
            }
        }





        //delete
        [HttpPost]
        public object delete(int id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.edu_cat_ranking.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                _context.edu_cat_ranking.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "xoá thành công!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi khi xóa.";
                return Json(msg);
            }
        }

      
    }
}