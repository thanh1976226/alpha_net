
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
    public class task002Controller : Controller
    {
        private readonly EIMDBContext _context;
        public task002Controller(EIMDBContext context)
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
            var query = from a in _context.edu_schools
                        join b in _context.Location
                          on a.country equals b.id
                        where a.flag == 1 && (jTablePara.Key == null || jTablePara.Key == "" || a.name.ToLower().Contains(jTablePara.Key.ToLower()))
                        && (jTablePara.Key4 == null || jTablePara.Key4 == "" || a.city.ToLower().Contains(jTablePara.Key4.ToLower()))
                         && (jTablePara.Key5 == null || jTablePara.Key5 == "" || a.level.ToString()==jTablePara.Key5)
                        select new
                        {
                            id = a.id,
                            name = a.name,
                            address = a.address,
                            country = b.name,
                         
                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "name", "address", "country");
            return Json(jdata);
        }

        //insert dữ liệu

        [HttpPost]
        public JsonResult insert([FromBody]edu_schools obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var query = from a in _context.edu_schools
                            where a.code == obj.code
                            select a;
                if(query.Count() == 0)
                {
                    edu_schools obj1 = new edu_schools();
                    obj1.code = obj.code;
                    obj1.name = obj.name;
                    obj1.city = obj.city;
                    //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                    obj1.level = obj.level;
                    obj1.address = obj.address;
                    obj1.country = obj.country;
                    obj1.district = obj.district;
                   
                    obj1.flag = 1;
                    obj1.creattime = DateTime.Now;

                    _context.edu_schools.Add(obj1);
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
        public JsonResult update([FromBody]edu_schools obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var rs = _context.edu_schools.SingleOrDefault(x => x.id == obj.id);
                if (rs != null)
                {
                    rs.id = obj.id;
                    rs.code = obj.code;
                    rs.name = obj.name;
                    rs.city = obj.city;
                    rs.updatetime = DateTime.Now;
                    rs.level = obj.level;
                    rs.address = obj.address;

                    rs.country = obj.country;
                   
                    rs.district = obj.district;
                  
            


                    _context.edu_schools.Update(rs);

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
                var query = from a in _context.edu_schools

                            where a.id == id
                            select new
                            {
                                id=a.id,
                                code = a.code,
                                name = a.name,
                                city = a.city,
                                level = a.level,
                                address = a.address,
                                country = a.country,
                                district = a.district,
                               
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
                var data = _context.edu_schools.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                _context.edu_schools.Update(data);
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
        public object gettreedataCountry()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.Location.OrderBy(x => x.id).Where(x => x.flag == 1 && x.parent_id==0);
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

        [HttpPost]
        public object gettreedataDistrict(string city)
            {
            var msg = new JMessage { Error = true };

            try
            {
                var rs = _context.Location.SingleOrDefault(x => x.code == city);
                var data = _context.Location.OrderBy(x => x.id).Where(x => x.flag == 1 && x.parent_id== rs.id);
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
        public object gettreedataCity(int country)
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.Location.OrderBy(x => x.id).Where(x => x.flag == 1 && x.parent_id == country);
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
        public object gettreedataCity1()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.Location.OrderBy(x => x.id).Where(x => x.flag == 1 );
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