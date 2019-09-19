using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using III.Admin.Controllers;

namespace ESEIM.Controllers
{
    [Area("Admin")]
    public class CategoryController : BaseController
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly EIMDBContext _context;
        private readonly ILogger _logger;
        private readonly IActionLogService _actionLog;
        private readonly AppSettings _appSettings;
        private readonly IUploadService _upload;


        public class JTableModelCustom : JTableModel
        {
            public string productcode { get; set; }
            public string productname { get; set; }
            public string unit { get; set; }
            public string describe { get; set; }
            public string FilePath { get; set; }
        }
        public CategoryController(EIMDBContext context, IUploadService upload, ILogger<CategoryController> logger, IOptions<AppSettings> appSettings, IHostingEnvironment hostingEnvironment, IActionLogService actionLog)
        {

            _context = context;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _actionLog = actionLog;
            _appSettings = appSettings.Value;
            _upload = upload;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public object JTable([FromBody]JTableModelCustom jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            var query = from a in _context.ProductCats

                        join b in listCommon on a.Unit equals b.CodeSet into b1
                        from b in b1.DefaultIfEmpty()
                        join c in listCommon on a.ProductGroup equals c.CodeSet into c1
                        from c in c1.DefaultIfEmpty()
                            //join b in _context.cms_extra_fields_groups on a.Group equals b.Id
                            //orderby b.Name
                        where (jTablePara.productcode == null || jTablePara.productcode == "" || a.ProductCode.ToLower().Contains(jTablePara.productcode.ToLower()))
                        && (jTablePara.productname == null || jTablePara.productname == "" || a.ProductName.ToString().ToLower().Contains(jTablePara.productname.ToLower()))
                        && (string.IsNullOrEmpty(jTablePara.unit) || a.Unit == jTablePara.unit)
                        && (jTablePara.describe == null || jTablePara.describe == "" || a.Note.ToLower().Contains(jTablePara.describe.ToLower()))
                        orderby a.ProductID
                        select new
                        {
                            //idd=test(),
                            id = a.ProductID,
                            productcode = a.ProductCode,
                            productname = a.ProductName,
                            unit = b != null ? b.ValueSet : "Không xác định",
                            productgroup = c != null ? c.ValueSet : "Không xác định",
                            pathimg = a.PathImg,
                            note = a.Note,

                        };
            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "productcode", "productname", "unit", "pathimg", "note", "productgroup");

            return Json(jdata);
        }

        //--------------------------------thêm mới---------------------------   
        [HttpPost]
        public object Insert([FromBody]ProductCat obj)
        {
            var msg = new JMessage();
            try
            {
                if (_context.ProductCats.FirstOrDefault(x=>x.ProductCode==obj.ProductCode)==null)
                {
                    ProductCat obj1 = new ProductCat();
                    obj1.ProductCode = obj.ProductCode;
                    obj1.ProductName = obj.ProductName;
                    obj1.ProductGroup = obj.ProductGroup;
                    obj1.Unit = obj.Unit;
                    obj1.Note = obj.Note;
                    obj1.PathImg = obj.PathImg;
                    //obj1.ProductGroup = null;s
                    obj1.CreatedBy = ESEIM.AppContext.UserName;
                    obj1.UpdatedBy = null;
                    obj1.CreatedTime = DateTime.Now.Date;
                    obj1.UpdatedTime = null;
                    obj1.DeletedBy = null;
                    obj1.DeletedTime = null;
                    obj1.IsDeleted = true;


                    _context.ProductCats.Add(obj1);
                    _context.SaveChanges();
                    msg.Error = false;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("CATEGORY_MSG_PRODUCT"));
                    //return msg;
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("CATEGORY_MSG_PRODUCT"));
                    //return msg;
                }
               
            }
            catch
            {
                msg.Error = true;
                //msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_ADD"));
            }
            return msg;

        }

        [HttpPost]
        public object Update([FromBody]ProductCat obj)
        {
            var msg = new JMessage();
            try
            {
                //var data = _context.EDMSCustomer.Where(x => x.CusCode == obj.CusCode).FirstOrDefault();
                obj.UpdatedTime = DateTime.Now.Date;
                obj.UpdatedBy = ESEIM.AppContext.UserName;
                obj.PathImg = obj.PathImg;

                _context.ProductCats.Update(obj);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("CATEGORY_MSG_PRODUCT"));

                return msg;
            }
            catch
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_FAILED"), CommonUtil.ResourceValue("CATEGORY_MSG_PRODUCT"));
                return msg;
            }

        }


        //----------------------------------------------Xóa-------------------------------
        [HttpPost]
        public object Delete(int Id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.ProductCats.FirstOrDefault(x => x.ProductID == Id);
                _context.ProductCats.Remove(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("CATEGORY_MSG_PRODUCT"));
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("CATEGORY_MSG_PRODUCT"));
                return Json(msg);
            }
        }


        public object GetItemDetail(int id)
        {
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            //ProductCat query=new ProductCat();
            var query = from ad in _context.ProductCats

                        join b in listCommon on ad.Unit equals b.CodeSet into b1
                        from b in b1.DefaultIfEmpty()
                        join c in listCommon on ad.ProductGroup equals c.CodeSet into c1
                        from c in c1.DefaultIfEmpty()
                        where ad.ProductID == id
                        select new
                        {
                            ProductCode = ad.ProductCode,
                            ProductName = ad.ProductName,
                            PathImg = ad.PathImg,
                            Note = ad.Note,
                            Unit = b != null ? b.ValueSet : "Không xác định",
                            ProductGroup = c != null ? c.ValueSet : "Không xác định",
                        };
            ProductCat query1 = new ProductCat();

            //var a = _context.ProductCats.AsNoTracking().Single(m => m.ProductID == id);
            return Json(query);
        }
        public object GetItem(int id)
        {

            var a = _context.ProductCats.AsNoTracking().Single(m => m.ProductID == id);
            return Json(a);
        }

        [HttpPost]
        public object GetProductUnit()
        {

            var data = _context.CommonSettings.Where(x => x.Group == "UNIT").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return data;


        }

        [HttpPost]

        public object GetProductGroup()
        {

            var data = _context.CommonSettings.Where(x => x.Group == "PRODUCT_GROUP").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return data;


        }

        [HttpPost]
        public object UploadImage(IFormFile fileUpload)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var upload = _upload.UploadImage(fileUpload);
                msg.Object = upload.Object;
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("CATEGORY_MSG_UPLOAD_FILE"));
            }
            return Json(msg);
        }
    }
}