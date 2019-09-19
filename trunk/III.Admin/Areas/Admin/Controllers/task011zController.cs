
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
    public class task011zController : Controller
    {
        private readonly EIMDBContext _context;
        public task011zController(EIMDBContext context)
        {
            _context = context;
           
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

            var query = from a in _context.edu_student

                        where a.flag == 1
                        select new
                        {
                            id = a.id,
                            facebook = a.facebook,
                            sky = a.sky,
                            google = a.google,

                            twitter = a.twitter,
                            wordpress = a.wordpress,
                            other = a.other,

                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "facebook", "sky", "google", "twitter", "wordpress", "other");
            return Json(jdata);
        }
        //insert dữ liệu

        //insert dữ liệu

        [HttpPost]
        public JsonResult insert([FromBody]edu_student obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                edu_student obj1 = new edu_student();
                    obj1.facebook = obj.facebook;
                    obj1.sky = obj.sky;
                    obj1.google = obj.google;
                    //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                    obj1.twitter = obj.twitter;
                    obj1.wordpress = obj.wordpress;
                    obj1.other = obj.other;
                    
                    obj1.flag = 1;

                    obj1.createtime = DateTime.Now;

                    _context.edu_student.Add(obj1);
                    _context.SaveChanges();
                    msg.Title = "Thêm thành công";

            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = "Có lỗi khi thêm ";
            }
            return Json(msg);
        }


        //update dữ liệu
        [HttpPost]
        public JsonResult update([FromBody]edu_student obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var rs = _context.edu_student.SingleOrDefault(x => x.id == obj.id);
                if (rs != null)
                {
                    rs.id = obj.id;
                    rs.facebook = obj.facebook;
                    rs.sky = obj.sky;
                    rs.google = obj.google;
                    rs.updatetime = DateTime.Now;
                    rs.twitter = obj.twitter;
                    rs.wordpress = obj.wordpress;

                    rs.other = obj.other;

                    


                    _context.edu_student.Update(rs);

                    _context.SaveChanges();
                    msg.Title = "Cập nhật thông tin thành công";
                    msg.Error = false;

                }
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi khi cập nhật";

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
                var query = from a in _context.edu_student

                            where a.id == id
                            select new
                            {
                                id = a.id,
                                facebook = a.facebook,
                                sky = a.sky,
                                google = a.google,

                                twitter = a.twitter,
                                wordpress = a.wordpress,
                                other = a.other,


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
                var data = _context.edu_student.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                _context.edu_student.Update(data);
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