using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Parsing.Structure;
using Microsoft.Extensions.Logging;
using ESEIM;

namespace III.Admin.Controllers
{
    public class JTableModelCustom : JTableModel
    {
        public string[] GroupUser { set; get; }
        public string Role { set; get; }
        public string UserName { set; get; }
        public string GivenName { set; get; }
        public string Email { set; get; }
        public string EmployeeCode { set; get; }
        public bool? Status { set; get; }
        public string DepartmentId { set; get; }
        public string BranchId { set; get; }
        public string RoleId { set; get; }
        public int Page { set; get; }
        public int Row { set; get; }
        public int ExportType { set; get; }
    }
    public class ChangeStatusUserModel : JTableModel
    {
        public List<string> ListId { set; get; }
        public string Reason { set; get; }
    }
    [Area("Admin")]
    public class UserController : BaseController
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly UserManager<AspNetUser> _roleManager;
        private readonly EIMDBContext _context;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IActionLogService _actionLog;

        public UserController(IOptions<AppSettings> appSettings, EIMDBContext context, UserManager<AspNetUser> userManager, UserManager<AspNetUser> roleManager, ILogger<UserController> logger, IHostingEnvironment hostingEnvironment, IActionLogService actionLog)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _actionLog = actionLog;

        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("[controller]/[action]/{view}")]
        //public IActionResult GetView(string view)
        //{
        //    return PartialView("_" + view);
        //}

        [HttpPost]
        public object GetGroupResource()
        {
            var rs = _context.AdGroupUsers.OrderBy(x => x.GroupUserId).Select(x => new { Id = x.GroupUserId, Title = x.Title, Code = x.GroupUserCode }).AsNoTracking().ToList();
            return Json(rs);
        }

        [HttpPost]
        public object GetRole()
        {
            var rs = _context.Roles.Where(x => x.Code != "ROOT" && x.Status).OrderBy(x => x.Ord).Select(x => new { x.Id, x.Title, x.Name }).AsNoTracking().ToList();
            return Json(rs);
        }

        [HttpPost]
        public object GetOrganization()
        {
            var rs = GetTreeData(0);
            return Json(rs);
        }

        [HttpPost]
        public object GetDepartment()
        {
            //var DepartId = _context.VIBOrganizations.Where(x => x.Code == "Department").Select(x => new { x.Id }).AsNoTracking().SingleOrDefault();
            //var rs = GetTreeData(1);
            var query = _context.AdGroupUsers
                .Where(x => x.IsEnabled)
                .OrderBy(o => o.GroupUserCode)
                .Select(x => new
                {
                    Code = x.GroupUserCode,
                    Name = x.Title,
                }).AsNoTracking().ToList();

            return Json(query);
        }

        [HttpPost]
        public object GetBranch()
        {
            //var BranId = _context.VIBOrganizations.Where(x => x.Code == "Branch").Select(x => new { x.Id }).AsNoTracking().SingleOrDefault();
            var rs = GetTreeData(2);
            return Json(rs);
        }

        [HttpPost]
        public object GetProfitCenter()
        {
            //var ProfitCenterId = _context.VIBOrganizations.Where(x => x.Code == "PC").Select(x => new { x.Id }).AsNoTracking().SingleOrDefault();
            //var rs = GetTreeData(3);
            //return Json(rs);

            var query = _context.AdGroupUsers
                .Where(x => x.ParentCode == "P000" && x.IsEnabled)
                .OrderBy(o => o.GroupUserCode)
                .Select(x => new
                {
                    Code = x.GroupUserCode,
                    Name = x.Title,
                }).ToList();

            return Json(query);
        }

        //[HttpPost]
        //public async Task<object> GetAccountExecutive([FromBody] AccountSearch search)
        //{
        //    //var AccountExecutiveId = _context.VIBOrganizations.Where(x => x.Code == "AE").Select(x => new { x.Id }).AsNoTracking().SingleOrDefault();
        //    //var rs = GetTreeData(4);
        //    var listAe = await _context.AdFmAcctExecs
        //                            .Where(x => string.IsNullOrEmpty(search.Name) || x.AcctExec.ToUpper().Contains(search.Name.ToUpper()) || x.AcctExecName.ToUpper().Contains(search.Name.ToUpper()))
        //                            .Skip((search.Page - 1) * search.Row).Take(search.Row)
        //                            .AsNoTracking().ToListAsync();

        //    return Json(listAe);
        //}

        [HttpGet]
        public object GetListGroupRole(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var query = from a in _context.AdUserInGroups
                            join b in _context.AdGroupUsers on a.GroupUserCode equals b.GroupUserCode
                            join c in _context.Roles on a.RoleId equals c.Id
                            where (a.UserId == id && a.IsMain == false)
                            select new
                            {
                                a.GroupUserCode,
                                GroupUser = b.Title,
                                Role = c.Title,
                                AppCode = a.ApplicationCode,
                                AppName = a.Application.Title,
                            };
                var rs = query.AsNoTracking().ToList();
                return Json(rs);
            }
            return null;
        }

        [NonAction]
        private IActionResult JTable(JTableModelCustom jTablePara, int userType = 0)
        {
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
                    return RedirectToAction("Logout", "Admin/Account");
                }
            }
            // Get list department and profit center of user login
            var listUserInGroup = new List<AdUserInGroup>();
            if (userType == 1)
            {
                listUserInGroup = _context.AdUserInGroups.Where(x => x.UserId == userId).ToList();
            }
            // Get list branch reference
            var listBranch = new List<string>();
            if (userType == 2)
            {
                var userInGroup = _context.AdUserInGroups.FirstOrDefault(x => x.UserId == userId && x.ApplicationCode == _appSettings.ClientId);
                if (!string.IsNullOrEmpty(userInGroup?.BranchReference))
                {
                    listBranch = userInGroup.BranchReference.Split(',').ToList();
                }
            }
            // Get user data
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.Users
                            //join gb in _context.VIBOrganizations on a.DepartmentId equals gb.OrgAddonCode into grpD
                            //from b in grpD.DefaultIfEmpty()
                        join gc in _context.AdOrganizations on a.BranchId equals gc.OrgAddonCode into grpB
                        from c in grpB.DefaultIfEmpty()
                            //join gd in _context.VIBOrganizations on a.ProfitCenterId equals gd.OrgAddonCode into grpPC
                            //from d in grpPC.DefaultIfEmpty()
                        join ge in _context.AdOrganizations on a.AccountExecutiveId equals ge.OrgAddonCode into grpAC
                        from e in grpAC.DefaultIfEmpty()
                            //let dept = a.VIBUserInGroups.Where(x => x.GroupUser.ParentCode == "D000" && x.UserId == a.Id).Select(x => x.GroupUser.Title)
                        where a.UserType != 10
                            && (string.IsNullOrEmpty(jTablePara.UserName) || (a.UserName != null && a.UserName.ToLower().Contains(jTablePara.UserName.ToLower())))
                            && (string.IsNullOrEmpty(jTablePara.GivenName) || (a.GivenName != null && a.GivenName.ToLower().Contains(jTablePara.GivenName.ToLower())))
                            && (string.IsNullOrEmpty(jTablePara.Email) || (a.Email != null && a.Email.ToLower().Contains(jTablePara.Email.ToLower())))
                            && (string.IsNullOrEmpty(jTablePara.EmployeeCode) || (a.EmployeeCode != null && a.EmployeeCode.ToLower().Contains(jTablePara.EmployeeCode.ToLower())))
                            && (jTablePara.Status == null || a.Active == jTablePara.Status)
                            && (jTablePara.BranchId == null || a.BranchId == jTablePara.BranchId)
                            && (jTablePara.DepartmentId == null || a.AdUserInGroups.Any(y => y.UserId == a.Id && y.IsMain && y.GroupUserCode == jTablePara.DepartmentId))
                            && (jTablePara.RoleId == null || a.AdUserInGroups.Any(y => y.UserId == a.Id && y.IsMain && y.RoleId == jTablePara.RoleId))
                            && (userType == 10 || session.UserType == 10 ||
                               (userType == 2 && (a.CreatedBy == session.UserName || a.BranchId == session.BranchId || listBranch.Any(y => y == a.BranchId))) ||
                               (userType == 1 && (a.CreatedBy == session.UserName || a.AdUserInGroups.Any(y => y.UserId == a.Id && y.IsMain && listUserInGroup.Any(z => z.GroupUserCode == y.GroupUserCode && z.RoleId == y.RoleId)))) ||
                               (userType == 0 && a.CreatedBy == session.UserName)
                            )
                        //orderby a.CreatedDate descending
                        select new
                        {
                            a.Id,
                            a.UserName,
                            a.Email,
                            a.FamilyName,
                            a.GivenName,
                            a.MiddleName,
                            a.EmployeeCode,
                            a.CreatedDate,
                            //a.DepartmentId,
                            //a.BranchId,
                            //a.ProfitCenterId,
                            //a.AccountExecutiveId,
                            a.Description,
                            a.Active,
                            a.UserType,
                            a.Reason,
                            a.Picture,
                            FullName = string.Join(" ", a.GivenName, a.FamilyName, a.MiddleName),
                            Role = "",
                            Application = "",
                            Department = "",//b != null ? b.OrgName : "",
                            Branch = c != null ? string.Join(" - ", c.OrgCode, c.OrgName) : "",
                            ProfitCenter = "",//d != null ? d.OrgName : "",
                            AccountExecutive = a.AccountExecutiveId,//e != null ? e.OrgName : "",
                        };
            var data = query.AsNoTracking().Select(x => new
            {
                x.Id,
                x.UserName,
                x.Email,
                x.FamilyName,
                x.GivenName,
                x.MiddleName,
                x.EmployeeCode,
                x.CreatedDate,
                x.Description,
                x.Active,
                x.UserType,
                x.Reason,
                x.FullName,
                x.Role,
                x.Application,
                x.Department,
                x.Branch,
                //x.ProfitCenter,
                x.AccountExecutive,
                x.Picture
            });
            var data1 = data.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking();

            var count = data1.Count();
            var data2 = data1.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            if (data2.Count > 0)
            {
                var listDepartment = _context.AdUserInGroups.Where(x => data2.Any(y => y.Id == x.UserId && x.IsMain));
                data2 = data2.Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.Email,
                    x.FamilyName,
                    x.GivenName,
                    x.MiddleName,
                    x.EmployeeCode,
                    x.CreatedDate,
                    x.Description,
                    x.Active,
                    x.UserType,
                    x.Reason,
                    x.FullName,
                    Role = listDepartment.Where(y => y.UserId == x.Id).Select(y => y.Role.Title).FirstOrDefault(),
                    Application = listDepartment.Where(y => y.UserId == x.Id).Select(y => y.Application.Title).FirstOrDefault(),
                    Department = listDepartment.Where(y => y.UserId == x.Id).Select(y => y.GroupUser.Title).FirstOrDefault(),
                    x.Branch,
                    //ProfitCenter = listDepartment.Where(y => y.GroupUser.ParentCode == "P000" && y.UserId == x.Id).Select(y => y.GroupUser.Title).FirstOrDefault(),
                    x.AccountExecutive,
                    x.Picture
                }).ToList();
            }
            var jdata = JTableHelper.JObjectTable(data2, jTablePara.Draw, count, "Id", "CreatedDate", "UserName", "Email", "FamilyName", "FullName", "GivenName", "MiddleName", "EmployeeCode", "Department", "Branch", "ProfitCenter", "AccountExecutive", "Description", "Active", "Reason", "UserType", "Role", "Application", "Picture");
            return Json(jdata);
        }

        [HttpPost]
        public object JTableOfUser([FromBody] JTableModelCustom jTablePara)
        {
            return JTable(jTablePara, 0);
        }

        [HttpPost]
        public object JTableOfDepartment([FromBody] JTableModelCustom jTablePara)
        {
            return JTable(jTablePara, 1);
        }

        [HttpPost]
        public object JTableOfBranch([FromBody] JTableModelCustom jTablePara)
        {
            return JTable(jTablePara, 2);
        }

        [HttpPost]
        public object JTableOfAdmin([FromBody] JTableModelCustom jTablePara)
        {
            return JTable(jTablePara, 10);
        }
        [HttpPost]
        public async Task<IActionResult> Insert(AspNetUserCustom obj, IFormFile image)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var us = await _context.Users.FirstOrDefaultAsync(x => x.UserName == obj.UserName);
                if (us == null)
                {
                    if (image != null && image.Length > 0)
                    {
                        var url = string.Empty;
                        var pathUpload = Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\images");
                        if (!Directory.Exists(pathUpload)) Directory.CreateDirectory(pathUpload);
                        var fileName = Path.GetFileName(image.FileName);
                        fileName = Path.GetFileNameWithoutExtension(fileName)
                         + "_"
                         + Guid.NewGuid().ToString().Substring(0, 8)
                         + Path.GetExtension(fileName);
                        var filePath = Path.Combine(pathUpload, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        url = "/uploads/images/" + fileName;
                        obj.Picture = url;
                    }
                    else
                    {
                        obj.Picture = "/images/default/no_user.png";
                    }
                    AspNetUser objtemp = new AspNetUser()
                    {
                        UserName = obj.UserName.ToLower(),
                        Email = obj.Email,
                        PhoneNumber = obj.PhoneNumber,
                        OfficeNumber = obj.OfficeNumber,
                        FamilyName = obj.FamilyName,
                        MiddleName = obj.MiddleName,
                        GivenName = obj.GivenName,
                        EmployeeCode = obj.EmployeeCode,
                        //DepartmentId = obj.DepartmentId,
                        BranchId = obj.BranchId,
                        //ProfitCenterId = obj.ProfitCenterId,
                        AccountExecutiveId = obj.AccountExecutiveId,
                        Active = obj.Active,
                        Note = obj.Note,
                        UserType = obj.UserType,
                        Description = obj.Description,
                        Reason = obj.Reason,
                        CreatedDate = DateTime.Now,
                        CreatedBy =ESEIM.AppContext.UserName,
                        NormalizedEmail = obj.Email.ToUpper(),
                        NormalizedUserName = obj.UserName.ToUpper(),
                        LockoutEnabled = true,
                        Picture = obj.Picture,
                        SecurityStamp = Guid.NewGuid().ToString(),
                    };
                    await _userManager.CreateAsync(objtemp, obj.Password);
                    //_context.Users.Add(objtemp);
                    //await _context.SaveChangesAsync();

                    // Add or update main role
                    if (!string.IsNullOrEmpty(obj.RoleId))
                    {
                        var userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == objtemp.Id);
                        if (userRole != null)
                        {
                            if (userRole.RoleId != obj.RoleId)
                            {
                                // Remove old role
                                _context.Remove(userRole);
                                // Add new role
                                var newUserRole = new IdentityUserRole<string>();
                                newUserRole.UserId = objtemp.Id;
                                newUserRole.RoleId = obj.RoleId;
                                _context.Add(newUserRole);
                            }
                        }
                        else
                        {
                            userRole = new IdentityUserRole<string>();
                            userRole.UserId = objtemp.Id;
                            userRole.RoleId = obj.RoleId;
                            _context.Add(userRole);
                        }

                        //if (string.IsNullOrEmpty(obj.ApplicationCode)) obj.ApplicationCode = "ADMIN";

                        // Add or update main department
                        var userInGroup = await _context.AdUserInGroups.FirstOrDefaultAsync(x => x.UserId == objtemp.Id && x.GroupUserCode == obj.DepartmentId && x.IsMain);
                        if (userInGroup == null && !string.IsNullOrEmpty(obj.DepartmentId))
                        {
                            userInGroup = new AdUserInGroup();
                            userInGroup.GroupUserCode = obj.DepartmentId;
                            userInGroup.ApplicationCode = obj.ApplicationCode;
                            userInGroup.UserId = objtemp.Id;
                            userInGroup.RoleId = obj.RoleId;
                            userInGroup.GrantAll = true;
                            userInGroup.IsMain = true;
                            _context.Add(userInGroup); // Add entity
                                                       // Add/Update user permission for department
                            UpdatePermissionUserByGroup(_context, userInGroup.GroupUserCode, userInGroup.UserId, userInGroup.RoleId, obj.ApplicationCode, null, null, null);
                        }
                        // Add or update main profit center
                        var profitCenter = await _context.AdUserInGroups.FirstOrDefaultAsync(x => x.UserId == objtemp.Id && x.GroupUserCode == obj.ProfitCenterId && x.IsMain);
                        if (profitCenter == null && !string.IsNullOrEmpty(obj.ProfitCenterId))
                        {
                            profitCenter = new AdUserInGroup();
                            profitCenter.GroupUserCode = obj.ProfitCenterId;
                            profitCenter.ApplicationCode = obj.ApplicationCode;
                            profitCenter.UserId = objtemp.Id;
                            profitCenter.RoleId = obj.RoleId;
                            profitCenter.GrantAll = true;
                            profitCenter.IsMain = true;
                            _context.Add(profitCenter); // Add entity
                                                        // Add/Update user permission for profit center
                            UpdatePermissionUserByGroup(_context, profitCenter.GroupUserCode, profitCenter.UserId, profitCenter.RoleId, obj.ApplicationCode, null, null, null);
                        }
                    }

                    var result = await _context.SaveChangesAsync();

                    //// Update permission
                    //UpdatePermissionUser(obj.UserType, objtemp.Id, obj.RoleId, obj.DepartmentId);

                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("ADM_USER_LBL_USER").ToLower());
                    ////_logger.LogInformation(LoggingEvents.LogDb, "Insert user successfully");

                }
                else
                {
                    msg.Error = true;
                    //msg.Title = "Nhân Viên Đã Tồn Tại";
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_EXITS"), CommonUtil.ResourceValue("ADM_USER_LBL_USER"));
                    //_logger.LogError(LoggingEvents.LogDb, "Insert User Fail");
                    //_actionLog.InsertActionLog("AspNetUsers", "Insert User Fail", null, null, "Error");
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                //msg.Title = "Nhân Viên Thêm Thất Bại";
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAILED"), CommonUtil.ResourceValue("ADM_USER_LBL_USER"));
                //_logger.LogError(LoggingEvents.LogDb, "Insert User Fail");
                //_actionLog.InsertActionLog("AspNetUsers", "An error occurred while Insert user", null, null, "Error");

            }
            return Json(msg);
        }
        //[HttpPost]
        //public async Task<JsonResult> Insert([FromBody]AspNetUserCustom obj)
        //{
        //    //_logger.LogInformation(LoggingEvents.LogDb, "Insert user");

        //    var msg = new JMessage() { Error = false };

        //    try
        //    {
        //        AspNetUser objtemp = new AspNetUser()
        //        {
        //            UserName = obj.UserName,
        //            Email = obj.Email,
        //            PhoneNumber = obj.PhoneNumber,
        //            OfficeNumber = obj.OfficeNumber,
        //            FamilyName = obj.FamilyName,
        //            GivenName = obj.GivenName,
        //            EmployeeCode = obj.EmployeeCode,
        //            DepartmentId = obj.DepartmentId,
        //            BranchId = obj.BranchId,
        //            ProfitCenterId = obj.ProfitCenterId,
        //            AccountExecutiveId = obj.AccountExecutiveId,
        //            Active = obj.Active,
        //            Note = obj.Note,
        //            Description = obj.Description,
        //            Reason = obj.Reason,
        //            CreatedDate = DateTime.Now,
        //            CreatedBy = EIM.AppContext.UserName
        //        };
        //        //temp.CreatedBy = User.Identity.Name;
        //        obj.Password = "passwordHere";
        //        var x = await _userManager.CreateAsync(objtemp, obj.Password);

        //        for (int i = 0; i < obj.TempSub.IdS.Count(); i++)
        //        {
        //            await _roleManager.AddToRoleAsync(objtemp, obj.TempSub.IdS[i]);
        //        }
        //        for (int i = 0; i < obj.TempSub.IdI.Length; i++)
        //        {
        //            AdUserInGroup objUserInGroup = new AdUserInGroup() { UserId = objtemp.Id, GroupUserId = obj.TempSub.IdI[i] };
        //            _context.AdUserInGroups.Add(objUserInGroup);
        //            _context.SaveChanges();
        //        }

        //        msg.Object = x;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("USER_USERNAME").ToLower());
        //        //_logger.LogInformation(LoggingEvents.LogDb, "Insert user successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Object = ex;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("MSG_ADD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower());
        //        //_logger.LogError(LoggingEvents.LogDb, "Insert user fail");
        //    }
        //    return Json(msg);
        //}
        [HttpPost]
        public async Task<IActionResult> Update(AspNetUserCustom obj, IFormFile image)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var us = await _context.Users.FirstOrDefaultAsync(x => x.Id == obj.Id);
                var objOld = CommonUtil.Clone(us);
                if (us != null)
                {
                    //us.UserName = obj.UserName.ToLower();
                    us.Email = obj.Email;
                    us.PhoneNumber = obj.PhoneNumber;
                    us.OfficeNumber = obj.OfficeNumber;
                    us.FamilyName = obj.FamilyName;
                    us.GivenName = obj.GivenName;
                    us.EmployeeCode = obj.EmployeeCode;
                    //us.DepartmentId = obj.DepartmentId;
                    us.BranchId = obj.BranchId;
                    // us.ProfitCenterId = obj.ProfitCenterId;
                    us.AccountExecutiveId = obj.AccountExecutiveId;
                    us.Active = obj.Active;
                    us.UserType = obj.UserType;
                    us.Note = obj.Note;
                    us.Description = obj.Description;
                    us.Reason = obj.Reason;
                    us.UpdatedDate = DateTime.Now;
                    us.UpdatedBy = ESEIM.AppContext.UserName;
                    us.NormalizedEmail = obj.Email.ToUpper();
                    us.NormalizedUserName = obj.UserName.ToUpper();
                    us.LockoutEnabled = true;
                    if (image != null && image.Length > 0)
                    {
                        var url = string.Empty;
                        var pathUpload = Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\images");
                        if (!Directory.Exists(pathUpload)) Directory.CreateDirectory(pathUpload);
                        var fileName = Path.GetFileName(image.FileName);
                        fileName = Path.GetFileNameWithoutExtension(fileName)
                         + "_"
                         + Guid.NewGuid().ToString().Substring(0, 8)
                         + Path.GetExtension(fileName);
                        var filePath = Path.Combine(pathUpload, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        url = "/uploads/images/" + fileName;
                        obj.Picture = url;
                        us.Picture = obj.Picture;
                    }

                    _context.Update(us);
                    // Update main role
                    if (!string.IsNullOrEmpty(obj.RoleId))
                    {
                        var userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == us.Id);
                        if (userRole != null)
                        {
                            if (userRole.RoleId != obj.RoleId)
                            {
                                // Remove old role
                                _context.Remove(userRole);
                                // Add new role
                                var newUserRole = new IdentityUserRole<string>();
                                newUserRole.UserId = us.Id;
                                newUserRole.RoleId = obj.RoleId;
                                _context.Add(newUserRole);
                            }
                        }
                        else
                        {
                            userRole = new IdentityUserRole<string>();
                            userRole.UserId = us.Id;
                            userRole.RoleId = obj.RoleId;
                            _context.Add(userRole);
                        }
                    }
                    // Add or update main department
                    //obj.ApplicationCode = obj.ApplicationCode ?? "ADMIN";
                    var listUserInGroup = await _context.AdUserInGroups.Where(x => x.UserId == us.Id).ToListAsync();
                    var userInGroup = listUserInGroup.FirstOrDefault(x => x.UserId == us.Id && x.IsMain);
                    if (userInGroup == null)
                    {
                        userInGroup = listUserInGroup.FirstOrDefault(x => x.UserId == us.Id && x.GroupUserCode == obj.DepartmentId && x.ApplicationCode == obj.ApplicationCode);
                        if (userInGroup == null)
                        {
                            if (!string.IsNullOrEmpty(obj.DepartmentId))
                            {
                                userInGroup = new AdUserInGroup();
                                userInGroup.GroupUserCode = obj.DepartmentId;
                                userInGroup.ApplicationCode = obj.ApplicationCode;
                                userInGroup.UserId = us.Id;
                                userInGroup.RoleId = obj.RoleId;
                                userInGroup.GrantAll = true;
                                userInGroup.IsMain = true;
                                _context.Add(userInGroup); // Add entity

                                // Add/Update user permission for department
                                UpdatePermissionUserByGroup(_context, userInGroup.GroupUserCode, userInGroup.UserId, userInGroup.RoleId, obj.ApplicationCode, null, null, null);
                            }
                        }
                        else
                        {
                            // Add/Update user permission for department
                            UpdatePermissionUserByGroup(_context, userInGroup.GroupUserCode, userInGroup.UserId, userInGroup.RoleId, userInGroup.ApplicationCode, userInGroup.RoleId == obj.RoleId ? null : obj.RoleId, userInGroup.GroupUserCode == obj.DepartmentId ? null : obj.DepartmentId, userInGroup.ApplicationCode == obj.ApplicationCode ? null : obj.ApplicationCode);

                            // Update user in group
                            userInGroup.ApplicationCode = obj.ApplicationCode;
                            userInGroup.RoleId = obj.RoleId;
                            userInGroup.GrantAll = true;
                            userInGroup.IsMain = true;
                            _context.Update(userInGroup); // Update entity
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(obj.DepartmentId))
                        {
                            // Add/Update user permission for department
                            UpdatePermissionUserByGroup(_context, userInGroup.GroupUserCode, userInGroup.UserId, userInGroup.RoleId, userInGroup.ApplicationCode, userInGroup.RoleId == obj.RoleId ? null : obj.RoleId, userInGroup.GroupUserCode == obj.DepartmentId ? null : obj.DepartmentId, userInGroup.ApplicationCode == obj.ApplicationCode ? null : obj.ApplicationCode);

                            // Remove department
                            _context.Remove(userInGroup);
                        }
                        else
                        {
                            if (userInGroup.GroupUserCode != obj.DepartmentId || userInGroup.RoleId != obj.RoleId || userInGroup.ApplicationCode != obj.ApplicationCode)
                            {
                                var refUserInGroup = listUserInGroup.Where(x => x.IsMain == false && x.UserId == us.Id && x.GroupUserCode == obj.DepartmentId && x.ApplicationCode == obj.ApplicationCode).ToList();
                                if (refUserInGroup.Count > 0)
                                {
                                    _context.RemoveRange(refUserInGroup); // Remove entity
                                }

                                // Add/Update user permission for department
                                UpdatePermissionUserByGroup(_context, userInGroup.GroupUserCode, userInGroup.UserId, userInGroup.RoleId, userInGroup.ApplicationCode, userInGroup.RoleId == obj.RoleId ? null : obj.RoleId, userInGroup.GroupUserCode == obj.DepartmentId ? null : obj.DepartmentId, userInGroup.ApplicationCode == obj.ApplicationCode ? null : obj.ApplicationCode);

                                userInGroup.GroupUserCode = obj.DepartmentId;
                                userInGroup.ApplicationCode = obj.ApplicationCode;
                                //userInGroup.UserId = us.Id;
                                userInGroup.RoleId = obj.RoleId;
                                userInGroup.GrantAll = true;
                                _context.Update(userInGroup); // Update entity
                            }
                        }
                    }
                    // Add or update main profit center
                    var profitCenter = listUserInGroup.FirstOrDefault(x => x.UserId == us.Id && x.IsMain && x.GroupUserCode.StartsWith("p"));
                    if (profitCenter == null)
                    {
                        profitCenter = listUserInGroup.FirstOrDefault(x => x.UserId == us.Id && x.GroupUserCode == obj.ProfitCenterId && x.ApplicationCode == obj.ApplicationCode);
                        if (profitCenter == null)
                        {
                            if (!string.IsNullOrEmpty(obj.ProfitCenterId))
                            {
                                profitCenter = new AdUserInGroup();
                                profitCenter.GroupUserCode = obj.ProfitCenterId;
                                profitCenter.ApplicationCode = obj.ApplicationCode;
                                profitCenter.UserId = us.Id;
                                profitCenter.RoleId = obj.RoleId;
                                profitCenter.GrantAll = true;
                                profitCenter.IsMain = true;
                                _context.Add(profitCenter); // Add entity
                                // Add/Update user permission for department
                                UpdatePermissionUserByGroup(_context, profitCenter.GroupUserCode, profitCenter.UserId, profitCenter.RoleId, obj.ApplicationCode, null, null, null);
                            }
                        }
                        else
                        {
                            // Add/Update user permission for department
                            UpdatePermissionUserByGroup(_context, profitCenter.GroupUserCode, profitCenter.UserId, profitCenter.RoleId, profitCenter.ApplicationCode, profitCenter.RoleId == obj.RoleId ? null : obj.RoleId, profitCenter.GroupUserCode == obj.ProfitCenterId ? null : obj.ProfitCenterId, profitCenter.ApplicationCode == obj.ApplicationCode ? null : obj.ApplicationCode);

                            // Update user in group
                            profitCenter.ApplicationCode = obj.ApplicationCode;
                            profitCenter.RoleId = obj.RoleId;
                            profitCenter.GrantAll = true;
                            profitCenter.IsMain = true;
                            _context.Update(profitCenter); // Update entity
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(obj.ProfitCenterId))
                        {
                            // Add/Update user permission for department
                            UpdatePermissionUserByGroup(_context, profitCenter.GroupUserCode, profitCenter.UserId, profitCenter.RoleId, profitCenter.ApplicationCode, profitCenter.RoleId == obj.RoleId ? null : obj.RoleId, profitCenter.GroupUserCode == obj.ProfitCenterId ? null : obj.ProfitCenterId, profitCenter.ApplicationCode == obj.ApplicationCode ? null : obj.ApplicationCode);

                            // Remove main profit center
                            _context.Remove(profitCenter);
                        }
                        else
                        {
                            if (profitCenter.GroupUserCode != obj.ProfitCenterId || profitCenter.RoleId != obj.RoleId || profitCenter.ApplicationCode != obj.ApplicationCode)
                            {
                                var refUserInGroup = listUserInGroup.Where(x => x.IsMain == false && x.UserId == us.Id && x.GroupUserCode == obj.ProfitCenterId && x.ApplicationCode == obj.ApplicationCode).ToList();
                                if (refUserInGroup.Count > 0)
                                {
                                    _context.RemoveRange(refUserInGroup); // Remove entity
                                }

                                // Add/Update user permission for department
                                UpdatePermissionUserByGroup(_context, profitCenter.GroupUserCode, profitCenter.UserId, profitCenter.RoleId, profitCenter.ApplicationCode, profitCenter.RoleId == obj.RoleId ? null : obj.RoleId, profitCenter.GroupUserCode == obj.ProfitCenterId ? null : obj.ProfitCenterId, profitCenter.ApplicationCode == obj.ApplicationCode ? null : obj.ApplicationCode);

                                profitCenter.GroupUserCode = obj.ProfitCenterId;
                                profitCenter.ApplicationCode = obj.ApplicationCode;
                                //profitCenter.UserId = us.Id;
                                profitCenter.RoleId = obj.RoleId;
                                profitCenter.GrantAll = true;
                                _context.Update(profitCenter); // Update entity
                            }
                        }
                    }

                    var result = await _context.SaveChangesAsync();
                    //// Update permission
                    //UpdatePermissionUser(obj.UserType, us.Id, obj.RoleId, obj.DepartmentId);
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("ADM_USER_LBL_USER"));
                }
                else
                {
                    msg.Error = true;
                    //msg.Title = "Nhân viên không tồn tại trong hệ thống !";
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_EMPLOYEE_NOT_EXITS"), CommonUtil.ResourceValue("ADM_USER_LBL_USER"));

                    //_logger.LogError(LoggingEvents.LogDb, "Update User Fail");
                    //_actionLog.InsertActionLog("AspNetUsers", "Update User Fail", null, null, "Error");
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                //msg.Title = "Cập nhật thất bại";
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_FAILED"), CommonUtil.ResourceValue("ADM_USER_LBL_USER"));

                //_logger.LogError(LoggingEvents.LogDb, "Update User Fail");
                //_actionLog.InsertActionLog("AspNetUsers", "An error occurred while Update user", null, null, "Error");
            }
            return Json(msg);
        }


        [HttpPost]
        public async Task<JsonResult> Deactive([FromBody]ChangeStatusUserModel obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var listUser = _context.Users.Where(x => obj.ListId.Count > 0 && obj.ListId.Any(y => y == x.Id));
                if (listUser.Any())
                {
                    foreach (var us in listUser)
                    {
                        us.Active = false;
                        us.Reason = obj.Reason;
                        us.UpdatedDate = DateTime.Now;
                        us.UpdatedBy = ESEIM.AppContext.UserName;

                        _context.Users.Update(us);
                        _context.Entry(us).State = EntityState.Modified;
                    }
                }

                var successCount = await _context.SaveChangesAsync();
                successCount = successCount / 2;
                if (successCount == obj.ListId.Count)
                {
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DEACTIVE_SUCCESS"), CommonUtil.ResourceValue("ADM_USER_LBL_USER"));

                }
                else
                {
                    if (successCount == 0)
                    {
                        msg.Error = true;
                        msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DEACTIVE_FAILED"), CommonUtil.ResourceValue("ADM_USER_LBL_USER"));

                        _actionLog.InsertActionLog("ASP_NET_USERS", "Deactive users fail", null, null, "Error");
                    }
                    else
                    {
                        msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DEACTIVE_COUNT_SUCCESS"), CommonUtil.ResourceValue("ADM_USER_LBL_USER").ToLower(), successCount.ToString(), obj.ListId.Count.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DEACTIVE_FAILED"), CommonUtil.ResourceValue("ADM_USER_LBL_USER"));
                _actionLog.InsertActionLog("ASP_NET_USERS", "Deactive users failed: " + ex.Message, null, null, "Error");

            }
            return Json(msg);
        }

        [HttpPost]
        public async Task<JsonResult> Active([FromBody]ChangeStatusUserModel obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var listUser = _context.Users.Where(x => obj.ListId.Count > 0 && obj.ListId.Any(y => y == x.Id));
                if (listUser.Any())
                {
                    foreach (var us in listUser)
                    {
                        us.Active = true;
                        us.Reason = obj.Reason;
                        us.UpdatedDate = DateTime.Now;
                        us.UpdatedBy = ESEIM.AppContext.UserName;

                        _context.Users.Update(us);
                        _context.Entry(us).State = EntityState.Modified;
                    }
                }

                var successCount = await _context.SaveChangesAsync();
                successCount = successCount / 2;
                if (successCount == obj.ListId.Count)
                {
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ACTIVE_SUCCESS"), CommonUtil.ResourceValue("ADM_USER_LBL_USER"));
                }
                else
                {
                    if (successCount == 0)
                    {
                        msg.Error = true;
                        msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ACTIVE_FAILED"), CommonUtil.ResourceValue("ADM_USER_LBL_USER"));
                        _actionLog.InsertActionLog("ASP_NET_USERS", "Active users fail", null, null, "Error");
                    }
                    else
                    {
                        msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ACTIVE_COUNT_SUCCESS"), CommonUtil.ResourceValue("ADM_USER_LBL_USER").ToLower(), successCount.ToString(), obj.ListId.Count.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ACTIVE_FAILED"), CommonUtil.ResourceValue("ADM_USER_LBL_USER"));
                _actionLog.InsertActionLog("ASP_NET_USERS", "Active users failed: " + ex.Message, null, null, "Error");

            }
            return Json(msg);
        }

        [HttpPost]
        public async Task<object> GetItem(string id)
        {
            try
            {
                //var user = _roleManager.Users.Single(x => x.Id == obj.Id /*&& x.ConcurrencyStamp == obj.ConcurrencyStamp*/);
                //user.AspNetUserRoles = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();
                var user = await _context.Users.Include(i => i.Branch).SingleAsync(x => x.Id == id);

                // Get Branch reference
                List<BranchRef> listBranchRef = new List<BranchRef>();
                var groupBranchRefByApp = _context.AdUserInGroups.Include(i => i.Application).Where(x => x.UserId == user.Id).GroupBy(g => g.Application);
                if (groupBranchRefByApp.Any())
                {
                    foreach (var app in groupBranchRefByApp)
                    {
                        var firstApp = app.First();
                        if (!string.IsNullOrEmpty(firstApp.BranchReference))
                        {
                            if (firstApp.BranchReference == "b_000")
                            {
                                var branchRef = new BranchRef();
                                branchRef.AppCode = app.Key.ApplicationCode;
                                branchRef.AppName = app.Key.Title;
                                branchRef.BranchCode = "000";
                                branchRef.BranchName = "All Branch";
                                // Add to list
                                listBranchRef.Add(branchRef);
                            }
                            else
                            {
                                var listBranchCode = firstApp.BranchReference.Split(',').ToList();
                                var listBranch = _context.AdOrganizations.Where(x => x.OrgGroup.HasValue && x.OrgGroup.Value == 2 && listBranchCode.Any(y => y == x.OrgAddonCode)).OrderBy(o => o.OrgCode).ToList();
                                if (listBranch.Count > 0)
                                {
                                    foreach (var br in listBranch)
                                    {
                                        var branchRef = new BranchRef();
                                        branchRef.AppCode = app.Key.ApplicationCode;
                                        branchRef.AppName = app.Key.Title;
                                        branchRef.BranchCode = br.OrgCode;
                                        branchRef.BranchName = br.OrgName;
                                        // Add to list
                                        listBranchRef.Add(branchRef);
                                    }
                                }
                            }
                        }
                    }
                }
                // Get department / Profit center
                var mainGroup = await _context.AdUserInGroups.Where(x => x.IsMain && x.UserId == user.Id).ToListAsync();
                //var mainDepartment = mainGroup.FirstOrDefault(x => !string.IsNullOrEmpty(x.GroupUserCode) && x.GroupUserCode.ToLower().Contains('d'));
                //var mainProfitCenter = mainGroup.FirstOrDefault(x => !string.IsNullOrEmpty(x.GroupUserCode) && x.GroupUserCode.ToLower().Contains('p'));

                var temp = new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    user.OfficeNumber,
                    user.FamilyName,
                    user.GivenName,
                    user.MiddleName,
                    user.EmployeeCode,
                    user.BranchId,
                    BranchName = user?.Branch == null ? null : string.Format(user?.Branch?.OrgCode + " - " + user?.Branch?.OrgName),
                    DepartmentId = mainGroup.Any() ? mainGroup[0]?.GroupUserCode : null,
                    //ProfitCenterId = mainProfitCenter?.GroupUserCode,
                    ApplicationCode = mainGroup.Any() ? mainGroup[0]?.ApplicationCode : null,
                    user.Active,
                    user.Description,
                    user.UserType,
                    user.IsExceeded,
                    user.Note,
                    user.Reason,
                    user.ConcurrencyStamp,
                    RoleId = mainGroup.Any() ? mainGroup[0].RoleId : null,
                    BranchReference = listBranchRef,
                    user.Picture
                };

                return Json(temp);
            }
            catch (Exception ex)
            {
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
            }
        }

        [HttpPost]
        public object GetTreeData(int? id)
        {
            //int temp = 0;
            //if (id != null)
            //{
            //    temp = (int)id;
            //}
            var temp = Convert.ToInt32(id);
            var data = _context.AdOrganizations.Where(x => (x.OrgGroup == id || temp == 0) && x.IsEnabled).OrderBy(x => x.OrgCode).AsNoTracking();
            //var dataOrder = GetSubTreeData(data.ToList(), temp, new List<TreeView>(), "");
            return data;
        }

        private List<TreeView> GetSubTreeData(List<AdOrganization> data, int parentid, List<TreeView> lstCategories, string tab)
        {
            tab += "- ";
            var contents = parentid == 0
                ? data.Where(x => x.OrgGroup == null).ToList()
                : data.Where(x => x.OrgGroup == parentid).ToList();
            foreach (var item in contents)
            {
                var category = new TreeView
                {
                    Id = item.OrgId,
                    Title = tab + item.OrgName,
                    HasChild = data.Any(x => x.OrgGroup == item.OrgId)
                };
                lstCategories.Add(category);
                if (category.HasChild) GetSubTreeData(data, item.OrgId, lstCategories, tab);
            }
            return lstCategories;
        }

        //[HttpPost]
        //public async Task<IActionResult> UploadUser(IFormFile fileUpload)
        //{
        //    var msg = new JMessage() { Error = true, Title = "File upload is invalid" };
        //    try
        //    {
        //        if (fileUpload != null && fileUpload.Length > 0)
        //        {
        //            //// Upload file to folder
        //            //var pathUpload = Path.Combine(_hostingEnvironment.WebRootPath, "files\\user");
        //            //if (!Directory.Exists(pathUpload)) Directory.CreateDirectory(pathUpload);

        //            //var fileName = string.Format("{0:yyyyMMddHHmmss}-{1}", DateTime.Now, fileUpload.FileName);
        //            //var filePath = Path.Combine(pathUpload, fileName);
        //            //using (var stream = new FileStream(filePath, FileMode.Create))
        //            //{
        //            //    await fileUpload.CopyToAsync(stream);
        //            //}

        //            // Read content from file
        //            ExcelEngine excelEngine = new ExcelEngine();
        //            IApplication application = excelEngine.Excel;
        //            IWorkbook workbook = application.Workbooks.Create();

        //            workbook = application.Workbooks.Open(fileUpload.OpenReadStream());
        //            IWorksheet worksheet = workbook.Worksheets[0];

        //            var listUser = new List<UserImportData>();
        //            ListUserFailed.ListUser = new List<UserFailed>();
        //            if (worksheet.Rows.Length > 1)
        //            {
        //                var title = worksheet.Rows[0].Cells;
        //                if (
        //                    title[0].DisplayText.Trim() == "Tên đăng nhập" &&
        //                    title[1].DisplayText.Trim() == "Chi nhánh" &&
        //                    title[2].DisplayText.Trim() == "Phòng ban" &&
        //                    title[3].DisplayText.Trim() == "Profit center" &&
        //                    title[4].DisplayText.Trim() == "Vai trò (role)" &&
        //                    title[5].DisplayText.Trim() == "Application"
        //                    )
        //                {
        //                    // Check total column is 6
        //                    if (title.Length == 6)
        //                    {
        //                        for (int i = 1; i < worksheet.Rows.Length; i++)
        //                        {
        //                            var cells = worksheet.Rows[i].Cells;
        //                            var userName = cells[0].DisplayText;
        //                            var branch = cells[1].DisplayText;
        //                            var department = cells[2].DisplayText;
        //                            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(department) || string.IsNullOrEmpty(branch)) continue;

        //                            var profitCenter = cells[3].DisplayText;
        //                            var accountExe = "";//cells[4].DisplayText;
        //                            var groupUser = "";//cells[5].DisplayText;
        //                            var role = cells[4].DisplayText;
        //                            var appCode = cells[5].DisplayText;
        //                            var active = "TRUE";//cells[7].DisplayText;

        //                            var user = await CheckImportUser(userName, department, branch, profitCenter, accountExe, groupUser, role, appCode, active);
        //                            user.RowNum = i;
        //                            listUser.Add(user);

        //                            // Add user result
        //                            UserFailed listuser = new UserFailed();
        //                            listuser.UserName = user.UserName;
        //                            listuser.Branch = user.Branch;
        //                            listuser.Department = user.Department;
        //                            listuser.ProfitCenter = user.ProfitCenter;
        //                            //listuser.AccountExecutive = user.AccountExecutive;
        //                            //listuser.GroupUser = user.GroupUser;
        //                            listuser.Role = user.Role;
        //                            listuser.Application = user.AppName;
        //                            //listuser.Status = true;//user.Status;
        //                            listuser.Validation = user.Error ? "FALSE" : "TRUE";
        //                            listuser.Message = user.ErrorUserName ? user.MessageUserName : user.ErrorEmployeeCode
        //                                                                  ? user.MessageEmployeeCode : user.ErrorDepartment
        //                                                                  ? user.MessageDepartment : user.ErrorBranch
        //                                                                  ? user.MessageBranch : user.ErrorRole
        //                                                                  ? user.MessageRole : string.Empty;

        //                            ListUserFailed.ListUser.Add(listuser);
        //                        }

        //                        msg.Error = false;
        //                        msg.Object = listUser;
        //                    }
        //                    else
        //                    {
        //                        msg.Error = true;
        //                        msg.Title = "Incorrect format file ";
        //                    }

        //                }
        //                else
        //                {
        //                    msg.Error = true;
        //                    msg.Title = "Incorrect format file ";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            msg.Title = "File upload is required";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        msg.Title = "File upload is invalid";
        //    }

        //    return Json(msg);
        //}

        //[HttpPost]
        //public async Task<JsonResult> InsertUsers([FromBody]List<UserImportData> listUser)
        //{
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {
        //        if (listUser.Count > 0)
        //        {
        //            int count = 0;
        //            foreach (var obj in listUser)
        //            {
        //                try
        //                {
        //                    var us = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(obj.UserName.ToLower()));
        //                    if (us == null)
        //                    {
        //                        AspNetUser objtemp = new AspNetUser()
        //                        {
        //                            UserName = obj.UserName.ToLower(),
        //                            Email = obj.Email,
        //                            //PhoneNumber = obj.PhoneNumber,
        //                            //OfficeNumber = obj.OfficeNumber,
        //                            //FamilyName = obj.FamilyName,
        //                            //MiddleName = obj.MiddleName,
        //                            GivenName = obj.FullName,
        //                            EmployeeCode = obj.EmployeeCode,
        //                            //DepartmentId = obj.DepartmentId,
        //                            BranchId = obj.BranchId,
        //                            //ProfitCenterId = obj.ProfitCenterId,
        //                            AccountExecutiveId = obj.AccountExecutiveId,
        //                            Active = obj.Status,
        //                            //Note = obj.Note,
        //                            //Description = obj.Description,
        //                            //Reason = obj.Reason,
        //                            UserType = 0,
        //                            CreatedDate = DateTime.Now,
        //                            CreatedBy = EIM.AppContext.UserName,
        //                            NormalizedEmail = obj.Email.ToUpper(),
        //                            NormalizedUserName = obj.UserName.ToUpper(),
        //                            LockoutEnabled = true,
        //                            SecurityStamp = Guid.NewGuid().ToString(),
        //                        };
        //                        _context.Users.Add(objtemp);
        //                        var result = await _context.SaveChangesAsync();
        //                        if (result > 0)
        //                        {
        //                            count++;

        //                            // Import role default
        //                            if (!obj.ErrorRole)
        //                            {
        //                                var userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == objtemp.Id && x.RoleId == obj.RoleId);
        //                                if (userRole == null)
        //                                {
        //                                    userRole = new IdentityUserRole<string>();
        //                                    userRole.UserId = objtemp.Id;
        //                                    userRole.RoleId = obj.RoleId;
        //                                    _context.Add(userRole);
        //                                }
        //                            }

        //                            obj.AppCode = obj.AppCode ?? "ADMIN";
        //                            // Import department
        //                            if (!obj.ErrorDepartment)
        //                            {
        //                                var userInGroup = await _context.VIBUserInGroups.FirstOrDefaultAsync(x => x.UserId == objtemp.Id && x.GroupUserCode == obj.DepartmentId);
        //                                if (userInGroup == null)
        //                                {
        //                                    userInGroup = new VIBUserInGroup();
        //                                    userInGroup.GroupUserCode = obj.DepartmentId;
        //                                    userInGroup.ApplicationCode = obj.AppCode;
        //                                    userInGroup.UserId = objtemp.Id;
        //                                    if (!obj.ErrorRole) userInGroup.RoleId = obj.RoleId;
        //                                    userInGroup.GrantAll = true;
        //                                    userInGroup.IsMain = true;
        //                                    _context.Add(userInGroup);
        //                                    // Add/Update user permission for department
        //                                    UpdatePermissionUserByGroup(_context, userInGroup.GroupUserCode, userInGroup.UserId, userInGroup.RoleId, obj.AppCode, null, null, null);
        //                                }
        //                            }
        //                            // Import profit center
        //                            if (!obj.ErrorProfitCenter)
        //                            {
        //                                var userInGroup = await _context.VIBUserInGroups.FirstOrDefaultAsync(x => x.UserId == objtemp.Id && x.GroupUserCode == obj.ProfitCenterId);
        //                                if (userInGroup == null)
        //                                {
        //                                    userInGroup = new VIBUserInGroup();
        //                                    userInGroup.GroupUserCode = obj.ProfitCenterId;
        //                                    userInGroup.ApplicationCode = obj.AppCode;
        //                                    userInGroup.UserId = objtemp.Id;
        //                                    if (!obj.ErrorRole) userInGroup.RoleId = obj.RoleId;
        //                                    userInGroup.GrantAll = true;
        //                                    userInGroup.IsMain = true;
        //                                    _context.Add(userInGroup);
        //                                    // Add/Update user permission for profit center
        //                                    UpdatePermissionUserByGroup(_context, userInGroup.GroupUserCode, userInGroup.UserId, userInGroup.RoleId, obj.AppCode, null, null, null);
        //                                }
        //                            }
        //                            await _context.SaveChangesAsync();
        //                            //// Import permission
        //                            //UpdatePermissionUser(0, objtemp.Id, obj.RoleId, obj.DepartmentId);

        //                            _actionLog.InsertActionLog("ASP_NET_USERS", $"Insert success user : \"{obj.UserName}\"", null, obj, "Insert", false, obj.UserName);
        //                        }
        //                    }
        //                }
        //                catch (Exception e)
        //                {
        //                    _actionLog.InsertActionLog("ASP_NET_USERS", $"Insert user \"{obj.UserName}\" failed: " + e.Message, null, null, "Error");
        //                }
        //            }
        //            // Save change all
        //            await _context.SaveChangesAsync();

        //            msg.Title = String.Format(CommonUtil.ResourceValue("MSG_IMPORT_USER_SUCCESS"), count, listUser.Count);
        //        }
        //        else
        //        {
        //            msg.Error = true;
        //            msg.Title = CommonUtil.ResourceValue("MSG_IMPORT_USER_EMPTY");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Object = ex;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("MSG_ADD_FAIL"), CommonUtil.ResourceValue("USER").ToLower());
        //        //_logger.LogError(LoggingEvents.LogDb, "Insert user fail");
        //        _actionLog.InsertActionLog("ASP_NET_USERS", "Insert list user failed: " + ex.Message, null, null, "Error");
        //    }

        //    return Json(msg);
        //}

        //[HttpGet]
        //public async Task<IActionResult> CheckUser(string userName)
        //{
        //    var msg = new JMessage() { Error = true };

        //    try
        //    {
        //        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(userName.ToLower()));
        //        if (user != null)
        //        {
        //            msg.Title = "Account is exists in system!";
        //        }
        //        else
        //        {
        //            var userAd = CheckUserInAD(userName);
        //            if (userAd.Error)
        //            {
        //                msg.Title = userAd.Message;
        //            }
        //            else
        //            {
        //                var userOms = GetEmployeeCode(userName);
        //                if (userOms.Error)
        //                {
        //                    msg.Title = userOms.Message;
        //                }
        //                else
        //                {
        //                    userAd.EmployeeCode = userOms.EmployeeCode;
        //                    msg.Error = false;
        //                }
        //            }

        //            msg.Object = userAd;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        msg.Title = "Account is invalid!";
        //    }

        //    return Json(msg);
        //}

        //[NonAction]
        //private UserInfoAd CheckUserInAD(string userName)
        //{
        //    var userAd = new UserInfoAd { Error = true };
        //    try
        //    {
        //        string objectDN = _appSettings.ADConfigs.ObjectDN;
        //        if (_ldapConnection == null || !_ldapConnection.Connected)
        //        {
        //            int ldapPort = _appSettings.ADConfigs.Port;
        //            int ldapVersion = LdapConnection.Ldap_V3;
        //            string ldapHost = _appSettings.ADConfigs.Host;
        //            string loginDN = _appSettings.ADConfigs.LoginDN;
        //            string password = _appSettings.ADConfigs.Password;

        //            _ldapConnection = new LdapConnection();
        //            _ldapConnection.Connect(ldapHost, ldapPort);
        //            _ldapConnection.Bind(ldapVersion, loginDN, password);
        //        }

        //        var entities = _ldapConnection.Search(objectDN, LdapConnection.SCOPE_SUB, $"(sAMAccountName={userName})", new string[] { "sAMAccountName", "mail", "name", "givenName", "sn", "userAccountControl", "objectGUID" }, false);
        //        string userDn = null, userAc = null;
        //        while (entities.hasMore())
        //        {
        //            var entity = entities.next();
        //            var account = entity.getAttribute("sAMAccountName");
        //            var email = entity.getAttribute("mail");
        //            var name = entity.getAttribute("name");
        //            //var givenName = entity.getAttribute("givenName");
        //            //var familyName = entity.getAttribute("sn");
        //            var userAccountControl = entity.getAttribute("userAccountControl");
        //            if (account != null && account.StringValue.ToLower() == userName.ToLower())
        //            {
        //                userAd.UserName = account?.StringValue;
        //                userAd.FullName = name?.StringValue;
        //                userAd.Email = email?.StringValue;
        //                userAc = userAccountControl?.StringValue;

        //                userDn = entity.DN;
        //                break;
        //            }
        //        }
        //        if (string.IsNullOrWhiteSpace(userDn))
        //        {
        //            userAd.Message = "Account is not exists in AD";
        //        }
        //        else if (!string.IsNullOrEmpty(userAc) && userAc.Equals("514"))
        //        {
        //            userAd.Message = "Account was disabled in AD";
        //        }
        //        else
        //        {
        //            userAd.Error = false;
        //        }
        //    }
        //    catch (LdapException e)
        //    {
        //        if (e.ResultCode == LdapException.INVALID_CREDENTIALS)
        //        {
        //            userAd.Message = "Account connect AD incorrect";
        //        }
        //        else
        //        {
        //            userAd.Message = "Account is not exists in AD";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        userAd.Message = "An error occurred while get info from AD";
        //    }

        //    return userAd;
        //}

        //[NonAction]
        //private UserInfoAd GetEmployeeCode(string userName)
        //{
        //    var userAd = new UserInfoAd { Error = true };
        //    try
        //    {
        //        if (_apiSoapClient == null || _apiSoapClient.State != CommunicationState.Opened)
        //        {
        //            _apiSoapClient = new apiSoapClient(apiSoapClient.EndpointConfiguration.apiSoap2, _appSettings.OMSConfigs.Url);
        //            _apiSoapClient.ChannelFactory.Credentials.Windows.ClientCredential.Domain = _appSettings.OMSConfigs.Domain;
        //            _apiSoapClient.ChannelFactory.Credentials.Windows.ClientCredential.UserName = _appSettings.OMSConfigs.Username;
        //            _apiSoapClient.ChannelFactory.Credentials.Windows.ClientCredential.Password = _appSettings.OMSConfigs.Password;
        //        }

        //        var user = Task.Run(() => _apiSoapClient.GetUserAsync(userName)).Result;
        //        if (user?.Body?.GetUserResult != null)
        //        {
        //            userAd.EmployeeCode = user.Body.GetUserResult.Code;
        //            userAd.Error = false;
        //        }
        //        else
        //        {
        //            userAd.Message = "Employee code is not exists";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        userAd.Message = "An error occurred while get employee code";
        //    }
        //    return userAd;
        //}

        //[NonAction]
        //private async Task<UserImportData> CheckImportUser(string userName, string department, string branch, string profitCenter, string accountExe, string groupUser, string role, string appCode, string active)
        //{
        //    //// Check Status
        //    //bool isActive;
        //    //if (!bool.TryParse(active, out isActive)) isActive = false;

        //    var userImport = new UserImportData
        //    {
        //        Error = false,
        //        UserName = userName,
        //        Department = department,
        //        Branch = branch,
        //        ProfitCenter = profitCenter,
        //        AccountExecutive = accountExe,
        //        Role = role,
        //        AppName = appCode,
        //        Status = true,//isActive
        //    };
        //    var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(userName.ToLower()));
        //    if (user != null)
        //    {
        //        userImport.FullName = user.GivenName;
        //        userImport.Email = user.Email;
        //        userImport.EmployeeCode = user.EmployeeCode;
        //        userImport.AccountExecutive = user.AccountExecutiveId;

        //        userImport.ErrorUserName = true;
        //        userImport.MessageUserName = "Account is exists in system";
        //    }
        //    else
        //    {
        //        var userAd = CheckUserInAD(userName);
        //        if (userAd.Error)
        //        {
        //            userImport.ErrorUserName = true;
        //            userImport.MessageUserName = userAd.Message;
        //        }
        //        else
        //        {
        //            userImport.UserName = userName;
        //            userImport.FullName = userAd.FullName;
        //            userImport.Email = userAd.Email;
        //            // Get Employee code
        //            var userOms = GetEmployeeCode(userName);
        //            if (userOms.Error)
        //            {
        //                userImport.ErrorEmployeeCode = true;
        //                userImport.MessageEmployeeCode = userOms.Message;
        //            }
        //            else
        //            {
        //                userImport.EmployeeCode = userOms.EmployeeCode;
        //                // Get Account Executive
        //                var fmAcctExec = await _context.VIBFmAcctExecs.FirstOrDefaultAsync(x => x.StaffCode == userOms.EmployeeCode);
        //                if (fmAcctExec != null)
        //                {
        //                    userImport.AccountExecutiveId = fmAcctExec.AcctExec;
        //                }
        //            }
        //            // Check department
        //            userImport.Department = department;
        //            //var vibOrg = await _context.VIBOrganizations.FirstOrDefaultAsync(x => x.OrgGroup.HasValue && x.OrgGroup.Value == 1 && x.OrgName.ToLower().Equals(department.ToLower()));
        //            var deparmentCode = "d" + department.ToLower();
        //            var vibGroup = await _context.VIBGroupUsers.FirstOrDefaultAsync(x => x.ParentCode == "D000" && (x.GroupUserCode == deparmentCode || (x.Description != null && x.Description.ToLower().Equals(department.ToLower()))));
        //            if (vibGroup != null)
        //            {
        //                userImport.DepartmentId = vibGroup.GroupUserCode;
        //                //userImport.Department = vibGroup.Title;
        //            }
        //            else
        //            {
        //                userImport.ErrorDepartment = true;
        //                userImport.MessageDepartment = "Department is not exists";
        //            }
        //            // Check branch
        //            userImport.Branch = branch;
        //            var vibOrg = await _context.VIBOrganizations.FirstOrDefaultAsync(x => x.OrgGroup.HasValue && x.OrgGroup.Value == 2 && (x.OrgCode != null && x.OrgCode.ToLower() == branch.ToLower()));
        //            if (vibOrg != null)
        //            {
        //                userImport.BranchId = vibOrg.OrgAddonCode;
        //                //userImport.Branch = vibOrg.OrgCode + " - " + vibOrg.OrgName;
        //            }
        //            else
        //            {
        //                userImport.ErrorBranch = true;
        //                userImport.MessageBranch = "Branch is not exists";
        //            }
        //            // Check profit center
        //            userImport.ProfitCenter = profitCenter;
        //            //vibOrg = await _context.VIBOrganizations.FirstOrDefaultAsync(x => x.OrgGroup.HasValue && x.OrgGroup.Value == 3 && x.OrgName.ToLower().Equals(profitCenter.ToLower()));
        //            var profitCenterCode = "p" + profitCenter.ToLower();
        //            vibGroup = await _context.VIBGroupUsers.FirstOrDefaultAsync(x => x.ParentCode == "P000" && (x.GroupUserCode == profitCenterCode || x.Description.ToLower().Equals(profitCenter.ToLower())));
        //            if (vibGroup != null)
        //            {
        //                userImport.ProfitCenterId = vibGroup.GroupUserCode;
        //                //userImport.ProfitCenter = vibGroup.Title;
        //            }
        //            else
        //            {
        //                userImport.ErrorProfitCenter = true;
        //                userImport.MessageProfitCenter = "Profit Center is not exists";
        //            }
        //            //// Check account executive
        //            //userImport.AccountExecutive = accountExe;
        //            //var acctExec = await _context.VIBFmAcctExecs.FirstOrDefaultAsync(x => x.AcctExec != null && x.AcctExec.ToLower().Equals(accountExe.ToLower()));
        //            //if (acctExec != null)
        //            //{
        //            //    userImport.AccountExecutiveId = acctExec.AcctExec;
        //            //    //userImport.AccountExecutive = acctExec.AcctExec + " - " + acctExec.AcctExecName;
        //            //}
        //            //else
        //            //{
        //            //    userImport.ErrorAccountExecutive = true;
        //            //    userImport.MessageAccountExecutive = "AE is not exists";
        //            //}
        //            // Check role
        //            userImport.Role = role;
        //            var vibRole = await _context.Roles.FirstOrDefaultAsync(x => x.Code.ToLower().Equals(role.ToLower()));
        //            if (vibRole != null)
        //            {
        //                userImport.RoleId = vibRole.Id;
        //                //userImport.Role = vibRole.Title;
        //            }
        //            else
        //            {
        //                userImport.ErrorRole = true;
        //                userImport.MessageRole = "Role is not exists";
        //            }
        //            // Check application
        //            userImport.AppName = appCode;
        //            var vibApp = await _context.VIBApplications.FirstOrDefaultAsync(x => x.ApplicationCode.ToLower().Equals(appCode.ToLower()) || x.Title.ToLower().Equals(appCode.ToLower()));
        //            if (vibApp != null)
        //            {
        //                userImport.AppCode = vibApp.ApplicationCode;
        //                //userImport.AppName = vibApp.Title;
        //            }
        //            else
        //            {
        //                userImport.ErrorApp = true;
        //                userImport.MessageApp = "Application is not exists";
        //            }
        //            // Check Status
        //            //userImport.Status = isActive;
        //        }
        //    }
        //    userImport.Error = userImport.ErrorUserName || userImport.ErrorEmployeeCode || userImport.ErrorDepartment || userImport.ErrorBranch || userImport.ErrorRole;
        //    userImport.Checked = !userImport.Error;

        //    return userImport;
        //}


    }

    public class UserPerModel
    {
        public string AppCode { set; get; }
        public string UserId { set; get; }
        public string GroupCode { set; get; }
        public string RoleId { set; get; }
        public bool IsExceeded { set; get; }
        public List<GroupUserPermission> GroupUsers { set; get; }
        public List<string> BranchRefs { set; get; }
    }

    public class GroupUserPermission
    {
        public string GroupCode { set; get; }
        public string RoleId { set; get; }
        public List<AdResourcePermission> Resources { set; get; }
    }

    public class GroupUserAll
    {
        public string GroupCode { set; get; }
        public string ParentCode { set; get; }
        public string Title { set; get; }
        public string RoleId { set; get; }
        public bool IsMain { set; get; }
        public bool IsChecked { set; get; }
        public bool HasChild { set; get; }
        public int? Ord { set; get; }
    }

    public class UserApi
    {
        public UserApi()
        {
            List = "/user/JTableOfUser";
        }

        public string List { set; get; }
        public string Insert { set; get; }
        public string Update { set; get; }
    }

    public class BranchRef
    {
        public string AppCode { set; get; }
        public string AppName { set; get; }
        public string BranchCode { set; get; }
        public string BranchName { set; get; }
    }

    public static class IQueryableExtensions
    {
        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();
        private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");
        private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");
        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
        private static readonly FieldInfo QueryCompilationContextFactoryField = typeof(Database).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_queryCompilationContextFactory");

        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            if (!(query is EntityQueryable<TEntity>)
                && !(query is InternalDbSet<TEntity>))
            {
                throw new ArgumentException("Invalid query");
            }

            var queryCompiler = (IQueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var nodeTypeProvider =
                (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);
            var parser = (IQueryParser)CreateQueryParserMethod.Invoke
                (queryCompiler, new object[] { nodeTypeProvider });
            var queryModel = parser.GetParsedQuery(query.Expression);
            var database = DataBaseField.GetValue(queryCompiler);
            var queryCompilationContextFactory =
                (IQueryCompilationContextFactory)QueryCompilationContextFactoryField.GetValue(database);
            var queryCompilationContext = queryCompilationContextFactory.Create(false);
            var modelVisitor =
                (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }
    }
}