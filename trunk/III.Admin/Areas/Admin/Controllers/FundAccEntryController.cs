using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using ESEIM;
using III.Domain.Enums;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class FundAccEntryController : Controller
    {
        private readonly EIMDBContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IFCMPushNotification _notification;


        public FundAccEntryController(EIMDBContext context, IHostingEnvironment hostingEnvironment, IFCMPushNotification notification)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _notification = notification;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Action 

        [HttpPost]
        public object JTable([FromBody]JTableModelAct jTablePara)
        {
            var fromDate = !string.IsNullOrEmpty(jTablePara.FromTime) ? DateTime.ParseExact(jTablePara.FromTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toDate = !string.IsNullOrEmpty(jTablePara.ToTime) ? DateTime.ParseExact(jTablePara.ToTime, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            string Currency = jTablePara.Currency;

            var query = from a in _context.FundAccEntrys
                        join b in _context.FundCatReptExpss.Where(x => x.IsDeleted == false)
                        on a.CatCode equals b.CatCode
                        where (!a.IsDeleted
                                  && ((fromDate == null) || (a.DeadLine.HasValue && a.DeadLine.Value.Date >= fromDate))
                                  && ((toDate == null) || (a.DeadLine.HasValue && a.DeadLine.Value.Date <= toDate)))
                                   && (string.IsNullOrEmpty(jTablePara.AetType) || (a.AetType.Equals(jTablePara.AetType)))
                                    && (string.IsNullOrEmpty(jTablePara.Status) || (a.Status.Equals(jTablePara.Status)))
                                    && (string.IsNullOrEmpty(jTablePara.IsPlan) || (a.IsPlan.Equals(Convert.ToBoolean(jTablePara.IsPlan))))
                               orderby a.CreatedTime
                        select new
                        {
                            CatName = b.CatName,
                            Id = a.Id,
                            AetCode = a.AetCode,
                            Title = a.Title,
                            AetType = a.AetType,
                            AetRelativeType = a.AetRelativeType,
                            AetDescription = a.AetDescription,
                            Total = a.Total,
                            Payer = a.Payer,
                            Currency = a.Currency,
                            Status = a.Status,
                            Receiptter = a.Receiptter,
                            DeadLine = a.DeadLine


                        };
            int count = query.Count();
            var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "AetCode", "Title", "AetType", "AetDescription", "Total", "Payer", "Receiptter", "Currency", "IsPlan", "Status", "CatName", "DeadLine");

            return Json(jdata);
        }

        [HttpPost]
        public (decimal totalReceipt, decimal totalExpense) Total(string fromDatePara, string toDatePara,string aetType, string status, string isPlan)
        {
            var fromDate = !string.IsNullOrEmpty(fromDatePara) ? DateTime.ParseExact(fromDatePara, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toDate = !string.IsNullOrEmpty(toDatePara) ? DateTime.ParseExact(toDatePara, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var query = from a in _context.FundAccEntrys
                        join b in _context.FundCatReptExpss.Where(x => x.IsDeleted == false)
                        on a.CatCode equals b.CatCode
                        where (!a.IsDeleted
                                  && ((fromDate == null) || (a.DeadLine.HasValue && a.DeadLine.Value.Date >= fromDate))
                                  && ((toDate == null) || (a.DeadLine.HasValue && a.DeadLine.Value.Date <= toDate)))
                                   && (string.IsNullOrEmpty(aetType) || (a.AetType.Equals(aetType)))
                                    && (string.IsNullOrEmpty(status) || (a.Status.Equals(status)))
                                    && (string.IsNullOrEmpty(isPlan) || (a.IsPlan.Equals(Convert.ToBoolean(isPlan))))
                        orderby a.CreatedTime
                        select new
                        {
                            CatName = b.CatName,
                            Id = a.Id,
                            AetCode = a.AetCode,
                            Title = a.Title,
                            AetType = a.AetType,
                            AetRelativeType = a.AetRelativeType,
                            AetDescription = a.AetDescription,
                            Total = a.Total,
                            Payer = a.Payer,
                            Currency = a.Currency,
                            Status = a.Status,
                            Receiptter = a.Receiptter

                        };
            decimal totalReceipt = query.Where(x => x.AetType == "Receipt").Sum(x => x.Total);
            decimal totalExpense = query.Where(x => x.AetType == "Expense").Sum(x => x.Total);
            return (totalReceipt, totalExpense);
            //int count = query.Count();
            //var data = query.AsQueryable().OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBegin).Take(jTablePara.Length);
            //var jdata = JTableHelper.JObjectTable(data.ToList(), jTablePara.Draw, count, "Id", "AetCode", "Title", "AetType", "AetDescription", "Total", "Payer", "Receiptter", "Currency", "IsPlan", "Status", "CatName");

            //return Json(jdata);
        }
        [HttpPost]
        public JsonResult Insert([FromBody]FundAccEntryModel data)
        {
            var msg = new JMessage() { Error = false };


            try
            {
                var checkExist = _context.FundAccEntrys.FirstOrDefault(x => x.AetCode.ToLower() == data.AetCode.ToLower());
                if (checkExist != null)
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("FAE_MSG_FAE"));//"Đã tồn tại phiếu thu chi !";
                }
                else
                {
                    var isplan = true;
                    var deadLine = !string.IsNullOrEmpty(data.DeadLine) ? DateTime.ParseExact(data.DeadLine, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                    if (deadLine <= DateTime.Now)
                    {
                        isplan = false;
                    }

                    var obj = new FundAccEntry
                    {
                        AetCode = data.AetCode,
                        AetDescription = data.AetDescription,
                        AetRelative = data.AetRelative,
                        AetRelativeType = data.AetRelativeType,
                        AetType = data.AetType,
                        CatCode = data.CatCode,
                        CreatedTime = DateTime.Now,
                        Currency = data.Currency,
                        DeadLine = deadLine,
                        IsDeleted = false,
                        Payer = data.Payer,
                        Receiptter = data.Receiptter,
                        Title = data.Title,
                        Total = data.Total,
                        Status = "CREATED",
                        GoogleMap = data.GoogleMap,
                        Address = data.Address,
                        IsPlan = isplan,

                    };
                    _context.FundAccEntrys.Add(obj);
                    _context.SaveChanges();
                    var entryTracking = new FundAccEntryTracking
                    {
                        AetCode = obj.AetCode,
                        CreatedBy = User.Identity.Name,
                        CreatedTime = DateTime.Now,
                        Action = obj.Status,
                        Note = obj.AetDescription
                    };
                    _context.FundAccEntryTrackings.Add(entryTracking);
                    _context.SaveChanges();

                    if (data.ListFileAccEntry.Count > 0)
                    {
                        foreach (var item in data.ListFileAccEntry)
                        {
                            var fundFile = new FundFiles
                            {
                                AetCode = obj.AetCode,
                                FileName = item.FileName,
                                FilePath = item.FilePath,
                                FileType = item.FileType,
                                CreatedBy = User.Identity.Name,
                                CreatedTime = DateTime.Now,
                            };

                            _context.FundFiless.Add(fundFile);
                        }

                        _context.SaveChanges();

                    }

                    msg.Title = String.Format(CommonUtil.ResourceValue("FAE_MSG_ADD_FAE_SUCCESS"));//"Thêm phiếu thu chi thành công !";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FAE_MSG_ADD_FAE_ERROR"));//"Có lỗi xảy ra khi thêm !";
            }
            return Json(msg);
        }

        [HttpPost]
        public object Update([FromBody]FundAccEntryModel data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var userName = User.Identity.Name;

                var checkRole = _context.Users.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());
                if (checkRole != null)
                {
                    var type = checkRole.UserType;
                    var role = string.Empty;
                    var userInGroup = _context.AdUserInGroups.FirstOrDefault(x => x.UserId == checkRole.Id && x.IsMain == true);
                    var obj = _context.FundAccEntrys.FirstOrDefault(x => x.AetCode.ToLower() == data.AetCode.ToLower());
                    if (userInGroup != null)
                    {
                        var roleId = userInGroup.RoleId;
                        var userRole = _context.Roles.FirstOrDefault(x => x.Id == roleId);
                        role = userRole.Code;
                    }
                    if (type == 10 || role == "001")
                    {
                        msg.Error = true;
                        msg.Title = String.Format(CommonUtil.ResourceValue("FAE_MSG_NO_RIGHT_EDIT"));//"Bạn không có quyền hạn để sửa phiếu thu / chi !";

                    }
                    else if (obj.Status == "APPROVED" || obj.Status == "REFUSE")
                    {
                        msg.Error = true;
                        msg.Title = String.Format(CommonUtil.ResourceValue("FAE_MSG_NOT_EDIT_FAE_APPROVAL"));//"Không được sửa phiếu thu chi đã được xét duyệt !";
                    }
                    else
                    {
                        if (obj != null)
                        {
                            var deadLine = !string.IsNullOrEmpty(data.DeadLine) ? DateTime.ParseExact(data.DeadLine, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                            if (deadLine <= DateTime.Now)
                            {
                                obj.IsPlan = false;
                            }
                            else
                            {
                                obj.IsPlan = true;
                            }
                            obj.AetCode = data.AetCode;
                            obj.AetDescription = data.AetDescription;
                            obj.AetRelative = data.AetRelative;
                            obj.AetRelativeType = data.AetRelativeType;
                            obj.AetType = data.AetType;
                            obj.CatCode = data.CatCode;
                            obj.UpdatedBy = User.Identity.Name;
                            obj.UpdatedTime = DateTime.Now;

                            obj.Payer = data.Payer;
                            obj.Receiptter = data.Receiptter;
                            obj.Title = data.Title;
                            obj.Total = data.Total;
                            obj.DeadLine = deadLine;
                            obj.Status = data.Status;
                            obj.GoogleMap = data.GoogleMap;
                            obj.Address = data.Address;

                            _context.FundAccEntrys.Update(obj);
                            _context.SaveChanges();

                            var updateTracking = false;
                            var objTracking = _context.FundAccEntryTrackings.LastOrDefault(x => x.AetCode.Equals(obj.AetCode));
                            if (objTracking != null)
                            {
                                if (objTracking.Action != obj.Status)
                                    updateTracking = true;
                            }

                            if (updateTracking)
                            {
                                var entryTracking = new FundAccEntryTracking
                                {
                                    AetCode = obj.AetCode,
                                    CreatedBy = User.Identity.Name,
                                    CreatedTime = DateTime.Now,
                                    Action = obj.Status,
                                    Note = obj.AetDescription
                                };

                                _context.FundAccEntryTrackings.Add(entryTracking);
                                _context.SaveChanges();
                            }

                            if (data.ListFileAccEntry.Count > 0)
                            {
                                foreach (var item in data.ListFileAccEntry)
                                {
                                    var fundFileObj = _context.FundFiless.FirstOrDefault(x => x.Id == item.Id);
                                    if (fundFileObj == null)
                                    {
                                        var fundFile = new FundFiles
                                        {
                                            AetCode = obj.AetCode,
                                            FileName = item.FileName,
                                            FilePath = item.FilePath,
                                            FileType = item.FileType,
                                            CreatedBy = User.Identity.Name,
                                            CreatedTime = DateTime.Now,
                                        };

                                        _context.FundFiless.Add(fundFile);
                                    }
                                    _context.SaveChanges();
                                }
                            }

                            if (data.ListFileAccEntryRemove.Count > 0)
                            {
                                foreach (var item in data.ListFileAccEntryRemove)
                                {
                                    var funFileRemove = _context.FundFiless.FirstOrDefault(x => x.Id == item.Id);
                                    if (funFileRemove != null)
                                        _context.FundFiless.Remove(funFileRemove);
                                }

                                _context.SaveChanges();
                            }
                            else
                            {
                                msg.Error = true;
                                msg.Title = String.Format(CommonUtil.ResourceValue("FAE_MSG_UPDATE_ERROR"));//"Cập nhật phiếu thu chi thất bại !";
                            }

                            msg.Object = obj;
                            msg.Title = String.Format(CommonUtil.ResourceValue("FAE_MSG_UPDATE_SUCCESS"));//"Cập nhật phiếu thu chi thành công !";


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FAE_MSG_UPDATE_FAIL"));//"Có lỗi xảy ra khi cập nhật!";
            }
            return Json(msg);
        }

        [HttpPost]
        public object Delete(int id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.FundAccEntrys.FirstOrDefault(x => x.Id == id);
                data.DeletedBy = ESEIM.AppContext.UserName;
                data.DeletedTime = DateTime.Now;
                data.IsDeleted = true;
                _context.FundAccEntrys.Update(data);
                _context.SaveChanges();

                msg.Error = false;
                msg.Title = String.Format(CommonUtil.ResourceValue("FAE_MSG_DELETE_SUCCESS"));//"Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("FAE_MSG_DELETE_ERROR"));//"Xóa thất bại";
                return Json(msg);
            }
        }

        #endregion

        #region Combobox 

        [HttpPost]
        public object GetCurrency()
        {
            var data = _context.FundAccEntrys.Select(x => new { Currency = x.Currency }).AsNoTracking().ToList();
            return data;
        }

        [HttpPost]
        public object GetCatName()
        {
            var data = _context.FundCatReptExpss.Where(x => x.IsDeleted == false).Select(x => new { CatName = x.CatName, CatCode = x.CatCode }).AsNoTracking().ToList();
            //var data = _context.FundCatReptExpss.Where(x => x.IsDeleted == false).Select(x => new { CatCode = x.CatCode }).AsNoTracking().ToList();
            return data;
        }

        [HttpPost]
        public object GetAetRelative()
        {
            var data = _context.FundAccEntrys.Select(x => new { AetRelative = x.AetRelative }).AsNoTracking().ToList();
            return data;
        }

        [HttpPost]
        public object GetUser()
        {
            var query = _context.Users.Where(x => x.Active == true && x.UserType != 10).Select(x => new { x.Id, x.GivenName }).AsNoTracking().ToList();
            return query;
        }

        [HttpPost]
        public string GenAETCode(string type, string catCode)
        {
            var str = string.Empty;
            var idMax = 1;
            var obj = _context.FundAccEntrys.LastOrDefault();
            if (obj != null)
                idMax = obj.Id + 1;
            string userName;
            userName = User.Identity.Name;
            str = string.Format("AET.{0}_{1}_{2}_{3}_{4}", idMax, type, catCode, userName, DateTime.Now.ToString("yyyyMMdd"));
            return str;
        }

        [HttpPost]
        public object GetListTitle()
        {
            var data = _context.FundAccEntrys.Where(x => x.IsDeleted == false).Select(x => new { Title = x.Title }).AsNoTracking().ToList();
            //var data = _context.FundCatReptExpss.Select(x => new { CatCode = x.CatCode}).AsNoTracking().ToList();
            return data;
        }

        [HttpPost]
        public object GetItem([FromBody] int id)
        {
            var isPermission = true;

            var userName = User.Identity.Name;

            var checkRole = _context.Users.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());
            if (checkRole != null)
            {
                var type = checkRole.UserType;
                var role = string.Empty;
                var userInGroup = _context.AdUserInGroups.FirstOrDefault(x => x.UserId == checkRole.Id && x.IsMain == true);
                if (userInGroup != null)
                {
                    var roleId = userInGroup.RoleId;
                    var userRole = _context.Roles.FirstOrDefault(x => x.Id == roleId);
                    role = userRole.Code;
                }
                if (type == 10 || role == "001")
                {
                    isPermission = false;
                }
            }

            var query = _context.FundAccEntrys.ToList();

            var data = from a in query
                       where a.Id == id
                       select new
                       {
                           a.Id,
                           a.AetCode,
                           a.AetType,
                           a.Title,
                           a.CatCode,
                           a.AetRelative,
                           a.AetRelativeType,
                           a.AetDescription,
                           a.Total,
                           a.Payer,
                           a.Currency,
                           DeadLine = a.DeadLine != null ? a.DeadLine.Value.ToString("dd/MM/yyyy") : null,
                           a.IsPlan,
                           a.Status,
                           a.GoogleMap,
                           a.Address,
                           a.Receiptter,
                           Action = _context.FundAccEntryTrackings.LastOrDefault(x => x.AetCode.Equals(a.AetCode) && (x.Action == "APPROVED" || x.Action == "REFUSE"))?.Action,
                           IsPermission = isPermission
                       };
            return data;
        }
       
        #endregion

        //xuất Exel
        [HttpGet]
        public ActionResult ExportExcel(string page, string row, string fromDatePara, string toDatePara, string title, string aetType, string total, string currency, string payer, string receiptter, string status, string isPlan , string orderBy)
        {
            int pageInt = int.Parse(page);
            int length = int.Parse(row);
            //Get data View
            //var listData = _context.FundAccEntrys.Where(x => x.IsPlan == false && x.IsDeleted == false).Select(x => new { x.Title, x.AetType, x.Total, x.Currency, x.Payer, x.Receiptter, x.Status }).OrderUsingSortExpression(orderBy).AsNoTracking().ToList();

            var fromDate = !string.IsNullOrEmpty(fromDatePara) ? DateTime.ParseExact(fromDatePara, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toDate = !string.IsNullOrEmpty(toDatePara) ? DateTime.ParseExact(toDatePara, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;

            var listData = (from a in _context.FundAccEntrys
                           join b in _context.FundCatReptExpss.Where(x => x.IsDeleted == false)
                           on a.CatCode equals b.CatCode
                           where (!a.IsDeleted
                                     && ((fromDate == null) || (a.DeadLine.HasValue && a.DeadLine.Value.Date >= fromDate))
                                     && ((toDate == null) || (a.DeadLine.HasValue && a.DeadLine.Value.Date <= toDate)))
                                      && (string.IsNullOrEmpty(aetType) || (a.AetType.Equals(aetType)))
                                       && (string.IsNullOrEmpty(status) || (a.Status.Equals(status)))
                                       && (string.IsNullOrEmpty(isPlan) || (a.IsPlan.Equals(Convert.ToBoolean(isPlan))))
                            select new
                           {
                               CatName = b.CatName,
                               Id = a.Id,
                               AetCode = a.AetCode,
                               Title = a.Title,
                               AetType = a.AetType,
                               AetRelativeType = a.AetRelativeType,
                               AetDescription = a.AetDescription,
                               Total = a.Total,
                               Payer = a.Payer,
                               Currency = a.Currency,
                               Status = a.Status,
                               Receiptter = a.Receiptter
                           }).OrderUsingSortExpression(orderBy).AsNoTracking().ToList();
            var listExport = new List<FundAccEntryExportModel>();
            var no = 1;
            foreach (var item in listData)
            {
                var itemExport = new FundAccEntryExportModel();

                itemExport.No = no;
                itemExport.CatName = item.CatName;
                itemExport.Title = item.Title;
                itemExport.AetType = item.AetType;
                itemExport.Total = item.Total;
                itemExport.Currency = item.Currency;
                itemExport.Payer = item.Payer;
                itemExport.Receiptter = item.Receiptter;
                itemExport.Status = item.Status;


                if (item.Status == "CREATED")
                {
                    itemExport.Status = "Khởi tạo";
                }
               else if (item.Status == "PENDING")
                {
                    itemExport.Status = "Đang chờ";
                }
                else if (item.Status == "APPROVED")
                {
                    itemExport.Status = "Đã duyệt";
                }
               else if (item.Status == "REFUSE")
                {
                    itemExport.Status = "Từ chối";
                }
                else 
                {
                    itemExport.Status = "Hủy bỏ";
                }

                if (item.AetType == "Receipt")
                {
                    itemExport.AetType = "Thu";
                }
                else if (item.AetType == "Expense")
                {
                    itemExport.AetType = "Chi";
                }
                //itemExport.LeaderIdea = item.LeaderIdea == "undefined" ? "Không ý kiến" : item.LeaderIdea;
                //itemExport.Percent = item.Percent.ToString();

                //if (item.Percent == 100)
                //{
                //    itemExport.Percent = "100%";
                //}
                //else if (item.Percent == 0)
                //{
                //    itemExport.Percent = "0%";
                //}
                //else
                //{
                //    itemExport.Percent = String.Format("{0:0.00}", item.Percent) + "%";
                //}
                no = no + 1;
                listExport.Add(itemExport);
            }

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2016;
            IWorkbook workbook = application.Workbooks.Create(1);
            workbook.Version = ExcelVersion.Excel2016;
            IWorksheet sheetRequest = workbook.Worksheets.Create("PhieuThuChi");
            workbook.Worksheets[0].Remove();
            IMigrantRange migrantRange = workbook.Worksheets[0].MigrantRange;
            sheetRequest.Range["A1"].ColumnWidth = 24;
            sheetRequest.Range["B1"].ColumnWidth = 24;
            sheetRequest.Range["C1"].ColumnWidth = 24;
            sheetRequest.Range["D1"].ColumnWidth = 24;
            sheetRequest.Range["E1"].ColumnWidth = 24;
            sheetRequest.Range["F1"].ColumnWidth = 24;
            sheetRequest.Range["G1"].ColumnWidth = 24;
            sheetRequest.Range["H1"].ColumnWidth = 24;
            sheetRequest.Range["I1"].ColumnWidth = 24;
            //sheetRequest.Range["I1"].ColumnWidth = 24;
            //sheetRequest.Range["J1"].ColumnWidth = 24;


            sheetRequest.Range["A1:I1"].Merge(true);

            sheetRequest.Range["A1"].Text = "Phiếu Thu Chi";
            sheetRequest.Range["A1"].CellStyle.Font.FontName = "Calibri";
            sheetRequest.Range["A1"].CellStyle.Font.Bold = true;
            sheetRequest.Range["A1"].CellStyle.Font.Size = 24;
            sheetRequest.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 176, 240);
            sheetRequest.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheetRequest.ImportData(listExport, 2, 1, true);

            sheetRequest["A2"].Text = "TT";
            sheetRequest["B2"].Text = "Tên danh mục";
            sheetRequest["C2"].Text = "Tiêu Đề";
            sheetRequest["D2"].Text = "Loại";
            sheetRequest["E2"].Text = "Số Tiền";
            sheetRequest["F2"].Text = "Loại Tiền";
            sheetRequest["G2"].Text = "Người Trả";
            sheetRequest["H2"].Text = "Người Nhận";
            sheetRequest["I2"].Text = "Trạng Thái";



            IStyle tableHeader = workbook.Styles.Add("TableHeaderStyle");
            tableHeader.Font.Color = ExcelKnownColors.White;
            tableHeader.Font.Bold = true;
            tableHeader.Font.Size = 11;
            tableHeader.Font.FontName = "Calibri";
            tableHeader.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            tableHeader.VerticalAlignment = ExcelVAlign.VAlignCenter;
            tableHeader.Color = Color.FromArgb(0, 0, 122, 192);
            tableHeader.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.None;
            tableHeader.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.None;
            tableHeader.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.None;
            tableHeader.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
            tableHeader.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
            sheetRequest["A2:I2"].CellStyle = tableHeader;
            sheetRequest.Range["A2:I2"].RowHeight = 20;
            sheetRequest.UsedRange.AutofitColumns();

            string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "ExportPhieuThuChi" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            workbook.Close();
            excelEngine.Dispose();
            ms.Position = 0;
            return File(ms, ContentType, fileName);
        }

        #region Tracking
        [HttpPost]
        public JsonResult InsertAccEntryTracking(string aetCode, string status, string note)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var userName = User.Identity.Name;

                var checkRole = _context.Users.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());
                if (checkRole != null)
                {
                    var type = checkRole.UserType;
                    var role = string.Empty;
                    var userInGroup = _context.AdUserInGroups.FirstOrDefault(x => x.UserId == checkRole.Id && x.IsMain == true);
                    if (userInGroup != null)
                    {
                        var roleId = userInGroup.RoleId;
                        var userRole = _context.Roles.FirstOrDefault(x => x.Id == roleId);
                        role = userRole.Code;
                    }
                    if (type == 10 || role == "001")
                    {
                        var entryTracking = new FundAccEntryTracking
                        {
                            AetCode = aetCode,
                            CreatedBy = User.Identity.Name,
                            CreatedTime = DateTime.Now,
                            Action = status,
                            Note = note
                        };
                        _context.FundAccEntryTrackings.Add(entryTracking);
                        _context.SaveChanges();

                        var obj = _context.FundAccEntrys.FirstOrDefault(x => x.AetCode.Equals(aetCode));
                        if (obj != null)
                        {
                            obj.Status = status;
                            _context.FundAccEntrys.Update(obj);
                            _context.SaveChanges();
                            var Notif1 = (from a in _context.FundAccEntrys.Where(x => x.IsDeleted == false && x.IsPlan == false && x.AetType == obj.AetType)
                                          join b in _context.FundAccEntryTrackings.Where(x => x.IsDeleted == false && x.Action == "APPROVED")
                                          on a.AetCode equals b.AetCode
                                          select new
                                          {
                                              a.CatCode,
                                              a.AetType,
                                              a.AetCode,
                                              a.DeadLine,
                                              a.Payer,
                                              a.Receiptter,
                                              a.Total,
                                          }).ToList();
                            var Notif2 = (from a in Notif1.Where(x => x.CatCode == obj.CatCode)
                                          join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == obj.AetType && x.catCode == obj.CatCode)
                                         on a.CatCode equals b.catCode
                                          where a.DeadLine >= b.fromTime && a.DeadLine <= b.toTime
                                          group a by new { a.CatCode, a.AetType }
                                         into list
                                          orderby list.Key.CatCode
                                          select new
                                          {
                                              list,
                                              total = list.Sum(x => x.Total),
                                              catCode = list.Key.CatCode,
                                              aetType = list.Key.AetType,
                                              maxDate = list.Max(x => x.DeadLine),
                                              minDate = list.Min(x => x.DeadLine),

                                          }).ToList();
                            var queryNotif = (from a in Notif2.Where(x => x.aetType == obj.AetType)
                                              join b in _context.ParamForWarnings.Where(x => x.isDeleted == false && x.aetType == obj.AetType)
                                              on a.catCode equals b.catCode
                                              where a.maxDate <= b.toTime && a.minDate >= b.fromTime && a.total >= b.total
                                              select new
                                              {
                                                  id = b.id,
                                                  maxTotal = b.total,
                                                  fromDate = b.fromTime,
                                                  toDate = b.toTime,
                                                  aetType = b.aetType,
                                                  catCode = b.catCode,
                                                  currency = b.currency,
                                                  total = a.total,
                                              }).ToList();
                            int count = queryNotif.Count();
                            if (count > 0)
                            {
                                SendPushNotification("Có một danh mục thu chi vượt hạn mức");
                            }
                        }

                        msg.Title = "Cập nhật trạng thái thành công !";

                    }
                    else
                    {
                        msg.Error = true;
                        msg.Title = "Bạn không có quyền để thực hiện nghiệp vụ này !";
                    }
                }
                else
                {
                    msg.Title = "Thêm phiếu thu chi thành công !";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi thêm !";
            }
            return Json(msg);
        }

        [HttpGet]
        public JsonResult GetAccTrackingDetail(string aetCode)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var obj = (from a in _context.FundAccEntryTrackings
                           where a.IsDeleted == false && a.AetCode.Equals(aetCode)
                           select new
                           {
                               a.Id,
                               a.Action,
                               a.AetCode,
                               a.FromDevice,
                               a.LocationGps,
                               a.LocationText,
                               a.Note,
                               a.IsDeleted,
                               a.CreatedBy,
                               CreatedTime = a.CreatedTime != null ? a.CreatedTime.Value.ToString("dd/MM/yyyy") : null,
                           }).ToList();

                msg.Object = obj;
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi lấy thông tin!";
            }
            return Json(msg);
        }
        #endregion

        #region File
        [HttpPost]
        public object GetListFundFiles(string aetCode)
        {
            try
            {
                var query = _context.FundFiless.Where(x => x.AetCode.Equals(aetCode) && x.IsDeleted == false).ToList();

                return Json(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult UploadFile(IFormFile file)
        {
            var msg = new JMessage();
            try
            {
                if (file != null)
                {
                    if (file != null && file.Length > 0)
                    {
                        var url = string.Empty;
                        var pathUpload = Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\files");
                        if (!Directory.Exists(pathUpload)) Directory.CreateDirectory(pathUpload);
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = Path.GetFileNameWithoutExtension(fileName)
                         + "_"
                         + Guid.NewGuid().ToString().Substring(0, 8)
                         + Path.GetExtension(fileName);
                        var filePath = Path.Combine(pathUpload, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        url = "/uploads/files/" + fileName;

                        var fileUpload = new FundFile
                        {
                            FileName = fileName,
                            FilePath = url,
                            FileType = string.Concat(".", file.FileName.Split('.')[1])
                        };
                        msg.Object = fileUpload;
                    }

                    msg.Error = false;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), "Tệp tin");
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_EXITS"), "Tệp tin");
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_ADD"));
            }
            return Json(msg);
        }

        [HttpPost]
        public JMessage RemoveFundFile(int? Id)
        {
            var msg = new JMessage();
            try
            {
                if (Id != null)
                {
                    var file = _context.FundFiless.FirstOrDefault(x => x.Id == Id);
                    _context.FundFiless.Remove(file);

                    //var fileReq = _context.EDMSReqFiles.FirstOrDefault(x => x.FileId == fileId);
                    //_context.EDMSReqFiles.Remove(fileReq);

                    _context.SaveChanges();

                    msg.Error = false;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), "Tệp tin");
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), "Tệp tin");
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_ADD"));
            }
            return msg;
        }

        [NonAction]
        public JsonResult SendPushNotification(string message)
        {
            var msg = new JMessage() { Error = false };

            var query = (from a in _context.FcmTokens
                         join b in _context.Users on a.UserId equals b.Id
                         select new
                         {
                             a.Token
                         }).AsNoTracking().Select(y => y.Token);
            if (query.Any())
            {
                var countToken = query.Count();
                if (countToken > 100000)
                {
                    int countPush = (query.Count() / 100000) + 1;
                    for (int i = 0; i < countPush; i++)
                    {
                        var listDevices = query.Skip(i * 1000).Take(100000).AsNoTracking().ToList();
                        var sendNotication = _notification.SendNotification("Khẩn cấp", "Thông báo mới", listDevices, new { message = message });
                    }
                }
                else
                {
                    var sendNotication = _notification.SendNotification("Khẩn cấp", "Thông báo mới", query.ToList(), new { message = message });
                }
            }
            else
            {
                msg.Error = true;
                msg.Title = "Chưa có tài khoản nào đăng nhập!";
            }

            return Json(msg);
        }
        #endregion

        public class FundAccEntrysJtableModel
        {
            public int Id { get; set; }
            public string AetCode { get; set; }
            public string Title { get; set; }
            public string AetType { get; set; }
            public string AetDescription { get; set; }
            public bool? IsPlan { get; set; }
            public string CatCode { get; set; }
            public DateTime? DeadLine { get; set; }
            public string AetRelative { get; set; }
            public string AetRelativeType { get; set; }
            public string Payer { get; set; }
            public string Receiptter { get; set; }
            public decimal Total { get; set; }
            public string Currency { get; set; }
            public string Status { get; set; }
            //public int ActivityId { get; set; }
            //public string ActCode { get; set; }
            //public string ActTitle { get; set; }
            //public string ActType { get; set; }
            //public string ActNote { get; set; }
            //public string ActMember { get; set; }
        }
        public class JTableModelAct : JTableModel
        {
            public int Id { get; set; }
            public string AetCode { get; set; }
            public string Title { get; set; }
            public string AetType { get; set; }
            public string AetDescription { get; set; }
            public string IsPlan { get; set; }
            public string CatCode { get; set; }
            public DateTime? DeadLine { get; set; }
            public string AetRelative { get; set; }
            public string AetRelativeType { get; set; }
            public string Payer { get; set; }
            public string Receiptter { get; set; }
            public decimal Total { get; set; }
            public string Currency { get; set; }
            public string Status { get; set; }
            public string FromTime { get; set; }
            public string ToTime { get; set; }
        }
        public class JTableModelFile : JTableModel
        {
            public int Id { get; set; }
            public string FileCode { get; set; }
            public string FileName { get; set; }
            public string FileType { get; set; }
            public string FileSize { get; set; }
            public string Description { get; set; }
            public string AetCode { get; set; }
        }

        public class FundAccEntryExportModel
        {
            public int? No { get; set; }
            public string CatName { get; set; }
            public string Title { get; set; }
            public string AetType { get; set; }
            public decimal Total { get; set; }
            public string Currency { get; set; }
            public string Payer { get; set; }
            public string Receiptter { get; set; }
            public string Status { get; set; }
        }

    }
}