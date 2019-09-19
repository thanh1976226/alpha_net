
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
    public class task001Controller : Controller
    {
        private readonly EIMDBContext _context;
        public task001Controller(EIMDBContext context)
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
            var query = from a in _context.edu_course
                        join b in _context.common_properties
                        on a.coursetype equals b.id
                        where a.flag == 1 && b.code.Contains("edu_type_") && (jTablePara.Key == null || jTablePara.Key == "" || a.course_name.ToLower().Contains(jTablePara.Key.ToLower()))
                        && (jTablePara.Key4 == null || jTablePara.Key4 == "" || a.coursetype.ToString()==jTablePara.Key4.ToLower())
                        select new
                        {
                            id = a.id,
                            course_code = a.course_code,
                            course_name = a.course_name,
                            coursetype=b.value,
                            price =a.price,
                            note=a.note,
                            userid=a.userid
                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "course_code", "course_name", "coursetype", "price", "note", "userid");
            return Json(jdata);
        }

        //insert dữ liệu

        [HttpPost]
        public JsonResult insert([FromBody]edu_course obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var query = from a in _context.edu_course
                            where a.course_code == obj.course_code
                            select a;
                if(query.Count() == 0)
                {
                    edu_course obj1 = new edu_course();
                    obj1.course_code = obj.course_code;
                    obj1.course_name = obj.course_name;
                    obj1.coursetype = obj.coursetype;
                    //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                    obj1.note = obj.note;
                    obj1.price = obj.price;
                    obj1.totalday = obj.totalday;
                    obj1.userid = obj.userid;
                    obj1.level = obj.level;
                    obj1.flag = 1;
                    obj1.createtime = DateTime.Now;

                    _context.edu_course.Add(obj1);
                    _context.SaveChanges();
                    msg.Title = "Thêm thành công";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Mã đã tồn tại";
                }
               
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
        public JsonResult update([FromBody]edu_course obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var rs = _context.edu_course.SingleOrDefault(x => x.id == obj.id);
                if (rs != null)
                {
                    rs.id = obj.id;
                    rs.course_name = obj.course_name;
                    rs.note = obj.note;
                    rs.coursetype = obj.coursetype;
                    rs.updatetime = DateTime.Now;
                    rs.price = obj.price;
                    rs.userid = obj.userid;

                    rs.course_code = obj.course_code;
                   
                    rs.totalday = obj.totalday;
                    rs.level = obj.level;


                    _context.edu_course.Update(rs);

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
                var query = from a in _context.edu_course

                            where a.id == id
                            select new
                            {
                                id=a.id,
                                course_name = a.course_name,
                                note = a.note,
                                coursetype = a.coursetype,
                                price = a.price,
                                userid = a.userid,
                                course_code = a.course_code,
                                totalday = a.totalday,
                                level = a.level
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
                var data = _context.edu_course.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                _context.edu_course.Update(data);
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

        [HttpPost]
        public object gettreedataCoursetype()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.common_properties.OrderBy(x => x.id).Where(x => x.flag == 1 && x.code.Contains("edu_type_"));
                msg.Object = data;
                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }

        [HttpPost]
        public object gettreedataLevel()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.common_properties.OrderBy(x => x.id).Where(x => x.flag == 1 && x.code.Contains("LevelStudent_"));
                msg.Object = data;
                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }


    }
}