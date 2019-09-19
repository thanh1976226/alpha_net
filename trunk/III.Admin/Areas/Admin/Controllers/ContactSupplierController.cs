using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class ContactSupplierController : BaseController
    {
        public class EDMSContactJtableModel : JTableModel
        {
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        public ContactSupplierController(EIMDBContext context, IUploadService upload)
        {
            _context = context;
            _upload = upload;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public object JTable([FromBody]EDMSContactJtableModel jTablePara)
        {
            var dateFrom = !string.IsNullOrEmpty(jTablePara.DateFrom) ? DateTime.ParseExact(jTablePara.DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var dateTo = !string.IsNullOrEmpty(jTablePara.DateTo) ? DateTime.ParseExact(jTablePara.DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.Contacts
                        where (string.IsNullOrEmpty(jTablePara.Phone) || a.MobilePhone.ToLower().Contains(jTablePara.Phone.ToLower()))
                        && (string.IsNullOrEmpty(jTablePara.Email) || a.Email.ToLower().Contains(jTablePara.Email.ToLower()))
                        && (dateFrom == null || (a.CreateTime.HasValue && a.CreateTime.Value.Date >= dateFrom))
                        && (dateTo == null || (a.CreateTime.HasValue && a.CreateTime.Value.Date <= dateTo))
                        select new
                        {
                            a.Id,
                            a.ContactName,
                            a.MobilePhone,
                            a.Email,
                            a.FilePath,
                            a.CreateTime
                        };
            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "ContactName", "MobilePhone", "Email", "FilePath", "CreateTime");
            return Json(jdata);
        }

        [HttpGet]
        public object GetItem(int id)
        {
            var data = _context.Contacts.FirstOrDefault(x => x.Id == id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult Insert([FromBody]Contact obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                obj.CreateTime = DateTime.Now;
                _context.Contacts.Add(obj);
                _context.SaveChanges();
                msg.Title = "Thêm liên hệ thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra!";
                msg.Object = ex;
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Update([FromBody]Contact obj)
        {
            var msg = new JMessage();
            try
            {
                obj.UpdateTime = DateTime.Now.Date;
                _context.Contacts.Update(obj);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Cập nhật thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra!";
                msg.Object = ex;
                return Json(msg);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var data = _context.Contacts.FirstOrDefault(x => x.Id == id);
                _context.Contacts.Remove(data);
                _context.SaveChanges();
                msg.Title = "Xóa liên hệ thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra!";
                msg.Object = ex;
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult UploadImage(IFormFile fileUpload)
        {
            var msg = new JMessage { Title = "", Error = false };
            try
            {
                var upload = _upload.UploadImage(fileUpload);
                msg.Object = upload.Object;
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi upload file!";
                msg.Object = ex;
            }
            return Json(msg);
        }
    }
}