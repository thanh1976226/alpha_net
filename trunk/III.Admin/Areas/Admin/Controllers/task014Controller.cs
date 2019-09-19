
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
    public class task014Controller : Controller
    {
        private readonly EIMDBContext _context;
        public task014Controller(EIMDBContext context)
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
            var query = from a in _context.common_properties
                        
                        where a.flag == 1 && a.code.Contains("InformationSUMMIT_")
                        select new
                        {
                            id = a.id,
                            value = a.value,
                            
                         
                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "value");
            return Json(jdata);
        }

        //insert dữ liệu

        [HttpPost]
        public JsonResult insert([FromBody]common_properties obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                
                var query = from a in _context.common_properties
                            where a.code == obj.code
                            select a;
                if(query.Count() == 0)
                {
                    common_properties obj1 = new common_properties();
                    obj1.code = "InformationSUMMIT_" + obj.code;
                
                    obj1.value = obj.value;
                    obj1.parent_id = obj.parent_id;
                    //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                    obj1.order = obj.order;
                  
                   
                    obj1.flag = 1;
                    obj1.createtime = DateTime.Now;

                    _context.common_properties.Add(obj1);
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
        public JsonResult update([FromBody]common_properties obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var rs = _context.common_properties.SingleOrDefault(x => x.id == obj.id);
                if (rs != null)
                {
                    rs.id = obj.id;
                    rs.code = obj.code;
                    rs.value = obj.value;
                    rs.parent_id = obj.parent_id;
                    //rs.updatetime = DateTime.Now;
                    rs.order = obj.order;
                  
                  
            


                    _context.common_properties.Update(rs);

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
                var query = from a in _context.common_properties

                            where a.id == id
                            select new
                            {
                                id=a.id,
                                code = a.code,
                                value = a.value,
                                parent_id = a.parent_id,
                                order = a.order,
                               
                               
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
                var data = _context.common_properties.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                _context.common_properties.Update(data);
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