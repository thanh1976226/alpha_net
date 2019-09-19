﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using System.Collections.Generic;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class FundCatReptExpsController : BaseController
    {
        public class FundCatReptExpsJtableModel
        {
            public int Id { get; set; }
            public string CatCode { get; set; }
            public string CatName { get; set; }
            public string CatParent { get; set; }
            public string CatType { get; set; }
            public string Note { get; set; }
        }
        private readonly EIMDBContext _context;
        public FundCatReptExpsController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelCat : JTableModel
        {
            public string CatCode { get; set; }
            public string CatName { get; set; }
            public string CatParent { get; set; }
            public string CatType { get; set; }
            public string Note { get; set; }
        }

        #region combobox
        //[HttpPost]
        //public object GetActivityType()
        //{
        //    var data = _context.CommonSettings.Where(x => x.Group == "ASSET_ACTIVITY_TYPE").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
        //    return data;
        //}


        [HttpPost]
        public object GetUser()
        {
            var query = _context.Users.Where(x => x.Active == true && x.UserType != 10).Select(x => new { x.Id, x.GivenName }).AsNoTracking().ToList();
            return query;
        }

        [HttpPost]
        public object GetCatParent()
        {
            var data = _context.FundCatReptExpss.Where(x => x.IsDeleted == false).Select(x => new { CatCode = x.CatCode }).AsNoTracking().ToList();
            //var data = _context.FundCatReptExpss.Select(x => new { CatCode = x.CatCode}).AsNoTracking().ToList();
            return data;
        }
        [HttpPost]
        public object GetCatCode()
        {
            var data = _context.FundCatReptExpss.Where(x => x.IsDeleted == false).Select(x => new { CatCode = x.CatCode }).AsNoTracking().ToList();
            //var data = _context.FundCatReptExpss.Where(x => x.IsDeleted == false).Select(x => new { CatCode = x.CatCode }).AsNoTracking().ToList();
            return data;
        }

        [HttpPost]
        public List<TreeView> GetTreeData([FromBody]TempSub obj)
        {
            //if (obj.IdI == null && obj.IdS == null)
            //{
            //    return null;
            //}
            if (obj.IdI == null)
            {
                var data = _context.FundCatReptExpss.OrderBy(x => x.CatName).AsNoTracking();
                var dataOrder = GetSubTreeData(data.ToList(), null, new List<TreeView>(), 0);
                return dataOrder;
            }
            else
            {
                string catCode = _context.FundCatReptExpss.Where(x => x.Id == obj.IdI[0]).Select(x => x.CatCode).FirstOrDefault();

                var data = _context.FundCatReptExpss.OrderBy(x => x.CatName).Where(x => (x.CatCode != catCode && x.CatParent != catCode)).AsNoTracking();
                var dataOrder = GetSubTreeData(data.ToList(), null, new List<TreeView>(), 0);
                return dataOrder;
            }
        }

        private List<TreeView> GetSubTreeData(List<FundCatReptExps> data, string catParent, List<TreeView> lstCategories, int tab)
        {
            //tab += "- ";
            var contents = catParent == null
                ? data.Where(x => x.CatParent == null).OrderBy(x => x.CatName).ToList()
                : data.Where(x => x.CatParent == catParent).OrderBy(x => x.CatName).ToList();
            foreach (var item in contents)
            {
                var category = new TreeView
                {
                    Id = item.Id,
                    Code = item.CatCode,
                    Title = item.CatName,
                    Level = tab,
                    HasChild = data.Any(x => x.CatParent == item.CatCode)
                };
                lstCategories.Add(category);
                if (category.HasChild) GetSubTreeData(data, item.CatCode, lstCategories, tab + 1);
            }
            return lstCategories;
        }

        #endregion

        #region action

        [HttpPost]
        public object JTable([FromBody]JTableModelCat jTablePara)
        {
            //int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            //var query = from a in _context.FundCatReptExpss
            //            where a.IsDeleted == false
            //            && (string.IsNullOrEmpty(jTablePara.CatCode) || a.CatCode.ToLower().Contains(jTablePara.CatCode.ToLower()))
            //            && (string.IsNullOrEmpty(jTablePara.CatName) || a.CatName.ToLower().Contains(jTablePara.CatName.ToLower()))
            //            && (string.IsNullOrEmpty(jTablePara.CatType) || a.CatType.ToLower().Contains(jTablePara.CatType.ToLower()))
            //            select new FundCatReptExpsJtableModel
            //            {

            //                Id = a.Id,
            //                CatCode = a.CatCode,
            //                CatName = a.CatName,
            //                CatParent = a.CatParent,
            //                CatType = a.CatType,
            //                Note = a.Note
            //            };
            //int count = query.Count();
            //var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            //var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "CatCode", "CatName", "CatParent", "CatType", "Note");
            //return Json(jdata);
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            if (string.IsNullOrEmpty(jTablePara.CatCode) && string.IsNullOrEmpty(jTablePara.CatName) && string.IsNullOrEmpty(jTablePara.CatType))
            {
                var query = from a in _context.FundCatReptExpss
                            where a.IsDeleted == false
                            select new FundCatReptExpsJtableModel
                            {
                                Id = a.Id,
                                CatCode = a.CatCode,
                                CatName = a.CatName,
                                CatParent = a.CatParent,
                                CatType = a.CatType,
                                Note = a.Note
                            };
                var data = query.OrderBy(x => x.Id).AsNoTracking();
                var listFunction = data as IList<FundCatReptExpsJtableModel> ?? data.ToList();

                var result = new List<FundCatReptExpsJtableModel>();
                foreach (var func in listFunction.Where(x => string.IsNullOrEmpty(x.CatParent)).OrderBy(x => x.Id))
                {
                    var listChild = GetFunctionChild(listFunction, func.CatCode, ". . . ");

                    var function = new FundCatReptExpsJtableModel();
                    function.Id = func.Id;
                    function.CatName = (listChild.Count > 0 ? "<i class='fa fa-folder-open icon-state-warning'></i> " : "<i class='fa fa-folder text-info'></i> ") + func.CatName;
                    function.CatCode = func.CatCode;
                    function.CatType = func.CatType;
                    function.CatParent = func.CatParent;
                    function.Note = func.Note;
                    //function.TotalRow = listFunction.Count;
                    result.Add(function);
                    if (listChild.Count > 0) result = result.Concat(listChild).ToList();
                }
                var count = result.Count();
                var res = result.Skip(intBeginFor).Take(jTablePara.Length).ToList();
                var jdata = JTableHelper.JObjectTable(res, jTablePara.Draw, count, "Id", "CatCode", "CatName", "CatParent", "CatType", "Note");
                return Json(jdata);
            }
            else
            {
                var query = from a in _context.FundCatReptExpss
                            where a.IsDeleted == false
                            && (string.IsNullOrEmpty(jTablePara.CatName) || a.CatName.ToLower().Contains(jTablePara.CatName.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.CatType) || a.CatType.ToLower().Contains(jTablePara.CatType.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.CatParent) || a.CatParent.ToLower().Contains(jTablePara.CatParent.ToLower()))
                            select new FundCatReptExpsJtableModel
                            {

                                Id = a.Id,
                                CatCode = a.CatCode,
                                CatName = a.CatName,
                                CatParent = a.CatParent,
                                CatType = a.CatType,
                                Note = a.Note
                            };
                int count = query.Count();
                var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length);
                var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "CatCode", "CatName", "CatParent", "CatType", "Note");
                return Json(jdata);
            }
        }

        [NonAction]
        private static List<FundCatReptExpsJtableModel> GetFunctionChild(IList<FundCatReptExpsJtableModel> listFunction, string parentCode, string level)
        {
            var result = new List<FundCatReptExpsJtableModel>();
            //var totalRow = listFunction.Count;
            var query = from func in listFunction
                        where func.CatParent == parentCode
                        orderby func.Id
                        select new FundCatReptExpsJtableModel
                        {
                            Id = func.Id,
                            CatName = func.CatName,
                            CatCode = func.CatCode,
                            CatParent = func.CatParent,
                            CatType = func.CatType,
                            Note = func.Note,
                        };

            var listFunc = query as IList<FundCatReptExpsJtableModel> ?? query.ToList();
            foreach (var func in listFunc)
            {
                var destination = GetFunctionChild(listFunction, func.CatCode, ". . . " + level);
                func.CatName = level + (destination.Count > 0 ? "<i class='fa fa-folder-open icon-state-warning'></i> " : "<i class='fa fa-folder text-info'></i> ") + func.CatName;
                result.Add(func);
                if (destination.Count > 0) result = result.Concat(destination).ToList();
            }
            return result;
        }

        [HttpPost]
        public object GetItem([FromBody] int id)
        {
            var data = _context.FundCatReptExpss.FirstOrDefault(x => x.Id == id);
            return data;
        }

        [HttpPost]
        public JsonResult Insert([FromBody]FundCatReptExps data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var checkExist = _context.FundCatReptExpss.FirstOrDefault(x => x.CatCode.ToLower() == data.CatCode.ToLower());
                if (checkExist != null)
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("FCRE_MSG_CAT_CODE"));//"Đã tồn danh mục quỹ!";
                }
                else
                {
                    data.CreatedBy = ESEIM.AppContext.UserName;
                    data.CreatedTime = DateTime.Now;
                    if(string.IsNullOrEmpty(data.CatParent))
                    {
                        data.CatParent = "CAT_FUND";
                    }
                    _context.FundCatReptExpss.Add(data);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("FCRE_MSG_ADD_DONE"));//"Thêm danh mục quỹ thành công";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FCRE_MSG_ADD_ERROR"));//"Có lỗi xảy ra khi thêm?";
            }
            return Json(msg);
        }

        [HttpPost]
        public object Update([FromBody]FundCatReptExps data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                data.UpdatedBy = ESEIM.AppContext.UserName;
                data.UpdatedTime = DateTime.Now;
                _context.FundCatReptExpss.Update(data);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("FCRE_MSG_UPDATE_DONE"));//"Cập nhật danh mục quỹ thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FCRE_MSG_UPDATE_ERROR"));//"Có lỗi xảy ra khi cập nhật!";
            }
            return Json(msg);
        }

        [HttpPost]
        public object Delete(int id)
        {
            var msg = new JMessage() { Error = false };
           
                try
                {
                var data = _context.FundCatReptExpss.FirstOrDefault(x => x.Id == id);
                if (data.CatCode != "CAT_FUND" && data.CatParent != null)
                {
                    data.DeletedBy = ESEIM.AppContext.UserName;
                    data.DeletedTime = DateTime.Now;
                    data.IsDeleted = true;

                    _context.FundCatReptExpss.Update(data);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("FCRE_MSG_DELETE_DONE"));//"Xóa thành công";
                    return Json(msg);
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Không đượcphép xóa danh mục này";
                    return Json(msg);
                }
                
                }
                catch (Exception ex)
                {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FCRE_MSG_DELETE_ERROR"));//"Có lỗi xảy ra khi xóa!";
                msg.Object = ex;
                return Json(msg);
                }
        }
        #endregion
    }
}