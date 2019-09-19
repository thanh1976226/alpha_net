using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class AssetCancelController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;

        public AssetCancelController(EIMDBContext context, IUploadService upload)
        {
            _context = context;
            _upload = upload;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public object JTable([FromBody]JTableModelAct jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            var query = (from a in _context.AssetCancelHeaders
                         where (!a.IsDeleted)
                         select new
                         {
                             a.AssetID,
                             a.Title,
                             a.Branch,
                             a.Quantity,
                             a.Person,
                             a.Note,
                             a.Status,
                             a.CancelTime
                         }).AsParallel();
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "AssetID", "Title", "Branch", "Quantity", "Person", "Note", "Status", "CancelTime");
            return Json(jdata);
        }
        
        public object JTableADD([FromBody]JTableModelAct jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            var query = (from a in _context.AssetCancelDetails
                         
                         select new
                         {
                             a.AssetID,
                             a.Title,
                             a.QuantityAsset,
                             a.Status,
                             a.Note

                         }).AsParallel();
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "AssetID", "Title", "Quantity", "Status", "Note");
            return Json(jdata);
        }

        public class JTableModelAct : JTableModel
        {
            public int AssetID { get; set; }
            public string TicketCode { get; set; }
            public string Title { get; set; }
            public string Branch { get; set; }
            public string Person { get; set; }
            public string Note { get; set; }
            public string Status { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedTime { get; set; }
            public string UpdatedBy { get; set; }
            public string UpdatedTime { get; set; }
            public string DeletedBy { get; set; }
            public string DeletedTime { get; set; }
            public bool IsDeleted { get; set; }
            public string Adress { get; set; }
            public string CancelTime { get; set; }
            public int Quantity { get; set; }
            public int QuantityAsset { get; set; }
            public string AssetName { get; set; }
            public string AssetUnit { get; set; }
            public string Cost { get; set; }
            public string ListImage { get; set; }
        }

        public class AssetCancelHeadersJtableModel
        {
            public int AssetID { get; set; }
            public string TicketCode { get; set; }
            public string Title { get; set; }
            public string Branch { get; set; }
            public string Person { get; set; }
            public string Note { get; set; }
            public int Quantity { get; set; }
            public string Status { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedTime { get; set; }
            public string UpdatedBy { get; set; }
            public string UpdatedTime { get; set; }
            public string DeletedBy { get; set; }
            public string DeletedTime { get; set; }
            public bool IsDeleted { get; set; }
            public string Adress { get; set; }
            public string CancelTime { get; set; }
        }

        public class AssetCancelDetailsJtableModel
        {
            public int AssetID { get; set; }
            public string TicketCode { get; set; }
            public string AssetName { get; set; }
            public string AssetUnit { get; set; }
            public string Status { get; set; }
            public string Title { get; set; }
            public int QuantityAsset { get; set; }
            public string Cost { get; set; }
            public string ListImage { get; set; }
            public string Note { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedTime { get; set; }
            public string UpdatedBy { get; set; }
            public string UpdatedTime { get; set; }
            public string DeletedBy { get; set; }
            public string DeletedTime { get; set; }
            public bool IsDeleted { get; set; }
            public string Adress { get; set; }
            public string CancelTime { get; set; }
        }

        public object GetStatus()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "ASSET_TRANSFER_HEADER").Select(x => new { Code = x.CodeSet, Name = x.ValueSet }).ToList();
            return data;
        }

        //public object GetBranch()
        //{
        //    var data = _context.AdOrganizations.Where(x => x.OrgGroup == 2).Select(x => new { Code = x.OrgParentCode, Name = x.OrgName }).ToList();
        //    return data;
        //}

        public object GetBranch()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "AREA").Select(x => new { Code = x.CodeSet, Name = x.ValueSet }).ToList();
            return data;
        }
        public object GetResponsible()
        {
            var data = _context.HREmployees.Select(x => new { Code = x.employee_code, Name = x.fullname }).ToList();
            return data;
        }

        [HttpPost]
        public JsonResult GenReqCode()
        {
            var monthNow = DateTime.Now.Month;
            var yearNow = DateTime.Now.Year;
            var reqCode = string.Empty;
            var no = 1;
            var noText = "01";
            var data = _context.AssetCancelHeaders.Where(x => x.CreatedTime.Value.Year == yearNow && x.CreatedTime.Value.Month == monthNow).ToList();
            if (data.Count > 0)
            {
                no = data.Count + 1;
                if (no < 10)
                {
                    noText = "0" + no;
                }
                else
                {
                    noText = no.ToString();
                }
            }
            reqCode = string.Format("{0}{1}{2}{3}", "TS_", "T" + monthNow + ".", yearNow + "_", noText);

            return Json(reqCode);
        }
        
        public JsonResult Insert([FromBody]AssetCancelHeadersJtableModel obj)
        {
            var msg = new JMessage { Title = "", Error = false };
            try
            {
                DateTime? CancelTime = !string.IsNullOrEmpty(obj.CancelTime) ? DateTime.ParseExact(obj.CancelTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                var data = _context.AssetCancelHeaders.FirstOrDefault(x => x.TicketCode == obj.TicketCode && x.IsDeleted == false);
                if (data == null)
                {
                    var dt = new AssetCancelHeader();
                    dt.Quantity = obj.Quantity;
                    dt.TicketCode = obj.TicketCode;
                    dt.Title = obj.Title;
                    dt.Branch = obj.Branch;
                    dt.Person = obj.Person;
                    dt.Note = obj.Note;
                    dt.Status = obj.Status;
                    dt.CancelTime = CancelTime;
                    dt.CreatedBy = ESEIM.AppContext.UserName;
                    dt.CreatedTime = DateTime.Now;
                    
                    _context.AssetCancelHeaders.Add(dt);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("SUCCESS")/*, CommonUtil.ResourceValue("DCD_MSG_TITLE_DCD")*/);
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("OK"));/*DCD_MSG_DOCUMENT_NOT*/
                }
            }
            catch (Exception)
            {
                msg.Error = true;
                // msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAILED")/*, CommonUtil.ResourceValue("DCD_MSG_TITLE_DCD")*/);
                msg.Title = "Lỗi";
            }
            return Json(msg);
        }

        [HttpPost]
        public object GetItem(int Id)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            var data = _context.AssetCancelHeaders.FirstOrDefault(x => x.AssetID == Id);
            if (data != null)
            {
                data.sStartTime = data.CancelTime.HasValue ? data.CancelTime.Value.ToString("dd/MM/yyyy") : null;
                msg.Object = data;
            }
            return Json(msg);
        }
        
        [HttpPost]
        public JsonResult Update([FromBody]AssetCancelHeadersJtableModel obj)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var data = _context.AssetCancelHeaders.FirstOrDefault(x => x.AssetID == obj.AssetID);
                data.Title = obj.Title;
                data.Branch = obj.Branch;
                data.Person = obj.Person;
                data.Note = obj.Note;
                data.Status = obj.Status;
                data.CancelTime = !string.IsNullOrEmpty(obj.CancelTime) ? DateTime.ParseExact(obj.CancelTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                data.UpdatedTime = DateTime.Now.Date;
                data.UpdatedBy = ESEIM.AppContext.UserName;

                _context.AssetCancelHeaders.Update(data);
                _context.SaveChanges();
                msg.Title = "Cập nhật phiếu điều chuyển thành công";
            }
            catch (Exception)
            {
                msg.Error = true;
                msg.Title = "Có lỗi khi cập nhật phiếu điều chuyển";
            }

            return Json(msg);
        }
        
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var data = _context.AssetCancelHeaders.FirstOrDefault(x => x.AssetID == Id);
                data.DeletedTime = DateTime.Now.Date;
                data.DeletedBy = ESEIM.AppContext.UserName;
                data.IsDeleted = true;

                _context.AssetCancelHeaders.Update(data);
                _context.SaveChanges();
                msg.Title = "Xóa dự án thành công!";
            }
            catch (Exception)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi xóa!";
            }
            return Json(msg);
        }

        public JsonResult InsertAsset([FromBody]AssetCancelDetailsJtableModel obj)
        {
            var msg = new JMessage { Title = "", Error = false };
            try
            {

                var checkexist = _context.AssetCancelDetails.FirstOrDefault(x => x.TicketCode.Equals(obj.TicketCode) && x.IsDeleted == false);
                if (checkexist == null)
                {
                    msg.Error = true;
                    msg.Title = "lưu trước khi thêm";

                }
                else
                {
                    var data = _context.AssetCancelDetails.FirstOrDefault(x => x.TicketCode.Equals(obj.TicketCode) && x.AssetName.Equals(obj.AssetName) && x.IsDeleted == false);

                    if (data == null)
                    {
                        var dt = new AssetCancelDetail()
                        {
                            AssetName = obj.AssetName,
                            TicketCode = obj.TicketCode,
                            //Title = obj.Title,
                            QuantityAsset = obj.QuantityAsset,
                            Note = obj.Note,
                            CreatedBy = ESEIM.AppContext.UserName,
                            CreatedTime = DateTime.Now
                        };
                        _context.AssetCancelDetails.Add(dt);
                        _context.SaveChanges();
                        msg.Title = "succes";
                    }
                    else
                    {
                        msg.Error = true;
                        msg.Title = "Tài sản đã tồn tại";
                        return Json(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                // msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAILED")/*, CommonUtil.ResourceValue("DCD_MSG_TITLE_DCD")*/);
                msg.Title = "lỗi";
            }
            return Json(msg);
        }

    }
}
