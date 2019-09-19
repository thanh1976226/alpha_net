
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
    public class task007Controller : Controller
    {
        private readonly EIMDBContext _context;
        public task007Controller(EIMDBContext context)
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
            var query = from a in _context.edu_student_transcripts

                        where a.flag == 1 && a.idstudent == 4 
                        select new
                        {
                            id = a.id,
                            subject = a.subject,
                            school = a.school,
                            classst = a.classst,

                            mark = a.mark,
                            starttime = a.starttime,
                            endtime = a.endtime,

                            teacher = a.teacher



                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "subject", "school", "classst", "mark", "starttime", "endtime", "teacher");
            return Json(jdata);
        }






        //insert dữ liệu

        //insert dữ liệu

        [HttpPost]
        public JsonResult insert([FromBody]edu_student_transcripts obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                edu_student_transcripts obj1 = new edu_student_transcripts();
                    obj1.subject = obj.subject;
                    obj1.mark = obj.mark;
                    obj1.school = obj.school;
                    //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                    obj1.starttime = obj.starttime;
                    obj1.endtime = obj.endtime;
                    obj1.note = obj.note;
                    obj1.teacher = obj.teacher;
                    obj1.language = obj.language;
                   obj1.idstudent = 4;
                   obj1.classst = obj.classst;
                    obj1.flag = 1;
                    obj1.createtime = DateTime.Now;

                    _context.edu_student_transcripts.Add(obj1);
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
        public JsonResult update([FromBody]edu_student_transcripts obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var rs = _context.edu_student_transcripts.SingleOrDefault(x => x.id == obj.id);
                if (rs != null)
                {
                    rs.id = obj.id;
                    rs.subject = obj.subject;
                    rs.mark = obj.mark;
                    rs.school = obj.school;
                    rs.updatetime = DateTime.Now;
                    rs.starttime = obj.starttime;
                    rs.endtime = obj.endtime;

                    rs.note = obj.note;

                    rs.teacher = obj.teacher;
                    rs.language = obj.language;
                    rs.classst = obj.classst;


                    _context.edu_student_transcripts.Update(rs);

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
                var query = from a in _context.edu_student_transcripts

                            where a.id == id
                            select new
                            {
                                id=a.id,
                                subject = a.subject,
                                mark = a.mark,
                                school = a.school,
                                starttime = a.starttime,
                                endtime = a.endtime,
                                note = a.note,
                                teacher = a.teacher,
                                idstudent = a.idstudent,
                                language = a.language,
                                classst = a.classst,
                           

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
                var data = _context.edu_student_transcripts.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                _context.edu_student_transcripts.Update(data);
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