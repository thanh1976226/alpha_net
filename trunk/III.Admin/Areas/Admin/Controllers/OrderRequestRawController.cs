using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ESEIM;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace III.Admin.Controllers
{
    public class CustomerRequestModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Keyword { get; set; }
        public int? Priority { get; set; }
        public string RequestTime { get; set; }
        public string url1 { get; set; }
        public string FileName1 { get; set; }
    }
    [Area("Admin")]
    public class OrderRequestRawController : BaseController
    {
        public class CustomerRequestJtable : JTableModel
        {
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        private readonly IHostingEnvironment _hostingEnvironment;

        public OrderRequestRawController(EIMDBContext context, IUploadService upload, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _upload = upload;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public object JTable([FromBody]CustomerRequestJtable jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var fromDate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toDate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.OrderRequestRaws
                        where ((fromDate == null) || (a.RequestTime.HasValue && a.RequestTime.Value.Date >= fromDate.Value.Date))
                            && ((toDate == null) || (a.RequestTime.HasValue && a.RequestTime.Value.Date <= toDate.Value.Date))
                            && (string.IsNullOrEmpty(jTablePara.Title) || a.Title.ToLower().Contains(jTablePara.Title.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.Content) || a.Content.ToLower().Contains(jTablePara.Content.ToLower()))
                        select new
                        {
                            a.Id,
                            a.Title,
                            a.Content,
                            a.FileName1,
                            a.File1,
                            a.Priority,
                            a.RequestTime,
                        };
            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "Title", "Content", "FileName1", "FilePath", "Priority", "RequestTime");
            return Json(jdata);
        }

        [HttpGet]
        public object GetAutocomplete(string key)
        {
            var listData = _context.GalaxyKeywords.Where(x => string.IsNullOrEmpty(key) || x.Keyword.ToLower().Contains(key.ToLower())).Select(x => x.Keyword);
            return listData;
        }

        [HttpPost]
        public JsonResult GetItem([FromBody]int id)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            var data = _context.OrderRequestRaws.FirstOrDefault(x => x.Id == id);
            if (data != null)
            {
                var model = new CustomerRequestModel
                {
                    Id = data.Id,
                    Title = data.Title,
                    Content = data.Content,
                    Phone = data.Phone,
                    Email = data.Email,
                    Priority = data.Priority,
                    RequestTime = data.RequestTime != null ? data.RequestTime.Value.ToString("dd/MM/yyyy") : "",
                    Keyword = data.Keyword,
                    url1 = data.File1,
                    FileName1 = data.FileName1,
                };
                msg.Object = model;
            }
            else
            {
                msg.Error = true;
                msg.Title = "Không tồn tại dữ liệu";
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Insert(CustomerRequestModel obj, IFormFile fileUpload)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                if (fileUpload != null)
                {
                    var upload = _upload.UploadFile(fileUpload, Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\files"));
                    if (!upload.Error)
                    {
                        var customerRequest = new OrderRequestRaw
                        {
                            Title = obj.Title,
                            Content = obj.Content,
                            Phone = obj.Phone,
                            Keyword = obj.Keyword,
                            Priority = obj.Priority,
                            CreatedTime = DateTime.Now,
                            RequestTime = !string.IsNullOrEmpty(obj.RequestTime) ? DateTime.ParseExact(obj.RequestTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                            FileName1 = fileUpload.FileName,
                            File1 = "/uploads/files/" + upload.Object.ToString(),
                        };
                        _context.OrderRequestRaws.Add(customerRequest);
                        _context.SaveChanges();
                        msg.Title = "Đã thêm yêu cầu thành công";
                    }
                }
                else
                {
                    var customerRequest = new OrderRequestRaw
                    {
                        Title = obj.Title,
                        Content = obj.Content,
                        Phone = obj.Phone,
                        Email = obj.Email,
                        Keyword = obj.Keyword,
                        Priority = obj.Priority,
                        CreatedTime = DateTime.Now,
                        RequestTime = !string.IsNullOrEmpty(obj.RequestTime) ? DateTime.ParseExact(obj.RequestTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null,
                    };
                    _context.OrderRequestRaws.Add(customerRequest);
                    _context.SaveChanges();
                    msg.Title = "Đã thêm yêu cầu thành công";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Thêm yêu cầu lỗi";
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Update(CustomerRequestModel obj, IFormFile fileUpload)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var data = _context.OrderRequestRaws.FirstOrDefault(x => x.Id == obj.Id);
                if (fileUpload != null)
                {
                    var upload = _upload.UploadFile(fileUpload, Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\files"));
                    if (!upload.Error)
                    {
                        data.Title = obj.Title;
                        data.Content = obj.Content;
                        data.Phone = obj.Phone;
                        data.Email = obj.Email;
                        data.Keyword = obj.Keyword;
                        data.Priority = obj.Priority;
                        data.RequestTime = !string.IsNullOrEmpty(obj.RequestTime) ? DateTime.ParseExact(obj.RequestTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                        data.FileName1 = fileUpload.FileName;
                        data.File1 = "/uploads/files/" + upload.Object.ToString();
                        _context.OrderRequestRaws.Update(data);
                        _context.SaveChanges();
                        msg.Title = "Cập nhật yêu cầu thành công";
                    }
                }
                else
                {
                    data.Title = obj.Title;
                    data.Content = obj.Content;
                    data.Phone = obj.Phone;
                    data.Email = obj.Email;
                    data.Keyword = obj.Keyword;
                    data.Priority = obj.Priority;
                    data.RequestTime = !string.IsNullOrEmpty(obj.RequestTime) ? DateTime.ParseExact(obj.RequestTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                    _context.OrderRequestRaws.Update(data);
                    _context.OrderRequestRaws.Update(data);
                    _context.SaveChanges();
                    msg.Title = "Cập nhật yêu cầu thành công";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Cập nhật yêu cầu lỗi";
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Delete([FromBody]int id)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var data = _context.OrderRequestRaws.FirstOrDefault(x => x.Id == id);
                if (data != null)
                {
                    _context.OrderRequestRaws.Remove(data);
                    _context.SaveChanges();
                    msg.Title = "Xóa yêu cầu thành công!";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "yêu cầu không tồn tại!";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi xóa!";
            }
            return Json(msg);
        }
    }
}