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
using System.Data;

namespace ESEIM.Controllers
{
    [Area("Admin")]
    public class edu_studentController : Controller
    {
        private readonly EIMDBContext _context;
        public edu_studentController(EIMDBContext context)
        {
            _context = context;
           
        }

        public class student
        {
            public int id_st { get; set; }
            public string code_st { get; set; }
            public string firstname_st { get; set; }
          
            public int? sex_st { get; set; }
            public DateTime? birthday_st { get; set; }
            public string address_st { get; set; }
            public int? city_st { get; set; }
            public int? districts_st { get; set; }
          
            public string email_st { get; set; }
            public string telephone_st { get; set; }
            public int? userid_st { get; set; }//người tạo
            public int? flag_st { get; set; }
            public bool? Public_st { get; set; }
            public string facebook_st { get; set; }
            public string sky_st { get; set; }
            public string google_st { get; set; }
            public string twitter_st { get; set; }
            public string wordpress_st { get; set; }
            public string other_st { get; set; }
            public string mobilephone_st { get; set; }
            public int? school_st { get; set; }
            public int? Class_st { get; set; }
            //public int? classSpecial_st { get; set; }
            public int? course_st { get; set; }
            public int? type_st { get; set; }
            public int? status_st { get; set; }
            public string note_st { get; set; }
            public string image_st { get; set; }
            public int? idrank_st { get; set; }
            public string objective_st { get; set; }//mục tiêu
            //public string inforSM_st { get; set; }
            public string noteinfor_st { get; set; }
            public string studying_oversea_st { get; set; }//muốn du học
            public string desired_score_st { get; set; }//điểm mong muốn
            public string chuyennganh_st { get; set; }
            public string file_id_st { get; set; }
            public string username_st { get; set; }
            public string password_st { get; set; }
            public DateTime? createtime_st { get; set; }
            public DateTime? updatetime_st { get; set; }
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
                        where a.flag == 1 &&  (jTablePara.Key == null || jTablePara.Key == "" || a.firstname.ToLower().Contains(jTablePara.Key.ToLower()))
                        select new
                        {
                            id_st = a.id,
                            firstname = a.firstname,
                            birthday = a.birthday,
                            phone = a.telephone
                        };
            var count = query.Count();
            var data = query.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            //var data = query.Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id_st", "firstname", "birthday", "phone");
            return Json(jdata);
        }
        [HttpGet]
        public object GetData(int id)
        {
            var msg = new JMessage { Error = false, Title = "" };
            var data = _context.edu_student.FirstOrDefault(x => x.id == id);
            if (data != null)
            {
                var model = new student
                {
                    id_st = data.id,
                    firstname_st = data.firstname,
                    email_st = data.email,
                    //Id = data.
                    address_st = data.address,
                    type_st = data.type,
                    userid_st = data.userid,
                    desired_score_st = data.desired_score,
                    code_st = data.code,
                    birthday_st = data.birthday,
                    telephone_st = data.telephone,
                    city_st = data.city,
                    course_st = data.course,
                    createtime_st = data.createtime,
                    mobilephone_st = data.mobilephone,
                    districts_st = data.districts,
                    chuyennganh_st = data.chuyennganh,
                    objective_st = data.objective,
                    note_st = data.note,
                    image_st = data.image,
                    studying_oversea_st = data.studying_oversea,


                    //address_st = data.address,
                    //address_st = data.address,
                    //address_st = data.address,
                    //address_st = data.address,
                    //address_st = data.address,
                    //address_st = data.address,
                    //address_st = data.address,
                    //address_st = data.address,
                    //address_st = data.address,
                    //address_st = data.address,


                };
                msg.Object = model;
            }
            else
            {
                msg.Error = true;
                msg.Title = "Lôi";
            }
            return Json(msg);
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

        //[HttpGet]
        //public object GetData(int id)
        //{
        //    var msg = new JMessage { Error = false, Title = "" };
        //    var data = _context.edu_student.FirstOrDefault(x => x.id == id);
        //    if (data != null)
        //    {
        //        var model = new student
        //        {
        //            id_st = data.id,
        //            firstname_st = data.firstname,
        //            email_st = data.email,
        //            //Id = data.
        //            address_st = data.address,
        //            type_st = data.type,
        //            userid_st = data.userid,
        //            desired_score_st = data.desired_score,
        //            code_st = data.code,
        //            birthday_st = data.birthday,
        //            telephone_st = data.telephone,
        //            city_st = data.city,
        //            course_st = data.course,
        //            createtime_st = data.createtime,
        //            mobilephone_st = data.mobilephone,
        //            districts_st = data.districts,
        //            chuyennganh_st = data.chuyennganh,
        //            objective_st = data.objective,
        //            note_st = data.note,
        //            image_st = data.image,
        //            studying_oversea_st = data.studying_oversea,


        //            //address_st = data.address,
        //            //address_st = data.address,
        //            //address_st = data.address,
        //            //address_st = data.address,
        //            //address_st = data.address,
        //            //address_st = data.address,
        //            //address_st = data.address,
        //            //address_st = data.address,
        //            //address_st = data.address,
        //            //address_st = data.address,


        //        };
        //        msg.Object = model;
        //    }
        //    else
        //    {
        //        msg.Error = true;
        //        msg.Title = "Lôi";
        //    }
        //    return Json(msg);
        //}
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
                var data = _context.edu_student.OrderBy(x => x.id).Where(x => x.flag == 1 && x.code.Contains("LevelStudent_"));
                msg.Object = data;
                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }


        //update dữ liệu
        [HttpPost]
        public JsonResult updateMXH([FromBody]edu_student obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var rs = _context.edu_student.SingleOrDefault(x => x.id == 4);
                if (rs != null)
                {
                    rs.id = obj.id;
                    rs.facebook = obj.facebook;
                    rs.sky = obj.sky;
                    rs.twitter = obj.twitter;
                    rs.updatetime = DateTime.Now;
                    rs.other = obj.other;
                    rs.userid = obj.userid;

                    rs.google = obj.google;

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





        [HttpGet]
        public object getitemMXH(int id)
        {
            try
            {
                //      var booking = _context.Location.SingleOrDefault(x => x.id == id);
                var query = from a in _context.edu_student

                            where a.id == 4
                            select new
                            {
                                id = a.id,
                                facebook = a.facebook,
                                sky = a.sky,
                                twitter = a.twitter,
                                google = a.google,
                                other = a.other,
                                wordpress = a.wordpress
                            };
                var data = query.Where(x => x.id == 4);

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
            }
        }


        //Kiểu học viên

        [HttpPost]
        public object gettreedataParent_idKHV()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.common_properties.OrderBy(x => x.id).Where(x => x.flag == 1 && x.code.Contains("TypeStudent_"));
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