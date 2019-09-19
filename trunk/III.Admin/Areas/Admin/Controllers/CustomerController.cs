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
    [Area("Admin")]
    public class CustomerController : BaseController
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
        private readonly IHostingEnvironment _hostingEnvironment;

        public CustomerController(EIMDBContext context, IUploadService upload, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _upload = upload;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Index

        [HttpPost]
        public object JTable([FromBody]JTableModelCustomer jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            var query = from a in _context.Customers
                        join b in listCommon on a.CusGroup equals b.CodeSet into b1
                        from b in b1.DefaultIfEmpty()
                        join c in listCommon on a.ActivityStatus equals c.CodeSet into c1
                        from c in c1.DefaultIfEmpty()
                        where (a.IsDeleted == false)
                            && (string.IsNullOrEmpty(jTablePara.CustomerCode) || a.CusCode.ToLower().Contains(jTablePara.CustomerCode.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.CustomerName) || a.CusName.ToLower().Contains(jTablePara.CustomerName.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.CustomerEmail) || a.Email.ToLower().Contains(jTablePara.CustomerEmail.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.CustomerPhone) || a.MobilePhone.ToLower().Contains(jTablePara.CustomerPhone.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.CustomerActivityStatus) || a.ActivityStatus == jTablePara.CustomerActivityStatus)
                            && (string.IsNullOrEmpty(jTablePara.CustomerGroup) || a.CusGroup.ToLower() == jTablePara.CustomerGroup.ToLower())
                            && (string.IsNullOrEmpty(jTablePara.Address) || a.Address.ToLower().Contains(jTablePara.Address.ToLower()))
                        select new
                        {
                            id = a.CusID,
                            cusCode = a.CusCode,
                            cusName = a.CusName,
                            cusEmail = a.Email,
                            cusAddress = a.Address,
                            cusMobilePhone = a.MobilePhone,
                            cusGroup = b != null ? b.ValueSet : "",
                            cusActivity = c != null ? c.CodeSet == "ACTIVE" ? c.CodeSet : c.CodeSet == "DEACTIVE" ? c.CodeSet : c.ValueSet : "Không xác định",
                        };
            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "id", "cusCode", "cusName", "cusEmail", "cusAddress", "cusMobilePhone", "cusGroup", "cusActivity");
            return Json(jdata);
        }

        [HttpPost]
        public JsonResult Insert([FromBody]Customer obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var checkExist = _context.Customers.FirstOrDefault(x => !x.IsDeleted && x.CusCode.ToLower() == obj.CusCode.ToLower());
                if (checkExist == null)
                {
                    obj.CreatedBy = ESEIM.AppContext.UserName;
                    obj.CreatedTime = DateTime.Now;
                    _context.Customers.Add(obj);
                    _context.SaveChanges();
                    msg.Title = "Thêm khách hàng thành công!";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Mã khách hàng đã tồn tại!";
                }
                return Json(msg);
            }
            catch
            {
                msg.Error = true;
                //msg.Object = ex;
                msg.Title = "Có lỗi khi thêm khách hàng!";
                return Json(msg);
            }

        }

        [HttpPost]
        public JsonResult Update([FromBody]Customer obj)
        {
            var msg = new JMessage();
            try
            {
                obj.UpdatedTime = DateTime.Now.Date;
                _context.Customers.Update(obj);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Đã lưu thay đổi";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra!";
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var msg = new JMessage();
            try
            {
                var data = _context.Customers.FirstOrDefault(
                        x => x.CusID == id
                    );
                data.IsDeleted = true;
                _context.Customers.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Xóa thành công!";
            }
            catch
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra!";
            }
            return Json(msg);
        }

        [HttpGet]
        public object GetItem(int id)
        {
            var a = _context.Customers.AsNoTracking().Single(m => m.CusID == id);
            return Json(a);
        }


        #region Combobox
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
        #endregion


        #endregion

        #region File
        public class JTableModelFile : JTableModel
        {
            public int? CustomerId { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public string CreatedTime { get; set; }
            public string UpdatedTime { get; set; }
        }
        [HttpPost]
        public object JTableFile([FromBody]JTableModelFile jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            int? searchCustomerId = jTablePara.CustomerId;
            string searchTitle = jTablePara.Title.ToLower();
            string searchType = jTablePara.Type.ToLower();
            DateTime? searchCreatedTime = !string.IsNullOrEmpty(jTablePara.CreatedTime) ? DateTime.ParseExact(jTablePara.CreatedTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? searchUpdatedTime = !string.IsNullOrEmpty(jTablePara.UpdatedTime) ? DateTime.ParseExact(jTablePara.UpdatedTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;

            var query = from a in _context.CustomerFiles
                        where ((a.CustomerID == searchCustomerId) &&
                               a.IsDeleted == false &&
                               a.Title.ToLower().Contains(searchTitle) &&
                               a.FileType.ToLower().Contains(searchType) &&
                               (searchCreatedTime == null || a.CreatedTime == searchCreatedTime) &&
                               (searchUpdatedTime == null || a.UpdatedTime == searchUpdatedTime))
                        select new
                        {
                            id = a.Id,
                            type = a.FileType,
                            title = a.Title,
                            description = a.Description,
                            createdTime = a.CreatedTime,
                            updatedTime = a.UpdatedTime
                        };

            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "type", "title", "description", "createdTime", "updatedTime");
            return Json(jdata);
        }

        [HttpPost]
        public JsonResult InsertFile([FromBody]CustomerFile obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                obj.FileType = System.IO.Path.GetExtension(obj.FileName);
                obj.CreatedTime = DateTime.Now.Date;
                _context.CustomerFiles.Add(obj);
                _context.SaveChanges();
                msg.Title = "Thêm tệp tin thành công!";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra!";
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult UpdateFile([FromBody]CustomerFile obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                obj.UpdatedTime = DateTime.Now.Date;
                _context.CustomerFiles.Update(obj);
                _context.SaveChanges();
                msg.Title = "Chỉnh sửa tệp tin thành công!";
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
        public JsonResult DeleteFile(int id)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var data = _context.CustomerFiles.FirstOrDefault(x => x.Id == id);
                data.IsDeleted = true;
                data.DeletedTime = DateTime.Now.Date;
                data.DeletedBy = ESEIM.AppContext.UserName;
                _context.CustomerFiles.Update(data);
                _context.SaveChanges();
                msg.Title = "Xóa tệp tin thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = ex.Message;
            }
            return Json(msg);
        }

        [HttpGet]
        public object GetFile(int id)
        {
            var a = _context.CustomerFiles.AsNoTracking().Single(m => m.Id == id);
            return Json(a);
        }

        [HttpPost]
        public object UploadFile(IFormFile fileUpload)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var upload = _upload.UploadFile(fileUpload, Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\files"));
                if (upload.Error)
                {
                    msg.Error = true;
                    msg.Title = upload.Title;
                }
                else
                {
                    msg.Object = upload.Object;
                }

            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi upload file!";
            }
            return Json(msg);
        }
        #endregion

        #region Contact
        public class JtableModelContact : JTableModel
        {
            public int? CustomerId { get; set; }
            public string ContactName { get; set; }
            public string ContactEmail { get; set; }
            public string ContactTelephone { get; set; }
            public string ContactMobilePhone { get; set; }
        }

        [HttpPost]
        public object JTableContact([FromBody]JtableModelContact jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            int? customerId = jTablePara.CustomerId;
            string contactName = jTablePara.ContactName.ToLower();
            string contactEmail = jTablePara.ContactEmail.ToLower();
            string contactTelephone = jTablePara.ContactTelephone;
            string contactMobilePhone = jTablePara.ContactMobilePhone;

            var query = from a in _context.Contacts
                        where (a.ContactType == customerId && a.ContactType != null &&
                               a.IsDeleted == false &&
                               a.Email.ToLower().Contains(contactEmail) &&
                               a.ContactName.ToLower().Contains(contactName) &&
                               a.Telephone.Contains(contactTelephone) &&
                               a.MobilePhone.Contains(contactMobilePhone))
                        select new
                        {
                            id = a.Id,
                            contactName = a.ContactName,
                            contactEmail = a.Email,
                            contactAddress = a.Address,
                            contactTelephone = a.Telephone,
                            contactMobilePhone = a.MobilePhone,
                        };

            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "contactName", "contactEmail", "contactAddress", "contactTelephone", "contactMobilePhone");

            return Json(jdata);
        }

        [HttpPost]
        public object InsertContact([FromBody]Contact obj)
        {
            var msg = new JMessage();
            try
            {
                obj.CreateTime = DateTime.Now.Date;
                _context.Contacts.Add(obj);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Thêm thành công";
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
        public object UploadImage(IFormFile fileUpload)
        {
            var msg = new JMessage();
            try
            {
                var upload = _upload.UploadImage(fileUpload);
                msg.Error = false;
                msg.Title = "";
                msg.Object = upload.Object;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi upload file!";
                msg.Object = ex;
                return Json(msg);
            }
        }
        public object DeleteContact(int id)
        {
            var msg = new JMessage();
            try
            {
                var data = _context.Contacts.FirstOrDefault(x => x.Id == id);
                data.IsDeleted = true;
                _context.Contacts.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Xóa thành công";
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
        [HttpGet]
        public object GetItemAdd(string code)
        {
            var a = _context.Customers.FirstOrDefault(m => m.CusCode == code);
            return Json(a);
        }
        [HttpGet]
        public object GetContact(int id)
        {
            var data = _context.Contacts.FirstOrDefault(x => x.Id == id);
            return Json(data);
        }

        [HttpPost]
        public object UpdateContact([FromBody]Contact obj)
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
        #endregion

        #region Extend
        public class JTableModelExtend : JTableModel
        {
            public int? CustomerId { get; set; }
            public string Extvalue { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }

        [HttpPost]
        public object JTableExtend([FromBody]JTableModelExtend jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            int? customerId = jTablePara.CustomerId;
            DateTime? fromdate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? todate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.CustomerExtends
                        where (a.isdeleted == false &&
                               a.customer_code == customerId)
                               && a.isdeleted == false
                               && ((fromdate == null || (a.created_time >= fromdate)) && (todate == null || (a.created_time <= todate)))
                               && (string.IsNullOrEmpty(jTablePara.Extvalue) || a.ext_value.ToLower().Contains(jTablePara.Extvalue.ToLower()))
                        select new
                        {
                            id = a.id,
                            code = a.ext_code,
                            value = a.ext_value,
                            created_time = a.created_time.Value.ToString("dd/MM/yyyy HH:mm:ss")
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "code", "value", "created_time");

            return Json(jdata);
        }

        [HttpPost]
        public object DeleteExtend(int id)
        {
            var msg = new JMessage();
            try
            {
                var data = _context.CustomerExtends.FirstOrDefault(x => x.id == id);
                data.isdeleted = true;
                _context.CustomerExtends.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Xóa thành công";
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
        public object InsertExtend([FromBody]CustomerExtend obj)
        {
            var msg = new JMessage();
            try
            {
                var query = from a in _context.CustomerExtends
                            where a.ext_code == obj.ext_code && a.isdeleted == false
                            select a;
                if (query.Count() == 0)
                {
                    obj.created_time = DateTime.Now;
                    obj.isdeleted = false;
                    _context.CustomerExtends.Add(obj);
                    _context.SaveChanges();
                    msg.Error = false;
                    msg.Title = "Thêm trường mở rộng thành công!";

                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Mã trường mở rộng đã tồn tại!";

                }
                return Json(msg);
            }
            catch
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra!";

                return Json(msg);
            }

        }
        [HttpGet]
        public object GetExtend(int id)
        {
            var data = _context.CustomerExtends.FirstOrDefault(x => x.id == id);
            return Json(data);
        }

        [HttpPost]
        public object UpdateExtend([FromBody]CustomerExtend obj)
        {
            var msg = new JMessage();
            try
            {
                obj.updated_time = DateTime.Now.Date;
                _context.CustomerExtends.Update(obj);
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
        #endregion

        #region Contract
        public class JTableModelContract : JTableModel
        {
            public string CustomerCode { get; set; }
            public string ContractCode { get; set; }
            public string Title { get; set; }
        }

        [HttpPost]
        public object JTableContract([FromBody]JTableModelContract jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;

            var query = from a in _context.ContractHeaders
                        where a.IsDeleted == false
                        && a.CusCode.Equals(jTablePara.CustomerCode)
                        && (string.IsNullOrEmpty(jTablePara.ContractCode) || (a.ContractCode.ToLower().Contains(jTablePara.ContractCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.Title) || (a.Title.ToLower().Contains(jTablePara.Title.ToLower())))
                        select new
                        {
                            id = a.ContractHeaderID,
                            code = a.ContractCode,
                            title = a.Title,
                            mainService = a.MainService,
                            budget = a.Budget,
                            currency = a.Currency,
                        };

            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "code", "title", "mainService", "budget", "currency");
            return Json(jdata);
        }
        #endregion

        #region CardJob
        public class JTableModelCardJob : JTableModel
        {
            public int CustomerId { get; set; }
            public string CardCode { get; set; }
            public string CardName { get; set; }
        }
        [HttpPost]
        public JsonResult JTableCardJob([FromBody]JTableModelCardJob jtablePara)
        {
            int intBegin = (jtablePara.CurrentPage - 1) * jtablePara.Length;

            var query = from a in _context.CardForWObj
                        join b in _context.WORKOSCards on a.CardCode equals b.CardCode into b1
                        from b in b1.DefaultIfEmpty()
                        where a.IsDeleted == false &&
                              a.CatObjCode.Equals("CUSTOMER") &&
                              a.ObjCode.Equals(_context.Customers.FirstOrDefault(x => x.CusID == jtablePara.CustomerId).CusCode)
                              && (string.IsNullOrEmpty(jtablePara.CardCode) || a.CardCode.ToLower().Contains(jtablePara.CardCode.ToLower()))
                              && (string.IsNullOrEmpty(jtablePara.CardName) || b.CardName.ToLower().Contains(jtablePara.CardName.ToLower()))
                        select new
                        {
                            Id = a.Id,
                            CardCode = a.CardCode,
                            Customer = a.ObjCode,
                            CardName = b.CardName
                        };

            int count = query.Count();
            var data = query.OrderUsingSortExpression(jtablePara.QueryOrderBy).Skip(intBegin).Take(jtablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jtablePara.Draw, count, "Id", "CardCode", "Customer", "CardName");
            return Json(jdata);
        }

        [HttpPost]
        public JsonResult GetTeams()
        {
            var data = _context.WORKOSTeams.Where(x => x.Flag == false).Select(x => new { Code = x.TeamCode, Name = x.TeamCode }).ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetBoards(string TeamCode)
        {
            var data = _context.WORKOSBoards.Where(x => x.TeamCode.Equals(TeamCode)).Select(x => new { Code = x.BoardCode, Name = x.BoardName }).ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetLists(string BoardCode)
        {
            var data = _context.WORKOSLists.Where(x => x.BoardCode.Equals(BoardCode)).Select(x => new { Code = x.ListCode, Name = x.ListName }).ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetCards(string ListCode)
        {
            var data = _context.WORKOSCards.Where(x => x.ListCode.Equals(ListCode)).Select(x => new { Code = x.CardCode, Name = x.CardName }).ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult AddCardRelative([FromBody] dynamic data)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                int customerId = (int)data.ObjCode.Value;
                string CardCode = data.CardCode.Value;
                string CatObjCode = data.CatObjCode.Value;
                string Relative = data.Relative;
                string CustomerCode = _context.Customers.FirstOrDefault(x => x.CusID == customerId).CusCode;

                if (_context.CardForWObj.Where(x => x.ObjCode.Equals(CustomerCode) && x.CardCode.Equals(CardCode) && x.CatObjCode.Equals("CUSTOMER") && x.IsDeleted == false).Count() > 0)
                {
                    msg.Title = "Khách hàng này đã tồn tại!";
                    return Json(msg);
                }

                CardForWObj obj = new CardForWObj
                {
                    CatObjCode = CatObjCode,
                    Relative = Relative,
                    CardCode = CardCode,
                    ObjCode = CustomerCode
                };

                _context.CardForWObj.Add(obj);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Thêm thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Title = "Có lỗi xảy ra!";
                msg.Object = ex;
                return Json(msg);
            }

        }

        #endregion
    }
}