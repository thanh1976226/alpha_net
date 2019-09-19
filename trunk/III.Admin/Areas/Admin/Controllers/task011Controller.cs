
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
    public class task011upController : Controller
    {
        private readonly EIMDBContext _context;
        public task011upController(EIMDBContext context)
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


        //update dữ liệu
        [HttpPost]
        public JsonResult update([FromBody]edu_student obj)
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
        public object getitem(int id)
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




    }
}