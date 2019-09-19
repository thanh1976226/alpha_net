using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using System.Data;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class CMSDocumentController : BaseController
    {
        public class CMSItemsJtableModel
        {
            public int id { get; set; }
            public string title { get; set; }
            public string alias { get; set; }
            public int? cat_id { get; set; }
            public bool published { get; set; }
            public string intro_text { get; set; }
            public string full_text { get; set; }

        }
        private readonly EIMDBContext _context;
        public CMSDocumentController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class CMSDocumentJTableModel : JTableModel
        {
            public string CurrencyCode { get; set; }
            public string DefaultPayment { get; set; }
            public string Note { get; set; }
        }

        #region combobox

        [HttpPost]
        public object GetUser()
        {
            var query = _context.Users.Where(x => x.Active == true && x.UserType != 10).Select(x => new { x.Id, x.GivenName }).AsNoTracking().ToList();
            return query;
        }

        [HttpPost]
        public object GetCurrency()
        {
            var data = _context.FundCurrencys.Select(x => new { Code = x.CurrencyCode, Name = x.Note }).AsNoTracking().ToList();
            return data;
        }
        #endregion

        #region action

        [HttpPost]
        public object JTable([FromBody]CMSDocumentJTableModel jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.cms_categories
                        //where  (string.IsNullOrEmpty(jTablePara.CurrencyCode) || a.CurrencyCode.ToLower().Contains(jTablePara.CurrencyCode.ToLower()))
                        // && (string.IsNullOrEmpty(jTablePara.DefaultPayment) || (a.DefaultPayment.Equals(Convert.ToBoolean(jTablePara.DefaultPayment))))
                        select new CMSItemsJtableModel
                        {
                            id = a.id,
                            //title = a.title,
                            alias=a.alias

                        };

            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "id", "title", "alias");
            return Json(jdata);
        }

        //[HttpPost]
        //public object GetItem([FromBody] int id)
        //{
        //    var data = _context.FundCurrencys.FirstOrDefault(x => x.Id == id);
        //    return data;
        //}

        //[HttpPost]
        //public JsonResult Insert([FromBody]FundCurrency data)
        //{
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {
        //        var checkExist = _context.FundCurrencys.FirstOrDefault(x => x.CurrencyCode.ToLower() == data.CurrencyCode.ToLower() && !x.IsDeleted);
        //        if (checkExist == null)
        //        {
        //            data.CreatedBy = ESEIM.AppContext.UserName;
        //            data.CreatedTime = DateTime.Now;
        //            if (data.DefaultPayment == true)
        //            {
        //                var listFundCurrencys = _context.FundCurrencys.Where(x => x.DefaultPayment == true).ToList();
        //                listFundCurrencys.ForEach(x => x.DefaultPayment = false);
        //                foreach (var item in listFundCurrencys)
        //                {
        //                    _context.FundCurrencys.Update(item);
        //                }
        //                _context.SaveChanges();
        //            }
        //            _context.FundCurrencys.Add(data);
        //            _context.SaveChanges();

        //            msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_ADD_DONE"));//"Thêm loại tiền thành công";
        //        }
        //        else
        //        {
        //            msg.Error = true;
        //            msg.Title = "Loại tiền đã tồn tại";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_ADD_ERROR"));//"Có lỗi xảy ra khi thêm?";
        //    }
        //    return Json(msg);
        //}

        //[HttpPost]
        //public object Update([FromBody]FundCurrency data)
        //{
        //    var msg = new JMessage() { Error = false };

        //    try
        //    {
        //        var obj = _context.FundCurrencys.FirstOrDefault(x => x.Id == data.Id);
        //        if (obj != null)
        //        {
        //            obj.DefaultPayment = data.DefaultPayment;

        //            if (data.DefaultPayment == true)
        //            {
        //                var listFundCurrencys = _context.FundCurrencys.Where(x => x.DefaultPayment == true).ToList();
        //                listFundCurrencys.ForEach(x => x.DefaultPayment = false);
        //                foreach (var item in listFundCurrencys)
        //                {
        //                    _context.FundCurrencys.Update(item);
        //                }
        //                _context.SaveChanges();
        //            }
        //            _context.FundCurrencys.Update(obj);
        //            _context.SaveChanges();
        //            msg.Title = "Cập nhật thành công";
        //        }
        //        else
        //        {
        //            msg.Error = true;
        //            msg.Title = "Loại tiền không tồn tại !";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_UPDATE_ERROR"));//"Có lỗi xảy ra khi cập nhật!";
        //    }
        //    return Json(msg);
        //}

        //[HttpPost]
        //public object Delete(int id)
        //{
        //    var msg = new JMessage { Error = true };
        //    try
        //    {
        //        var data = _context.FundCurrencys.FirstOrDefault(x => x.Id == id);
        //        data.DeletedBy = ESEIM.AppContext.UserName;
        //        data.DeletedTime = DateTime.Now;
        //        data.IsDeleted = true;
        //        if (data.DefaultPayment == true)
        //        {
        //            var check = _context.FundCurrencys.FirstOrDefault(x => x.DefaultPayment == false);
        //            check.DefaultPayment = true;
        //            _context.FundCurrencys.Update(check);
        //        }
        //        _context.FundCurrencys.Update(data);

        //        _context.SaveChanges();

        //        msg.Error = false;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_DELETE_DONE"));//"Xóa thành công";

        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_DELETE_ERROR"));//"Xóa thất bại";
        //        return Json(msg);
        //    }
        //}
        //[HttpPost]
        //public object SetDefaultPayment(int id)
        //{
        //    var msg = new JMessage { Error = false };
        //    try
        //    {
        //        var data = _context.FundCurrencys.FirstOrDefault(x => x.Id == id);
        //        data.UpdatedBy = ESEIM.AppContext.UserName;
        //        data.UpdatedTime = DateTime.Now;
        //        data.DefaultPayment = true;

        //        var listFundCurrencys = _context.FundCurrencys.Where(x => x.DefaultPayment == true).ToList();
        //        listFundCurrencys.ForEach(x => x.DefaultPayment = false);
        //        foreach (var item in listFundCurrencys)
        //        {
        //            _context.FundCurrencys.Update(item);
        //        }
        //        _context.SaveChanges();
        //        msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_UPDATE_SUCCES"));//"Cập nhật thành công";
        //        return Json(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_UPDATE_ERROR")); //"Cập nhật thất bại";
        //        return Json(msg);
        //    }

        //}
        #endregion
    }
}