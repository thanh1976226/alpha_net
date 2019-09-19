using System;
using System.Collections.Generic;
using System.Globalization;
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
    [Area("Admin")]
    public class EDMSMaterialJtableModel : JTableModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FromTo { get; set; }
        public string DateTo { get; set; }
    }
    public class EDMSMaterialAttributeJtableModel : JTableModel
    {
        public int? ProductId { get; set; }
    }
    public class EDMSMaterialFileJtableModel : JTableModel
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Fomart { get; set; }
    }
    public class EDMSMaterialProductController : Controller
    {
        private readonly EIMDBContext _context;

        public EDMSMaterialProductController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public object JTable([FromBody]EDMSMaterialJtableModel jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            DateTime? fromDate = !string.IsNullOrEmpty(jTablePara.FromTo) ? DateTime.ParseExact(jTablePara.FromTo, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? toDate = !string.IsNullOrEmpty(jTablePara.DateTo) ? DateTime.ParseExact(jTablePara.DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.EDMSMaterialProducts.AsNoTracking()
                        where !a.IsDeleted
                            && (string.IsNullOrEmpty(jTablePara.Code) || a.ProductCode.ToLower().Contains(jTablePara.Code.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.Name) || a.ProductName.ToLower().Contains(jTablePara.Name.ToLower()))
                            && ((fromDate == null) || (a.CreatedTime.HasValue && a.CreatedTime.Value.Date >= fromDate))
                            && ((toDate == null) || (a.CreatedTime.HasValue && a.CreatedTime.Value.Date <= toDate))
                        select new
                        {
                            a.Id,
                            a.ProductCode,
                            a.ProductName,
                            a.CreatedTime
                        };
            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "ProductCode", "ProductName", "CreatedTime");
            return Json(jdata);
        }
        [HttpPost]
        public object GetItem(int id)
        {
            var getItem = _context.EDMSMaterialProducts.FirstOrDefault(x => x.Id==id);
            return Json(getItem);
        }
        [HttpPost]
        public JsonResult Insert([FromBody]EDMSMaterialProduct obj)
        {
            var msg = new JMessage { Title = "", Error = false };
            try
            {
                var model = _context.EDMSMaterialProducts.Where(x => x.ProductCode.Equals(obj.ProductCode)).FirstOrDefault();
                if (model == null)
                {
                    //obj.CreatedBy = ESEIM.AppContext.UserName;
                    obj.CreatedTime = DateTime.Now;
                    _context.EDMSMaterialProducts.Add(obj);
                    _context.SaveChanges();
                    msg.Title = "Thêm vật tư thành công";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Mã vật tư đã tồn tại!";
                }
            }
            catch
            {
                msg.Error = true;
                msg.Title = "Thêm vật tư lỗi!";
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Update([FromBody]EDMSMaterialProduct obj)
        {
            var msg = new JMessage { Title = "", Error = false };
            try
            {
                var item = _context.EDMSMaterialProducts.FirstOrDefault(x => x.ProductCode == obj.ProductCode);
                item.ProductName = obj.ProductName;
                item.Note = obj.Note;
                item.Barcode = obj.Barcode;
                item.GroupCode = obj.GroupCode;
                item.TypeCode = obj.TypeCode;
                item.Status = obj.Status;
                //item.UpdatedBy = ESEIM.AppContext.UserName;
                item.UpdatedTime = DateTime.Now;
                _context.EDMSMaterialProducts.Update(item);
                _context.SaveChanges();
                msg.Title = "Cập nhập vật tư thành công";
            }
            catch
            {
                msg.Error = true;
                msg.Title = "Cập nhập vật tư lỗi!";
            }
            return Json(msg);
        }
        [HttpPost]
        public async Task<JsonResult> Delete([FromBody]int id)
        {
            var mess = new JMessage { Error = false, Title = "" };
            try
            {
                var material = await _context.EDMSMaterialProducts.FirstOrDefaultAsync(x => x.Id == id);
                material.IsDeleted = true;
                _context.SaveChanges();
                mess.Title = "Xóa vật tư thành công";
            }
            catch (Exception ex)
            {
                mess.Title = "Xóa vật tư lỗi";
                mess.Error = true;
            }
            return Json(mess);
        }
        
    }
}