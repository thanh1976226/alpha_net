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

namespace III.Admin.Controllers
{
    public class MaterialJtableModel : JTableModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FromTo { get; set; }
        public string DateTo { get; set; }
    }
    public class MaterialAttributeJtableModel : JTableModel
    {
        public int? ProductId { get; set; }
    }
    public class MaterialFileJtableModel : JTableModel
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Fomart { get; set; }
    }
    [Area("Admin")]
    public class MaterialProductController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        private readonly IHostingEnvironment _hostingEnvironment;


        public MaterialProductController(EIMDBContext context, IUploadService upload, IHostingEnvironment hostingEnvironment)
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
        public object JTable([FromBody]MaterialJtableModel jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            DateTime? fromDate = !string.IsNullOrEmpty(jTablePara.FromTo) ? DateTime.ParseExact(jTablePara.FromTo, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? toDate = !string.IsNullOrEmpty(jTablePara.DateTo) ? DateTime.ParseExact(jTablePara.DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.MaterialProducts.AsNoTracking()
                        join b in _context.MaterialProductGroups on a.GroupCode equals b.Code into b1
                        from b in b1.DefaultIfEmpty()
                        join c in _context.MaterialTypes on a.TypeCode equals c.Code into c1
                        from c in c1.DefaultIfEmpty()
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
                            GroupName = b != null ? b.Name : null,
                            TypeName = c != null ? c.Name : null,
                            Accessory = _context.MaterialAttributes.FirstOrDefault(x => x.ProductId == a.Id) != null ? "1" : "",
                            a.CreatedTime
                        };
            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "ProductCode", "ProductName", "GroupName", "TypeName", "Accessory", "CreatedTime");
            return Json(jdata);
        }
        [HttpPost]
        public object JTableAttribute([FromBody]MaterialAttributeJtableModel jTablePara)
        {

            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            int? Id = jTablePara.ProductId;
            var query = from a in _context.MaterialAttributes
                        where a.ProductId == Id && Id != null
                        select new
                        {
                            a.Id,
                            a.Attribute,
                            a.AttributeValue,
                            a.Note
                        };

            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "Attribute", "AttributeValue", "Note");
            return Json(jdata);
        }
        [HttpPost]
        public object JTableFile([FromBody]MaterialFileJtableModel jTablePara)
        {
            if (string.IsNullOrEmpty(jTablePara.ProductCode))
            {
                var data = new List<object>();
                var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, 0, "Id", "Name", "Fomart", "CreatedBy", "CreatedTime");
                return Json(jdata);
            }
            else
            {
                int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
                var query = from a in _context.MaterialFiles
                            where a.ProductCode == jTablePara.ProductCode
                            && (string.IsNullOrEmpty(jTablePara.Name) || a.Name.ToLower().Contains(jTablePara.Name.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.Fomart) || a.Fomart.ToLower().Contains(jTablePara.Fomart.ToLower()))

                            select new
                            {
                                a.Id,
                                a.Name,
                                a.Fomart,
                                a.Soure,
                                a.CreatedBy,
                                a.CreatedTime
                            };

                var count = query.Count();
                var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
                var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "Name", "Fomart", "Soure", "CreatedBy", "CreatedTime");
                return Json(jdata);
            }
        }
        [HttpPost]
        public object GetAttribute()
        {
            var query = _context.MaterialTypes.AsParallel().Select(x => new { x.Code, x.Name });
            return query;
        }
        [HttpPost]
        public object GetCategoryGroup()
        {
            var query = _context.MaterialProductGroups.AsParallel().Select(x => new { x.Code, x.Name });
            return query;
        }
        [HttpPost]
        public object GetItem(int id)
        {
            var getItem = _context.MaterialProducts.FirstOrDefault(x => x.Id == id);
            return Json(getItem);
        }
        [HttpPost]
        public JsonResult Insert([FromBody]MaterialProduct obj)
        {
            var msg = new JMessage { Title = "", Error = false };
            try
            {
                var model = _context.MaterialProducts.Where(x => x.ProductCode.Equals(obj.ProductCode)).FirstOrDefault();
                if (model == null)
                {
                    obj.CreatedBy = ESEIM.AppContext.UserName;
                    obj.CreatedTime = DateTime.Now;
                    _context.MaterialProducts.Add(obj);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("MLP_MSG_SUPPLIES"));
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_EXITS"), CommonUtil.ResourceValue("MLP_CURD_LBL_SUPPLIES_CODE"));
                }
            }
            catch
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAILED"), CommonUtil.ResourceValue("MLP_MSG_SUPPLIES"));
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Update([FromBody]MaterialProduct obj)
        {
            var msg = new JMessage { Title = "", Error = false };
            try
            {
                var item = _context.MaterialProducts.FirstOrDefault(x => x.ProductCode == obj.ProductCode);
                item.ProductName = obj.ProductName;
                item.Note = obj.Note;
                item.Barcode = obj.Barcode;
                item.GroupCode = obj.GroupCode;
                item.TypeCode = obj.TypeCode;
                item.Status = obj.Status;
                item.UpdatedBy = ESEIM.AppContext.UserName;
                item.UpdatedTime = DateTime.Now;
                _context.MaterialProducts.Update(item);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("MLP_MSG_SUPPLIES"));
            }
            catch
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_FAILED"), CommonUtil.ResourceValue("MLP_MSG_SUPPLIES"));
            }
            return Json(msg);
        }
        [HttpPost]
        public async Task<JsonResult> Delete([FromBody]int id)
        {
            var mess = new JMessage { Error = false, Title = "" };
            try
            {
                var material = await _context.MaterialProducts.FirstOrDefaultAsync(x => x.Id == id);
                material.IsDeleted = true;
                _context.SaveChanges();
                mess.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("MLP_MSG_SUPPLIES"));
            }
            catch (Exception ex)
            {
                mess.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("MLP_MSG_SUPPLIES"));
                mess.Error = true;
            }
            return Json(mess);
        }

        #region Attribute
        [HttpPost]
        public object GetItemAttribute([FromBody]int id)
        {
            var query = _context.MaterialAttributes.Select(x => new { x.Id, x.Attribute, x.AttributeValue, x.Note }).FirstOrDefault(x => x.Id == id);
            return query;
        }
        [HttpPost]
        public JsonResult InsertAttribute([FromBody]MaterialAttribute obj)
        {
            var msg = new JMessage { Title = "", Error = false };
            try
            {
                _context.MaterialAttributes.Add(obj);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("MLP_MSG_PROPETIES_ADD"));
            }
            catch
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAILED"), CommonUtil.ResourceValue("MLP_MSG_PROPETIES_ADD"));
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult UpdateAttribute([FromBody]MaterialAttribute obj)
        {
            var msg = new JMessage { Title = "", Error = false };
            try
            {
                _context.MaterialAttributes.Update(obj);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("MLP_MSG_PROPETIES_ADD"));
            }
            catch
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_FAILED"), CommonUtil.ResourceValue("MLP_MSG_PROPETIES_ADD"));
            }
            return Json(msg);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteAttribute([FromBody]int id)
        {
            var mess = new JMessage { Error = false, Title = "" };
            try
            {
                var material = await _context.MaterialAttributes.FirstOrDefaultAsync(x => x.Id == id);
                _context.MaterialAttributes.Remove(material);
                _context.SaveChanges();
                mess.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("MLP_MSG_PROPETIES_ADD"));
            }
            catch (Exception ex)
            {
                mess.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("MLP_MSG_PROPETIES_ADD"));
                mess.Error = true;
            }
            return Json(mess);
        }
        #endregion

        #region File
        [HttpPost]
        public object GetItemFile([FromBody]int id)
        {
            var query = _context.MaterialFiles.Select(x => new { x.Id, x.Name, x.Soure }).FirstOrDefault(x => x.Id == id);
            return query;
        }

        [HttpPost]
        public JsonResult InsertFile(MaterialFile obj, IFormFile fileUpload)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var upload = _upload.UploadFile(fileUpload, Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\files"));
                if (upload.Error)
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("MLP_MSG_FILE"), CommonUtil.ResourceValue("MLP_MSG_FILE_SERVER_FAIL"));
                }
                else
                {
                    var file = new MaterialFile
                    {
                        Name = obj.Name,
                        Fomart = Path.GetExtension(fileUpload.FileName),
                        CreatedBy = ESEIM.AppContext.UserName,
                        CreatedTime = DateTime.Now,
                        ProductCode = obj.ProductCode,
                        Soure = "/uploads/files/" + upload.Object.ToString(),
                    };
                    _context.MaterialFiles.Add(file);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("MLP_MSG_FILE"), CommonUtil.ResourceValue("MLP_MSG_FILE_SUCCESS"));
                }
            }
            catch (Exception ex)
            {
                msg.Title = String.Format(CommonUtil.ResourceValue("MLP_MSG_FILE"), CommonUtil.ResourceValue("MLP_MSG_FILE_FAIL"));
                msg.Error = true;
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult UpdateFile(MaterialFile obj, IFormFile fileUpload)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                if (fileUpload != null)
                {
                    var upload = _upload.UploadFile(fileUpload, Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\files"));
                    if (upload.Error)
                    {
                        msg.Error = true;
                        msg.Title = String.Format(CommonUtil.ResourceValue("MLP_MSG_FILE"), CommonUtil.ResourceValue("MLP_MSG_FILE_SERVER_FAIL"));
                    }
                    else
                    {
                        var getItem = _context.MaterialFiles.FirstOrDefault(x => x.Id == obj.Id);
                        getItem.Name = obj.Name;
                        getItem.Soure = "/uploads/files/" + upload.Object.ToString();
                        _context.MaterialFiles.Update(getItem);
                        _context.SaveChanges();
                        msg.Title = String.Format(CommonUtil.ResourceValue("MLP_MSG_FILE_UPDATE"), CommonUtil.ResourceValue("MLP_MSG_FILE_SUCCESS"));
                    }
                }
                else
                {
                    var getItem = _context.MaterialFiles.FirstOrDefault(x => x.Id == obj.Id);
                    getItem.Name = obj.Name;
                    _context.MaterialFiles.Update(getItem);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("MLP_MSG_FILE_UPDATE"), CommonUtil.ResourceValue("MLP_MSG_FILE_SUCCESS"));
                }
            }
            catch (Exception ex)
            {
                msg.Title = String.Format(CommonUtil.ResourceValue("MLP_MSG_FILE_UPDATE"), CommonUtil.ResourceValue("MLP_MSG_FILE_FAIL"));
                msg.Error = true;
            }
            return Json(msg);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteFile([FromBody]int id)
        {
            var mess = new JMessage { Error = false, Title = "" };
            try
            {
                var file = await _context.MaterialFiles.FirstOrDefaultAsync(x => x.Id == id);
                _context.MaterialFiles.Remove(file);
                _context.SaveChanges();
                mess.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("MLP_FILE"));
            }
            catch (Exception ex)
            {
                mess.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("MLP_FILE"));
                mess.Error = true;
            }
            return Json(mess);
        }
        #endregion
    }
}