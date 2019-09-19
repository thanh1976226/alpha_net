using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class AccountLoginController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AccountLoginController(EIMDBContext context, UserManager<AspNetUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody]AccountLoginModel obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            var us = await _context.Users.FirstOrDefaultAsync(x => x.Id == obj.Id);
            if (us != null)
            {
                var checkPassword = await _userManager.CheckPasswordAsync(us, obj.PasswordOld);
                if (checkPassword)
                {
                    string code = await _userManager.GeneratePasswordResetTokenAsync(us);
                    var result = await _userManager.ResetPasswordAsync(us, code, obj.PasswordNew);
                    if (result.Succeeded)
                    {
                        var a = await _context.SaveChangesAsync();
                        msg.Title = "Cập nhập mật khẩu thành công";
                    }
                    else
                    {
                        msg.Error = true;
                        msg.Title = "Có lỗi khi cập nhập mật khẩu";
                    }
                }
                else
                {
                    msg.Error = true;
                    msg.Title = "Mật khẩu cũ nhập không đúng";
                }
            }
            else
            {
                msg.Error = true;
                msg.Title = "Tài khoản không tồn tại";
            }
            return Json(msg);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeImage(AccountLoginModel obj, IFormFile image)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var us = await _context.Users.FirstOrDefaultAsync(x => x.Id == obj.Id);
                if (us != null)
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
                        us.Picture = url;
                        var result = await _context.SaveChangesAsync();
                        msg.Title = "Cập nhập ảnh đại diện thành công";
                    }
                }
                else
                {
                    msg.Title = "Tài khoản không tồn tại";
                    msg.Error = true;
                }
            }
            catch (Exception ex)
            {
                msg.Title = "Cập nhập ảnh đại diện lỗi";
                msg.Error = true;
            }
            return Json(msg);
        }

        [HttpPost]
        public async Task<object> GetItem()
        {
            try
            {
                var Id = ESEIM.AppContext.UserId;
                var user = await _context.Users.SingleAsync(x => x.Id == Id);
                var temp = new
                {
                    user.Id,
                    user.UserName,
                    user.GivenName,
                    user.Picture,
                };
                return Json(temp);
            }
            catch (Exception ex)
            {
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
            }
        }
    }
    public class AccountLoginModel
    {
        public string Id { get; set; }
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
        public string InputPasswordNew { get; set; }
    }
}