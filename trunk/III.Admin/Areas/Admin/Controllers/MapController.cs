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
    public class MapController : BaseController
    {
        public class JTableModelCustomer : JTableModel
        {
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public string CustomerEmail { get; set; }
            public string CustomerGroup { get; set; }
            public string CustomerActivityStatus { get; set; }
            public string Address { get; set; }
        }
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;

        public MapController(EIMDBContext context, IUploadService upload)
        {
            _context = context;
            _upload = upload;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public object GetAll()
        {
            var a = _context.MapDataGpss.OrderBy(x => x.Id).AsNoTracking().ToList();
            return Json(a);
        }

        [HttpPost]
        public JsonResult Insert(PackingModel obj, IFormFile pictureFile)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                if (pictureFile != null && pictureFile.Length > 0)
                {
                    _upload.UploadImage(pictureFile);
                }
                var model = new MapDataGps
                {
                    Title = obj.title,
                    Icon = obj.Icon,
                    //Icon = obj.Icon.Contains("SHOP") ? SHOP_ICON : obj.Icon.Contains("PAGODA") ? PAGODA_ICON : obj.Icon.Contains("PARK") ? PARK_ICON : obj.Icon.Contains("COMPANY") ? COMPANY_ICON : null,
                    PolygonGPS = obj.Gis_data,
                };
                _context.MapDataGpss.Add(model);
                _context.SaveChanges();
                msg.Title = "Thêm mới địa điểm thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Thêm mới địa điểm lỗi";
            }
            return Json(msg);
        }






        #region Customer
        [HttpPost]
        public object searchCustomer([FromBody]MapSearch search)
        {
            var customerCode = search.CustomerCode != null ? search.CustomerCode.ToLower() : "";
            var areas = search.areas;
            var groups = search.groups;
            var types = search.types;
            var roles = search.roles;
            var query = (from a in _context.Customers
                         join b1 in _context.CommonSettings on a.Area equals b1.CodeSet into b2
                         from b in b2.DefaultIfEmpty()
                         join c1 in _context.CommonSettings on a.CusGroup equals c1.CodeSet into c2
                         from c in c2.DefaultIfEmpty()
                         join d1 in _context.CommonSettings on a.CusType equals d1.CodeSet into d2
                         from d in d2.DefaultIfEmpty()
                         join e1 in _context.CommonSettings on a.Role equals e1.CodeSet into e2
                         from e in e2.DefaultIfEmpty()
                         where (string.IsNullOrEmpty(search.CustomerCode) || a.CusCode.ToLower().Contains(customerCode))
                         && (areas.Count == 0 || areas.Contains(a.Area))
                         && (groups.Count == 0 || groups.Contains(a.CusGroup))
                         && (types.Count == 0 || types.Contains(a.CusType))
                         && (roles.Count == 0 || roles.Contains(a.Role))
                         && (!string.IsNullOrEmpty(a.GoogleMap))
                         select new
                         {
                             a.CusID,
                             a.CusCode,
                             a.CusName,
                             a.Area,
                             a.CusGroup,
                             a.Role,
                             a.CusType,
                             a.ActivityStatus,
                             AreaTxt = b != null ? b.ValueSet : "",
                             CusGroupTxt = c != null ? c.ValueSet : "",
                             CusTypeTxt = d != null ? d.ValueSet : "",
                             RoleTxt = e != null ? e.ValueSet : "",
                             a.GoogleMap,
                             a.Address
                         }).ToList();
            var group = query.GroupBy(x => x.CusID).ToList();
            var list = new List<Object>();
            foreach (var item in group)
            {
                foreach (var item1 in item)
                {
                    var dt = new
                    {
                        item1.CusID,
                        item1.CusCode,
                        item1.CusName,
                        item1.Area,
                        item1.CusGroup,
                        item1.Role,
                        item1.CusType,
                        item1.ActivityStatus,
                        item1.AreaTxt,
                        item1.CusGroupTxt,
                        item1.CusTypeTxt,
                        item1.RoleTxt,
                        item1.GoogleMap,
                        item1.Address
                    };
                    list.Add(dt);
                    break;
                }
            }
            return Json(list);
        }



        [HttpPost]
        public object GetCustomerGroup()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "CUSTOMER_GROUP").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return data;
        }

        [HttpPost]
        public object GetCustomerStatus()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "STATUS").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return data;
        }

        [HttpPost]
        public JsonResult GetListArea()
        {
            var msg = new JMessage() { Error = false };

            var data = from a in _context.CommonSettings
                       where a.Group == "AREA" && a.IsDeleted == false
                       select new
                       {
                           Code = a.CodeSet,
                           Name = a.ValueSet,
                       };
            msg.Object = data;

            return Json(msg);
        }
        [HttpPost]
        public JsonResult GetListCutomerType()
        {
            var msg = new JMessage() { Error = false };

            var data = from a in _context.CommonSettings
                       where a.Group == "TYPE" && a.IsDeleted == false
                       select new
                       {
                           Code = a.CodeSet,
                           Name = a.ValueSet,
                       };
            msg.Object = data;

            return Json(msg);
        }
        [HttpPost]
        public JsonResult GetListCutomerRole()
        {
            var msg = new JMessage() { Error = false };

            var data = from a in _context.CommonSettings
                       where a.Group == "ROLE" && a.IsDeleted == false
                       select new
                       {
                           Code = a.CodeSet,
                           Name = a.ValueSet,
                       };
            msg.Object = data;

            return Json(msg);
        }
        [HttpPost]
        public JsonResult GetAllCustomer()
        {
            var msg = new JMessage() { Error = false };

            var data = from a in _context.Customers
                       where a.IsDeleted == false
                       select new
                       {
                           Code = a.CusCode,
                           Name = a.CusName,
                       };
            msg.Object = data;
            return Json(msg);
        }
        #endregion

        #region Supplier 
        [HttpPost]
        public object searchSupplier([FromBody]MapSearch search)
        {
            var customerCode = search.CustomerCode != null ? search.CustomerCode.ToLower() : "";
            var areas = search.areas;
            var groups = search.groups;
            var types = search.types;
            var roles = search.roles;
            var query = (from a in _context.Customers
                         join b1 in _context.CommonSettings on a.Area equals b1.CodeSet into b2
                         from b in b2.DefaultIfEmpty()
                         join c1 in _context.CommonSettings on a.CusGroup equals c1.CodeSet into c2
                         from c in c2.DefaultIfEmpty()
                         join d1 in _context.CommonSettings on a.CusType equals d1.CodeSet into d2
                         from d in d2.DefaultIfEmpty()
                         join e1 in _context.CommonSettings on a.Role equals e1.CodeSet into e2
                         from e in e2.DefaultIfEmpty()
                         where (string.IsNullOrEmpty(search.CustomerCode) || a.CusCode.ToLower().Contains(customerCode))
                         && (areas.Count == 0 || areas.Contains(a.Area))
                         && (groups.Count == 0 || groups.Contains(a.CusGroup))
                         && (types.Count == 0 || types.Contains(a.CusType))
                         && (roles.Count == 0 || roles.Contains(a.Role))
                         && (!string.IsNullOrEmpty(a.GoogleMap))
                         select new
                         {
                             a.CusID,
                             a.CusCode,
                             a.CusName,
                             a.Area,
                             a.CusGroup,
                             a.Role,
                             a.CusType,
                             a.ActivityStatus,
                             AreaTxt = b != null ? b.ValueSet : "",
                             CusGroupTxt = c != null ? c.ValueSet : "",
                             CusTypeTxt = d != null ? d.ValueSet : "",
                             RoleTxt = e != null ? e.ValueSet : "",
                             a.GoogleMap,
                             a.Address
                         }).ToList();
            var group = query.GroupBy(x => x.CusID).ToList();
            var list = new List<Object>();
            foreach (var item in group)
            {
                foreach (var item1 in item)
                {
                    var dt = new
                    {
                        item1.CusID,
                        item1.CusCode,
                        item1.CusName,
                        item1.Area,
                        item1.CusGroup,
                        item1.Role,
                        item1.CusType,
                        item1.ActivityStatus,
                        item1.AreaTxt,
                        item1.CusGroupTxt,
                        item1.CusTypeTxt,
                        item1.RoleTxt,
                        item1.GoogleMap,
                        item1.Address
                    };
                    list.Add(dt);
                    break;
                }
            }
            return Json(list);
        }

        [HttpPost]
        public object GetSupplierGroup()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "CUSTOMER_GROUP").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return data;
        }

        [HttpPost]
        public object GetSupplierStatus()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "STATUS").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return data;
        }

        [HttpPost]
        public JsonResult GetListAreaSupplier()
        {
            var msg = new JMessage() { Error = false };

            var data = from a in _context.CommonSettings
                       where a.Group == "AREA" && a.IsDeleted == false
                       select new
                       {
                           Code = a.CodeSet,
                           Name = a.ValueSet,
                       };
            msg.Object = data;

            return Json(msg);
        }
        [HttpPost]
        public JsonResult GetListSupplierType()
        {
            var msg = new JMessage() { Error = false };

            var data = from a in _context.CommonSettings
                       where a.Group == "TYPE" && a.IsDeleted == false
                       select new
                       {
                           Code = a.CodeSet,
                           Name = a.ValueSet,
                       };
            msg.Object = data;

            return Json(msg);
        }
        [HttpPost]
        public JsonResult GetListSupplierRole()
        {
            var msg = new JMessage() { Error = false };

            var data = from a in _context.CommonSettings
                       where a.Group == "ROLE" && a.IsDeleted == false
                       select new
                       {
                           Code = a.CodeSet,
                           Name = a.ValueSet,
                       };
            msg.Object = data;

            return Json(msg);
        }
        #endregion



        //[HttpPost]
        //public object JTable([FromBody]JTableModelCustomer jTablePara)
        //{
        //    int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
        //    var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
        //    var query = from a in _context.Customers
        //                join b in listCommon on a.CusGroup equals b.CodeSet into b1
        //                from b in b1.DefaultIfEmpty()
        //                join c in listCommon on a.ActivityStatus equals c.CodeSet into c1
        //                from c in c1.DefaultIfEmpty()
        //                where (a.IsDeleted == false)
        //                    && (string.IsNullOrEmpty(jTablePara.CustomerCode) || a.CusCode.ToLower().Contains(jTablePara.CustomerCode.ToLower()))
        //                    && (string.IsNullOrEmpty(jTablePara.CustomerName) || a.CusName.ToLower().Contains(jTablePara.CustomerName.ToLower()))
        //                    && (string.IsNullOrEmpty(jTablePara.CustomerEmail) || a.Email.ToLower().Contains(jTablePara.CustomerEmail.ToLower()))
        //                    && (string.IsNullOrEmpty(jTablePara.CustomerPhone) || a.MobilePhone.ToLower().Contains(jTablePara.CustomerPhone.ToLower()))
        //                    && (string.IsNullOrEmpty(jTablePara.CustomerActivityStatus) || a.ActivityStatus == jTablePara.CustomerActivityStatus)
        //                    && (string.IsNullOrEmpty(jTablePara.CustomerGroup) || a.CusGroup.ToLower() == jTablePara.CustomerGroup.ToLower())
        //                    && (string.IsNullOrEmpty(jTablePara.Address) || a.Address.ToLower().Contains(jTablePara.Address.ToLower()))
        //                select new
        //                {
        //                    id = a.CusID,
        //                    cusCode = a.CusCode,
        //                    cusName = a.CusName,
        //                    cusEmail = a.Email,
        //                    cusAddress = a.Address,
        //                    cusMobilePhone = a.MobilePhone,
        //                    cusGroup = b != null ? b.ValueSet : "",
        //                    cusActivity = c != null ? c.CodeSet == "ACTIVE" ? c.CodeSet : c.CodeSet == "DEACTIVE" ? c.CodeSet : c.ValueSet : "Không xác định",
        //                };
        //    var count = query.Count();
        //    var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
        //    var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "cusCode", "cusName", "cusEmail", "cusAddress", "cusMobilePhone", "cusGroup", "cusActivity");
        //    return Json(jdata);
        //}
        //[HttpPost]
        //public object search([FromBody]MapSearch search)
        //{
        //    var customerCode = search.CustomerCode!=null? search.CustomerCode.ToLower():"";
        //    var areas= search.areas;
        //    var groups= search.groups;
        //    var types =search.types;
        //    var roles = search.roles;
        //    var query = (from a in _context.Customers
        //                 join b1 in _context.CommonSettings on a.Area equals b1.CodeSet into b2
        //                 from b in b2.DefaultIfEmpty()
        //                 join c1 in _context.CommonSettings on a.CusGroup equals c1.CodeSet into c2
        //                 from c in c2.DefaultIfEmpty()
        //                 join d1 in _context.CommonSettings on a.CusType equals d1.CodeSet into d2
        //                 from d in d2.DefaultIfEmpty()
        //                 join e1 in _context.CommonSettings on a.Role equals e1.CodeSet into e2
        //                 from e in e2.DefaultIfEmpty()
        //                 where (string.IsNullOrEmpty(search.CustomerCode) || a.CusCode.ToLower().Contains(customerCode))
        //                 && (areas.Count==0||areas.Contains(a.Area))
        //                 && (groups.Count == 0 || groups.Contains(a.CusGroup))
        //                 && (types.Count == 0 || types.Contains(a.CusType))
        //                 && (roles.Count == 0 || roles.Contains(a.Role))
        //                 && (!string.IsNullOrEmpty(a.GoogleMap))
        //                 select new
        //                 {
        //                     a.CusID,
        //                     a.CusCode,
        //                     a.CusName,
        //                     a.Area,
        //                     a.CusGroup,
        //                     a.Role,
        //                     a.CusType,
        //                     a.ActivityStatus,
        //                     AreaTxt = b!=null?b.ValueSet:"",
        //                     CusGroupTxt = c!=null?c.ValueSet:"",
        //                     CusTypeTxt = d != null ? d.ValueSet : "",
        //                     RoleTxt = e != null ? e.ValueSet : "",
        //                     a.GoogleMap,
        //                     a.Address
        //                 }).ToList();
        //    var group = query.GroupBy(x => x.CusID).ToList();
        //    var list = new List<Object>();
        //    foreach(var item in group)
        //    {
        //        foreach (var item1 in item)
        //        {
        //            var dt = new  {
        //                item1.CusID,
        //                item1.CusCode,
        //                item1.CusName,
        //                item1.Area,
        //                item1.CusGroup,
        //                item1.Role,
        //                item1.CusType,
        //                item1.ActivityStatus,
        //                item1.AreaTxt,
        //                item1.CusGroupTxt,
        //                item1.CusTypeTxt,
        //                item1.RoleTxt,
        //                item1.GoogleMap,
        //                item1.Address
        //            };
        //            list.Add(dt);
        //            break;
        //        }
        //    }
        //    return Json(list);
        //}
        //[HttpPost]
        //public JsonResult Insert([FromBody]EDMSCustomer obj)
        //{
        //    var msg = new JMessage { Error = false, Title = "" };
        //    try
        //    {
        //        var checkExist = _context.Customers.FirstOrDefault(x =>!x.IsDeleted && x.CusCode.ToLower() == obj.CusCode.ToLower());
        //        if (checkExist != null)
        //        {
        //            obj.CreatedBy = ESEIM.AppContext.UserName;
        //            obj.CreatedTime = DateTime.Now;
        //            _context.Customers.Add(obj);
        //            _context.SaveChanges();
        //            msg.Title = "Thêm khách hàng thành công!";
        //        }
        //        else
        //        {
        //            msg.Error = true;
        //            msg.Title = "Mã khách hàng đã tồn tại!";
        //        }
        //        return Json(msg);
        //    }
        //    catch
        //    {
        //        msg.Error = true;
        //        //msg.Object = ex;
        //        msg.Title = "Có lỗi khi thêm khách hàng!";
        //        return Json(msg);
        //    }

        //}

        //[HttpPost]
        //public JsonResult Update([FromBody]EDMSCustomer obj)
        //{
        //    var msg = new JMessage();
        //    try
        //    {
        //        obj.UpdatedTime = DateTime.Now.Date;
        //        _context.Customers.Update(obj);
        //        _context.SaveChanges();
        //        msg.Error = false;
        //        msg.Title = "Đã lưu thay đổi";
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra!";
        //    }
        //    return Json(msg);
        //}

        //[HttpPost]
        //public JsonResult Delete(int id)
        //{
        //    var msg = new JMessage();
        //    try
        //    {
        //        var data = _context.Customers.FirstOrDefault(
        //                x => x.CusID == id
        //            );
        //        data.IsDeleted = true;
        //        _context.Customers.Update(data);
        //        _context.SaveChanges();
        //        msg.Error = false;
        //        msg.Title = "Xóa thành công!";
        //    }
        //    catch
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra!";
        //    }
        //    return Json(msg);
        //}

        //[HttpGet]
        //public object GetItem(int id)
        //{
        //    var a = _context.Customers.AsNoTracking().Single(m => m.CusID == id);
        //    return Json(a);
        //}



        //#region Combobox
        //[HttpPost]
        //public object GetCustomerGroup()
        //{
        //    var data = _context.CommonSettings.Where(x => x.Group == "CUSTOMER_GROUP").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
        //    return data;
        //}

        //[HttpPost]
        //public object GetCustomerStatus()
        //{
        //    var data = _context.CommonSettings.Where(x => x.Group == "STATUS").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
        //    return data;
        //}

        //[HttpPost]
        //public JsonResult GetListArea()
        //{
        //    var msg = new JMessage() { Error = false };

        //    var data = from a in _context.CommonSettings
        //               where a.Group == "AREA" && a.IsDeleted == false
        //               select new
        //               {
        //                   Code = a.CodeSet,
        //                   Name = a.ValueSet,
        //               };
        //    msg.Object = data;

        //    return Json(msg);
        //}
        //[HttpPost]
        //public JsonResult GetListCutomerType()
        //{
        //    var msg = new JMessage() { Error = false };

        //    var data = from a in _context.CommonSettings
        //               where a.Group == "TYPE" && a.IsDeleted == false
        //               select new
        //               {
        //                   Code = a.CodeSet,
        //                   Name = a.ValueSet,
        //               };
        //    msg.Object = data;

        //    return Json(msg);
        //}
        //[HttpPost]
        //public JsonResult GetListCutomerRole()
        //{
        //    var msg = new JMessage() { Error = false };

        //    var data = from a in _context.CommonSettings
        //               where a.Group == "ROLE" && a.IsDeleted == false
        //               select new
        //               {
        //                   Code = a.CodeSet,
        //                   Name = a.ValueSet,
        //               };
        //    msg.Object = data;

        //    return Json(msg);
        //}
        //[HttpPost]
        //public JsonResult getAllCustomer()
        //{
        //    var msg = new JMessage() { Error = false };

        //    var data = from a in _context.Customers
        //               where a.IsDeleted == false
        //               select new
        //               {
        //                   Code = a.CusCode,
        //                   Name = a.CusName,
        //               };
        //    msg.Object = data;
        //    return Json(msg);
        //}
        //#endregion



        //#region File
        //public class JTableModelFile : JTableModel
        //{
        //    public int? CustomerId { get; set; }
        //    public string Title { get; set; }
        //    public string Type { get; set; }
        //    public string CreatedTime { get; set; }
        //    public string UpdatedTime { get; set; }
        //}
        //[HttpPost]
        //public object JTableFile([FromBody]JTableModelFile jTablePara)
        //{
        //    int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
        //    int? searchCustomerId = jTablePara.CustomerId;
        //    string searchTitle = jTablePara.Title.ToLower();
        //    string searchType = jTablePara.Type.ToLower();
        //    DateTime? searchCreatedTime = !string.IsNullOrEmpty(jTablePara.CreatedTime) ? DateTime.ParseExact(jTablePara.CreatedTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
        //    DateTime? searchUpdatedTime = !string.IsNullOrEmpty(jTablePara.UpdatedTime) ? DateTime.ParseExact(jTablePara.UpdatedTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;

        //    var query = from a in _context.EDMSCustomerFiles
        //                where ((a.CustomerID == searchCustomerId) &&
        //                       a.IsDeleted == false &&
        //                       a.Title.ToLower().Contains(searchTitle) &&
        //                       a.FileType.ToLower().Contains(searchType) &&
        //                       (searchCreatedTime == null || a.CreatedTime == searchCreatedTime) &&
        //                       (searchUpdatedTime == null || a.UpdatedTime == searchUpdatedTime))
        //                select new
        //                {
        //                    id = a.Id,
        //                    type = a.FileType,
        //                    title = a.Title,
        //                    description = a.Description,
        //                    createdTime = a.CreatedTime,
        //                    updatedTime = a.UpdatedTime
        //                };

        //    var count = query.Count();
        //    var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
        //    var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "type", "title", "description", "createdTime", "updatedTime");
        //    return Json(jdata);
        //}

        //[HttpPost]
        //public object InsertFile([FromBody]EDMSCustomerFile obj)
        //{
        //    var msg = new JMessage { Error = false, Title = "" };
        //    try
        //    {
        //        obj.FileType = System.IO.Path.GetExtension(obj.FileName);
        //        obj.CreatedTime = DateTime.Now.Date;
        //        _context.EDMSCustomerFiles.Add(obj);
        //        _context.SaveChanges();
        //        msg.Title = "Thêm khách hàng thành công";
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra!";
        //    }
        //    return Json(msg);
        //}

        //[HttpPost]
        //public object UpdateFile([FromBody]EDMSCustomerFile obj)
        //{
        //    var msg = new JMessage();
        //    try
        //    {
        //        obj.UpdatedTime = DateTime.Now.Date;
        //        _context.EDMSCustomerFiles.Update(obj);
        //        _context.SaveChanges();

        //        msg.Error = false;
        //        msg.Title = "Đã lưu thay đổi";

        //        return msg;
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra!";
        //        msg.Object = ex;
        //        return msg;
        //    }

        //}

        //[HttpPost]
        //public object DeleteFile(int id)
        //{
        //    var msg = new JMessage();
        //    try
        //    {
        //        var data = _context.EDMSCustomerFiles.FirstOrDefault(x => x.Id == id);
        //        data.IsDeleted = true;
        //        data.DeletedTime = DateTime.Now.Date;
        //        data.DeletedBy = ESEIM.AppContext.UserName;
        //        _context.EDMSCustomerFiles.Update(data);
        //        _context.SaveChanges();
        //        msg.Error = false;
        //        msg.Title = "Xóa thành công";
        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = ex.Message;
        //        return Json(msg);
        //    }
        //}

        //[HttpGet]
        //public object GetFile(int id)
        //{
        //    var a = _context.EDMSCustomerFiles.AsNoTracking().Single(m => m.Id == id);
        //    return Json(a);
        //}

        //[HttpPost]
        //public object UploadFile(IFormFile fileUpload)
        //{
        //    var msg = new JMessage { Error = false, Title = "" };
        //    try
        //    {
        //        var upload = _upload.UploadFile(fileUpload);
        //        if (upload.Error)
        //        {
        //            msg.Error = true;
        //            msg.Title = upload.Title;
        //        }
        //        else
        //        {
        //            msg.Object = upload.Object;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra khi upload file!";
        //    }
        //    return Json(msg);
        //}
        //#endregion

        //#region Contact
        //public class JtableModelContact : JTableModel
        //{
        //    public int? CustomerId { get; set; }
        //    public string ContactName { get; set; }
        //    public string ContactEmail { get; set; }
        //    public string ContactTelephone { get; set; }
        //    public string ContactMobilePhone { get; set; }
        //}

        //[HttpPost]
        //public object JTableContact([FromBody]JtableModelContact jTablePara)
        //{
        //    int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
        //    int? customerId = jTablePara.CustomerId;
        //    string contactName = jTablePara.ContactName.ToLower();
        //    string contactEmail = jTablePara.ContactEmail.ToLower();
        //    string contactTelephone = jTablePara.ContactTelephone;
        //    string contactMobilePhone = jTablePara.ContactMobilePhone;

        //    var query = from a in _context.Contacts
        //                where (a.ContactType == customerId && a.ContactType != null &&
        //                       a.IsDeleted == false &&
        //                       a.Email.ToLower().Contains(contactEmail) &&
        //                       a.ContactName.ToLower().Contains(contactName) &&
        //                       a.Telephone.Contains(contactTelephone) &&
        //                       a.MobilePhone.Contains(contactMobilePhone))
        //                select new
        //                {
        //                    id = a.Id,
        //                    contactName = a.ContactName,
        //                    contactEmail = a.Email,
        //                    contactAddress = a.Address,
        //                    contactTelephone = a.Telephone,
        //                    contactMobilePhone = a.MobilePhone,
        //                };

        //    var count = query.Count();
        //    var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
        //    var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "contactName", "contactEmail", "contactAddress", "contactTelephone", "contactMobilePhone");

        //    return Json(jdata);
        //}

        //[HttpPost]
        //public object InsertContact([FromBody]Contact obj)
        //{
        //    var msg = new JMessage();
        //    try
        //    {
        //        obj.CreateTime = DateTime.Now.Date;
        //        _context.Contacts.Add(obj);
        //        _context.SaveChanges();
        //        msg.Error = false;
        //        msg.Title = "Thêm thành công";
        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra!";
        //        msg.Object = ex;
        //        return Json(msg);
        //    }
        //}

        //[HttpPost]
        //public object UploadImage(IFormFile fileUpload)
        //{
        //    var msg = new JMessage();
        //    try
        //    {
        //        var upload = _upload.UploadImage(fileUpload);
        //        msg.Error = false;
        //        msg.Title = "";
        //        msg.Object = upload.Object;
        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra khi upload file!";
        //        msg.Object = ex;
        //        return Json(msg);
        //    }
        //}
        //public object DeleteContact(int id)
        //{
        //    var msg = new JMessage();
        //    try
        //    {
        //        var data = _context.Contacts.FirstOrDefault(x => x.Id == id);
        //        data.IsDeleted = true;
        //        _context.Contacts.Update(data);
        //        _context.SaveChanges();
        //        msg.Error = false;
        //        msg.Title = "Xóa thành công";
        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra!";
        //        msg.Object = ex;
        //        return Json(msg);
        //    }
        //}
        //[HttpGet]
        //public object GetItemAdd(string code)
        //{
        //    var a = _context.Customers.FirstOrDefault(m => m.CusCode == code);
        //    return Json(a);
        //}
        //[HttpGet]
        //public object GetContact(int id)
        //{
        //    var data = _context.Contacts.FirstOrDefault(x => x.Id == id);
        //    return Json(data);
        //}

        //[HttpPost]
        //public object UpdateContact([FromBody]Contact obj)
        //{
        //    var msg = new JMessage();
        //    try
        //    {
        //        obj.UpdateTime = DateTime.Now.Date;
        //        _context.Contacts.Update(obj);
        //        _context.SaveChanges();
        //        msg.Error = false;
        //        msg.Title = "Cập nhật thành công";
        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra!";
        //        msg.Object = ex;
        //        return Json(msg);
        //    }
        //}
        //#endregion

        //#region Extend
        //public class JTableModelExtend : JTableModel
        //{
        //    public int? CustomerId { get; set; }
        //}

        //[HttpPost]
        //public object JTableExtend([FromBody]JTableModelExtend jTablePara)
        //{
        //    int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
        //    int? customerId = jTablePara.CustomerId;

        //    var query = from a in _context.EDMSCustomerExtends
        //                where (a.isdeleted == false &&
        //                       a.customer_code == customerId)
        //                select new
        //                {
        //                    id = a.id,
        //                    code = a.ext_code,
        //                    value = a.ext_value
        //                };
        //    int count = query.Count();
        //    var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
        //    var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "code", "value");

        //    return Json(jdata);
        //}

        //[HttpPost]
        //public object DeleteExtend(int id)
        //{
        //    var msg = new JMessage();
        //    try
        //    {
        //        var data = _context.EDMSCustomerExtends.FirstOrDefault(x => x.id == id);
        //        data.isdeleted = true;
        //        _context.EDMSCustomerExtends.Update(data);
        //        _context.SaveChanges();
        //        msg.Error = false;
        //        msg.Title = "Xóa thành công";
        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra!";
        //        msg.Object = ex;
        //        return Json(msg);
        //    }

        //}

        //[HttpPost]
        //public object InsertExtend([FromBody]EDMSCustomerExtend obj)
        //{
        //    var msg = new JMessage();
        //    try
        //    {
        //        var query = from a in _context.EDMSCustomerExtends
        //                    where a.ext_code == obj.ext_code
        //                    select a;
        //        if (query.Count() == 0)
        //        {
        //            obj.created_time = DateTime.Now.Date;
        //            _context.EDMSCustomerExtends.Add(obj);
        //            _context.SaveChanges();
        //            msg.Error = false;
        //            msg.Title = "Thêm trường mở rộng thành công!";

        //        }
        //        else
        //        {
        //            msg.Error = true;
        //            msg.Title = "Mã trường mở rộng đã tồn tại!";

        //        }
        //        return Json(msg);
        //    }
        //    catch
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra!";

        //        return Json(msg);
        //    }

        //}
        //[HttpGet]
        //public object GetExtend(int id)
        //{
        //    var data = _context.EDMSCustomerExtends.FirstOrDefault(x => x.id == id);
        //    return Json(data);
        //}

        //[HttpPost]
        //public object UpdateExtend([FromBody]EDMSCustomerExtend obj)
        //{
        //    var msg = new JMessage();
        //    try
        //    {
        //        obj.updated_time = DateTime.Now.Date;
        //        _context.EDMSCustomerExtends.Update(obj);
        //        _context.SaveChanges();
        //        msg.Error = false;
        //        msg.Title = "Cập nhật thành công";
        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra!";
        //        msg.Object = ex;
        //        return Json(msg);
        //    }
        //}
        //#endregion

        //#region Contract
        //public class JTableModelContract : JTableModel
        //{
        //    public string CustomerCode { get; set; }
        //}

        //[HttpPost]
        //public object JTableContract([FromBody]JTableModelContract jTablePara)
        //{
        //    int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;

        //    var query = from a in _context.ContractHeaders
        //                where a.IsDeleted == false && a.CusCode.Equals(jTablePara.CustomerCode)
        //                select new
        //                {
        //                    id = a.ContractHeaderID,
        //                    code = a.ContractCode,
        //                    title = a.Title,
        //                    mainService = a.MainService,
        //                    budget = a.Budget,
        //                    currency = a.Currency,
        //                };

        //    int count = query.Count();
        //    var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length).AsNoTracking().ToList();
        //    var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "code", "title", "mainService", "budget", "currency");
        //    return Json(jdata);
        //}
        //#endregion

        //#region CardJob
        //public class JTableModelCardJob : JTableModel
        //{
        //    public int CustomerId { get; set; }
        //}
        //[HttpPost]
        //public JsonResult JTableCardJob([FromBody]JTableModelCardJob jtablePara)
        //{
        //    int intBegin = (jtablePara.CurrentPage - 1) * jtablePara.Length;

        //    var query = from a in _context.CardForWObj
        //                where a.IsDeleted == false &&
        //                      a.CatObjCode.Equals("CUSTOMER") &&
        //                      a.ObjCode.Equals(_context.Customers.FirstOrDefault(x => x.CusID == jtablePara.CustomerId).CusCode)
        //                select new
        //                {
        //                    Id = a.Id,
        //                    CardCode = a.CardCode,
        //                    Customer = a.ObjCode,
        //                    CardName = (_context.WORKOSCards.FirstOrDefault(x => x.CardCode.Equals(a.CardCode)).CardName)
        //                };

        //    int count = query.Count();
        //    var data = query.OrderUsingSortExpression(jtablePara.QueryOrderBy).Skip(intBegin).Take(jtablePara.Length).AsNoTracking().ToList();
        //    var jdata = JTableHelper.JObjectTable(data, jtablePara.Draw, count, "Id", "CardCode", "Customer", "CardName");
        //    return Json(jdata);
        //}

        //[HttpPost]
        //public JsonResult GetTeams()
        //{
        //    var data = _context.WORKOSTeams.Where(x => x.Flag == false).Select(x => new { Code = x.TeamCode, Name = x.TeamCode }).ToList();
        //    return Json(data);
        //}

        //[HttpPost]
        //public JsonResult GetBoards(string TeamCode)
        //{
        //    var data = _context.WORKOSBoards.Where(x => x.TeamCode.Equals(TeamCode)).Select(x => new { Code = x.BoardCode, Name = x.BoardName }).ToList();
        //    return Json(data);
        //}

        //[HttpPost]
        //public JsonResult GetLists(string BoardCode)
        //{
        //    var data = _context.WORKOSLists.Where(x => x.BoardCode.Equals(BoardCode)).Select(x => new { Code = x.ListCode, Name = x.ListName }).ToList();
        //    return Json(data);
        //}

        //[HttpPost]
        //public JsonResult GetCards(string ListCode)
        //{
        //    var data = _context.WORKOSCards.Where(x => x.ListCode.Equals(ListCode)).Select(x => new { Code = x.CardCode, Name = x.CardName }).ToList();
        //    return Json(data);
        //}

        //[HttpPost]
        //public JsonResult AddCardRelative([FromBody] dynamic data)
        //{
        //    var msg = new JMessage() { Error = true };
        //    try
        //    {
        //        int customerId = (int)data.ObjCode.Value;
        //        string CardCode = data.CardCode.Value;
        //        string CatObjCode = data.CatObjCode.Value;
        //        string Relative = data.Relative;
        //        string CustomerCode = _context.Customers.FirstOrDefault(x => x.CusID == customerId).CusCode;

        //        if (_context.CardForWObj.Where(x => x.ObjCode.Equals(CustomerCode) && x.CardCode.Equals(CardCode) && x.CatObjCode.Equals("CUSTOMER") && x.IsDeleted == false).Count() > 0)
        //        {
        //            msg.Title = "Khách hàng này đã tồn tại!";
        //            return Json(msg);
        //        }

        //        CardForWObj obj = new CardForWObj
        //        {
        //            CatObjCode = CatObjCode,
        //            Relative = Relative,
        //            CardCode = CardCode,
        //            ObjCode = CustomerCode
        //        };

        //        _context.CardForWObj.Add(obj);
        //        _context.SaveChanges();

        //        msg.Error = false;
        //        msg.Title = "Thêm thành công";
        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Title = "Có lỗi xảy ra!";
        //        msg.Object = ex;
        //        return Json(msg);
        //    }

        //}

        //#endregion
    }
    public class PackingModel
    {
        public int Id { get; set; }
        //public string Company_Code { get; set; }
        public string title { get; set; }
        public string Gis_data { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        //public DateTime? CreateDate { get; set; }
        //public int? CreateBy { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public int? UpdateBy { get; set; }
        //public int Flag { get; set; }
        //public string vendor_code { get; set; }
        public string Icon { get; set; }
        //public string Address { get; set; }
    }
    public class MapSearch
    {
        public MapSearch()
        {
            areas = new List<string>();
            groups = new List<string>();
            types = new List<string>();
            roles = new List<string>();
        }
        public string CustomerCode { get; set; }
        public List<string> areas { get; set; }
        public List<string> groups { get; set; }
        public List<string> types { get; set; }
        public List<string> roles { get; set; }
        
    }
}