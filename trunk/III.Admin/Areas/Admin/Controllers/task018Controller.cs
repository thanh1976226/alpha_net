
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
    public class task018Controller : Controller
    {
        private readonly EIMDBContext _context;
        public task018Controller(EIMDBContext context)
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
            public string Key2 { get; set; }
            public string Key7 { get; set; }

        }

        //view dữ liệu ra bảng
        public object jtablePT([FromBody]JTableModelCustom jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.edu_invoice
                        where a.ticket_type == 1 && (jTablePara.Key2 == null || jTablePara.Key2 == "" || a.ticket_code.ToLower().Contains(jTablePara.Key2.ToLower()))
                        select new
                        {
                            ticket_id = a.ticket_id,
                            ticket_code = a.ticket_code,
                            createtime = a.createtime,
                            ticket_title = a.ticket_title,
                            person_pay = a.person_pay,
                            payment_mode = a.payment_mode,
                            note = a.note,
                            pay=a.pay
                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("ticket_id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "ticket_id", "ticket_code", "createtime", "ticket_title", "person_pay", "payment_mode", "note", "pay");
            return Json(jdata);
        }


        public object jtablePC([FromBody]JTableModelCustom jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.edu_invoice
                        where a.ticket_type == 2 && (jTablePara.Key7 == null || jTablePara.Key7 == "" || a.ticket_code.ToLower().Contains(jTablePara.Key7.ToLower()))
                        select new
                        {
                            ticket_id = a.ticket_id,
                            ticket_code = a.ticket_code,
                            createtime = a.createtime,
                            ticket_title = a.ticket_title,
                            person_pay = a.person_pay,
                            payment_mode = a.payment_mode,
                            note = a.note,
                            pay = a.pay
                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("ticket_id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "ticket_id", "ticket_code", "createtime", "ticket_title", "person_pay", "payment_mode", "note", "pay");
            return Json(jdata);
        }

        //insert dữ liệu

        [HttpPost]
        public JsonResult insert([FromBody]edu_invoice obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var query = from a in _context.edu_invoice
                            where a.ticket_code == obj.ticket_code
                            select a;
                if(query.Count() == 0)
                {
                    edu_invoice obj1 = new edu_invoice();
                    obj1.ticket_code = obj.ticket_code;
                    obj1.ticket_title = obj.ticket_title;
                    obj1.pay = obj.pay;
                    //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                    obj1.ticket_type = 1;
                    obj1.currency = obj.currency;
                    obj1.note = obj.note;
                    obj1.person_pay = obj.person_pay;
                    obj1.payment_mode = obj.payment_mode;
                    obj1.createtime = DateTime.Now;

                    _context.edu_invoice.Add(obj1);
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

        [HttpPost]
        public JsonResult insertC([FromBody]edu_invoice obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var query = from a in _context.edu_invoice
                            where a.ticket_code == obj.ticket_code
                            select a;
                if (query.Count() == 0)
                {
                    edu_invoice obj1 = new edu_invoice();
                    obj1.ticket_code = obj.ticket_code;
                    obj1.ticket_title = obj.ticket_title;
                    obj1.pay = obj.pay;
                    //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                    obj1.ticket_type = 2;
                    obj1.currency = obj.currency;
                    obj1.note = obj.note;
                    obj1.person_pay = obj.person_pay;
                    obj1.payment_mode = obj.payment_mode;
                    obj1.createtime = DateTime.Now;

                    _context.edu_invoice.Add(obj1);
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
        [HttpPost]
        public object gettreedataCurrency()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.edu_currency.OrderBy(x => x.currencyid);
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