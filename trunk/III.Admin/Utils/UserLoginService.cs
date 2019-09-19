using ESEIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESEIM.Utils
{
    public interface IUserLoginService
    {
        SessionUserLogin GetSessionUser(string userId);
    }

    public class UserLoginService : IUserLoginService
    {
        private EIMDBContext _context;
        private IParameterService _parameterService;

        public UserLoginService(EIMDBContext context,IParameterService parameterService)
        {
            _context = context;
            _parameterService = parameterService;
        }

        public SessionUserLogin GetSessionUser(string userId)
        {
            var session = new SessionUserLogin();
            var user = _context.Users.FirstOrDefault(x => x.Id == userId && x.Active == true);
            if (user != null)
            {
                session.UserId = user.Id;
                session.UserName = user.UserName;
                session.FullName = user.GivenName;
                session.Email = user.Email;
                session.EmployeeCode = user.EmployeeCode;
                session.CompanyCode = "TAMLONG";
                session.SessionTimeOut = _parameterService.GetSessionTimeout();
                session.ExpireTimeSpan = DateTime.Now.AddMinutes(session.SessionTimeOut);
                session.Picture = user.Picture;

                //session.CompanyCode = user.Company_Code;
                //session.CompanyCode = "CO-3I";
                var permissions = _context.AdPermissions
                                            .Where(x => x.UserId == userId)
                                            .Select(x => new PermissionObject
                                            {
                                                //FunctionId = x.FunctionId,
                                                FunctionCode = x.Function.FunctionCode,
                                                FunctionTitle = x.Function.Title,
                                                //ResourceId = x.ResourceId,
                                                ResourceCode = x.Resource.ResourceCode,
                                                ResourceTitle = x.Resource.Title,
                                                ResourceApi = x.Resource.Api,
                                                GroupUserCode = x.GroupUserCode,
                                                GroupUserTitle = x.GroupUser.Title,
                                                RoleId = x.RoleId,
                                                RoleTitle = x.Role.Title,
                                            });
                session.Permissions = permissions.ToList();
            }

            return session;
        }
    }
}
