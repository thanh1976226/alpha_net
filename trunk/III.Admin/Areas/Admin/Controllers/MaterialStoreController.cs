using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class MaterialStoreController : BaseController
    {
        private readonly EIMDBContext _context;

        public MaterialStoreController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelStore : JTableModel
        {
            public string StoreCode { get; set; }
            public string StoreName { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }

        [HttpPost]
        public object Jtable([FromBody]JTableModelStore jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;

            string storeCode = jTablePara.StoreCode.ToLower();
            string storeName = jTablePara.StoreName.ToLower();
            DateTime? fromDate = string.IsNullOrEmpty(jTablePara.FromDate) ? (DateTime?)null : DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime? toDate = string.IsNullOrEmpty(jTablePara.ToDate) ? (DateTime?)null : DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var query = from a in _context.MaterialStores
                        where (a.IsDeleted == false &&
                               a.StoreCode.ToLower().Contains(storeCode) &&
                               a.StoreName.ToLower().Contains(storeName) &&
                               ((fromDate == null || (a.CreatedTime >= fromDate)) &&
                               (toDate == null || (a.CreatedTime <= toDate))))
                        select new
                        {
                            id = a.StoreId,
                            code = a.StoreCode,
                            name = a.StoreName,
                            location = a.Location,
                            description = a.Description,
                            manager = a.UserId,
                            createdTime = a.CreatedTime
                        };

            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "id", "code", "name", "location", "description", "manager", "createdTime");
            return Json(jdata);
        }

        [HttpPost]
        public object GetUser()
        {
            var query = _context.Users.Where(x => x.UserType == 10).Select(x => new { x.Id, x.UserName, x.GivenName }).AsNoTracking().ToList();
            return query;
        }

        [HttpPost]
        public object GetBranch()
        {
            var query = _context.AdOrganizations.Select(x => new { x.OrgId, x.OrgName }).AsNoTracking().ToList();
            return query;
        }

        [HttpPost]
        public object Insert([FromBody]MaterialStore data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var model = _context.MaterialStores.Where(x => x.StoreCode.Equals(data.StoreCode)).FirstOrDefault();
                if (model == null)
                {
                    data.CreatedBy = ESEIM.AppContext.UserName;
                    data.CreatedTime = DateTime.Now;

                    _context.MaterialStores.Add(data);
                    _context.SaveChanges();

                    msg.Title = "Thêm thành công";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Danh mục kho đã tồn tại!";
                }
                
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi thêm!";
                msg.Object = ex;
                return Json(msg);
            }
        }

        [HttpPost]
        public object GetStore(int id)
        {
            var data = _context.MaterialStores.FirstOrDefault(x => x.StoreId == id);
            return Json(data);
        }

        [HttpPost]
        public object Update([FromBody]MaterialStore data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                if (data != null)
                {
                    data.UpdatedBy = ESEIM.AppContext.UserName;
                    data.UpdatedTime = DateTime.Now;

                    _context.MaterialStores.Update(data);
                    _context.SaveChanges();

                    msg.Title = "Cập nhật thành công";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Có lỗi xảy ra khi cập nhật!";
                }
               
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
        public object Delete(int id)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var data = _context.MaterialStores.FirstOrDefault(x => x.StoreId == id);
                data.DeletedBy = ESEIM.AppContext.UserName;
                data.DeletedTime = DateTime.Now;
                data.IsDeleted = true;

                _context.MaterialStores.Update(data);
                _context.SaveChanges();

                msg.Title = "Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi xóa!";
                msg.Object = ex;
                return Json(msg);
            }
        }

        #region
        //dropdown
        [HttpPost]
        public object GetStoreStatuss()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "STATUS").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return data;
        }

        [HttpPost]
        public JsonResult GetStoreAreas()
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
        public JsonResult GetStoreExtends()
        {
            var msg = new JMessage() { Error = false };

            var data = from a in _context.CommonSettings
                       where a.Group == "STORE_EXTEND" && a.IsDeleted == false
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