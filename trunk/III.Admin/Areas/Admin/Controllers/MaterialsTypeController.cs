using System;
using System.Collections.Generic;
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
    public class MaterialsTypeController : BaseController
    {
        private readonly EIMDBContext _context;
        public MaterialsTypeController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelMaterial : JTableModel
        {
            public string MaterialCode { get; set; }
            public string MaterialName { get; set; }
            public int? MaterialParent { get; set; }
        }

        [HttpPost]
        public object JTable([FromBody]JTableModelMaterial jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            string code = jTablePara.MaterialCode.ToLower();
            string name = jTablePara.MaterialName.ToLower();
            int? parent = jTablePara.MaterialParent;

            var query = from a in _context.MaterialTypes
                        where (a.IsDeleted == false &&
                               a.Code.ToLower().Contains(code) &&
                               a.Name.ToLower().Contains(name) &&
                               (parent == null || parent == a.ParentId))
                        select new
                        {
                            id = a.Id,
                            code = a.Code,
                            name = a.Name,
                            description = a.Description
                        };
            int count = query.Count();
            //var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length).AsNoTracking().ToList();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "id", "code", "name", "description");
            return Json(jdata);
        }

        [HttpPost]
        public object Insert([FromBody]MaterialType data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                if(_context.MaterialTypes.FirstOrDefault(x=>x.Code==data.Code)==null)
                {
                    data.CreatedBy = ESEIM.AppContext.UserName;
                    data.CreatedTime = DateTime.Now;

                    _context.MaterialTypes.Add(data);
                    _context.SaveChanges();

                    msg.Title = "Thêm thành công loại vật tư!";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Mã loại vật tư đã tồn tại!";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi thêm loại vật tư!";
                msg.Object = ex;
            }
            return Json(msg);
        }

        [HttpPost]
        public object Delete(int id)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var data = _context.MaterialTypes.FirstOrDefault(x => x.Id == id);
                data.IsDeleted = true;
                data.DeletedBy = ESEIM.AppContext.UserName;
                data.DeletedTime = DateTime.Now;

                _context.MaterialTypes.Update(data);
                _context.SaveChanges();

                msg.Title = "Xóa thành công loại vật tư!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi xóa loại vật tư!";
                msg.Object = ex;
                return Json(msg);
            }
        }
        [HttpPost]
        public object GetData(int id)
        {
            var data = _context.MaterialTypes.FirstOrDefault(x => x.Id == id);
            return Json(data);
        }

        [HttpPost]
        public object Update([FromBody]MaterialType data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                data.UpdatedBy = ESEIM.AppContext.UserName;
                data.UpdatedTime = DateTime.Now;
                _context.MaterialTypes.Update(data);
                _context.SaveChanges();

                msg.Title = "Cập nhật thành công loại vật tư!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi cập nhật loại vật tư!";
                msg.Object = ex;
                return Json(msg);
            }
        }

        [HttpPost]
        public object GetAllData()
        {
            var data = _context.MaterialTypes.Select(x => new { x.Id, x.Name }).AsNoTracking().ToList();
            return Json(data);
        }
    }
}