using System;
using System.Threading.Tasks;
using ESEIM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ESEIM.Utils;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace III.Admin.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected EIMDBContext DbContext
        {
            get { return HttpContext.RequestServices.GetService<EIMDBContext>(); }
        }
        protected IUserLoginService UserLoginService
        {
            get { return HttpContext.RequestServices.GetService<IUserLoginService>(); }
        }
        protected IParameterService ParameterService
        {
            get { return HttpContext.RequestServices.GetService<IParameterService>(); }
        }
        [NonAction]
        protected bool UpdatePermissionUserByGroup(EIMDBContext context, string groupCode, string userId, string roleId, string appCode, string newRoleId = null, string newGroupCode = null, string newAppCode = null)
        {
            IQueryable<AdPermission> listPermissionDefault;
            if (newAppCode == null)
            {
                if (newRoleId == null)
                {
                    if (newGroupCode == null || newGroupCode == groupCode)
                    {
                        listPermissionDefault = context.AdPermissions.Where(x => x.GroupUserCode == groupCode && x.UserId == null && x.RoleId == roleId && x.ApplicationCode == appCode);
                    }
                    else
                    {
                        // Remove old permission
                        var listPermissionUser = context.AdPermissions.Where(x => x.GroupUserCode == groupCode && x.UserId != null && x.UserId == userId && x.RoleId == roleId && x.ApplicationCode == appCode);
                        if (listPermissionUser.Any()) context.RemoveRange(listPermissionUser);

                        // Get new default permission
                        listPermissionDefault = context.AdPermissions.Where(x => x.GroupUserCode == newGroupCode && x.UserId == null && x.RoleId == roleId && x.ApplicationCode == appCode);
                    }
                }
                else
                {
                    // Remove old permission
                    var listPermissionUser = context.AdPermissions.Where(x => x.GroupUserCode == groupCode && x.UserId != null && x.UserId == userId && x.RoleId == roleId && x.ApplicationCode == appCode);
                    if (listPermissionUser.Any()) context.RemoveRange(listPermissionUser);

                    if (newGroupCode == null || newGroupCode == groupCode)
                    {
                        // Get new default permission
                        listPermissionDefault = context.AdPermissions.Where(x => x.GroupUserCode == groupCode && x.UserId == null && x.RoleId == newRoleId && x.ApplicationCode == appCode);
                    }
                    else
                    {
                        // Get new default permission
                        listPermissionDefault = context.AdPermissions.Where(x => x.GroupUserCode == newGroupCode && x.UserId == null && x.RoleId == newRoleId && x.ApplicationCode == appCode);
                    }
                }
            }
            else
            {
                if (newRoleId == null)
                {
                    if (newGroupCode == null || newGroupCode == groupCode)
                    {
                        listPermissionDefault = context.AdPermissions.Where(x => x.GroupUserCode == groupCode && x.UserId == null && x.RoleId == roleId && x.ApplicationCode == newAppCode);
                    }
                    else
                    {
                        // Remove old permission
                        var listPermissionUser = context.AdPermissions.Where(x => x.GroupUserCode == groupCode && x.UserId != null && x.UserId == userId && x.RoleId == roleId && x.ApplicationCode == appCode);
                        if (listPermissionUser.Any()) context.RemoveRange(listPermissionUser);

                        // Get new default permission
                        listPermissionDefault = context.AdPermissions.Where(x => x.GroupUserCode == newGroupCode && x.UserId == null && x.RoleId == roleId && x.ApplicationCode == newAppCode);
                    }
                }
                else
                {
                    // Remove old permission
                    var listPermissionUser = context.AdPermissions.Where(x => x.GroupUserCode == groupCode && x.UserId != null && x.UserId == userId && x.RoleId == roleId && x.ApplicationCode == appCode);
                    if (listPermissionUser.Any()) context.RemoveRange(listPermissionUser);

                    if (newGroupCode == null || newGroupCode == groupCode)
                    {
                        // Get new default permission
                        listPermissionDefault = context.AdPermissions.Where(x => x.GroupUserCode == groupCode && x.UserId == null && x.RoleId == newRoleId && x.ApplicationCode == newAppCode);
                    }
                    else
                    {
                        // Get new default permission
                        listPermissionDefault = context.AdPermissions.Where(x => x.GroupUserCode == newGroupCode && x.UserId == null && x.RoleId == newRoleId && x.ApplicationCode == newAppCode);
                    }
                }
            }

            // Insert new permission of user
            if (listPermissionDefault.Any())
            {
                foreach (var per in listPermissionDefault)
                {
                    // Add new permission
                    var permission = new AdPermission();
                    permission.ApplicationCode = per.ApplicationCode;
                    permission.FunctionCode = per.FunctionCode;
                    permission.ResourceCode = per.ResourceCode;
                    permission.GroupUserCode = per.GroupUserCode;
                    permission.RoleId = per.RoleId;
                    permission.UserId = userId;
                    context.AdPermissions.Add(permission);
                }
            }

            return true;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool isAjaxCall = "XMLHttpRequest" == Request.Headers["x-requested-with"];
            if (User.Identity.IsAuthenticated)
            {
                bool isExpired = false;
                var userId = ESEIM.AppContext.UserId;
                var session = HttpContext.GetSessionUser();
                if (session?.UserId == null)
                {
                    session = UserLoginService.GetSessionUser(userId);
                    if (session != null)
                    {
                        HttpContext.SetSessionUser(session);
                    }
                    else
                    {
                        HttpContext.Session.Clear();
                        context.Result = new RedirectResult(Url.Action("Logout", "Admin/Account"));
                    }
                }
                else
                {
                    isExpired = session.ExpireTimeSpan < DateTime.Now;
                }

                // Get controller - action
                var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
                var controllerName = descriptor.ControllerName;
                var actionName = descriptor.ActionName;

                // Check Expired session
                if (isExpired)
                {
                    if (!controllerName.Equals("Account") || !actionName.Equals("Logout"))
                    {
                        //if (isAjaxCall)
                        //{
                        //HttpContext.Session.Clear();
                        //Task.Run(() => HttpContext.Authentication.SignOutAsync("Cookies"));
                        //Task.Run(() => HttpContext.Authentication.SignOutAsync("oidc"));
                        //}
                        //else
                        //{
                        HttpContext.Session.Clear();
                        context.Result = new RedirectResult(Url.Action("Logout", "Admin/Account"));
                        //}
                        //context.Result = new RedirectResult(Url.Action(actionName, controllerName));
                    }
                }
                else
                {
                    // Set session timeout
                    var timeOut = ParameterService.GetSessionTimeout();
                    session.ExpireTimeSpan = DateTime.Now.AddMinutes(timeOut);
                    HttpContext.SetSessionUser(session);

                    // Check lock day
                    //if (session?.UserType != 10 && ParameterService.IsLockDay())
                    //{
                    //    if (controllerName != "Home" || actionName != "SystemLocked")
                    //    {
                    //        if (isAjaxCall)
                    //        {
                    //            context.Result = new JsonResult(new { Error = true, Title = "System Locked! You do not have access to this function.", data = "", draw = 1, recordsFiltered = 0, recordsTotal = 0 });
                    //        }
                    //        else
                    //        {
                    //            context.Result = new RedirectResult(Url.Action("SystemLocked", "Home"));
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    // Check permission
                    //    if (session?.UserType != 10 && !session.HasPermission(controllerName, actionName))
                    //    {
                    //        if (isAjaxCall)
                    //        {
                    //            context.Result = new JsonResult(new { Error = true, Title = "Access Denied! You do not have access to this function.", data = "", draw = 1, recordsFiltered = 0, recordsTotal = 0 });
                    //        }
                    //        else
                    //        {
                    //            context.Result = new RedirectResult(Url.Action("Permission", "Home"));
                    //        }
                    //    }
                    //}
                }
            }
            else
            {
                //context.Result = new RedirectResult(Url.Action("Logout", "Home"));
                if (isAjaxCall)
                {
                    HttpContext.Session.Clear();
                    //Task.Run(() => HttpContext.Authentication.SignOutAsync("Cookies"));
                    //Task.Run(() => HttpContext.Authentication.SignOutAsync("oidc"));
                    context.Result = new RedirectResult(Url.Action("Logout", "Admin/Account"));
                }
            }

            base.OnActionExecuting(context);
        }

        [NonAction]
        protected object GetCurrencyBase()
        {
            var data = DbContext.CommonSettings.Where(x => x.Group == "CURRENCY_TYPE").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return data;
        }
    }
    public class MapOfRole
    {
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public List<MapOfApplication> MapOfApplication { get; set; }
    }

    public class MapOfApplication
    {
        public string ApplicationCode { get; set; }
        public string ApplicationName { get; set; }
        public List<MapOfGroupRole> MapOfGroupRole { get; set; }
    }

    public class MapOfGroupRole
    {
        public string GroupUserCode { get; set; }
        public string GroupUserName { get; set; }

        public string RoleCode { get; set; }
        public string RoleName { get; set; }

        public bool IsMain { get; set; }

        public List<MapOfFunction> MapOfFunction { get; set; }
    }

    public class MapOfFunction
    {
        public string FunctionCode { get; set; }
        public string FunctionName { get; set; }
        public List<MapOfResource> MapOfResource { get; set; }
    }

    public class MapOfResource
    {
        public string ResourceCode { get; set; }
        public string ResourceName { get; set; }
        public string Api { get; set; }
        public bool Scope { get; set; }
        public bool Status { get; set; }
    }
}