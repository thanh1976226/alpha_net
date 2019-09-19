﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using Microsoft.Extensions.Logging;
using III.Admin.Controllers;

namespace III.Admin.Controllers
{
    public class FunctionModel
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string TitleJoin { set; get; }
        public string Code { set; get; }
        //public int? ParentId { set; get; }
        public string ParentCode { set; get; }
        public int? Ord { set; get; }
        public string Description { set; get; }
        //public int TotalRow { set; get; }
        public bool IsChecked { set; get; }
        public int Level { get; set; }
        public bool HasChild { get; set; }
    }
    public class JTableModelFunctionCustom : JTableModel
    {
        public string AppCode { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
        public int? Id { set; get; }
    }
    [Area("Admin")]
    public class FunctionController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly ILogger _logger;
        private readonly IActionLogService _actionLog;


        public FunctionController(EIMDBContext context, ILogger<FunctionController> logger, IActionLogService actionLog)
        {
            _context = context;
            _logger = logger;
            _actionLog = actionLog;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public object GetAll()
        {
            //_logger.LogInformation(LoggingEvents.LogDb, "Get list function");
            //_actionLog.InsertActionLog("VIB_FUNCTION", "Get list function", null, null, "GetAll");

            var rs = _context.AdFunctions.OrderBy(x => x.Title).Select(x => new { x.FunctionId, x.Title, x.FunctionCode, x.ParentCode, x.Ord, x.Description }).AsNoTracking().ToList();
            return Json(rs);
        }
        //[HttpGet]
        //public object GetByName(string name)
        //{
        //    //_logger.LogInformation(LoggingEvents.LogDb, "Get list function");
        //    var rs = _context.VIBFunctions.Where(x => name == null || name == "" || x.Title.ToLower().Contains(name.ToLower())).OrderBy(x => x.Title).Select(x => new { x.Id, x.Title, x.Code, x.ParentId, x.Ord, x.Description }).AsNoTracking().ToList();
        //    return Json(rs);
        //}
        //[HttpGet]
        //public object GetByNameAddParent(string name)
        //{
        //	//_logger.LogInformation(LoggingEvents.LogDb, "Get list function");
        //	//_actionLog.InsertActionLog("VIB_FUNCTION", "Get list function", null, null, "GetAll");

        //	try
        //	{
        //		if (string.IsNullOrEmpty(name))
        //		{
        //			var rs = _context.VIBFunctions
        //				.OrderBy(x => x.Ord)
        //				.Select(x => new { x.FunctionId, x.Title, x.FunctionCode, x.ParentCode, x.Ord, x.Description })
        //				.AsNoTracking().ToList();
        //			return Json(rs);
        //		}
        //		else
        //		{
        //			var rs = _context.VIBFunctions
        //				.Where(x => x.Title.ToLower().Contains(name.ToLower()))
        //				.OrderBy(x => x.Ord)
        //				.Select(x => new { x.FunctionId, x.Title, x.FunctionCode, x.ParentCode, x.Ord, x.Description })
        //				.AsNoTracking().ToList();
        //			var listIdAdd = new List<Object>();
        //			List<int> listParentTreeId = new List<int>();
        //			foreach (var item in rs)
        //			{
        //				GetParentTreeId(item.Id, listParentTreeId);
        //			}
        //			List<int> listTreeId = GetlistTreeId(listParentTreeId);
        //			foreach (var id in listTreeId)
        //			{
        //				var function = _context.VIBFunctions.SingleOrDefault(x => x.Id == id);
        //				listIdAdd.Add(new { Id = function.Id, Title = function.Title, Code = function.Code, ParentId = function.ParentId, Ord = function.Ord, Description = function.Description });
        //			}
        //			return Json(listIdAdd);
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		var msg = new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_LOAD_FAIL"), CommonUtil.ResourceValue("DATA").ToLower()), Object = ex };
        //		//_logger.LogInformation(LoggingEvents.LogDb, "Get list function fail");
        //		return Json(msg);
        //	}
        //}
        //private List<int> GetParentTreeId(int? id, List<int> listParentTreeId)
        //{
        //	if (id != null)
        //	{
        //		var func = _context.VIBFunctions.SingleOrDefault(x => x.Id == id);
        //		if (func != null)
        //		{
        //			listParentTreeId.Add((int)id);
        //			GetParentTreeId(func.ParentId, listParentTreeId);
        //		}
        //	}
        //	return listParentTreeId;
        //}
        //Delete Duplicate Id in list
        private List<int> GetlistTreeId(List<int> listTreeId)
        {
            List<int> noDupes = listTreeId.Distinct().ToList();
            return noDupes;
        }
        //[HttpGet]
        //public object GetGroupResource()
        //{
        //	var rs = _context.VIBGroupResources.OrderBy(x => x.Id).AsNoTracking().ToList();
        //	return Json(rs);
        //}

        [HttpPost]
        public object JTable([FromBody]JTableModelFunctionCustom jTablePara)
        {
            //_logger.LogInformation(LoggingEvents.LogDb, "Get list function");
            //_actionLog.InsertActionLog("VIB_FUNCTION", "Get list function", null, null, "JTable");

            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            if (string.IsNullOrEmpty(jTablePara.Name) && string.IsNullOrEmpty(jTablePara.AppCode))
            {
                var query = from x in _context.AdFunctions
                            select new FunctionModel
                            {
                                Id = x.FunctionId,
                                Title = x.Title,
                                Code = x.FunctionCode,
                                ParentCode = x.ParentCode,
                                Ord = x.Ord,
                                Description = x.Description
                            };
                var data = query.OrderBy(x => x.Title).AsNoTracking();
                var listFunction = data as IList<FunctionModel> ?? data.ToList();

                var result = new List<FunctionModel>();
                foreach (var func in listFunction.Where(x => string.IsNullOrEmpty(x.ParentCode)).OrderBy(x => x.Title))
                {
                    var listChild = GetFunctionChild(listFunction, func.Code, ". . . ");

                    var function = new FunctionModel();
                    function.Id = func.Id;
                    function.Title = (listChild.Count > 0 ? "<i class='fa fa-folder-open icon-state-warning'></i> " : "<i class='fa fa-folder text-info'></i> ") + func.Title;
                    function.Code = func.Code;
                    function.ParentCode = func.ParentCode;
                    function.Ord = func.Ord;
                    function.Description = func.Description;
                    //function.TotalRow = listFunction.Count;
                    result.Add(function);
                    if (listChild.Count > 0) result = result.Concat(listChild).ToList();
                }
                var count = result.Count();
                var res = result.Skip(intBeginFor).Take(jTablePara.Length).ToList();
                var jdata = JTableHelper.JObjectTable(res, jTablePara.Draw, count, "Id", "Title", "Code", "ParentCode", "Ord", "Description");
                return Json(jdata);
            }
            else
            {
                var query = _context.AdFunctions
                    .Where(p => (string.IsNullOrEmpty(jTablePara.Name) || p.Title.ToLower().Contains(jTablePara.Name.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.AppCode) || p.FunctionCode.ToLower().Contains(jTablePara.AppCode.ToLower())))
                    .OrderBy(x => x.Title)
                    .Select(x => new { Id = x.FunctionId, x.Title, Code = x.FunctionCode, x.ParentCode, x.Ord, x.Description })
                    .AsNoTracking();
                var count = query.Count();

                var data = query
                    .Skip(intBeginFor)
                    .Take(jTablePara.Length).AsNoTracking()
                    .ToList();
                var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "Title", "Code", "ParentCode", "Ord", "Description");
                return Json(jdata);
            }
        }
        private static List<FunctionModel> GetFunctionChild(IList<FunctionModel> listFunction, string parentCode, string level)
        {
            var result = new List<FunctionModel>();
            //var totalRow = listFunction.Count;
            var query = from func in listFunction
                        where func.ParentCode == parentCode
                        orderby func.Title
                        select new FunctionModel
                        {
                            Id = func.Id,
                            Title = func.Title,
                            Code = func.Code,
                            ParentCode = func.ParentCode,
                            Ord = func.Ord,
                            Description = func.Description,
                            //TotalRow = totalRow,
                        };

            var listFunc = query as IList<FunctionModel> ?? query.ToList();
            foreach (var func in listFunc)
            {
                var destination = GetFunctionChild(listFunction, func.Code, ". . . " + level);
                func.Title = level + (destination.Count > 0 ? "<i class='fa fa-folder-open icon-state-warning'></i> " : "<i class='fa fa-folder text-info'></i> ") + func.Title;
                result.Add(func);
                if (destination.Count > 0) result = result.Concat(destination).ToList();
            }
            return result;
        }
        [HttpPost]
        public object JTableResourceByFunctionId([FromBody]JTableModelFunctionCustom jTablePara)
        {
            //_logger.LogInformation(LoggingEvents.LogDb, "Get list Resource of Function");
            //_actionLog.InsertActionLog("VIB_FUNCTION", "Get list Resource of Function", null, null, "JTable");

            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from x in _context.AdResources
                            //join g in _context.VIBGroupResources on x.GroupResourceId equals g.Id
                        join p in _context.AdPrivileges on x.ResourceCode equals p.ResourceCode
                        where p.FunctionCode == jTablePara.Code && x.Status == true
                        select new AdResource
                        {
                            ResourceId = x.ResourceId,
                            Title = x.Title,
                            ResourceCode = x.ResourceCode,
                            ParentCode = x.ParentCode,
                            Ord = x.Ord,
                            Description = x.Description,
                            //GroupResourceId = x.GroupResourceId,
                            //GroupResourceTitle = g.Title,
                            Path = x.Path,
                            Api = x.Api,
                            Status = x.Status
                        };
            var data = query.AsNoTracking();//.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking();
            var count = data.Count();
            var res = jTablePara.Length > 0 ? data.Skip(intBeginFor).Take(jTablePara.Length).ToList() : data.ToList();
            var jdata = JTableHelper.JObjectTable(res, jTablePara.Draw, count, "Id", "Title", "ResourceCode", "ParentCode", "Ord", "Description", /*"GroupResourceId", "GroupResourceTitle", */"Path", "Api", "Status");
            return Json(jdata);
        }

        //// Get table when using tree View
        //[HttpPost]
        //public object JTableResourceByFunctionId([FromBody]JTableModelFunctionCustom jTablePara)
        //{
        //    //_logger.LogInformation(LoggingEvents.LogDb, "Get list Resource of Function");
        //    int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
        //    var query = from x in _context.VIBResources
        //                join g in _context.VIBGroupResources on x.GroupResourceId equals g.Id
        //                join p in _context.VIBPrivileges on x.Id equals p.ResourceId
        //                where p.FunctionId == jTablePara.Id && x.Status == true
        //                select new ResourceModel
        //                {
        //                    Id = x.Id,
        //                    Title = x.Title,
        //                    Code = x.Code,
        //                    ParentId = x.ParentId,
        //                    Ord = x.Ord,
        //                    Description = x.Description,
        //                    //GroupResourceId = x.GroupResourceId,
        //                    //GroupResourceTitle = g.Title,
        //                    Path = x.Path,
        //                    Api = x.Api,
        //                    Status = x.Status
        //                };
        //    var data = query.OrderBy(x => x.Title).AsNoTracking();
        //    var listResource = data as IList<ResourceModel> ?? data.ToList();
        //    var result = new List<ResourceModel>();
        //    foreach (var resour in listResource.Where(x => x.ParentId == null))
        //    {
        //        var resource = new ResourceModel();
        //        resource.Id = resour.Id;
        //        resource.Title = resour.Title;
        //        resource.Code = resour.Code;
        //        resource.ParentId = resour.ParentId;
        //        resource.Ord = resour.Ord;
        //        resource.Description = resour.Description;
        //        //resource.GroupResourceId = resour.GroupResourceId;
        //        //resource.GroupResourceTitle = resour.GroupResourceTitle;
        //        resource.Path = resour.Path;
        //        resource.Api = resour.Api;
        //        resource.Status = resour.Status;
        //        result.Add(resource);
        //        result = result.Concat(GetResourceChild(listResource, resour.Id, ". . . ")).ToList();
        //    }
        //    var count = result.Count();
        //    var res = result.Skip(intBeginFor).Take(jTablePara.Length).ToList();
        //    var jdata = JTableHelper.JObjectTable(res, jTablePara.Draw, count, "Id", "Title", "Code", "ParentId", "Ord", "Description", /*"GroupResourceId", "GroupResourceTitle", */"Path", "Api", "Status");
        //    return Json(jdata);
        //}
        //private static List<ResourceModel> GetResourceChild(IList<ResourceModel> listResource, int parentId, string level)
        //{
        //    var result = new List<ResourceModel>();
        //    var query = from resour in listResource
        //                where resour.ParentId == parentId
        //                orderby resour.Title
        //                select new ResourceModel
        //                {
        //                    Id = resour.Id,
        //                    Title = level + resour.Title,
        //                    Code = resour.Code,
        //                    ParentId = resour.ParentId,
        //                    Ord = resour.Ord,
        //                    Description = resour.Description,
        //                    //GroupResourceId = resour.GroupResourceId,
        //                    //GroupResourceTitle = resour.GroupResourceTitle,
        //                    Path = resour.Path,
        //                    Api = resour.Api,
        //                    Status = resour.Status
        //                };

        //    var listResour = query as IList<ResourceModel> ?? query.ToList();
        //    foreach (var rs in listResour)
        //    {
        //        result.Add(rs);
        //        var destination = GetResourceChild(listResource, rs.Id, ". . . " + level);
        //        result = result.Concat(destination).ToList();
        //    }
        //    return result;
        //}

        [HttpPost]
        public JsonResult Insert([FromBody]AdFunction obj)
        {
            //_logger.LogInformation(LoggingEvents.LogDb, "Insert function");
            var msg = new JMessage() { Error = false };
            try
            {
                var func = _context.AdFunctions.FirstOrDefault(x => x.FunctionCode == obj.FunctionCode);
                if (func == null)
                {
                    func = new AdFunction();
                    func.FunctionCode = obj.FunctionCode;
                    func.Title = obj.Title;
                    func.Description = obj.Description;
                    func.ParentCode = obj.ParentCode;
                    func.CreatedDate = DateTime.Now;
                    _context.AdFunctions.Add(func);
                    var a = _context.SaveChanges();
                    //msg.Title = "Thêm khoản mục thành công";
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                    //_logger.LogInformation(LoggingEvents.LogDb, "Insert function successfully");
                    _actionLog.InsertActionLog("VIB_FUNCTION", "Insert function successfully", null, obj, "Insert");

                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("ERR_EXIST"), CommonUtil.ResourceValue("FUNC_CODE"));
                    //_logger.LogError(LoggingEvents.LogDb, "Insert application fail");
                    _actionLog.InsertActionLog("VIB_FUNCTION", "Insert function failed: Function code is exists", null, null, "Error");

                    return Json(msg);
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                //try
                //{
                //    if (ex.InnerException.Data["HelpLink.EvtID"].ToString() == "2627")
                //    {
                //        //msg.Title = "Mã đã tồn tại trong hệ thống";
                //        msg.Title = String.Format(CommonUtil.ResourceValue("ERR_EXIST"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                //        //_logger.LogError(LoggingEvents.LogDb, "Insert function fail");
                //        _actionLog.InsertActionLog("VIB_FUNCTION", "Insert function fail", null, null, "Error");


                //        return Json(msg);
                //    }
                //}
                //catch (Exception)
                //{
                //}

                msg.Object = ex;
                //msg.Title = "Có lỗi khi thêm khoản mục";
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAIL"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                //_logger.LogError(LoggingEvents.LogDb, "Insert function fail");
                _actionLog.InsertActionLog("VIB_FUNCTION", "Insert function failed: " + ex.Message, null, null, "Error");

                return Json(msg);
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult Update([FromBody]AdFunction obj)
        {
            //_logger.LogInformation(LoggingEvents.LogDb, "Update function");
            var msg = new JMessage() { Error = false };
            try
            {
                var objUpdate = _context.AdFunctions.SingleOrDefault(x => x.FunctionId == obj.FunctionId);
                var objOld = CommonUtil.Clone(objUpdate);
                if (objUpdate.FunctionCode != obj.FunctionCode)
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("ERR_CODE_CHANGE"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                    //_logger.LogError(LoggingEvents.LogDb, "Update function fail");
                    _actionLog.InsertActionLog("VIB_FUNCTION", "Update function failed: Function code can't change", null, null, "Error");

                }
                else
                {
                    objUpdate.Title = obj.Title;
                    objUpdate.ParentCode = obj.ParentCode;
                    objUpdate.Ord = obj.Ord;
                    objUpdate.Description = obj.Description;
                    objUpdate.UpdatedDate = DateTime.Now;
                    var a = _context.SaveChanges();
                    //msg.Title = "Sửa khoản mục thành công";
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                    //_logger.LogInformation(LoggingEvents.LogDb, "Update function successfully");
                    _actionLog.InsertActionLog("VIB_FUNCTION", "Update function successfully", objOld, obj, "Update");

                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                //try
                //{
                //    if (ex.InnerException.Data["HelpLink.EvtID"].ToString() == "2627")
                //    {
                //        //msg.Title = "Mã đã tồn tại trong hệ thống";
                //        msg.Title = String.Format(CommonUtil.ResourceValue("ERR_EXIST"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                //        //_logger.LogError(LoggingEvents.LogDb, "Update function fail");
                //        _actionLog.InsertActionLog("VIB_FUNCTION", "Update function fail", null, null, "Error");

                //        return Json(msg);
                //    }
                //}
                //catch (Exception)
                //{
                //}

                msg.Error = true;
                //msg.Title = "Có lỗi khi sửa khoản mục";
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_FAIL"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                //_logger.LogError(LoggingEvents.LogDb, "Update function fail");
                _actionLog.InsertActionLog("VIB_FUNCTION", "Update function failed: " + ex.Message, null, null, "Error");

            }
            return Json(msg);
        }
        [HttpPost]
        public object Delete([FromBody]int id)
        {
            //_logger.LogInformation(LoggingEvents.LogDb, "Delete function");
            try
            {
                AdFunction obj = _context.AdFunctions.SingleOrDefault(x => x.FunctionId == id);
                if (obj != null)
                {
                    var objChild = _context.AdFunctions.SingleOrDefault(x => x.ParentCode == obj.FunctionCode);
                    if (objChild == null)
                    {
                        var appFunc = _context.AdAppFunctions.FirstOrDefault(x => x.FunctionCode == obj.FunctionCode);
                        var priv = _context.AdPrivileges.FirstOrDefault(x => x.FunctionCode == obj.FunctionCode);
                        var pms = _context.AdPermissions.FirstOrDefault(x => x.FunctionCode == obj.FunctionCode);
                        if (appFunc != null || priv != null || pms != null)
                        {
                            _actionLog.InsertActionLog("VIB_FUNCTION", "Delete function fail", null, null, "Error");
                            return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("ERR_OBJ_REF"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC")) });
                        }
                        else
                        {
                            _context.Remove(obj);
                            _context.SaveChanges();
                            //_logger.LogInformation(LoggingEvents.LogDb, "Delete function successfully");
                            _actionLog.InsertActionLog("VIB_FUNCTION", "Delete function successfully", obj, null, "Delete");

                            //return Json(new JMessage() { Error = false, Title = "Xóa khoản mục thành công." });
                            return Json(new JMessage() { Error = false, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC")) });
                        }
                    }
                    else
                    {
                        //_logger.LogError(LoggingEvents.LogDb, "Delete function fail");
                        _actionLog.InsertActionLog("VIB_FUNCTION", "Delete function fail", null, null, "Error");

                        return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_CHILD"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC")) });
                    }
                }
                else
                {
                    //_logger.LogError(LoggingEvents.LogDb, "Delete function fail");
                    _actionLog.InsertActionLog("VIB_FUNCTION", "Delete function fail", null, null, "Error");

                    return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC")) });
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(LoggingEvents.LogDb, "Delete function fail");
                _actionLog.InsertActionLog("VIB_FUNCTION", "Delete function failed: " + ex.Message, null, null, "Error");

                //return Json(new JMessage() { Error = true, Title = "Có lỗi khi xóa khoản mục.", Object = ex.Message });
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC")) });
            }
        }

        [HttpPost]
        public object DeleteItems([FromBody]List<int> listIdI)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                //_logger.LogInformation(LoggingEvents.LogDb, "Delete list Function");
                List<int> listId = new List<int>();
                List<int> listRef = new List<int>();
                List<int> listDel = new List<int>();
                List<int> listDelFinal = new List<int>();
                List<AdFunction> listFunction = new List<AdFunction>();
                foreach (var id in listIdI)
                {
                    AdFunction obj = _context.AdFunctions.FirstOrDefault(x => x.FunctionId == id);
                    if (obj != null)
                    {
                        var appFunc = _context.AdAppFunctions.FirstOrDefault(x => x.FunctionCode == obj.FunctionCode);
                        var priv = _context.AdPrivileges.FirstOrDefault(x => x.FunctionCode == obj.FunctionCode);
                        var pms = _context.AdPermissions.FirstOrDefault(x => x.FunctionCode == obj.FunctionCode);
                        if (appFunc == null && priv == null && pms == null)
                        {
                            listId.Add(id);
                        }
                    }
                }
                if (listId.Any())
                {
                    // Find list Id haven't child , this list can delete (list A)
                    foreach (var id in listId)
                    {
                        AdFunction obj = _context.AdFunctions.FirstOrDefault(x => x.FunctionId == id);
                        if (obj != null)
                        {
                            var idRef = _context.AdFunctions.FirstOrDefault(x => x.ParentCode == obj.FunctionCode);
                            if (idRef == null)
                            {
                                listDel.Add(id);
                            }
                        }
                    }
                    if (listDel.Count == 0)
                    {
                        msg.Error = true;
                        msg.Title = String.Format(CommonUtil.ResourceValue("ERR_LIST_OBJ_HAS_CHILD"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                        //_logger.LogError(LoggingEvents.LogDb, "Delete list Function fail");
                        _actionLog.InsertActionLogDeleteItem("VIB_FUNCTION", "Delete list Function fail", null, null, "Error");
                    }
                    else
                    {
                        // Find final list Id (parent, grandparent,...) begin from (list A), this list can delete.
                        listDelFinal = listDelFinal.Concat(listDel).ToList();
                        foreach (var id in listDel)
                        {
                            var listAdd = FindParentInList(listId, id, new List<int>());
                            listDelFinal = listDelFinal.Concat(listAdd).ToList();
                        }
                        // Find list Id in selected list can't delete.
                        foreach (var id in listId)
                        {
                            if (!listDelFinal.Contains(id))
                            {
                                listRef.Add(id);
                            }
                        }
                        // case exist list can't delete.
                        if (listRef.Count > 0)
                        {
                            foreach (var idDel in listDelFinal)
                            {
                                AdFunction objDel = _context.AdFunctions.FirstOrDefault(x => x.FunctionId == idDel);
                                listFunction.Add(objDel);
                                _context.AdFunctions.Attach(objDel);
                                _context.AdFunctions.Remove(objDel);
                            }
                            _context.SaveChanges();
                            msg.Error = true;
                            msg.Title = String.Format(CommonUtil.ResourceValue("DEL_SUCCESS_LIST_ITEM_BUT_HAS_CHILD"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                            //_logger.LogError(LoggingEvents.LogDb, "Delete part of the Function list successfully");
                            _actionLog.InsertActionLogDeleteItem("VIB_FUNCTION", "Delete part of the Function list successfully", listFunction.ToArray(), null, "Delete");

                        }
                        // case full list parameter can delete.
                        else
                        {
                            foreach (var idDel in listDelFinal)
                            {
                                AdFunction objDel = _context.AdFunctions.FirstOrDefault(x => x.FunctionId == idDel);
                                listFunction.Add(objDel);
                                _context.AdFunctions.Attach(objDel);
                                _context.AdFunctions.Remove(objDel);
                            }
                            _context.SaveChanges();
                            msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_LIST_SUCCESS"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                            //_logger.LogError(LoggingEvents.LogDb, "Delete list Function successfully");
                            _actionLog.InsertActionLogDeleteItem("VIB_FUNCTION", "Delete list Function successfully", listFunction.ToArray(), null, "Delete");
                        }
                    }
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("ERR_LIST_OBJ_REF"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                    //_logger.LogError(LoggingEvents.LogDb, "Delete list Function fail");
                    _actionLog.InsertActionLogDeleteItem("VIB_FUNCTION", "Delete list Function fail", null, null, "Error");
                }

            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_LIST_FAIL"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC"));
                //_logger.LogError(LoggingEvents.LogDb, "Delete list Function fail");
                _actionLog.InsertActionLogDeleteItem("VIB_FUNCTION", "Delete list Function failed: " + ex.Message, null, null, "Error");
            }
            return Json(msg);
        }
        private List<int> FindParentInList(List<int> listId, int id, List<int> listParentId)
        {
            AdFunction obj = _context.AdFunctions.FirstOrDefault(x => x.FunctionId == id);
            if (obj != null)
            {
                var parentObj = _context.AdFunctions.FirstOrDefault(x => x.FunctionCode == obj.ParentCode);
                if (parentObj != null && listId.Contains(parentObj.FunctionId))
                {
                    listParentId.Add(parentObj.FunctionId);
                    FindParentInList(listId, parentObj.FunctionId, listParentId);
                }
            }
            return listParentId;
        }

        [HttpPost]
        public JsonResult GetItem([FromBody]int? id)
        {
            if (id == null || id < 0)
            {
                return Json("");
            }
            var a = _context.AdFunctions.AsNoTracking().First(m => m.FunctionId == id);
            return Json(a);
        }

        #region Resource Attribute
        //[HttpPost]
        //public JsonResult UpdateResAtribute([FromBody]List<ESResAttribute> model)
        //{
        //	//_logger.LogInformation(LoggingEvents.LogDb, "Update resource atribute");
        //	var msg = new JMessage() { Error = false };
        //	try
        //	{
        //		foreach (var item in model)
        //		{
        //			_context.ESResAttributes.Update(item);
        //			_context.Entry(item).State = EntityState.Modified;
        //			var a = _context.SaveChanges();
        //		}
        //		//msg.Title = "Sửa khoản mục thành công";
        //		msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("ATTRIBUTE"));
        //		//_logger.LogInformation(LoggingEvents.LogDb, "Update resource atribute successfully");
        //		_actionLog.InsertActionLog("ESResAttribute", "Update resource atribute successfully", null, null, "Update");

        //	}
        //	catch (Exception)
        //	{
        //		msg.Error = true;
        //		//msg.Title = "Có lỗi khi sửa khoản mục";
        //		msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_FAIL"), CommonUtil.ResourceValue("ATTRIBUTE"));
        //		//_logger.LogError(LoggingEvents.LogDb, "Update resource atribute fail");
        //		_actionLog.InsertActionLog("ESResAttribute", "Update resource atribute fail", null, null, "Error");

        //	}
        //	return Json(msg);
        //}
        //[HttpPost]
        //public object DeleteResAtribute([FromBody]List<ESResAttribute> id)
        //{
        //	//_logger.LogInformation(LoggingEvents.LogDb, "Delete resource atribute");
        //	try
        //	{
        //		List<ESResAttribute> listResAtribute = new List<ESResAttribute>();
        //		foreach (var item in id)
        //		{
        //			ESResAttribute obj = new ESResAttribute();
        //			listResAtribute.Add(obj);
        //			obj.Id = item.Id;
        //			_context.ESResAttributes.Attach(obj);
        //			_context.ESResAttributes.Remove(obj);
        //			_context.SaveChanges();
        //		}
        //		//_logger.LogInformation(LoggingEvents.LogDb, "Delete resource atribute successfully");
        //		_actionLog.InsertActionLogDeleteItem("ESResAttribute", "Delete resource atribute successfully", listResAtribute.ToArray(), null, "Delete");

        //		//return Json(new JMessage() { Error = false, Title = "Xóa khoản mục thành công." });
        //		return Json(new JMessage() { Error = false, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("ATTRIBUTE")) });
        //	}
        //	catch (Exception ex)
        //	{
        //		//_logger.LogError(LoggingEvents.LogDb, "Delete resource atribute fail");
        //		_actionLog.InsertActionLogDeleteItem("ESResAttribute", "An error occurred while Delete resource atribute", null, null, "Error");

        //		//return Json(new JMessage() { Error = true, Title = "Có lỗi khi xóa khoản mục.", Object = ex.Message });
        //		return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("ATTRIBUTE")), Object = ex.Message });
        //	}
        //}
        //[HttpPost]
        //public JsonResult GetResAtribute([FromBody]int? id)
        //{
        //	if (id == null || id < 0)
        //	{
        //		return Json("");
        //	}
        //	var a = _context.ESResAttributes.Where(m => m.ResourceId == id).ToList();
        //	return Json(a);
        //}
        //[HttpPost]
        //public object InsertResAtribute([FromBody]List<ESResAttribute> model)
        //{
        //	//_logger.LogInformation(LoggingEvents.LogDb, "Insert resource atribute");
        //	var msg = new JMessage() { Error = false };
        //	try
        //	{
        //		foreach (var item in model)
        //		{
        //			_context.ESResAttributes.Add(item);
        //			var a = _context.SaveChanges();
        //		}
        //		msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("ATTRIBUTE")); //"Áp dụng thành công.";
        //																													   //_logger.LogInformation(LoggingEvents.LogDb, "Insert resource atribute succesfully");
        //		_actionLog.InsertActionLog("ESResAttribute", "Insert resource atribute succesfully", model.ToArray(), null, "Insert");

        //	}
        //	catch (Exception ex)
        //	{
        //		msg.Object = ex;
        //		msg.Error = true;
        //		msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAIL"), CommonUtil.ResourceValue("ATTRIBUTE")); //"Có lỗi khi Áp dụng khoản mục";
        //																													//_logger.LogError(LoggingEvents.LogDb, "Insert resource atribute fail");
        //		_actionLog.InsertActionLog("ESResAttribute", "An error occurred while Insert resource atribute", null, null, "Error");

        //	}
        //	return Json(msg);
        //} 
        #endregion

        [HttpPost]
        public object Resort([FromBody]List<AdFunction> model)
        {

            var msg = new JMessage() { Error = false };
            try
            {
                foreach (var item in model)
                {
                    _context.AdFunctions.Attach(item);
                    _context.Entry(item).Property(x => x.Ord).IsModified = true;
                    _context.SaveChanges();
                }
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_SORT_SUCCESS"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC")); //"Sắp xếp các khoản mục thành công.";
            }
            catch (Exception)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_SORT_FAIL"), CommonUtil.ResourceValue("ADM_FUNC_TITLE_FUNC")); //"Có lỗi khi sắp xếp khoản mục";
            }
            return Json(msg);
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
                var data = _context.AdFunctions.OrderBy(x => x.Title).AsNoTracking();
                var dataOrder = GetSubTreeData(data.ToList(), null, new List<TreeView>(), 0);
                return dataOrder;
            }
            else
            {
                string functionCode = _context.AdFunctions.Where(x => x.FunctionId == obj.IdI[0]).Select(x => x.FunctionCode).FirstOrDefault();

                var data = _context.AdFunctions.OrderBy(x => x.Title).Where(x => (x.FunctionCode != functionCode && x.ParentCode != functionCode)).AsNoTracking();
                var dataOrder = GetSubTreeData(data.ToList(), null, new List<TreeView>(), 0);
                return dataOrder;
            }
        }

        private List<TreeView> GetSubTreeData(List<AdFunction> data, string parentCode, List<TreeView> lstCategories, int tab)
        {
            //tab += "- ";
            var contents = parentCode == null
                ? data.Where(x => x.ParentCode == null).OrderBy(x => x.Title).ToList()
                : data.Where(x => x.ParentCode == parentCode).OrderBy(x => x.Title).ToList();
            foreach (var item in contents)
            {
                var category = new TreeView
                {
                    Id = item.FunctionId,
                    Code = item.FunctionCode,
                    Title = item.Title,
                    Level = tab,
                    HasChild = data.Any(x => x.ParentCode == item.FunctionCode)
                };
                lstCategories.Add(category);
                if (category.HasChild) GetSubTreeData(data, item.FunctionCode, lstCategories, tab + 1);
            }
            return lstCategories;
        }
        [HttpPost]
        public object GetByParent([FromBody]TempSub obj)
        {
            if (obj.IdS[0] == null || obj.IdS[0] == "")
            {
                var temp = Convert.ToInt32(obj.IdI[0]);
                return Json(_context.AdFunctions.Where(x => (x.ParentCode == obj.IdS[0]) || (temp == 0 && x.ParentCode == null)).OrderBy(x => x.Ord).AsNoTracking().ToList());
            }
            else
            {
                var temp = Convert.ToInt32(obj.IdI[0]);
                return Json(_context.AdFunctions.Where(x => ((x.ParentCode == obj.IdS[0]) || (temp == 0 && x.ParentCode == null))).OrderBy(x => x.Ord).AsNoTracking().ToList());
            }
        }

        [HttpPost]
        public JsonResult GetResourceByFunctionId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json("");
            }
            var query = from a in _context.AdResources
                        join b in _context.AdPrivileges on a.ResourceCode equals b.ResourceCode
                        where b.FunctionCode == id
                        select new
                        {
                            Id = a.ResourceId,
                            Code = a.ResourceCode,
                            a.Description,
                            //a.GroupResourceId,
                            a.ParentCode,
                            a.Path,
                            a.Api,
                            a.Title,
                            a.Ord,
                            b.ResourceCode,
                        };
            var rs = query.AsNoTracking().ToList();
            return Json(rs);
        }
        [HttpPost]
        public List<TreeViewResource> GetTreeResourceData(int? id)
        {
            var data = _context.AdResources.Where(x => x.Status == true).OrderBy(x => x.Title).AsNoTracking();
            var dataOrder = GetSubTreeResourceData(data.ToList(), null, new List<TreeViewResource>(), "");
            //if (id == null || id < 0)
            //{
            //    return null;
            //}
            //var query = from a in _context.VIBFunctions
            //            where !_context.VIBPrivileges.Any(es => (es.ResourceId == a.Id ))
            //            select a;
            //List<VIBFunction> rs = query.ToList<VIBFunction>();
            //var dataOrder = GetSubTreeData(rs, 0, new List<TreeView>(), "");
            return dataOrder;




            //var query = from a in _context.VIBResources
            //            join b in _context.VIBPrivileges on a.Id equals b.ResourceId
            //            where b.FunctionId == id
            //            select new
            //            {
            //                b.ResourceId
            //            };
            //var data1 = query.AsNoTracking().ToList();
            //var data = _context.VIBResources.OrderBy(x => x.Ord).AsNoTracking().ToList();
            //var data2 = data;
            //foreach (var item in data1)
            //{
            //    for (var i = 0; i < data2.Count; i++)
            //    {
            //        if (item.ResourceId == data2[i].Id && data2[i].ParentId != null)
            //        {
            //            data.RemoveAt(i);
            //        }
            //    }
            //}
            //var dataOrder = GetSubTreeResourceData(data, 0, new List<TreeView>(), "");
            //return dataOrder;
        }
        private List<TreeViewResource> GetSubTreeResourceData(List<AdResource> data, string parentCode, List<TreeViewResource> lstCategories, string tab)
        {
            //tab += "- ";
            var contents = parentCode == null
                ? data.Where(x => x.ParentCode == null).OrderBy(x => x.Title).ToList()
                : data.Where(x => x.ParentCode == parentCode).OrderBy(x => x.Title).ToList();
            foreach (var item in contents)
            {
                var category = new TreeViewResource
                {
                    Id = item.ResourceId,
                    Code = item.ResourceCode,
                    Title = item.Title,
                    TitleJoin = tab + item.Title + " (Api: " + item.Api + ")",
                    ParentCode = item.ParentCode,
                    Path = item.Path,
                    Api = item.Api,
                    Description = item.Description,
                    Status = item.Status,
                    HasChild = data.Any(x => x.ParentCode == item.ResourceCode)
                };
                lstCategories.Add(category);
                if (category.HasChild) GetSubTreeResourceData(data, item.ResourceCode, lstCategories, tab + "- ");
            }
            return lstCategories;
        }

        //Insert parentResource first
        [HttpPost]
        public async Task<JsonResult> AddResource([FromBody]FuncResourceModel obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var func = _context.AdFunctions.Where(p => p.FunctionCode == obj.FunctionCode).AsNoTracking().SingleOrDefault();
                if (func != null)
                {
                    // Add resource
                    if (obj.ResourceAdd != null && obj.ResourceAdd.Count > 0)
                    {
                        foreach (var resCode in obj.ResourceAdd)
                        {
                            var resource = await _context.AdResources.FirstOrDefaultAsync(x => x.ResourceCode == resCode);
                            if (resource != null)
                            {
                                var privilege = await _context.AdPrivileges.FirstOrDefaultAsync(x => x.FunctionCode == func.FunctionCode && x.ResourceCode == resCode);
                                if (privilege == null)
                                {
                                    privilege = new AdPrivilege();
                                    privilege.FunctionCode = func.FunctionCode;
                                    privilege.ResourceCode = resource.ResourceCode;
                                    _context.Add(privilege);
                                }
                            }
                        }
                    }
                    // Remove function
                    if (obj.ResourceDel != null && obj.ResourceDel.Count > 0)
                    {
                        foreach (var resCode in obj.ResourceDel)
                        {
                            var resource = await _context.AdResources.FirstOrDefaultAsync(x => x.ResourceCode == resCode);
                            if (resource != null)
                            {
                                var privilege = await _context.AdPrivileges.FirstOrDefaultAsync(x => x.FunctionCode == func.FunctionCode && x.ResourceCode == resCode);
                                if (privilege != null)
                                {
                                    _context.Remove(privilege);
                                }
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                    msg.Title = "Cập nhập tài nguyên cho chức năng thành công";

                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Chức năng đã tồn tại!";
                    //_logger.LogError(LoggingEvents.LogDb, "Insert function fail");
                }
            }
            catch (Exception)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAIL"), CommonUtil.ResourceValue("RESOURCE")); //"Có lỗi khi thêm chức năng";
                //_logger.LogError(LoggingEvents.LogDb, "Insert Resource fail");
                //_actionLog.InsertActionLog("VIBPrivilege", "An error occurred while Insert Resource", null, null, "Error");

            }
            return Json(msg);
        }

        ////Insert parentResource first
        //[HttpPost]
        //public JsonResult AddResource([FromBody]VIBPrivilege obj)
        //{
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {
        //        //_logger.LogInformation(LoggingEvents.LogDb, "Insert Resource of Function");
        //        var func = _context.VIBFunctions.Where(p => p.FunctionCode == obj.FunctionCode).AsNoTracking().SingleOrDefault();
        //        var resour = _context.VIBResources.Where(p => p.ResourceCode == obj.ResourceCode).AsNoTracking().SingleOrDefault();
        //        if (func != null && resour != null)
        //        {
        //            var privilege = _context.VIBPrivileges
        //                .Where(p => p.FunctionCode == obj.FunctionCode && p.ResourceCode == obj.ResourceCode)
        //                .AsNoTracking().SingleOrDefault();
        //            if (privilege != null)
        //            {
        //                msg.Error = true;
        //                msg.Title = String.Format(CommonUtil.ResourceValue("ERR_EXIST"),
        //                    CommonUtil.ResourceValue("RESOURCE"));
        //                //_logger.LogError(LoggingEvents.LogDb, "Insert Resource fail");
        //                _actionLog.InsertActionLog("VIBPrivilege", "Insert Resource fail", null, null, "Error");

        //            }
        //            else
        //            {
        //                if (resour.ParentCode == null)
        //                {
        //                    var objAdd = new VIBPrivilege();
        //                    objAdd.ResourceCode = obj.ResourceCode;
        //                    objAdd.FunctionCode = obj.FunctionCode;

        //                    _context.VIBPrivileges.Add(objAdd);
        //                    _context.SaveChanges();
        //                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"),
        //                        CommonUtil.ResourceValue("RESOURCE"));
        //                    //_logger.LogInformation(LoggingEvents.LogDb, "Insert Resource successfully");
        //                    _actionLog.InsertActionLog("VIBPrivilege", "Insert Resource successfully", null, obj, "Insert");

        //                }
        //                else
        //                {
        //                    var privilegeParent = _context.VIBPrivileges
        //                        .Where(p => p.FunctionCode == obj.FunctionCode && p.ResourceCode == resour.ResourceCode)
        //                        .AsNoTracking().SingleOrDefault();
        //                    if (privilegeParent == null)
        //                    {
        //                        msg.Error = true;
        //                        msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_PARENT_FIRST"),
        //                            CommonUtil.ResourceValue("RESOURCE")); // "Bạn phải add cha trước";
        //                                                                   //_logger.LogError(LoggingEvents.LogDb, "Insert Resource fail");
        //                        _actionLog.InsertActionLog("VIBPrivilege", "Insert Resource fail", null, null, "Error");

        //                    }
        //                    else
        //                    {
        //                        var objAdd1 = new VIBPrivilege();
        //                        objAdd1.ResourceCode = obj.ResourceCode;
        //                        objAdd1.FunctionCode = obj.FunctionCode;

        //                        _context.VIBPrivileges.Add(objAdd1);
        //                        _context.SaveChanges();
        //                        msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"),
        //                            CommonUtil.ResourceValue("RESOURCE")); //"Thêm thành công";
        //                                                                   //_logger.LogInformation(LoggingEvents.LogDb, "Insert Resource successfully");
        //                        _actionLog.InsertActionLog("VIBPrivilege", "Insert Resource successfully", null, obj, "Insert");

        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            msg.Error = true;
        //            msg.Title = String.Format(CommonUtil.ResourceValue("ERR_NOT_CHECKED"),
        //                CommonUtil.ResourceValue("RESOURCE"));
        //            //_logger.LogError(LoggingEvents.LogDb, "Insert Resource fail");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        msg.Error = true;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAIL"), CommonUtil.ResourceValue("RESOURCE")); //"Có lỗi khi thêm chức năng";
        //        //_logger.LogError(LoggingEvents.LogDb, "Insert Resource fail");
        //        _actionLog.InsertActionLog("VIBPrivilege", "An error occurred while Insert Resource", null, null, "Error");

        //    }
        //    return Json(msg);
        //}
        //[HttpPost]
        //public JsonResult AddResource([FromBody]VIBPrivilege obj)
        //{
        //    //_logger.LogInformation(LoggingEvents.LogDb, "Add resource");
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {
        //        if (!checkExistPrivileges(obj.FunctionId, obj.ResourceId))
        //        {
        //            var dataResource = _context.VIBResources.Where(p => p.Id == obj.ResourceId).AsNoTracking().SingleOrDefault();
        //            if (dataResource.ParentId != null && !checkExistPrivileges(obj.FunctionId, (int)dataResource.ParentId))
        //            {
        //                VIBPrivilege vibPrivilege = new VIBPrivilege();
        //                vibPrivilege.ResourceId = (int)dataResource.ParentId;
        //                vibPrivilege.FunctionId = obj.FunctionId;
        //                _context.VIBPrivileges.Add(vibPrivilege);
        //            }
        //            _context.VIBPrivileges.Add(obj);
        //            var a = _context.SaveChanges();
        //            msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("RESOURCE")); //"Thêm tài nguyên thành công";
        //            //_logger.LogInformation(LoggingEvents.LogDb, "Add resource successfully");
        //        }
        //        else
        //        {
        //            msg.Error = true;
        //            msg.Title = String.Format(CommonUtil.ResourceValue("ERR_EXIST"), CommonUtil.ResourceValue("RESOURCE")); //"Tài nguyên đã tồn tại";
        //            //_logger.LogError(LoggingEvents.LogDb, "Add resource fail");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Object = ex;
        //        msg.Error = true;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAIL"), CommonUtil.ResourceValue("RESOURCE")); //"Chưa thêm được tài nguyên";
        //        //_logger.LogError(LoggingEvents.LogDb, "Add resource fail");
        //    }
        //    return Json(msg);
        //}
        //private Boolean checkExistPrivileges(int functionId, int resourceId)
        //{
        //    var Privileges = _context.VIBPrivileges.Where(p => p.FunctionId == functionId && p.ResourceId == resourceId).AsNoTracking().FirstOrDefault();
        //    return Privileges == null ? false : true;
        //}
        [HttpPost]
        public object DeletePrivilege([FromBody]AdPrivilege obj)
        {
            try
            {
                //_logger.LogInformation(LoggingEvents.LogDb, "Delete Resource of Function");
                var dataResource = _context.AdResources
                    .Where(p => p.ParentCode == obj.ResourceCode).AsNoTracking().ToList();
                if (dataResource.Count > 0)
                {
                    foreach (var items in dataResource)
                    {
                        var privilege = _context.AdPrivileges
                            .Where(p => p.ResourceCode == items.ResourceCode && p.FunctionCode == obj.FunctionCode)
                            .AsNoTracking().SingleOrDefault();
                        if (privilege != null)
                        {
                            //_logger.LogError(LoggingEvents.LogDb, "Delete Resource fail");
                            return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_CHILD"), CommonUtil.ResourceValue("RESOURCE")) });
                        }
                    }
                }
                var privilegeDel = _context.AdPrivileges
                    .Where(p => p.ResourceCode == obj.ResourceCode && p.FunctionCode == obj.FunctionCode)
                    .AsNoTracking().SingleOrDefault();
                _context.AdPrivileges.Attach(privilegeDel);
                _context.AdPrivileges.Remove(privilegeDel);

                _context.SaveChanges();
                //_logger.LogInformation(LoggingEvents.LogDb, "Delete Resource successfully");
                _actionLog.InsertActionLog("VIBPrivilege", "Delete Resource successfully", obj, null, "Delete");

                return Json(new JMessage() { Error = false, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("RESOURCE")) });
            }
            catch (Exception ex)
            {
                //_logger.LogError(LoggingEvents.LogDb, "Delete Resource fail");
                _actionLog.InsertActionLog("VIBPrivilege", "An error occurred while Delete Resource", null, null, "Error");

                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("RESOURCE")) });
            }
        }

        //[HttpPost]
        //public object DeletePrivilege([FromBody]VIBPrivilege obj)
        //{
        //    try
        //    {
        //        var dataResource = _context.VIBResources
        //                    .Where(p => p.ParentId == obj.ResourceId).AsNoTracking().ToList();
        //        if (dataResource.Count > 0)
        //        {
        //            foreach (var items in dataResource)
        //            {
        //                var dataPrivilege = _context.VIBPrivileges
        //                    .Where(p => p.ResourceId == items.Id && p.FunctionId == obj.FunctionId)
        //                    .AsNoTracking().SingleOrDefault();
        //                if (dataPrivilege != null)
        //                {
        //                    _context.VIBPrivileges.Attach(dataPrivilege);
        //                    _context.VIBPrivileges.Remove(dataPrivilege);
        //                }
        //            }
        //        }

        //        var obj1 = _context.VIBPrivileges.Where(p => p.FunctionId == obj.FunctionId && p.ResourceId == obj.ResourceId).AsNoTracking().SingleOrDefault();
        //        _context.VIBPrivileges.Attach(obj1);
        //        _context.VIBPrivileges.Remove(obj1);

        //        _context.SaveChanges();
        //        return Json(new JMessage() { Error = false, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("RESOURCE"))/*"Xóa khoản mục thành công."*/ });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("RESOURCE"))/*"Có lỗi khi xóa khoản mục." */});
        //    }
        //}


        public class FuncResourceModel
        {
            public string FunctionCode { get; set; }
            public List<string> ResourceAdd { get; set; }
            public List<string> ResourceDel { get; set; }
        }
    }
}