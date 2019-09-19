
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
    public class UI1Controller : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly EIMDBContext _context;
        private readonly ILogger _logger;
        private readonly IActionLogService _actionLog;
        private readonly AppSettings _appSettings;
        public class JTableModelCustom : JTableModel
        {
            public string Key { get; set; }
            public string Key1 { get; set; }
            public string Key2 { get; set; }
        }
        public UI1Controller(EIMDBContext context, IOptions<AppSettings> appSettings, IHostingEnvironment hostingEnvironment, IActionLogService actionLog)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _actionLog = actionLog;
            _appSettings = appSettings.Value;
        }
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public object JTable([FromBody]JTableModelCustom jTablePara)
        {

            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.Jnana_news_cat
                        where a.cat_status == 1 && (jTablePara.Key == null || jTablePara.Key == "" || a.cat_title.ToLower().Contains(jTablePara.Key.ToLower()))
                        &&(jTablePara.Key1 == null || jTablePara.Key1 == "" || (a.created_time > DateTime.Parse(jTablePara.Key1)))
                        && (jTablePara.Key2 == null || jTablePara.Key2 == "" || (a.created_time < DateTime.Parse(jTablePara.Key2)))
                        select new
                        {
                            id = a.id,
                            cat_code = a.cat_code,
                            cat_title = a.cat_title,
                            cat_description = a.cat_description,
                            cat_parent_code = a.cat_parent_code,
                            created_time=a.created_time,
                            cat_avarta = a.cat_avarta
                        };

            var count = query.Count();
            var data = query
                .OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "cat_code", "cat_title", "cat_description", "created_time", "cat_parent_code", "cat_avarta");
            return Json(jdata);
        }

        public async Task<JsonResult> Insert(Jnana_news_cat obj, IFormFile cat_avarta)
        {

            var msg = new JMessage() { Error = false, ID = 1 };
            try
            {
                Jnana_news_cat rs = _context.Jnana_news_cat.FirstOrDefault(x => x.cat_code == obj.cat_code);
                if (rs == null)
                {

                    var iccat_avarta = "";

                    if (cat_avarta != null && cat_avarta.Length > 0)
                    {
                        var pathUpload = Path.Combine(_hostingEnvironment.WebRootPath, "cat_avartas\\avatar");
                        if (!Directory.Exists(pathUpload)) Directory.CreateDirectory(pathUpload);

                        var fileName = DateTimeOffset.Now.ToUnixTimeMilliseconds() + cat_avarta.FileName;
                        var filePath = Path.Combine(pathUpload, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await cat_avarta.CopyToAsync(stream);
                        }
                        iccat_avarta = "/cat_avartas/avatar/" + fileName;
                    }
                    if (iccat_avarta != "")
                    {
                        obj.cat_avarta = iccat_avarta;
                    }


                    obj.created_time = DateTime.Now;
                    obj.cat_status = 1;
                    _context.Jnana_news_cat.Add(obj);
                    _context.SaveChanges();

                    msg.Title = "Thêm mới danh mục thành công";
                    msg.Error = false;
                    _actionLog.InsertActionLog("Jnana_news_cat", "Insert new category successfully", null, obj, "Insert");
                }
                else
                {

                    msg.Title = "Mã đã tồn tại";
                    msg.Error = true;
                }
            }
            catch (Exception ex)
            {
                msg.ID = 0;
                msg.Error = true;
                msg.Object = ex;
                msg.Title = "Có lỗi khi thêm khoản mục";
                _actionLog.InsertActionLog("news_category", "Insert new category fail", null, obj, "Insert");
            }
            return Json(msg);
        }
        public async Task<JsonResult> Update(Jnana_news_cat obj, IFormFile cat_avarta)
        {
            var msg = new JMessage() { Error = false, ID = 1 };
            try
            {

                Jnana_news_cat rs = _context.Jnana_news_cat.FirstOrDefault(x => x.id == obj.id);
                if (rs != null)
                {

                    var iccat_avarta = "";

                    if (cat_avarta != null && cat_avarta.Length > 0)
                    {
                        var pathUpload = Path.Combine(_hostingEnvironment.WebRootPath, "cat_avartas\\avatar");
                        if (!Directory.Exists(pathUpload)) Directory.CreateDirectory(pathUpload);

                        var fileName = DateTimeOffset.Now.ToUnixTimeMilliseconds() + cat_avarta.FileName;
                        var filePath = Path.Combine(pathUpload, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await cat_avarta.CopyToAsync(stream);
                        }
                        iccat_avarta = "/cat_avartas/avatar/" + fileName;
                    }
                    if (iccat_avarta != "")
                    {
                        rs.cat_avarta = iccat_avarta;
                    }
                    var query = from a in _context.Jnana_news_articles
                                join b in _context.Jnana_news_cat
                                on a.cat_code equals b.cat_code
                                where b.cat_code == rs.cat_code
                                select a;
                    foreach (var item in query)
                    {
                        item.cat_code = obj.cat_code;
                        item.update_time = DateTime.Now;
                        _context.Jnana_news_articles.Update(item);
                    }
                    rs.cat_code = obj.cat_code;
                    rs.cat_title = obj.cat_title;
                    rs.cat_description = obj.cat_description;
                    rs.cat_parent_code = obj.cat_parent_code;
                    rs.update_time = DateTime.Now;
                    _context.Jnana_news_cat.Update(rs);
                    _context.SaveChanges();
                    msg.Title = "Sửa thông tin danh mục thành công";
                    msg.Error = false;
                    _actionLog.InsertActionLog("Jnana_news_cat", "update category successfully", rs, obj, "Update");
                }
                else
                {
                    msg.Title = "Xảy ra lỗi, vui lòng thử lại.";
                    msg.Error = true;
                }


            }
            catch (Exception ex)
            {
                msg.ID = 0;
                msg.Error = true;
                msg.Object = ex;
                msg.Title = "Có lỗi khi sửa khoản mục";
                _actionLog.InsertActionLog("Jnana_news_cat", "update category fail", null, obj, "Update");
            }
            return Json(msg);
        }
        [HttpGet]
        public object GetItem(int? id)
        {

            if (id == null || id < 0)
            {
                return Json("");
            }
            var a = _context.Jnana_news_cat.AsNoTracking().Single(m => m.id == id);
            return Json(a);
        }
        [HttpPost]
        public object Delete(int id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.Jnana_news_cat.FirstOrDefault(x => x.id == id);
                data.cat_status = 0;
                _context.Jnana_news_cat.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Xóa danh mục thành công!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi khi xóa!";
                return Json(msg);
            }
        }
        [HttpPost]
        public object gettreedataCategory()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.Jnana_news_cat.OrderBy(x => x.id);
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
        public object DeleteItems([FromBody]List<int> listIdI)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                foreach (var id in listIdI)
                {
                    Jnana_news_cat obj = _context.Jnana_news_cat.FirstOrDefault(x => x.id == id);
                    if (obj != null)
                    {
                        obj.cat_status = 0;
                        _context.Jnana_news_cat.Update(obj);
                        _context.SaveChanges();
                    }
                }
                msg.Title = "Xóa danh mục thành công!";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_LIST_FAIL"), CommonUtil.ResourceValue("RESOURCE"));
            }
            return Json(msg);
        }



    }
}