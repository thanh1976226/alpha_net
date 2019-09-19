
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
    public class testController : Controller
    {
        private readonly EIMDBContext _context;
        public testController(EIMDBContext context)
        {
            _context = context;
                  
        }
        public class test
        {
            public int id_st { get; set; }
            public string firstname { get; set; }
            public string code { get; set; }
            public int? sex { get; set; }
            public DateTime? birthday { get; set; }

            public int id_con { get; set; }
            public string name { get; set; }
            public int? relationship { get; set; }
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
                        join b in _context.edu_student_contact
                        on a.id equals b.studentID
                        where a.flag == 1 && b.flag==1
                     
                        select new
                        {
                            id_st = a.id,
                            code=a.code,
                            firstname = a.firstname,
                            sex = a.sex,
                            birthday = a.birthday,
                            id_con = b.id,
                            name = b.name,
                            relationship = b.relationship
                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("id_con").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id_st", "code", "firstname", "sex", "birthday", "id_con", "name", "relationship");
            return Json(jdata);
        }

        //insert dữ liệu

        [HttpPost]
        public JsonResult insert([FromBody]test obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var query = from a in _context.edu_student
                            where a.code == obj.code
                            select a;
                if(query.Count() == 0)
                {
                    edu_student obj1 = new edu_student();
                    obj1.code = obj.code;
                    obj1.firstname = obj.firstname;
                    obj1.sex = obj.sex;
                    //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                    obj1.birthday = obj.birthday;
                    obj1.flag = 1;
                    obj1.createtime = DateTime.Now;
                    _context.edu_student.Add(obj1);
                    _context.SaveChanges();


                    edu_student_contact obj2 = new edu_student_contact();
                    var rs = _context.edu_student.OrderByDescending("id").FirstOrDefault();
                    obj2.studentID = rs.id;
                    obj2.flag = 1;
                    obj2.relationship = obj.relationship;
                    obj2.name = obj.name;
                    _context.edu_student_contact.Add(obj2);

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
        public JsonResult update([FromBody]test obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var rs = _context.edu_student.SingleOrDefault(x => x.id == obj.id_st);
                var rs1 = _context.edu_student_contact.SingleOrDefault(x => x.id == obj.id_con);
                if (rs != null && rs1 !=null)
                {
                    rs.id = obj.id_st;
                    rs.code = obj.code;
                    rs.firstname = obj.firstname;
                    rs.sex = obj.sex;
                    rs.updatetime = DateTime.Now;
                    rs.birthday = obj.birthday;
                 


                    _context.edu_student.Update(rs);
                    rs1.name = obj.name;
                    rs1.relationship = obj.relationship;
                    _context.edu_student_contact.Update(rs1);
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
        public object getitem(int id_con)
        {
            try
            {
          //      var booking = _context.Location.SingleOrDefault(x => x.id == id);
                var query = from a in _context.edu_student
                            join b in _context.edu_student_contact
                            on a.id equals b.studentID
                            where b.id == id_con
                            select new
                            {
                                id_st=a.id,
                                firstname = a.firstname,
                                sex = a.sex,
                                birthday = a.birthday,
                                code = a.code,
                                name = b.name,
                                relationship = b.relationship,
                              id_con=b.id,
                            };
                var data = query.Where(x => x.id_con == id_con);

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
            }
        }





        //delete
        [HttpPost]
        public object delete(int id_con)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.edu_student_contact.FirstOrDefault(x => x.id == id_con);
                data.flag = 0;
                _context.edu_student_contact.Update(data);
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