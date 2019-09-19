
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
    public class task008Controller : Controller
    {
        private readonly EIMDBContext _context;
        public task008Controller(EIMDBContext context)
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
            public string Key5 { get; set; }

        }
        public static int id_city;

        //view dữ liệu ra bảng
        public object jtable([FromBody]JTableModelCustom jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.edu_skillmaster
                        where a.flag==1
                        select new
                        {
                            id_skillmaster = a.id_skillmaster,
                            code = a.code,
                            name = a.name,

                        };
            var count = query.Count();
            var data = query.Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id_skillmaster", "code", "name");
            return Json(jdata);
        }

        //insert dữ liệu

        [HttpPost]
        public JsonResult insert([FromBody]edu_skillmaster obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var query = from a in _context.edu_skillmaster// lệnh linq này để tìm xem cái mã code ấy đã tồn tại chưea
                            //phải truyền điều kiện where vào thì nó mới tìm đc =.=
                            where a.code == obj.code
                            select a;
                if (query.Count() == 0)
                {
                    edu_skillmaster obj1 = new edu_skillmaster();
                    obj1.code = obj.code;
                    obj1.name = obj.name;
                    obj1.user_id = obj.user_id;
                    obj1.createtime = DateTime.Now;
                    //   obj1.updatetime = DateTime.Now; tương tự cái kia, thời gian update
                    obj1.flag = 1;
                    obj1.language = obj.language;
                    _context.edu_skillmaster.Add(obj1);
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
        public JsonResult update([FromBody]edu_skillmaster obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var rs = _context.edu_skillmaster.SingleOrDefault(x => x.id_skillmaster == obj.id_skillmaster);
                if (rs != null)
                {
                    rs.id_skillmaster = obj.id_skillmaster;
                    rs.code = obj.code;
                    rs.name = obj.name;
                    rs.user_id = obj.user_id;
                    rs.updatetime = DateTime.Now;
                  //  rs.createtime = DateTime.Now; chỉ khi nào cập nhật thì mới có cái này, đây là thời gian tạo
                    rs.flag = 1;
                    rs.language = obj.language;

                    _context.edu_skillmaster.Update(rs);

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
                var query = from a in _context.edu_skillmaster
                            where a.id_skillmaster == id
                            select new
                            {
                                id_skillmaster=a.id_skillmaster,
                                code = a.code,
                                name = a.name,

                            };
                var data = query.Where(x => x.id_skillmaster == id);

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
                var data = _context.edu_skillmaster.FirstOrDefault(x => x.id_skillmaster == id);
                data.flag = 0;
                _context.edu_skillmaster.Update(data);
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
        public object gettreedataParent_id()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.edu_skillmaster.OrderBy(x => x.id_skillmaster);
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