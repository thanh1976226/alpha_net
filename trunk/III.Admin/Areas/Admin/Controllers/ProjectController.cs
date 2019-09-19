using System;
using System.Collections.Generic;
using System.Linq;
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
    [Area("Admin")]
    public class ProjectController : BaseController
    {
        public class JtableProject
        {
            public DateTime? StartTime { get; set; }
            public DateTime? EndTime { get; set; }
        }


        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ProjectController(EIMDBContext context, IUploadService upload, IHostingEnvironment hostingEnvironment)
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
        public object GetCustomers()
        {
            var data = _context.Customers.Select(x => new { Code = x.CusCode, Name = x.CusName }).ToList();
            return Json(data);
        }
        [HttpPost]
        public object GetItemCustomers(int id)
        {
            var a = _context.Projects.FirstOrDefault(m => m.Id == id);
            return Json(a);
            //var data = _context.Customers.Where(x=>x.CusID==id).Select(x => new { Code = x.CusCode }).ToList();
            //return Json(data);
        }
        [HttpPost]
        public JsonResult GetProjectUnit()
        {
            var data = GetCurrencyBase();
            return Json(data);
        }
        [HttpPost]
        public JsonResult GetProjectLanguage()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "PROGRAM_LANGUAGE").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetProjectStatus()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "PRO_STATUS").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return Json(data);
        }
        #endregion

        #region index
        [HttpPost]
        public object JTable([FromBody]JTableModelProject jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            DateTime? fromDate = !string.IsNullOrEmpty(jTablePara.StartTime) ? DateTime.ParseExact(jTablePara.StartTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? toDate = !string.IsNullOrEmpty(jTablePara.EndTime) ? DateTime.ParseExact(jTablePara.EndTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            double? budget = !string.IsNullOrEmpty(jTablePara.Budget) ? double.Parse(jTablePara.Budget) : (double?)null;
            var query = from a in _context.Projects
                        join b2 in _context.CommonSettings.Where(x=>!x.IsDeleted) on a.Currency equals b2.CodeSet into b1
                        from b in b1.DefaultIfEmpty()
                        where (!a.FlagDeleted)
                        && (string.IsNullOrEmpty(jTablePara.ProjectCode) || ((!string.IsNullOrEmpty(a.ProjectCode) && a.ProjectCode.ToLower().Contains(jTablePara.ProjectCode.ToLower()))))
                        && (string.IsNullOrEmpty(jTablePara.ProjectTitle) || (!string.IsNullOrEmpty(a.ProjectTitle) && (a.ProjectTitle.ToLower().Contains(jTablePara.ProjectTitle.ToLower()))))
                        && (fromDate == null || (a.StartTime >= fromDate)) 
                        && (toDate == null || (a.EndTime <= toDate))
                        && (budget == null || (a.Budget >= budget))
                        select new 
                        {
                            a.Id,
                            a.ProjectCode,
                            a.ProjectTitle,
                            Currency = b != null ? b.ValueSet : "Không xác định",
                            a.Budget,
                            a.StartTime,
                            a.EndTime,
                        };
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var count = query.Count();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "ProjectCode", "ProjectTitle", "Currency", "Budget", "StartTime", "EndTime");
            return Json(jdata);
        }
        [HttpPost]
        public JsonResult Insert([FromBody]ProjectModel obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                if (!_context.Projects.Any(x=>x.ProjectCode == obj.ProjectCode && x.FlagDeleted != true))
                {

                    double? budget = !string.IsNullOrEmpty(obj.Budget) ? double.Parse(obj.Budget) : (double?)null;
                    double? set = !string.IsNullOrEmpty(obj.SetPriority) ? double.Parse(obj.SetPriority) : (double?)null;
                    DateTime? fromto = !string.IsNullOrEmpty(obj.StartTime) ? DateTime.ParseExact(obj.StartTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                    DateTime? dateto = !string.IsNullOrEmpty(obj.EndTime) ? DateTime.ParseExact(obj.EndTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                    Project data = new Project()
                    {
                        ProjectCode = obj.ProjectCode,
                        ProjectTitle = obj.ProjectTitle,
                        Budget = budget,
                        Currency = obj.Currency,
                        StartTime = fromto,
                        EndTime = dateto,
                        PrjSkillKeyword = obj.PrjSkillKeyword,
                        PrjMode = obj.PrjMode,
                        SetPriority = set,
                        PrjStatus = obj.PrjStatus,
                        CreatedBy = ESEIM.AppContext.UserName,
                        CreatedTime = DateTime.Now
                    };
                    _context.Projects.Add(data);
                    _context.SaveChanges();
                    msg.Title = "Thêm dự án thành công!";
                    return Json(msg);
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Mã dự án đã tồn tại";

                }
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi khi thêm dự án xảy ra!";
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult InsertProjectTabCustomer([FromBody]ProjectCustomerAdd obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var query = from a in _context.ProjectCustomers
                            where a.CustomerCode == obj.CusCode && a.ProjectCode == obj.ProjectCode && a.FlagDeleted == false
                            select a;
                if (query.Count() == 0)
                {


                    ProjectCustomer data = new ProjectCustomer()
                    {
                        CustomerCode = obj.CusCode,
                        ProjectCode = obj.ProjectCode,
                        CreatedBy = ESEIM.AppContext.UserName,
                        CreatedTime = DateTime.Now
                    };
                    _context.ProjectCustomers.Add(data);
                    _context.SaveChanges();
                    msg.Title = "Thêm khách hàng cho dự án thành công!";
                    return Json(msg);
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Mã khách hàng đã tồn tại!";

                }
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi khi thêm khách hàng cho dự án!";
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult Update([FromBody]Project obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                obj.UpdatedTime = DateTime.Now.Date;
                obj.UpdatedBy = ESEIM.AppContext.UserName;
                _context.Projects.Update(obj);
                _context.SaveChanges();
                msg.Title = "Cập nhập dự án thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi khi cập nhập dự án!";
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var data = _context.Projects.FirstOrDefault(x => x.Id == id);
                _context.Projects.Remove(data);
                _context.SaveChanges();
                msg.Title = "Xóa dự án thành công!";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi xóa!";
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult DeleteProjectTabCustomer(int id)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var data = _context.ProjectCustomers.FirstOrDefault(x => x.Id == id);
                data.FlagDeleted = true;
                _context.ProjectCustomers.Update(data);
                _context.SaveChanges();
                msg.Title = "Xóa khách hàng của dự án thành công!";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi xóa!";
            }
            return Json(msg);
        }

        [HttpGet]
        public object GetItem(int id)
        {
            var a = _context.Projects.FirstOrDefault(m => m.Id == id);
            return Json(a);
        }
        [HttpGet]
        public object GetItemAdd(string code)
        {
            var a = _context.Projects.FirstOrDefault(m => m.ProjectCode == code);
            return Json(a);
        }
        #endregion

        #region Supplier
        public class JTableProjectCustomer : JTableModel
        {
            public int? ProjectId { get; set; }
            public string CustomerCode { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }

        }
        [HttpPost]
        public object JTableSupplier([FromBody]JTableModelSupplier jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            //int? searchSupplierId = jTablePara.SupplierId;
            //string searchTitle = jTablePara.Title.ToLower();
            //string searchType = jTablePara.Type.ToLower();
            //DateTime? searchCreatedTime = !string.IsNullOrEmpty(jTablePara.CreatedTime) ? DateTime.ParseExact(jTablePara.CreatedTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            //DateTime? searchUpdatedTime = !string.IsNullOrEmpty(jTablePara.UpdatedTime) ? DateTime.ParseExact(jTablePara.UpdatedTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.ProjectSuppliers
                        join b in _context.Suppliers on a.SupplierCode equals b.SupCode into b1
                        from b in b1.DefaultIfEmpty()
                            //where ((a.SupplierId == searchSupplierId) &&
                            //       a.IsDeleted == false &&
                            //       a.Title.ToLower().Contains(searchTitle) &&
                            //       a.FileType.ToLower().Contains(searchType) &&
                            //       (searchCreatedTime == null || a.CreatedTime == searchCreatedTime) &&
                            //       (searchUpdatedTime == null || a.UpdatedTime == searchUpdatedTime))
                        select new
                        {
                            a.Id,
                            a.SupplierCode,
                            SupName = b != null ? b.SupName : null,
                            Address = b != null ? b.Address : null,
                            Email = b != null ? b.Email : null
                        };

            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "SupplierCode", "SupName", "Address", "Email");
            return Json(jdata);
        }

        [HttpPost]
        public object UpdateContractFile([FromBody]ContractFile obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                obj.UpdatedBy = ESEIM.AppContext.UserName;
                obj.UpdatedTime = DateTime.Now;
                _context.ContractFiles.Update(obj);
                _context.SaveChanges();

                msg.Title = "Cập nhật thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi cập nhật!";
                msg.Object = ex;
                return Json(msg);
            }
        }


        [HttpPost]
        public object InsertFile([FromBody]SupplierFile obj)
        {
            var msg = new JMessage();
            try
            {
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
        public object GetContractFile(int id)
        {
            var data = _context.ContractFiles.FirstOrDefault(x => x.ContractFileID == id);
            return Json(data);
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
            var a = _context.ProjectFiles.FirstOrDefault(m => m.Id == id);
            return Json(a);
        }
        #endregion

        #region Customer
        public class JtableModelContact : JTableModel
        {
            public int? SupplierId { get; set; }
            public string ContactName { get; set; }
            public string ContactEmail { get; set; }
            public string ContactTelephone { get; set; }
            public string ContactMobilePhone { get; set; }
        }

        [HttpPost]
        public object JTableCustomer([FromBody]JTableProjectCustomer jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var da = _context.Projects.FirstOrDefault(m => m.Id == jTablePara.ProjectId);
            //int? headerId = jTablePara.ProjectId;
            if (jTablePara.ProjectId == null)
            {
                var list = new List<object>();
                return JTableHelper.JObjectTable(list, jTablePara.Draw, 0, "Id", "CustomerCode", "CusName", "Address", "Email");
            }
            DateTime? fromdate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? todate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.ProjectCustomers
                        join b in _context.Customers on a.CustomerCode equals b.CusCode into b1
                        from b in b1.DefaultIfEmpty()

                        where !a.FlagDeleted && !string.IsNullOrEmpty(a.CustomerCode)
                              && (a.ProjectCode == da.ProjectCode)
                        && (string.IsNullOrEmpty(jTablePara.CustomerCode) || a.CustomerCode.ToLower().Contains(jTablePara.CustomerCode.ToLower()))
                        && ((fromdate == null ? true : (a.CreatedTime >= fromdate)) && (todate == null ? true : (a.CreatedTime <= todate)))
                        //where ((a.SupplierId == searchSupplierId) &&
                        //       a.IsDeleted == false &&
                        //       a.Title.ToLower().Contains(searchTitle) &&
                        //       a.FileType.ToLower().Contains(searchType) &&
                        //       (searchCreatedTime == null || a.CreatedTime == searchCreatedTime) &&
                        //       (searchUpdatedTime == null || a.UpdatedTime == searchUpdatedTime))
                        select new
                        {
                            a.Id,
                            a.CustomerCode,
                            CusName = b != null ? b.CusName : null,
                            Address = b != null ? b.Address : null,
                            Email = b != null ? b.Email : null
                        };

            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "CustomerCode", "CusName", "Address", "Email");
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

        #region Member
        public class JTableModelMember : JTableModel
        {
            public int? ProjectId { get; set; }
            public string fullname { get; set; }
            public string Position { get; set; }
        }

        [HttpPost]
        public object JTableMember([FromBody]JTableModelMember jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var da = _context.Projects.FirstOrDefault(x => x.Id == jTablePara.ProjectId);
            if (jTablePara.ProjectId == null)
            {
                var list = new List<object>();
                return JTableHelper.JObjectTable(list, jTablePara.Draw, 0, "Id", "Name", "Position", "Email", "Active");
            }
            var query = from a in _context.ProjectMembers
                        join b in _context.Users on a.MemberCode equals b.Id into b1
                        from b in b1.DefaultIfEmpty()
                        where !a.FlagDeleted && !string.IsNullOrEmpty(a.MemberCode)
                        && (string.IsNullOrEmpty(jTablePara.fullname) || string.Join(" ", b.GivenName, b.FamilyName, b.MiddleName).ToLower().Contains(jTablePara.fullname.ToLower()))
                        && (string.IsNullOrEmpty(jTablePara.Position) || a.Position.ToLower().Contains(jTablePara.Position.ToLower()))
                        && (a.ProjectCode == da.ProjectCode)
                        && (!a.FlagDeleted)
                        select new
                        {
                            a.Id,
                            Name = b != null ? string.Join(" ", b.GivenName, b.FamilyName, b.MiddleName) : null,
                            a.Position,
                            Email = b != null ? b.Email : null,
                            Active = b != null ? b.Active : false,
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "Name", "Position", "Email", "Active");

            return Json(jdata);
        }

        [HttpPost]
        public object DeleteProjectTabMember(int id)
        {
            var msg = new JMessage();
            try
            {
                var data = _context.ProjectMembers.FirstOrDefault(x => x.Id == id);
                data.FlagDeleted = true;
                _context.ProjectMembers.Update(data);
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
        public object InsertProjectTabMember([FromBody]EDMSProjectTabMember obj)
        {
            var msg = new JMessage();
            try
            {
                var query = from a in _context.ProjectMembers
                            where a.MemberCode == obj.MemberCode && a.ProjectCode == obj.ProjectCode && a.FlagDeleted == false
                            select a;
                if (query.Count() == 0)
                {


                    ProjectMember data = new ProjectMember()
                    {
                        MemberCode = obj.MemberCode,
                        ProjectCode = obj.ProjectCode,
                        Position = obj.Position,
                        CreatedBy = ESEIM.AppContext.UserName,
                        CreatedTime = DateTime.Now
                    };
                    _context.ProjectMembers.Add(data);
                    _context.SaveChanges();
                    msg.Title = "Thêm thành viên cho dự án thành công!";
                    return Json(msg);
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Thành viên muốn thêm đã tồn tại trong dự án!";

                }
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi thêm thành viên vào dự án!";
                msg.Object = ex;
                return Json(msg);
            }
        }

        [HttpPost]
        public object UpdateProjectTabMember([FromBody]EDMSProjectTabMember obj)
        {
            var msg = new JMessage();
            try
            {
                var data = _context.ProjectMembers.FirstOrDefault(x => x.MemberCode == obj.Member && x.ProjectCode == obj.ProjectCode);
                data.Position = obj.Position;
                data.MemberCode = obj.MemberCode;
                data.UpdatedBy = ESEIM.AppContext.UserName;
                data.UpdatedTime = DateTime.Now;
                _context.ProjectMembers.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Cập nhật thành viên thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi cập nhật thành viên!";
                msg.Object = ex;
                return Json(msg);
            }
        }

        [HttpGet]
        public object GetMember(int id)
        {
            var data = _context.ProjectMembers.FirstOrDefault(x => x.Id == id);
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

        [HttpPost]
        public object GetUser()
        {
            var query = _context.Users.Where(x => x.Active && x.UserName != "admin").Select(x => new { x.Id, UserName = x.GivenName }).AsNoTracking().ToList();
            return query;
        }

        #endregion

        #region File
        public class JTableModelFile : JTableModel
        {
            public int? ProjectId { get; set; }
            public string FileName { get; set; }

            public string FromDate { get; set; }

            public string ToDate { get; set; }
        }
        [HttpPost]
        public object JTableFile([FromBody]JTableModelFile jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var da = _context.Projects.FirstOrDefault(x => x.Id == jTablePara.ProjectId);
            if (jTablePara.ProjectId == null)
            {
                var list = new List<object>();
                return JTableHelper.JObjectTable(list, jTablePara.Draw, 0, "Id", "FileName", "ReposName", "CreatedTime", "Tags", "Description");
            }
            DateTime? fromDate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? toDate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.ProjectFiles
                        join b in _context.Users on a.Tags equals b.Id into b1
                        from b in b1.DefaultIfEmpty()
                        join c in _context.EDMSRepositorys on a.Category equals c.ReposID into c1
                        from c in c1.DefaultIfEmpty()
                        where !a.FlagDeleted
                            && ((fromDate == null || (a.CreatedTime >= fromDate)) && (toDate == null || (a.CreatedTime <= toDate)))
                            && (string.IsNullOrEmpty(jTablePara.FileName) || a.FileName.ToLower().Contains(jTablePara.FileName.ToLower()))
                            && (a.ProjectCode == da.ProjectCode)
                        select new
                        {
                            a.Id,
                            FileName = a.FileName,
                            ReposName = c != null ? c.ReposName : null,
                            a.CreatedTime,
                            Tags = b != null ? string.Join(" ", b.GivenName, b.FamilyName, b.MiddleName) : null,
                            a.Description,
                            a.FileUrl
                            //a.Position,
                            //Email = b != null ? b.Email : null,
                            //Active = b != null ? b.Active : false,
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "FileName", "ReposName", "CreatedTime", "Tags", "Description", "FileUrl");

            return Json(jdata);
        }
        [HttpPost]
        public JsonResult InsertProjectFile([FromBody]ProjectFile obj)
        {
            var msg = new JMessage() { Error = false };
            //var getRepository = _context.EDMSRepositorys.Where(x => !x.IsDeleted).OrderByDescending(x => x.ReposID).AsNoTracking();

            try
            {
                var query = _context.ProjectFiles.FirstOrDefault(x => x.FileCode == obj.FileCode && x.Tags == obj.Tags);
                if (query == null)
                {
                    obj.CreatedBy = ESEIM.AppContext.UserName;
                    obj.CreatedTime = DateTime.Now;
                    _context.ProjectFiles.Add(obj);
                    _context.SaveChanges();
                    msg.Title = "Thêm tệp tin cho dự án thành công";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Tệp tin và nhân viên được Tags đã tồn tại trong dự án!";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi thêm tệp tin!";
                msg.Object = ex;
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult UpdateProjectFile([FromBody]ProjectFile obj)
        {
            var msg = new JMessage();
            try
            {
                obj.UpdatedTime = DateTime.Now.Date;
                _context.ProjectFiles.Update(obj);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Cập nhật tệp tin cho dự án thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi cập nhật tệp tin!";
                msg.Object = ex;
            }
            return Json(msg);
        }
        [HttpPost]
        public object DeleteProjectFile(int id)
        {
            var msg = new JMessage();
            try
            {
                var data = _context.ProjectFiles.FirstOrDefault(x => x.Id == id);
                data.FlagDeleted = true;
                _context.ProjectFiles.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Xóa tệp tin thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi xóa tệp tin!";
                msg.Object = ex;
                return Json(msg);
            }
        }
        [HttpPost]
        public object GetProject()
        {
            var query = _context.Projects.Where(x => !x.FlagDeleted).Select(x => new { Code = x.ProjectCode, Name = x.ProjectTitle });
            return query;
        }
        #endregion

        #region  Payment
        public class JTableModelPayment : JTableModel
        {
            public int? ProjectId { get; set; }
            public string PayTitle { get; set; }
            public int? PaymentType { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }
        [HttpPost]
        public object JTableProjectTabPayment([FromBody]JTableModelPayment jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            //bool x=true;
            //if (jTablePara.PaymentType == 0)
            //    x = false;
            //else if(jTablePara.PaymentType == 1)
            //    x = true;

            DateTime? fromDate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? toDate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var payType = jTablePara.PaymentType == 1 ? true : false;
            var da = _context.Projects.Where(m => m.Id == jTablePara.ProjectId).Select(x => x.ProjectCode).FirstOrDefault();
            if (jTablePara.ProjectId == null)
            {
                var list = new List<object>();
                return JTableHelper.JObjectTable(list, jTablePara.Draw, 0, "PayCode", "PayTitle", "PayType", "MoneyTotal", "CreatedTime");
            }
            var query = from a in _context.MaterialPaymentTickets
                        where (a.PayObjId == da)
                        && ((fromDate == null || (a.CreatedTime >= fromDate)) && (toDate == null || (a.CreatedTime <= toDate)))
                        && (string.IsNullOrEmpty(jTablePara.PayTitle) || (!string.IsNullOrEmpty(a.PayTitle) && a.PayTitle.ToLower().Contains(jTablePara.PayTitle.ToLower())))
                        && ((jTablePara.PaymentType == null) || (jTablePara.PaymentType != null && a.PayType == payType))
                        select new
                        {
                            a.PayCode,
                            a.PayTitle,
                            a.PayType,
                            a.MoneyTotal,
                            a.CreatedTime,
                        };

            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "PayCode", "PayTitle", "PayType", "MoneyTotal", "CreatedTime");
            return Json(jdata);

        }
        //[HttpPost]
        //public object GetPaymentType()
        //{
        //    var data = _context.CommonSetting.Where(x => x.Group == "PAY_TYPE").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
        //    return data;
        //}

        //[HttpPost]
        //public object GetObjPayment()
        //{
        //    var listSupplier = _context.Suppliers.Where(x => !x.IsDeleted).Select(x => new { Id = x.SupID, Name = x.SupName });
        //    var listCustomer = _context.EDMSCustomer.Where(x => !x.IsDeleted).Select(x => new { Id = x.CusID, Name = x.CusName });
        //    var list = listSupplier.Concat(listCustomer);
        //    return list;
        //}
        #endregion

        #region note
        public class JTableNote : JTableModel
        {
            public int? ProjectId { get; set; }
            public string Title { get; set; }
            public string Tags { get; set; }
        }
        [HttpPost]
        public object JTableProjectNote([FromBody]JTableNote jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var da = _context.Projects.FirstOrDefault(x => x.Id == jTablePara.ProjectId);
            if (jTablePara.ProjectId == null)
            {
                var list = new List<object>();
                return JTableHelper.JObjectTable(list, jTablePara.Draw, 0, "Id", "Title", "Note", "Name", "CreatedBy", "CreatedTime");
            }
            var query = from a in _context.ProjectNotes
                        join b in _context.Users on a.Tags equals b.Id into b1
                        from b in b1.DefaultIfEmpty()
                        where !a.IsDeleted
                        && (string.IsNullOrEmpty(jTablePara.Tags) || (a.Tags.ToLower().Contains(jTablePara.Tags.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.Title) || a.Title.ToLower().Contains(jTablePara.Title.ToLower()))
                        && (a.ProjectCode == da.ProjectCode)
                        select new
                        {
                            a.Id,
                            a.Title,
                            a.Note,
                            Name = b != null ? string.Join(" ", b.GivenName, b.FamilyName, b.MiddleName) : null,
                            a.CreatedBy,
                            a.CreatedTime
                        };
            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "Title", "Note", "Name", "CreatedBy", "CreatedTime");
            return Json(jdata);
        }
        [HttpPost]
        public JsonResult InsertProjectTabNote([FromBody]EDMSProjectTabNote obj)
        {
            var msg = new JMessage { Error = false };
            try
            {
                var query = _context.ProjectNotes.FirstOrDefault(x => x.ProjectCode == obj.ProjectCode && x.Tags == obj.MemberCode && !x.IsDeleted);
                var ver = _context.Projects.FirstOrDefault(x => x.ProjectCode == obj.ProjectCode);
                if (query == null)
                {
                    ProjectNote data = new ProjectNote()
                    {
                        Tags = obj.MemberCode,
                        ProjectCode = obj.ProjectCode,
                        Title = obj.Title,
                        Note = obj.Note,
                        ProjectVersion = ver.Version,
                        CreatedBy = ESEIM.AppContext.UserName,
                        CreatedTime = DateTime.Now
                    };
                    _context.ProjectNotes.Add(data);
                    _context.SaveChanges();
                    msg.Title = "Thêm ghi chú cho dự án thành công!";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Ghi chú muốn thêm đã tồn tại trong dự án!";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi thêm ghi chú vào dự án!";
                msg.Object = ex;
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult UpdateProjectTabNote([FromBody]EDMSProjectTabNote obj)
        {
            var msg = new JMessage();
            try
            {
                var data = _context.ProjectNotes.FirstOrDefault(x => x.Tags == obj.Tags && x.ProjectCode == obj.ProjectCode && !x.IsDeleted);
                data.Title = obj.Title;
                data.Note = obj.Note;
                data.Tags = obj.MemberCode;
                data.UpdatedBy = ESEIM.AppContext.UserName;
                data.UpdatedTime = DateTime.Now;
                _context.ProjectNotes.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Cập nhật ghi chú thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi cập nhật!";
                msg.Object = ex;
            }
            return Json(msg);
        }
        [HttpPost]
        public object DeleteprojectTabNote(int id)
        {
            var msg = new JMessage();
            try
            {
                var data = _context.ProjectNotes.FirstOrDefault(x => x.Id == id);
                data.IsDeleted = true;
                _context.ProjectNotes.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Xóa thành ghi chú công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi xóa ghi chú!";
                msg.Object = ex;
                return Json(msg);
            }

        }
        [HttpGet]
        public object GetNote(int id)
        {
            var data = _context.ProjectNotes.FirstOrDefault(x => x.Id == id);
            return Json(data);
        }
        #endregion

        #region CardJob
        public class JTableModelCardJob : JTableModel
        {
            public int? ProjectId { get; set; }
        }
        [HttpPost]
        public object JTableCardJob([FromBody]JTableModelCardJob jtablePara)
        {
            int intBegin = (jtablePara.CurrentPage - 1) * jtablePara.Length;
            if (jtablePara.ProjectId == null)
            {
                var list = new List<object>();
                return JTableHelper.JObjectTable(list, jtablePara.Draw, 0, "Id", "CardCode", "Project", "CardName");
            }
            var query = from a in _context.CardForWObj
                        where a.IsDeleted == false &&
                              a.CatObjCode.Equals("PROJECT") &&
                              a.ObjCode.Equals(_context.Projects.FirstOrDefault(x => x.Id == jtablePara.ProjectId).ProjectCode)
                        select new
                        {
                            Id = a.Id,
                            CardCode = a.CardCode,
                            Project = a.ObjCode,
                            CardName = (_context.WORKOSCards.FirstOrDefault(x => x.CardCode.Equals(a.CardCode)).CardName)
                        };

            int count = query.Count();
            var data = query.OrderUsingSortExpression(jtablePara.QueryOrderBy).Skip(intBegin).Take(jtablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jtablePara.Draw, count, "Id", "CardCode", "Project", "CardName");
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
                int projectId = (int)data.ObjCode.Value;
                string CardCode = data.CardCode.Value;
                string CatObjCode = data.CatObjCode.Value;
                string Relative = data.Relative;
                string ProjectCode = _context.Projects.FirstOrDefault(x => x.Id == projectId).ProjectCode;

                if (_context.CardForWObj.Where(x => x.ObjCode.Equals(ProjectCode) && x.CatObjCode.Equals("PROJECT") && x.CardCode.Equals(CardCode) && x.IsDeleted == false).Count() > 0)
                {
                    msg.Title = "Dự án này đã tồn tại!";
                    return Json(msg);
                }

                CardForWObj obj = new CardForWObj
                {
                    CatObjCode = CatObjCode,
                    Relative = Relative,
                    CardCode = CardCode,
                    ObjCode = ProjectCode
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


    public class JTableModelProject : JTableModel
    {
        public string ProjectCode { get; set; }
        public string ProjectTitle { get; set; }
        public string Budget { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class ProjectCustomerAdd
    {
        public string CusCode { get; set; }
        public string ProjectCode { get; set; }
    }

    public class EDMSProjectTabMember
    {

        public string Position { get; set; }
        public string ProjectCode { get; set; }
        public string MemberCode { get; set; }
        public string Member { get; set; }
    }

    public class EDMSProjectTabNote
    {
        public string ProjectCode { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public string MemberCode { get; set; }
        public string Tags { get; set; }
    }
    public class ProjectModel
    {
        public string ProjectCode { get; set; }
        public string ProjectTitle { get; set; }
        public string Currency { get; set; }
        public string Budget { get; set; }
        public string PrjSkillKeyword { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string PrjMode { get; set; }
        public string SetPriority { get; set; }
        public string PrjStatus { get; set; }

    }
}