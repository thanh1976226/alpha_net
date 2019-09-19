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
    public class FundCurrencyController : BaseController
    {
        public class FundCurrencysJtableModel
        {
            public int Id { get; set; }
            public string CurrencyCode { get; set; }
            public bool? DefaultPayment { get; set; }
            public string Note { get; set; }
        }
        private readonly EIMDBContext _context;
        public FundCurrencyController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelFc : JTableModel
        {
            public string CurrencyCode { get; set; }
            public bool? DefaultPayment { get; set; }
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
        public object JTable([FromBody]JTableModelFc jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.FundCurrencys
                        where a.IsDeleted == false
                        && (string.IsNullOrEmpty(jTablePara.CurrencyCode) || a.CurrencyCode.ToLower().Contains(jTablePara.CurrencyCode.ToLower()))

                        select new FundCurrencysJtableModel
                        {
                            Id = a.Id,
                            CurrencyCode = a.CurrencyCode,
                            DefaultPayment = a.DefaultPayment,
                            Note = a.Note
                        };

            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "CurrencyCode", "DefaultPayment", "Note");
            return Json(jdata);
        }

        [HttpPost]
        public object GetItem([FromBody] int id)
        {
            var data = _context.FundCurrencys.FirstOrDefault(x => x.Id == id);
            return data;
        }

        [HttpPost]
        public JsonResult Insert([FromBody]FundCurrency data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var checkExist = _context.FundCurrencys.FirstOrDefault(x => x.CurrencyCode.ToLower() == data.CurrencyCode.ToLower() && x.IsDeleted == true);
                var checkExist2 = _context.FundCurrencys.FirstOrDefault(x => x.CurrencyCode.ToLower() == data.CurrencyCode.ToLower() && x.IsDeleted == false);
                if (checkExist != null)
                {
                    checkExist.IsDeleted = false;
                    checkExist.DefaultPayment = data.DefaultPayment;
                    checkExist.Note = data.Note;
                    _context.FundCurrencys.Update(checkExist);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_ADD_DONE"));//"Thêm loại tiền thành công";
                }
                else if (checkExist2 != null)
                {
                    msg.Error = true;
                    msg.Title = "Loại tiền đã tồn tại";
                }
                else
                {
                    data.CreatedBy = ESEIM.AppContext.UserName;
                    data.CreatedTime = DateTime.Now;
                    if (data.DefaultPayment == true)
                    {
                        var check = _context.FundCurrencys.FirstOrDefault(x => x.DefaultPayment == true);
                        check.DefaultPayment = false;
                        _context.FundCurrencys.Update(check);
                    }
                    _context.FundCurrencys.Add(data);
                    _context.SaveChanges();

                    msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_ADD_DONE"));//"Thêm loại tiền thành công";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_ADD_ERROR"));//"Có lỗi xảy ra khi thêm?";
            }
            return Json(msg);
        }

        [HttpPost]
        public object Update([FromBody]FundCurrency data)
        {
            var msg = new JMessage() { Error = false };

            try
            {
                var obj = _context.FundCurrencys.FirstOrDefault(x => x.CurrencyCode.ToLower() == data.CurrencyCode.ToLower() && x.Id != data.Id && x.IsDeleted == false);
                var obj1 = _context.FundCurrencys.FirstOrDefault(x => x.CurrencyCode.ToLower() == data.CurrencyCode.ToLower() && x.Id != data.Id && x.IsDeleted == true);
                var obj2 = _context.FundCurrencys.FirstOrDefault(x => x.Id != data.Id);

                if (obj != null)
                {
                    msg.Error = true;
                    msg.Title = "Đã tồn tại loại tiền !";
                }
                else if(obj1 != null)
                {
                    var query = _context.FundCurrencys.ToList();
                    foreach (var item in query)
                    {
                        if(item.Id == data.Id)
                        {
                            item.IsDeleted = true;
                            item.DefaultPayment = false;
                            _context.FundCurrencys.Update(item);
                        }
                        else if(item.CurrencyCode.ToLower() == data.CurrencyCode.ToLower())
                        {
                            item.IsDeleted = false;
                            item.DefaultPayment = data.DefaultPayment;
                            _context.FundCurrencys.Update(item);
                        }
                        
                    }
                    _context.SaveChanges();
                    msg.Title = "Cập nhật thành công";
                }
                else
                {
                    var listObj = _context.FundCurrencys.Where(x => !x.IsDeleted).ToList();
                    foreach (var item in listObj)
                    {


                        if (item.CurrencyCode.ToLower() == data.CurrencyCode.ToLower())
                        {
                            if (data.DefaultPayment == true)
                            {
                                item.CurrencyCode = data.CurrencyCode;
                                item.DefaultPayment = true;
                                item.Note = data.Note;
                                item.UpdatedBy = ESEIM.AppContext.UserName;
                                item.UpdatedTime = DateTime.Now;
                                _context.FundCurrencys.Update(item);
                            }
                            else
                            {
                                item.CurrencyCode = data.CurrencyCode;
                                item.DefaultPayment = false;
                                item.Note = data.Note;
                                item.UpdatedBy = ESEIM.AppContext.UserName;
                                item.UpdatedTime = DateTime.Now;
                                _context.FundCurrencys.Update(item);

                            }


                        }
                        else
                        {
                            if (data.DefaultPayment == true)
                            {
                                item.DefaultPayment = false;
                                _context.FundCurrencys.Update(item);
                            }
                            else
                            {
                                var query1 = _context.FundCurrencys.FirstOrDefault(x => x.IsDeleted == false && x.CurrencyCode.ToLower() != data.CurrencyCode.ToLower());
                                query1.DefaultPayment = true;
                                _context.Update(query1);
                            }
                         }                        

                    }
                    
                    _context.SaveChanges();
                    msg.Title = "Cập nhật thành công";




                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_UPDATE_ERROR"));//"Có lỗi xảy ra khi cập nhật!";
            }
            return Json(msg);
        }

        [HttpPost]
        public object Delete(int id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.FundCurrencys.FirstOrDefault(x => x.Id == id);
                data.DeletedBy = ESEIM.AppContext.UserName;
                data.DeletedTime = DateTime.Now;
                data.IsDeleted = true;
                if (data.DefaultPayment == true)
                {
                    var check = _context.FundCurrencys.FirstOrDefault(x => x.DefaultPayment == false);
                    check.DefaultPayment = true;
                    _context.FundCurrencys.Update(check);
                }
                _context.FundCurrencys.Update(data);

                _context.SaveChanges();

                msg.Error = false;
                msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_DELETE_DONE"));//"Xóa thành công";

                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_DELETE_ERROR"));//"Xóa thất bại";
                return Json(msg);
            }
        }
        [HttpPost]
        public object SetDefaultPayment(int id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.FundCurrencys.FirstOrDefault(x => x.Id == id);
                data.UpdatedBy = ESEIM.AppContext.UserName;
                data.UpdatedTime = DateTime.Now;
                data.DefaultPayment = true;
                _context.FundCurrencys.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_UPDATE_SUCCES"));//"Cập nhật thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_UPDATE_ERROR")); //"Cập nhật thất bại";
                return Json(msg);
            }
        }
        public object UnSetDefaultPayment(int id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.FundCurrencys.Where(x => x.Id != id && x.DefaultPayment == true).FirstOrDefault();
                data.UpdatedBy = ESEIM.AppContext.UserName;
                data.UpdatedTime = DateTime.Now;
                data.DefaultPayment = false;
                _context.FundCurrencys.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FCC_MSG_UPDATE_ERROR"));//"Cập nhật thất bại";
                return Json(msg);
            }
        }
        #endregion
    }
}