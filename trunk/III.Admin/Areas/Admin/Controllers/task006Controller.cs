
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
    public class task006Controller : Controller
    {
        private readonly EIMDBContext _context;
        public task006Controller(EIMDBContext context)
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
            var query = from a in _context.edu_relative_student
                        join b in _context.edu_student
                        on a.idstudentb equals b.id
                        join c in _context.common_properties
                        on a.idrelative equals c.id 
                        where a.flag == 1 && a.idstudenta == 4 
                        select new
                        {
                            id = a.id,
                            firstname = b.firstname,
                            idrelative = c.value,
                            idstudentb= a.idstudentb
                            //    course_name = a.course_name,


                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "firstname", "idstudentb", "idrelative");
            return Json(jdata);
        }




     

        //insert dữ liệu

        [HttpPost]
        public JsonResult insert([FromBody]edu_relative_student obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var query = from a in _context.edu_relative_student
                            where a.idstudenta == 4 && a.idstudentb == obj.idstudentb
                            select a;
                if (query.Count() == 0)
                {
                    edu_relative_student obj2 = new edu_relative_student();
                    edu_relative_student obj1 = new edu_relative_student();
                    var rs = _context.common_properties.SingleOrDefault(x => x.id == obj.idrelative);
                    var rs2 = _context.edu_student.SingleOrDefault(x => x.id == 4);
                    obj1.idstudenta = 4;
                    obj1.idstudentb = obj.idstudentb;
                    obj1.idrelative = obj.idrelative;
                    obj1.flag = 1;
                    _context.edu_relative_student.Add(obj1);

                    obj2.idstudenta = obj.idstudentb;
                    obj2.idstudentb = 4;
                    if (rs2.sex == 0)//nữ
                    {
                        if (rs.value.Contains("Anh") || rs.value.Contains("Chị"))
                        {
                            obj2.idrelative = 489;
                        }
                        if (rs.value.Contains("Em"))
                        {
                            obj2.idrelative = 4;
                        }
                    }

                    if (rs2.sex == 1)//nam
                    {
                        if (rs.value.Contains("Anh") || rs.value.Contains("Chị"))
                        {
                            obj2.idrelative = 5;
                        }
                        if (rs.value.Contains("Em"))
                        {
                            obj2.idrelative = 3;
                        }
                    }
                    obj2.flag = 1;
                    _context.edu_relative_student.Add(obj2);

                    _context.SaveChanges();
                    msg.Title = "Thêm thành công";
                }
               else
                {
                    msg.Error = true;
                    msg.Title = "Hai người này đã được thêm mối quan hệ ";
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
        public JsonResult update([FromBody]edu_relative_student obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {

                var rs4 = _context.common_properties.SingleOrDefault(x => x.id == obj.idrelative);
                var rs3 = _context.edu_student.SingleOrDefault(x => x.id == 4);
              //  var rs2 = _context.edu_relative_student.SingleOrDefault(x => x.id == obj.id);//trước
                var rs = _context.edu_relative_student.SingleOrDefault(x => x.id == obj.id);//student đang xét
                var rs1 = _context.edu_relative_student.SingleOrDefault(x => x.idstudenta == rs.idstudentb && x.idstudentb == rs.idstudenta);//student ace
                if (rs != null)
                {
                   

                    if (obj.idstudentb == rs.idstudentb)
                    {
                        rs.id = obj.id;
                        rs.idstudenta = 4;
                        rs.idstudentb = obj.idstudentb;
                        rs.idrelative = obj.idrelative;
                        _context.edu_relative_student.Update(rs);


                        rs1.idstudentb = 4;
                        rs1.idstudenta = obj.idstudentb;

                        if (rs3.sex == 0)//nữ
                        {
                            if (rs4.value.Contains("Anh") || rs4.value.Contains("Chị"))
                            {
                                rs1.idrelative = 489;
                            }
                            if (rs4.value.Contains("Em"))
                            {
                                rs1.idrelative = 4;
                            }
                        }

                        if (rs3.sex == 1)//nam
                        {
                            if (rs4.value.Contains("Anh") || rs4.value.Contains("Chị"))
                            {
                                rs1.idrelative = 5;
                            }
                            if (rs4.value.Contains("Em"))
                            {
                                rs1.idrelative = 3;
                            }
                        }
                        _context.edu_relative_student.Update(rs1);

                        _context.SaveChanges();
                        msg.Title = "Cập nhật thông tin thành công";
                        msg.Error = false;

                    }

                    else
                    {
                        msg.Title = "Không được sửa tên người thân";
                        msg.Error = true;
                    }

                 
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
                var query = from a in _context.edu_relative_student

                            where a.id == id
                            select new
                            {
                                id=a.id,
                                idstudenta = a.idstudenta,
                                idstudentb = a.idstudentb,
                                idrelative = a.idrelative,
                               
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
                var data = _context.edu_relative_student.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                var data2 = _context.edu_relative_student.FirstOrDefault(x => x.idstudenta == data.idstudentb && x.idstudentb == data.idstudenta);
                data2.flag = 0;
                _context.edu_relative_student.Update(data);
                _context.edu_relative_student.Update(data2);
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
        public object gettreedataRelationship(int idstudentb)
        {
            var msg = new JMessage { Error = true };

            try
            {
                var rs = _context.edu_student.SingleOrDefault(x => x.id == idstudentb);
                if (rs.sex == 0)
                {
                    var data = _context.common_properties.OrderBy(x => x.id).Where(x => x.flag == 1 && (x.code.Contains("relative_sister")));
                    msg.Object = data;
                    msg.Error = false;
                }
                if (rs.sex == 1)
                {
                    var data = _context.common_properties.OrderBy(x => x.id).Where(x => x.flag == 1 && ( x.code.Contains("relative_brother")));
                    msg.Object = data;
                    msg.Error = false;
                }

            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }

        [HttpPost]
        public object gettreedataIdstudentb()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.edu_student.OrderBy(x => x.id).Where(x => x.id != 4);
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