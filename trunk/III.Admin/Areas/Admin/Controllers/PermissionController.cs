﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class PermissionController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly IActionLogService _actionLog;

        public PermissionController(EIMDBContext context, IActionLogService actionLog)
        {
            _context = context;
            _actionLog = actionLog;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetResource([FromBody]ObjGetResourceModel obj)
        {
            List<AdResourcePermission> result = new List<AdResourcePermission>();

            try
            {
                var listPermissionDefault = await _context.AdPermissions.Include(i => i.Function).Where(x => x.ApplicationCode == obj.AppCode && x.UserId == null && x.RoleId == obj.RoleId && obj.ListGUserId.Any(y => y == x.GroupUserCode)).ToListAsync();
                var listPrivileges = await _context.AdPrivileges.Include(x => x.Function).Include(x => x.Resource).Where(x => x.Resource.Status && obj.ListFuncId.Any(y => y == x.FunctionCode)).ToListAsync();
                if (listPrivileges.Count > 0)
                {
                    var groupFunction = listPrivileges.GroupBy(g => g.Function).OrderBy(o => o.Key.ParentCode).ThenBy(t => t.Key.FunctionCode).ToList();
                    if (groupFunction.Count > 0)
                    {
                        foreach (var groupfunc in groupFunction)
                        {
                            var function = groupfunc.Key;
                            // Get all resource of function
                            var listPrivilegeOfFunction = listPrivileges.Where(x => x.FunctionCode == function.FunctionCode).ToList();
                            if (listPrivilegeOfFunction.Count > 0)
                            {
                                var defaultFunction = new AdResourcePermission();
                                defaultFunction.Id = function.FunctionId;
                                defaultFunction.Code = function.FunctionCode;
                                defaultFunction.Title = function.Title;
                                defaultFunction.Description = function.Description;
                                defaultFunction.Ord = function.Ord;
                                defaultFunction.ParentCode = function.ParentCode;
                                defaultFunction.FunctionCode = function.FunctionCode;
                                defaultFunction.FunctionName = function.Title;
                                defaultFunction.HasPermission = true;
                                defaultFunction.IsFunction = true;
                                result.Add(defaultFunction); // Add first function

                                var query = from pr in listPrivilegeOfFunction
                                            join gfr in groupfunc on pr.ResourceCode equals gfr.ResourceCode into grpFunc
                                            from fr in grpFunc.DefaultIfEmpty()
                                            select new AdResourcePermission
                                            {
                                                Id = pr.PrivilegeId,
                                                Code = pr.Resource.ResourceCode,
                                                Title = pr.Resource.Title,
                                                Description = pr.Resource.Description,
                                                Api = pr.Resource.Api,
                                                Path = pr.Resource.Path,
                                                Ord = pr.Resource.Ord,
                                                Style = pr.Resource.Style,
                                                Scope = pr.Resource.Scope,
                                                ParentCode = pr.Resource.ParentCode,
                                                FunctionCode = pr.FunctionCode,
                                                FunctionName = pr.Function.Title,
                                                IsFunction = false,
                                                HasPermission = !obj.IsMultiple && listPermissionDefault.Any(x => x.FunctionCode == pr.FunctionCode && x.ResourceCode == pr.ResourceCode)
                                            };
                                result.AddRange(query);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                JMessage objex = new JMessage() { Error = true, Object = ex };
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> UpdatePermission([FromBody]PermissionModel model)
        {
            JMessage msg = new JMessage { Error = true, Title = string.Format(CommonUtil.ResourceValue("MSG_UPDATE_FAIL"), CommonUtil.ResourceValue("PERMISSION").ToLower()) };
            try
            {
                model.Resources = model.Resources.Where(x => !x.IsFunction).ToList();
                if (model.GroupCodes.Count > 0)
                {
                    var listFunctionChild = await _context.AdFunctions.Where(x => x.FunctionCode == model.FunctionCode || x.ParentCode == model.FunctionCode || x.Parent.ParentCode == model.FunctionCode).ToListAsync();
                    var listGroupUser = await _context.AdGroupUsers.Where(x => model.GroupCodes.Any(y => y == x.GroupUserCode)).ToListAsync();
                    var listUserInGroup = await _context.AdUserInGroups.Where(x => model.GroupCodes.Any(y => y == x.GroupUserCode) && x.RoleId == model.RoleId).ToListAsync();
                    var listPermissionAll = await _context.AdPermissions.Where(x => x.ApplicationCode == model.ApplicationCode && x.RoleId == model.RoleId && listGroupUser.Any(y => y.GroupUserCode == x.GroupUserCode) && (string.IsNullOrEmpty(model.FunctionCode) || listFunctionChild.Any(y => y.FunctionCode == x.FunctionCode))).ToListAsync();
                    var listPermissionDefault = listPermissionAll.Where(x => x.UserId == null).ToList();
                    var listPermissionUser = listPermissionAll.Where(x => x.UserId != null).ToList();
                    if (listGroupUser.Count > 0)
                    {
                        foreach (var groupUser in listGroupUser)
                        {
                            if (!model.IsMultiple)
                            {
                                // Remove permission default
                                var delPermissionDefault = listPermissionDefault.Where(x => x.GroupUserCode == groupUser.GroupUserCode && !model.Resources.Any(y => y.HasPermission && !y.IsFunction && y.FunctionCode == x.FunctionCode && y.Code == x.ResourceCode));
                                _context.RemoveRange(delPermissionDefault);

                                // Remove permission user
                                var delPermissionUser = listPermissionUser.Where(x => x.GroupUserCode == groupUser.GroupUserCode && !model.Resources.Any(y => y.HasPermission && !y.IsFunction && y.FunctionCode == x.FunctionCode && y.Code == x.ResourceCode));
                                _context.RemoveRange(delPermissionUser);
                            }

                            // Add permission default
                            var addPermissionDefault = model.Resources.Where(x => x.HasPermission && !x.IsFunction && !listPermissionDefault.Any(y => y.GroupUserCode == groupUser.GroupUserCode && y.FunctionCode == x.FunctionCode && y.ResourceCode == x.Code))
                                .Select(x => new AdPermission
                                {
                                    ApplicationCode = model.ApplicationCode,
                                    FunctionCode = x.FunctionCode,
                                    ResourceCode = x.Code,
                                    GroupUserCode = groupUser.GroupUserCode,
                                    RoleId = model.RoleId,
                                    UserId = null,
                                });
                            _context.AddRange(addPermissionDefault);

                            // Add permission user
                            var listUser = listUserInGroup.Where(x => x.GroupUserCode == groupUser.GroupUserCode).ToList();
                            //var permissionUser = listPermissionUser.Where(x => x.GroupUserCode == groupUser.GroupUserCode).GroupBy(g => g.UserId).ToList();
                            if (listUser.Count > 0)
                            {
                                foreach (var perUser in listUser)
                                {
                                    var addPermissionUser = model.Resources.Where(x => x.HasPermission && !x.IsFunction && x.Scope == false && !listPermissionUser.Any(y => y.GroupUserCode == groupUser.GroupUserCode && y.FunctionCode == x.FunctionCode && y.ResourceCode == x.Code))
                                        .Select(x => new AdPermission
                                        {
                                            ApplicationCode = model.ApplicationCode,
                                            FunctionCode = x.FunctionCode,
                                            ResourceCode = x.Code,
                                            GroupUserCode = groupUser.GroupUserCode,
                                            RoleId = model.RoleId,
                                            UserId = perUser.UserId,
                                        });
                                    _context.AddRange(addPermissionUser);
                                }
                            }
                        }
                    }

                    var result = await _context.SaveChangesAsync();
                }
                _actionLog.InsertActionLog("VIB_PERMISSION", "Update define permission for deparment/profit center success", null, null, "Update");

                msg.Error = false;
                msg.Title = string.Format(CommonUtil.ResourceValue("MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("PERMISSION").ToLower());
            }
            catch (Exception ex)
            {
                _actionLog.InsertActionLog("VIB_PERMISSION", "Update define permission failed: " + ex.Message, null, null, "Error");
                msg.Object = ex;
            }

            return Json(msg);
        }

    }

}