using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using Microsoft.Extensions.Options;
using ESEIM;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class MaterialProductGroupController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly AppSettings _appSettings;
        public class JTableModelCustom : JTableModel
        {
            public string code { get; set; }
            public string name { get; set; }
            public string parenid { get; set; }
            public string key1 { get; set; }
        }
        public MaterialProductGroupController(EIMDBContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
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
            var query = from a in _context.MaterialProductGroups
                            //join b in _context.cms_extra_fields_groups on a.Group equals b.Id
                            //orderby b.Name
                        where (jTablePara.code == null || jTablePara.code == "" || a.Code.ToLower().Contains(jTablePara.code.ToLower()))
                        && (jTablePara.name == null || jTablePara.name == "" || a.Name.ToString().ToLower().Contains(jTablePara.name.ToLower()))
                        && (jTablePara.parenid == null || jTablePara.parenid == "" || a.Name.Contains(jTablePara.parenid))
                        select new
                        {
                            Id = a.Id,
                            Code = a.Code,
                            Name = a.Name,
                            ParenID = a.ParentID,
                            Description = a.Description
                        };
            var count = query.Count();

            var data = query
                .OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "Code", "Name", "ParenID", "Description");

            return Json(jdata);
        }
        [HttpPost]
        public JsonResult Insert([FromBody]MaterialProductGroup obj)
        {
            var msg = new JMessage { Title = "", Error = false };
            try
            {
                var exist = _context.MaterialProductGroups.FirstOrDefault(x => x.Code == obj.Code);
                if (exist != null)
                {
                    msg.Error = true;
                    msg.Title = "Mã nhóm vật tư đã tồn tại";
                }
                else
                {
                    obj.CreatedBy = ESEIM.AppContext.UserName;
                    obj.CreatedTime = DateTime.Now;
                    _context.MaterialProductGroups.Add(obj);
                    _context.SaveChanges();
                    msg.Title = "Thêm nhóm vật tư thành công";
                }
            }
            catch
            {
                msg.Error = true;
                msg.Title = "Thêm nhóm vật tư lỗi";
            }
            return Json(msg);
        }
        [HttpPost]
        public object Update([FromBody]MaterialProductGroup obj)
        {
            var msg = new JMessage();
            try
            {
                obj.UpdatedTime = DateTime.Now.Date;
                _context.MaterialProductGroups.Update(obj);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = "Đã lưu thay đổi";

                return msg;
            }
            catch
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra!";
                return msg;
            }
        }
        [HttpPost]
        public object Delete(int id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.MaterialProductGroups.FirstOrDefault(x => x.Id == id);
                _context.MaterialProductGroups.Remove(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "Xóa thành công!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi khi xóa!";
                return Json(msg);
            }
        }
        public object GetItem(int id)
        {

            //if (id == null || id < 0)
            //{
            //    return Json("");
            //}
            var a = _context.MaterialProductGroups.AsNoTracking().Single(m => m.Id == id);
            return Json(a);
        }
        [HttpPost]
        public object gettreedataCategory()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.ProductCats.OrderBy(x => x.ProductID);
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
        public object gettreedataCoursetype()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.MaterialProductGroups.GroupBy(x => x.Name).Select(x => x.First()).Where(d => d.ParentID == null);
                var dataa = data.Distinct();
                msg.Object = dataa;

                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }
        [HttpPost]
        public object gettreedataLevel()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.MaterialProductGroups.GroupBy(x => x.Name).Select(x => x.First()).Where(d => d.ParentID == null).ToList();
                var dataa = data.Distinct();
                msg.Object = dataa;

                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }
    }
}
