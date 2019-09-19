using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace III.Admin.Controllers
{
    public class JTableModelSupplier : JTableModel
    {
        public string SupplierCode { set; get; }
        public string SupplierName { set; get; }
        public string SupplierEmail { set; get; }
        public string Address { set; get; }
        public string Phone { get; set; }
        public string SupplierGroup { set; get; }
        public string Status { set; get; }

    }
    [Area("Admin")]
    public class SupplierController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SupplierController(EIMDBContext context, IUploadService upload, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _upload = upload;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region GetComboboxValue
        [HttpPost]
        public JsonResult GetSupplierGroup()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "SUPPLIER_GROUP").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return Json(data);
        }
        [HttpPost]
        public JsonResult GetSupplierStatus()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "SUPPLIER_STATUS").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return Json(data);
        }
        #endregion

        #region index
        [HttpPost]
        public object JTable([FromBody]JTableModelSupplier jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            var query = from a in _context.Suppliers
                        join b in listCommon on a.SupGroup equals b.CodeSet into b1
                        from b in b1.DefaultIfEmpty()
                        join c in listCommon on a.Status equals c.CodeSet into c1
                        from c in c1.DefaultIfEmpty()
                        where (a.IsDeleted == false)
                        && (string.IsNullOrEmpty(jTablePara.SupplierCode) || (a.SupCode.ToLower().Contains(jTablePara.SupplierCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SupplierName) || (a.SupName.ToLower().Contains(jTablePara.SupplierName.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SupplierEmail) || (a.Email.ToLower().Contains(jTablePara.SupplierEmail.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.Address) || (a.Address.ToLower().Contains(jTablePara.Address.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.Phone) || (a.Mobile.ToLower().Contains(jTablePara.Phone.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SupplierGroup) || (a.SupGroup.Equals(jTablePara.SupplierGroup)))
                        && (string.IsNullOrEmpty(jTablePara.Status) || (a.Status.Equals(jTablePara.Status)))
                        select new
                        {
                            a.SupID,
                            a.SupCode,
                            a.SupName,
                            a.Email,
                            a.Address,
                            a.Telephone,
                            a.Mobile,
                            SupGroup = b != null ? b.ValueSet : "Không xác định",
                            Status = c != null ? c.CodeSet == "SUPPLIER_ACTIVE" ? c.CodeSet : c.CodeSet == "SUPPLIER_DEACTIVE" ? c.CodeSet : c.ValueSet : "Không xác định",
                        };
            var data = query.Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var count = query.Count();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "SupID", "SupCode", "SupName", "Email", "Address", "Telephone", "Mobile", "SupGroup", "Status");
            return Json(jdata);
        }
        [HttpPost]
        public JsonResult Insert([FromBody]Supplier obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var checkExist = _context.Suppliers.FirstOrDefault(x => !x.IsDeleted && x.SupCode.ToLower() == obj.SupCode.ToLower());
                if (checkExist == null)
                {
                    obj.CreatedBy = ESEIM.AppContext.UserName;
                    obj.CreatedTime = DateTime.Now;
                    _context.Suppliers.Add(obj);
                    _context.SaveChanges();
                    msg.Title = "Thêm thành công";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Mã nhà cung cấp đã tồn tại";
                }
            }
            catch
            {
                msg.Error = true;
                msg.Title = "Có lỗi khi thêm ";
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult Update([FromBody]Supplier obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                obj.UpdatedTime = DateTime.Now.Date;
                _context.Suppliers.Update(obj);
                _context.SaveChanges();

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
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var data = _context.Suppliers.FirstOrDefault(
                        x => x.SupID == id
                    );
                data.IsDeleted = true;
                _context.Suppliers.Update(data);
                _context.SaveChanges();
                msg.Title = "Xóa thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra";
            }
            return Json(msg);
        }

        [HttpGet]
        public object GetItem(int id)
        {
            var a = _context.Suppliers.FirstOrDefault(m => m.SupID == id);
            return Json(a);
        }

        [HttpGet]
        public object GetItemAdd(string code)
        {
            var a = _context.Suppliers.FirstOrDefault(m => m.SupCode == code);
            return Json(a);
        }
        #endregion

        #region File
        public class JTableModelFile : JTableModel
        {
            public int? SupplierId { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }
        [HttpPost]
        public object JTableFile([FromBody]JTableModelFile jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            int? searchSupplierId = jTablePara.SupplierId;
            //string searchTitle = jTablePara.Title.ToLower();
            //string searchType = jTablePara.Type.ToLower();
            DateTime? fromDate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? toDate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;

            var query = from a in _context.SupplierFiles
                        where ((a.SupplierId == searchSupplierId)
                        && a.IsDeleted == false
                        && (jTablePara.Type == null || jTablePara.Type == "" || a.FileType.ToLower().Contains(jTablePara.Type.ToLower()))
                        && (jTablePara.Title == null || jTablePara.Title == "" || a.Title.ToLower().Contains(jTablePara.Title.ToLower()))
                        && ((fromDate == null || (a.CreatedTime >= fromDate)) && (toDate == null ? true : (a.CreatedTime <= toDate))))
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
        public object InsertFile([FromBody]SupplierFile obj)
        {
            var msg = new JMessage();
            try
            {
                obj.FileType = System.IO.Path.GetExtension(obj.FileName);
                obj.CreatedBy = ESEIM.AppContext.UserName;
                obj.CreatedTime = DateTime.Now.Date;
                _context.SupplierFiles.Add(obj);
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
        public object UploadFile(IFormFile fileUpload)
        {
            var msg = new JMessage();
            try
            {
                var upload = _upload.UploadFile(fileUpload, Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\files"));
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

        [HttpPost]
        public object UpdateFile([FromBody]SupplierFile obj)
        {
            var msg = new JMessage();
            try
            {
                obj.UpdatedTime = DateTime.Now.Date;
                _context.SupplierFiles.Update(obj);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Đã lưu thay đổi";

                return msg;
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra!";
                return msg;
            }

        }

        [HttpPost]
        public object DeleteFile(int id)
        {
            var msg = new JMessage();
            try
            {
                var data = _context.SupplierFiles.FirstOrDefault(x => x.Id == id);
                data.IsDeleted = true;
                data.DeletedTime = DateTime.Now.Date;
                _context.SupplierFiles.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = ex.Message;
                return Json(msg);
            }
        }

        [HttpGet]
        public object GetFile(int id)
        {
            var a = _context.SupplierFiles.AsNoTracking().Single(m => m.Id == id);
            return Json(a);
        }
        #endregion

        #region contact
        public class JtableModelContact : JTableModel
        {
            public int? SupplierId { get; set; }
            public string ContactName { get; set; }
            public string ContactEmail { get; set; }
            public string ContactTelephone { get; set; }
            public string ContactMobilePhone { get; set; }
        }

        [HttpPost]
        public object JTableContact([FromBody]JtableModelContact jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            int? supplierId = jTablePara.SupplierId;
            string contactName = jTablePara.ContactName.ToLower();
            string contactEmail = jTablePara.ContactEmail.ToLower();
            string contactTelephone = jTablePara.ContactTelephone;
            string contactMobilePhone = jTablePara.ContactMobilePhone;

            var query = from a in _context.Contacts
                        where (a.CusSupId == supplierId && a.CusSupId != null &&
                               a.IsDeleted == false &&
                               a.Email.ToLower().Contains(contactEmail) &&
                               a.ContactName.ToLower().Contains(contactName) &&
                               a.Telephone.Contains(contactTelephone) &&
                               a.MobilePhone.Contains(contactMobilePhone))
                        select new
                        {
                            id = a.Id,
                            contactId = a.CusSupId,
                            contactName = a.ContactName,
                            contactEmail = a.Email,
                            contactAddress = a.Address,
                            contactTelephone = a.Telephone,
                            contactMobilePhone = a.MobilePhone,
                        };

            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "contactId", "contactName", "contactEmail", "contactAddress", "contactTelephone", "contactMobilePhone");

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

        [HttpPost]
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

        #region extend
        public class JTableModelExtend : JTableModel
        {
            public int? SupplierId { get; set; }
        }

        [HttpPost]
        public object JTableExtend([FromBody]JTableModelExtend jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            int? customerId = jTablePara.SupplierId;

            var query = from a in _context.SupplierExtends
                        where (a.isdeleted == false &&
                               a.supplier_code == customerId)
                        select new
                        {
                            id = a.id,
                            code = a.ext_code,
                            value = a.ext_value
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "code", "value");

            return Json(jdata);
        }

        [HttpPost]
        public object DeleteExtend(int id)
        {
            var msg = new JMessage();
            try
            {
                var data = _context.SupplierExtends.FirstOrDefault(x => x.id == id);
                data.isdeleted = true;
                _context.SupplierExtends.Update(data);
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
        public object InsertExtend([FromBody]SupplierExtend obj)
        {
            var msg = new JMessage();
            try
            {
                var query = from a in _context.SupplierExtends
                            where a.ext_code == obj.ext_code
                            select a;
                if (query.Count() == 0)
                {
                    obj.created_time = DateTime.Now.Date;
                    _context.SupplierExtends.Add(obj);
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

            //try
            //{               
            //    obj.created_time = DateTime.Now.Date;
            //    _context.SupplierExtends.Add(obj);
            //    _context.SaveChanges();
            //    msg.Error = false;
            //    msg.Title = "Thêm thành công";
            //    return Json(msg);
            //}
            //catch (Exception ex)
            //{
            //    msg.Error = true;
            //    msg.Title = "Có lỗi xảy ra!";
            //    msg.Object = ex;
            //    return Json(msg);
            //}
        }

        [HttpGet]
        public object GetExtend(int id)
        {
            var data = _context.SupplierExtends.FirstOrDefault(x => x.id == id);
            return Json(data);
        }

        [HttpPost]
        public object UpdateExtend([FromBody]SupplierExtend obj)
        {
            var msg = new JMessage();
            try
            {
                obj.updated_time = DateTime.Now.Date;
                _context.SupplierExtends.Update(obj);
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

        #region CardJob
        public class JTableModelCardJob : JTableModel
        {
            public int SupplierId { get; set; }
        }
        [HttpPost]
        public JsonResult JTableCardJob([FromBody]JTableModelCardJob jtablePara)
        {
            int intBegin = (jtablePara.CurrentPage - 1) * jtablePara.Length;

            var query = from a in _context.CardForWObj
                        where a.IsDeleted == false &&
                              a.CatObjCode.Equals("SUPPLIER") &&
                              a.ObjCode.Equals(_context.Suppliers.FirstOrDefault(x => x.SupID == jtablePara.SupplierId).SupCode)
                        select new
                        {
                            Id = a.Id,
                            CardCode = a.CardCode,
                            Supplier = a.ObjCode,
                            CardName = (_context.WORKOSCards.FirstOrDefault(x => x.CardCode.Equals(a.CardCode)).CardName)
                        };

            int count = query.Count();
            var data = query.OrderUsingSortExpression(jtablePara.QueryOrderBy).Skip(intBegin).Take(jtablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jtablePara.Draw, count, "Id", "CardCode", "Supplier", "CardName");
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
                int supplierId = (int)data.ObjCode.Value;
                string CardCode = data.CardCode.Value;
                string CatObjCode = data.CatObjCode.Value;
                string Relative = data.Relative;
                string SupplierCode = _context.Suppliers.FirstOrDefault(x => x.SupID == supplierId).SupCode;

                if (_context.CardForWObj.Where(x => x.ObjCode.Equals(SupplierCode) && x.CardCode.Equals(CardCode) && x.CatObjCode.Equals("SUPPLIER") && x.IsDeleted == false).Count() > 0)
                {
                    msg.Title = "Nhà cung cấp này đã tồn tại!";
                    return Json(msg);
                }

                CardForWObj obj = new CardForWObj
                {
                    CatObjCode = CatObjCode,
                    Relative = Relative,
                    CardCode = CardCode,
                    ObjCode = SupplierCode
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

        #region
        //dropdown
        [HttpPost]
        public JsonResult GetListSupplierArea()
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
    }
}