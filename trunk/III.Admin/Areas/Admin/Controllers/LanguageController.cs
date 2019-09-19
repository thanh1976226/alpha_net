using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using III.Admin.Controllers;
using ESEIM.Controllers;

namespace III.Admin.Controllers
{
    public class JTableLanguageModelCustom : JTableModel
    {
        public string Culture { set; get; }
        public string Name { set; get; }
    }


    [Area("Admin")]
    public class LanguageController : BaseController
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly UserManager<AspNetUser> _roleManager;
        private readonly EIMDBContext _context;
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;

        public LanguageController(EIMDBContext context, UserManager<AspNetUser> userManager, UserManager<AspNetUser> roleManager, ILanguageService languageService, ILogger<LanguageController> logger)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _languageService = languageService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Caption()
        {
            return View("Index");
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, VaryByQueryKeys = new[] { "lang" })]
        public IActionResult SetCulture(string lang)
        {
            var language = _context.AdLanguages.AsNoTracking().FirstOrDefault(x => x.IsDeleted == false && x.IsEnabled == true && x.Culture.Equals(lang));
            if (language != null)
            {
                var cookieKey = "_CULTURE";

                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(15);

                Response.Cookies.Append(cookieKey, language.Culture, options);

                _languageService.SetLanguage(language);
                _languageService.SetResource(lang);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 2 * 60 * 60, VaryByQueryKeys = new[] { "lang" })]
        public IActionResult Translation(string lang)
        {
            var resourceObject = _languageService.GetResource();
            return Ok(resourceObject);
        }
        [HttpGet]
        public object getAll()
        {
            var data = _context.AdLanguages.AsNoTracking().ToList();
            return Json(data);
        }

        [HttpPost]
        public object JTable([FromBody]JTableModelCustom jTablePara)
        {
            ////_logger.LogInformation(LoggingEvents.LogDb, "Get list language ");
            var count = _context.AdLanguages.Count();
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var data = _context.AdLanguages
                                .Skip(intBeginFor).Take(jTablePara.Length)
                                .AsNoTracking()
                                .Select(x => new { x.LanguageId, x.Culture, DisplayName = string.Join("", x.DisplayName, Convert.ToBoolean(x.IsDefault) ? " (default)" : ""), x.Icon, x.IsDefault, x.IsEnabled, x.CreatedDate })
                                .OrderUsingSortExpression(jTablePara.QueryOrderBy)
                                .ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "Culture", "DisplayName", "Icon", "IsDefault", "IsEnabled", "CreatedDate");
            return Json(jdata);
        }
        private AdLanguageText SetAdLanguageText(AdLanguageText obj)
        {
            AdLanguageText obj2 = new AdLanguageText();
            obj2.Caption = obj.Caption;
            obj2.Value = "";

            obj2.LanguageId = obj.LanguageId;
            obj2.CreatedBy = obj.CreatedBy;
            obj2.CreatedDate = obj.CreatedDate;
            obj2.UpdatedBy = obj.UpdatedBy;
            obj2.UpdatedDate = obj.UpdatedDate;
            obj2.DeletedBy = obj.DeletedBy;
            obj2.DeletedDate = obj.DeletedDate;
            obj2.IsDeleted = obj.IsDeleted;
            return obj2;
        }
        //FromBody
        [HttpPost]
        public JsonResult Insert([FromBody]AdLanguage obj)
        {
            //_logger.LogInformation(LoggingEvents.LogDb, "Insert language ");
            var msg = new JMessage() { Error = false };
            try
            {
                var caption = _context.AdLanguages.FirstOrDefault(x => x.Culture == obj.Culture);
                if (caption == null)
                {
                    obj.CreatedDate = DateTime.Now;
                    if (obj.IsDefault == true)
                    {
                        foreach (var item in _context.AdLanguages.Where(w => w.IsDefault == true))
                        {
                            item.IsDefault = false;
                            _context.AdLanguages.Update(item);
                            _context.SaveChanges();
                        }
                    }
                    _context.AdLanguages.Add(obj);
                    var a = _context.SaveChanges();
                    //Insert Caption for language new
                    var Language = _context.AdLanguages.Where(p => p.Culture.Equals(obj.Culture)).AsNoTracking().SingleOrDefault();
                    foreach (var item in _context.AdLanguageTexts.Where(w => w.LanguageId == 1))
                    {
                        var itemLanguageNew = SetAdLanguageText(item);
                        itemLanguageNew.LanguageId = Language.LanguageId;
                        _context.AdLanguageTexts.Add(itemLanguageNew);
                    }
                    _context.SaveChanges();
                    //msg.Title = "Add item successfully";
                    msg.Title = String.Format(CommonUtil.ResourceValue("MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("LANGUAGE"));
                    //_logger.LogInformation(LoggingEvents.LogDb, "Insert language successfully");
                }
                else
                {
                    msg.Error = true;
                    //msg.Title = "Item is exists!";
                    msg.Title = String.Format(CommonUtil.ResourceValue("ERR_EXIST"), CommonUtil.ResourceValue("LANGUAGE"));
                    //_logger.LogError(LoggingEvents.LogDb, "Insert language fail");
                    return Json(msg);
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                //msg.Title = "Add item failed";
                msg.Title = String.Format(CommonUtil.ResourceValue("MSG_ADD_FAIL"), CommonUtil.ResourceValue("LANGUAGE"));
                //_logger.LogError(LoggingEvents.LogDb, "Insert language fail");
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult Update([FromBody]AdLanguage obj)
        {
            //_logger.LogInformation(LoggingEvents.LogDb, "Update language");
            var msg = new JMessage() { Error = false };
            try
            {
                var caption = _context.AdLanguages.FirstOrDefault(x => x.Culture == obj.Culture && x.LanguageId != obj.LanguageId);
                if (caption == null)
                {
                    if (obj.IsDefault == true)
                    {
                        foreach (var item in _context.AdLanguages.Where(w => w.IsDefault == true))
                        {
                            item.IsDefault = false;
                        }
                    }
                    _context.AdLanguages.Update(obj);
                    _context.Entry(obj).State = EntityState.Modified;
                    var a = _context.SaveChanges();
                    //msg.Title = "Update item successfully!";
                    msg.Title = String.Format(CommonUtil.ResourceValue("MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("LANGUAGE"));
                    //_logger.LogInformation(LoggingEvents.LogDb, "Update language successfully");
                }
                else
                {
                    msg.Error = true;
                    //msg.Title = "Item is exists!";
                    msg.Title = String.Format(CommonUtil.ResourceValue("ERR_EXIST"), CommonUtil.ResourceValue("LANGUAGE"));
                    //_logger.LogError(LoggingEvents.LogDb, "Update language fail");
                    return Json(msg);
                }

            }
            catch (Exception)
            {
                msg.Error = true;
                //msg.Title = "Update item fail!";
                msg.Title = String.Format(CommonUtil.ResourceValue("MSG_UPDATE_FAIL"), CommonUtil.ResourceValue("LANGUAGE"));
                //_logger.LogError(LoggingEvents.LogDb, "Update language fail");
            }
            return Json(msg);
        }
        [HttpPost]
        public object Delete(int id)
        {
            //_logger.LogInformation(LoggingEvents.LogDb, "Delete language");
            try
            {
                AdLanguage obj = new AdLanguage();
                obj.LanguageId = id;
                _context.AdLanguages.Attach(obj);
                _context.AdLanguages.Remove(obj);
                _context.SaveChanges();
                //return Json(new JMessage() { Error = false, Title = "Delete item successfully!" });
                //_logger.LogInformation(LoggingEvents.LogDb, "Delete language successfully");
                return Json(new JMessage() { Error = false, Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("LANGUAGE")) });
            }
            catch (Exception ex)
            {
                //_logger.LogError(LoggingEvents.LogDb, "Delete language fail");
                //return Json(new JMessage() { Error = true, Title = "Delete item fail!" });
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_FAIL"), CommonUtil.ResourceValue("LANGUAGE")) });
            }
        }
        [HttpPost]
        public object DeleteItems([FromBody]List<int> id)
        {
            //_logger.LogInformation(LoggingEvents.LogDb, "Delete list language");
            try
            {
                for (int i = 0; i < id.Count; i++)
                {
                    AdLanguage obj = new AdLanguage();
                    obj.LanguageId = id[i];
                    _context.AdLanguages.Attach(obj);
                    _context.AdLanguages.Remove(obj);
                    _context.SaveChanges();
                }
                //_logger.LogInformation(LoggingEvents.LogDb, "Delete list language successfully");
                //return Json(new JMessage() { Error = false, Title = "Delete items successfully!" });
                return Json(new JMessage() { Error = false, Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_LIST_SUCCESS"), CommonUtil.ResourceValue("LANGUAGE")) });
            }
            catch (Exception ex)
            {
                //_logger.LogError(LoggingEvents.LogDb, "Delete list language fail");
                //return Json(new JMessage() { Error = true, Title = "Delete items fail!" });
                return Json(new JMessage() { Error = false, Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_LIST_FAIL"), CommonUtil.ResourceValue("LANGUAGE")) });
            }
        }
        [HttpPost]
        public JsonResult GetItem(int? id)
        {

            if (id == null || id < 0)
            {
                return Json("");
            }
            var a = _context.AdLanguages.AsNoTracking().Single(m => m.LanguageId == id);
            return Json(a);
        }
    }
}


