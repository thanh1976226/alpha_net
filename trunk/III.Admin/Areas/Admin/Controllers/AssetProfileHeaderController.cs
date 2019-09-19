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
    public class AssetProfileHeaderController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;

        public AssetProfileHeaderController(EIMDBContext context, IUploadService upload)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }


        public object JTable([FromBody]JTableModelAct jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            var query = (from a in _context.AssetTransferDetails

                         select new
                         {
                             a.AssetID,
                             a.AssetName,
                             a.AssetCode,
                             a.Quantity,
                             a.CostValue,
                             a.CreatedBy,
                             a.CreatedTime,
                             a.Status,
                             a.Note,
                             a.ListImage,
                             a.UpdatedBy,
                             a.UpdatedTime,


                         }).AsParallel();
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "AssetID", "AssetName", "AssetCode", "Quantity", "CostValue", "CreatedBy", "CreatedTime", "Status", "Note", "ListImage", "UpdatedBy", "UpdatedTime");
            return Json(jdata);
        }

        public object JTableTicket([FromBody]JTableModelAct jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = (from a in _context.AssetTransferHeaders
                         join b in _context.AdGroupUsers on a.DepartTransf equals b.GroupUserId.ToString() into b1
                         from b2 in b1.DefaultIfEmpty()
                         join c in _context.HREmployees on a.UserTransf equals c.Id.ToString() into c1
                         from c2 in c1.DefaultIfEmpty()
                         join d in _context.CommonSettings.Where(x => x.Group == "ASSET_TRANSFER_HEADER") on a.Status equals d.CodeSet into d1
                         from d2 in d1.DefaultIfEmpty()
                         where (!a.IsDeleted)

                         //join c in _context.AdGroupUsers on a.DepartReceived equals c.GroupUserId.ToString() into c1
                         //from c2 in c1.DefaultIfEmpty()
                         //join b in listCommon on a.ActType equals b.CodeSet into b1

                         select new
                         {
                             a.AssetID,
                             a.Ticketcode,
                             DepartTransf = b2.Title,
                             DepartReceived = b2.Title,
                             a.StartTime,
                             a.ReceivedTime,
                             a.Ticket,
                             a.QRcode,
                             a.LocationTransf,
                             a.ReceivedLocation,
                             UserTransf = c2.fullname,
                             UserReceived = c2.fullname,
                             Status = d2.ValueSet,
                             a.Note

                         }).AsParallel();
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Ticket", "AssetID", "Ticketcode", "DepartTransf", "DepartReceived", "StartTime", "ReceivedTime", "QRcode", "LocationTransf", "ReceivedLocation", "UserTransf", "UserReceived", "Status", "Note");
            return Json(jdata);
        }

        public object JTableADD([FromBody]JTableModelAct jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;

            var query = (from a in _context.AssetTransferDetails
                         join b in _context.AssetTransferHeaders on a.Ticketcode equals b.Ticketcode into b1
                         from b2 in b1.DefaultIfEmpty()
                             //join c in _context.AdGroupUsers on b.Departtransf equals c.GroupUserId.ToString() into c1 
                             //from c2 in c1.DefaultIfEmpty()

                         select new
                         {
                             a.AssetID,
                             a.Quantity,
                             a.Status,
                             a.Note,
                             a.AssetName,
                             b2.DepartTransf,
                             b2.DepartReceived,
                             a.Ticketcode
                         }).AsParallel();
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "AssetName", "AssetID", "Quantity", "Status", "DepartTransf", "Ticketcode", "DepartReceived");
            return Json(jdata);
        }

        public class JTableModelAct : JTableModel
        {
            public string Ticketcode { get; set; }
            public string LocationTransf { get; set; }
            public string DepartTransf { get; set; }
            public string Note { get; set; }
            public string UserTransf { get; set; }
            public string Ticket { get; set; }
            public string DepartReceived { get; set; }
            public string UserReceived { get; set; }
            public string StartTime { get; set; }
            public string ReceivedLocation { get; set; }
            public int AssetID { get; set; }
            public string ReceivedTime { get; set; }

        }

        public class AssetTransferHeadersJtableModel
        {
            public int AssetID { get; set; }
            public string Ticketcode { get; set; }
            public string LocationTransf { get; set; }
            public string DepartTransf { get; set; }
            public string StartTime { get; set; }
            public string ReceivedTime { get; set; }
            public string UserTransf { get; set; }
            public string UserReceived { get; set; }
            public string DepartReceived { get; set; }
            public string Status { get; set; }
            public string Ticket { get; set; }
            public Boolean IsDeleted { get; set; }
            public string ReceivedLocation { get; set; }
            public string Note { get; set; }
            public string UpdatedBy { get; set; }
            public string QRcode { get; set; }

        }

        //public class AssetTransferDetailsJtableModel
        //{
        //    public int AssetID { get; set; }
        //    public string AssetCode { get; set; }

        //    public string Quantity { get; set; }
        //    public string CostValue { get; set; }
        //    public string ListImage { get; set; }
        //    public string AssetName { get; set; }
        //    public string Status { get; set; }
        //    public string Note { get; set; }
        //    public string CreatedBy { get; set; }
        //    public string CreatedTime { get; set; }
        //    public string UpdatedBy { get; set; }
        //    public string UpdatedTime { get; set; }
        //    public string Ticketcode { get; set; }
        //    public string sStartTime { get; set; }
        //    public string sReceivedTime { get; set; }





        //}


        //combobox
        public object GetDonvichuyen()
        {
            var data = _context.AdGroupUsers.Select(x => new { Code = x.GroupUserId, Name = x.Title }).ToList();
            return data;
        }

        public object GetDonvinhan()
        {
            var data = _context.AdGroupUsers.Select(x => new { Code = x.GroupUserId, Name = x.Title }).ToList();
            return data;
        }

        public object GetStatus()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "ASSET_TRANSFER_HEADER").Select(x => new { Code = x.CodeSet, Name = x.ValueSet }).ToList();
            return data;
        }

        public object GetNguoichuyen()
        {
            var data = _context.HREmployees.Select(x => new { Code = x.Id, Name = x.fullname }).ToList();
            return data;
        }

        public object GetNguoinhan()
        {
            var data = _context.HREmployees.Select(x => new { Code = x.Id, Name = x.fullname }).ToList();
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
            var data = _context.AssetTransferHeaders.Where(x => x.CreatedTime.Value.Year == yearNow && x.CreatedTime.Value.Month == monthNow).ToList();
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


        public JsonResult Insert([FromBody]AssetTransferHeadersJtableModel obj)
        {
            var msg = new JMessage { Title = "", Error = false };
            try
            {
                DateTime? startTime = !string.IsNullOrEmpty(obj.StartTime) ? DateTime.ParseExact(obj.StartTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                DateTime? receivedTime = !string.IsNullOrEmpty(obj.ReceivedTime) ? DateTime.ParseExact(obj.ReceivedTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                var data = _context.AssetTransferHeaders.FirstOrDefault(x => x.Ticketcode == obj.Ticketcode && x.IsDeleted == false);
                if (data == null)
                {
                    var dt = new AssetTransferHeader();
                    dt.Ticketcode = obj.Ticketcode;
                    dt.Ticket = obj.Ticket;
                    dt.DepartTransf = obj.DepartTransf;
                    dt.DepartReceived = obj.DepartReceived;
                    dt.UserTransf = obj.UserTransf;
                    dt.UserReceived = obj.UserReceived;
                    dt.LocationTransf = obj.LocationTransf;
                    dt.ReceivedLocation = obj.ReceivedLocation;
                    dt.Note = obj.Note;
                    dt.StartTime = startTime;
                    dt.ReceivedTime = receivedTime;
                    dt.CreatedBy = ESEIM.AppContext.UserName;
                    dt.CreatedTime = DateTime.Now;
                    dt.Status = obj.Status;
                    // dt.fullname = obj.fullname;
                    _context.AssetTransferHeaders.Add(dt);
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
                msg.Title = "lỗi";
            }
            return Json(msg);
        }


        //[HttpPost]
        //public object GetItem(int Id)
        //{
        //    var msg = new JMessage() { Error = false, Title = "" };
        //    var data = _context.AssetTransferHeaders.FirstOrDefault(x => x.AssetID == Id);
        //    if (data != null)
        //    {
        //        data.sStartTime = data.StartTime.HasValue ? data.StartTime.Value.ToString("dd/MM/yyyy") : null;
        //        data.sReceivedTime = data.ReceivedTime.HasValue ? data.ReceivedTime.Value.ToString("dd/MM/yyyy") : null;
        //        msg.Object = data;
        //    }
        //    return Json(msg);
        //}

        //[HttpPost]
        //public JsonResult Update([FromBody]AssetTransferHeadersJtableModel obj)
        //{
        //    var msg = new JMessage { Error = false, Title = "" };
        //    try
        //    {
        //        var data = _context.AssetTransferHeaders.FirstOrDefault(x => x.AssetID == obj.AssetID);
        //        data.Ticket = obj.Ticket;
        //        data.DepartTransf = obj.DepartTransf;
        //        data.UserTransf = obj.UserTransf;
        //        data.LocationTransf = obj.LocationTransf;
        //        data.StartTime = !string.IsNullOrEmpty(obj.StartTime) ? DateTime.ParseExact(obj.StartTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
        //        data.DepartReceived = obj.DepartReceived;
        //        data.UserReceived = obj.UserReceived;
        //        data.ReceivedTime = !string.IsNullOrEmpty(obj.ReceivedTime) ? DateTime.ParseExact(obj.ReceivedTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
        //        data.ReceivedLocation = obj.ReceivedLocation;
        //        data.Status = obj.Status;
        //        data.Note = obj.Note;
        //        data.UpdatedTime = DateTime.Now.Date;
        //        data.UpdatedBy = ESEIM.AppContext.UserName;

        //        _context.AssetTransferHeaders.Update(data);
        //        _context.SaveChanges();
        //        msg.Title = "Cập nhật phiếu điều chuyển thành công";
        //    }
        //    catch (Exception)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi khi cập nhật phiếu điều chuyển";
        //    }

        //    return Json(msg);
        //}

        //[HttpPost]
        //public JsonResult Delete(int Id)
        //{
        //    var msg = new JMessage { Error = false, Title = "" };
        //    try
        //    {
        //        var data = _context.AssetTransferHeaders.FirstOrDefault(x => x.AssetID == Id);
        //        data.DeletedTime = DateTime.Now.Date;
        //        data.DeletedBy = ESEIM.AppContext.UserName;
        //        data.IsDeleted = true;

        //        _context.AssetTransferHeaders.Update(data);
        //        _context.SaveChanges();
        //        msg.Title = "Xóa dự án thành công!";
        //    }
        //    catch (Exception)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi xảy ra khi xóa!";
        //    }
        //    return Json(msg);
        //}
    }

}