
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
    public class task003Controller : Controller
    {
        private readonly EIMDBContext _context;
        public task003Controller(EIMDBContext context)
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
                        && (jTablePara.Key4 == null || jTablePara.Key4 == "" || a.coursetype.ToString() == jTablePara.Key4.ToLower())
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
        public JsonResult insert([FromBody]edu_class obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var query = from a in _context.edu_class
                            where a.code == obj.code
                            select a;
                if(query.Count() == 0)
                {
                    edu_class obj1 = new edu_class();
                    obj1.code = obj.code;
                    obj1.name = obj.name;
                    obj1.idcrouse = obj.idcrouse;
                    //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                    obj1.numberstudent = obj.numberstudent;
                    obj1.schoolstart = obj.schoolstart;
                    obj1.schoolend = obj.schoolend;
                    obj1.schoolday = obj.schoolday;
                    obj1.status = obj.status;


                    obj1.classroom_id = obj.classroom_id;
                    obj1.teacher_id = obj.teacher_id;
                    obj1.fee = obj.fee;
                    obj1.datestart = obj.datestart;
                    obj1.dateend = obj.dateend;

                    obj1.sale_off_price = obj.sale_off_price;
                    obj1.start_sale_off = obj.start_sale_off;
                    obj1.end_sale_off = obj.end_sale_off;
                    obj1.sale_off_note = obj.sale_off_note;
                    obj1.location_id = obj.location_id;


                    obj1.flag = 1;
                    obj1.creattime = DateTime.Now;

                    _context.edu_class.Add(obj1);
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
        public JsonResult update([FromBody]edu_class obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var rs = _context.edu_class.SingleOrDefault(x => x.id == obj.id);
                if (rs != null)
                {
                    rs.id = obj.id;
                    rs.code = obj.code;
                    rs.name = obj.name;
                    rs.idcrouse = obj.idcrouse;
                    rs.updatetime = DateTime.Now;
                    rs.schoolstart = obj.schoolstart;
                    rs.userid = obj.userid;

                    rs.schoolend = obj.schoolend;

                    rs.schoolday = obj.schoolday;
                    rs.status = obj.status;


                    rs.classroom_id = obj.classroom_id;
                    rs.teacher_id = obj.teacher_id;
                    rs.fee = obj.fee;
                    rs.datestart = obj.datestart;

                    rs.dateend = obj.dateend;
                    rs.sale_off_price = obj.sale_off_price;

                    rs.start_sale_off = obj.start_sale_off;

                    rs.end_sale_off = obj.end_sale_off;
                    rs.sale_off_note = obj.sale_off_note;
                    rs.location_id = obj.location_id;


                    _context.edu_class.Update(rs);

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
                var query = from a in _context.edu_class

                            where a.id == id
                            select new
                            {
                                id=a.id,
                                code = a.code,
                                name = a.name,
                                idcrouse = a.idcrouse,
                                numberstudent = a.numberstudent,
                                userid = a.userid,
                                schoolstart = a.schoolstart,
                                schoolend = a.schoolend,
                                schoolday = a.schoolday,
                                status = a.status,
                                classroom_id = a.classroom_id,
                                teacher_id = a.teacher_id,
                                fee = a.fee,
                                datestart = a.datestart,
                                dateend = a.dateend,
                                sale_off_price = a.sale_off_price,
                                start_sale_off = a.start_sale_off,
                                end_sale_off = a.end_sale_off,
                                sale_off_note = a.sale_off_note,
                                location_id = a.location_id,
                            };
                var data = query.Where(x => x.id == id);

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
            }
        }

        ////delete
        //[HttpPost]
        //public object delete(int id)
        //{
        //    var msg = new JMessage { Error = true };
        //    try
        //    {
        //        var data = _context.edu_course.FirstOrDefault(x => x.id == id);
        //        data.flag = 0;
        //        _context.edu_course.Update(data);
        //        _context.SaveChanges();
        //        msg.Error = false;
        //        msg.Title = "xoá thành công!";
        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi khi xóa.";
        //        return Json(msg);
        //    }
        //}

        [HttpPost]
        public object gettreedataCourse()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.edu_course.OrderBy(x => x.id).Where(x => x.flag == 1);
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
        public object gettreedataLocation()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.edu_location.OrderBy(x => x.id).Where(x => x.flag == 1 );
                msg.Object = data;
                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }
        //
        [HttpPost]
        public object gettreedataStatus()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.common_properties.OrderBy(x => x.id).Where(x => x.flag == 1 && x.code.Contains("class_status_"));
                msg.Object = data;
                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }



        //view dữ liệu ra bảng
        public object jtableGV([FromBody]JTableModelCustom jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.edu_class_teacher
                        where a.class_id == 41
                        select new
                        {
                            id = a.id,
                            teacher_id = a.teacher_id,
                            starttime = a.starttime,
                            endtime = a.endtime
                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "teacher_id", "starttime", "endtime");
            return Json(jdata);
        }

        [HttpPost]
        public object gettreedataSkills()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.edu_skillmaster.OrderBy(x => x.id_skillmaster).Where(x => x.flag == 1);
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