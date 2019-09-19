using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ESEIM;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


using Microsoft.AspNetCore.Identity;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using System.IO;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class ProporCurrentController : BaseController
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<AspNetRole> _roleManager;
        private readonly EIMDBContext _context;
        private readonly AppSettings _appSettings;

        public class JTableProporCurrentModelCustom : JTableModel
        {
            public string CustomerName { get; set; }
            public string Area { get; set; }
            public string Staff { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }
        public class ProporCurrentModel
        {
            public int Id { get; set; }
            public string CustomerName { get; set; }
            public string Area { get; set; }
            public decimal? Proportion { get; set; }
            public decimal? TotalCanImp { get; set; }
            public string Staff { get; set; }
            public DateTime? CreatedTime { get; set; }
        }
        public class ProporCurrentExportModel
        {
            public int No { get; set; }
            public string CustomerName { get; set; }
            public string Area { get; set; }
            public string Proportion { get; set; }
            public string TotalCanImp { get; set; }
            public string Staff { get; set; }
            public string CreatedTime { get; set; }
        }
        public ProporCurrentController(IOptions<AppSettings> appSettings, EIMDBContext context, UserManager<AspNetUser> userManager, RoleManager<AspNetRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }


        public object JTable([FromBody]JTableProporCurrentModelCustom jTablePara)
        {
            var count = 0;
            var intBeginFor = 1;
            try
            {
                //DateTime? fromDate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "MM/dd/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                //DateTime? toDate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "MM/dd/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;

                intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;

                //var query = (from a in _context.VcSettingRoutes.Where(x => x.IsDeleted != true)
                //             join d1 in _context.E_Companys.Where(x => x.Flag == 1) on a.Node equals d1.Company_Code into d2
                //             from d in d2.DefaultIfEmpty()
                //             join b1 in _context.Romooc_Driver on a.CreatedBy equals b1.Username into b2
                //             from b in b2.DefaultIfEmpty()
                //             join c1 in _context.Common_Settings.Where(x => x.Group == "AREA") on d.Area equals c1.Code into c2
                //             from c in c2.DefaultIfEmpty()
                //             where
                //             a.CurrentStatus != "ROUTE_CANCEL"
                //             && (string.IsNullOrEmpty(jTablePara.CustomerName) || (d != null && !string.IsNullOrEmpty(d.Company_Name) && d.Company_Name.ToLower().Contains(jTablePara.CustomerName.ToLower())))
                //             && (string.IsNullOrEmpty(jTablePara.Area) || (c != null && !string.IsNullOrEmpty(c.Code) && c.Code == jTablePara.Area))
                //             && (string.IsNullOrEmpty(jTablePara.Staff) || (b != null && !string.IsNullOrEmpty(b.Name) && b.Name.ToLower().Contains(jTablePara.Staff.ToLower())))
                //             && (string.IsNullOrEmpty(jTablePara.FromDate) || (a.CreatedTime >= fromDate))
                //             && (string.IsNullOrEmpty(jTablePara.ToDate) || (a.CreatedTime <= toDate))
                //             orderby d.Id descending
                //             select new
                //             {
                //                 Id = a.Id,
                //                 CustomerName = d != null ? d.Company_Name : "",
                //                 Area = c != null ? c.Value : "",
                //                 ProporCurrent = a.ProporCurrent,
                //                 TotalCanImp = a.TotalCanImp,
                //                 Staff = b != null ? b.Name : "",
                //                 CreatedTime = a.CreatedTime,
                //             });

                var query = FuncJTable(jTablePara.CustomerName, jTablePara.Area, jTablePara.Staff, jTablePara.FromDate, jTablePara.ToDate);

                count = query.Count();
                var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
                var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "CustomerName", "Area", "Proportion", "TotalCanImp", "Staff", "CreatedTime");
                return Json(jdata);
            }
            catch (Exception ex)
            {
                //var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
                //var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "Id", "CustomerName", "Area", "ProporCurrent", "TotalCanImp", "Staff", "CreatedTime");
                return Json("");
            }
        }


        [NonAction]
        public IQueryable<ProporCurrentModel> FuncJTable(string CustomerName, string Area, string Staff, string FromDate, string ToDate)
        {
            DateTime? fromDate = !string.IsNullOrEmpty(FromDate) ? DateTime.ParseExact(FromDate, "MM/dd/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            DateTime? toDate = !string.IsNullOrEmpty(ToDate) ? DateTime.ParseExact(ToDate, "MM/dd/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;

            var query = (from a in _context.VcSettingRoutes.Where(x => x.IsDeleted != true)
                         join d1 in _context.E_Companys.Where(x => x.Flag == 1) on a.Node equals d1.Company_Code into d2
                         from d in d2.DefaultIfEmpty()
                         join b1 in _context.Romooc_Driver on a.CreatedBy equals b1.Username into b2
                         from b in b2.DefaultIfEmpty()
                         join c1 in _context.EDMSCommonSettings.Where(x => x.Group == "AREA") on d.Area equals c1.CodeSet into c2
                         from c in c2.DefaultIfEmpty()
                         where
                         a.CurrentStatus != "ROUTE_CANCEL"
                         && (string.IsNullOrEmpty(CustomerName) || (d != null && !string.IsNullOrEmpty(d.Company_Name) && d.Company_Name.ToLower().Contains(CustomerName.ToLower())))
                         && (string.IsNullOrEmpty(Area) || (c != null && !string.IsNullOrEmpty(c.CodeSet) && c.CodeSet == Area))
                         && (string.IsNullOrEmpty(Staff) || (b != null && !string.IsNullOrEmpty(b.Name) && b.Name.ToLower().Contains(Staff.ToLower())))
                         && (string.IsNullOrEmpty(FromDate) || (a.CreatedTime >= fromDate))
                         && (string.IsNullOrEmpty(ToDate) || (a.CreatedTime <= toDate))
                         group new { a, b, c, d } by new { d.Company_Name }
                         into grp
                         orderby grp.Key.Company_Name
                         select new ProporCurrentModel
                         {
                             Id = grp.FirstOrDefault(x1 => x1.a.CreatedTime == grp.Max(y1 => y1.a.CreatedTime)).a.Id,
                             CustomerName = grp.FirstOrDefault(x1 => x1.a.CreatedTime == grp.Max(y1 => y1.a.CreatedTime)).d.Company_Name,
                             //CustomerName = d != null ? d.Company_Name : "",
                             Area = grp.FirstOrDefault(x1 => x1.a.CreatedTime == grp.Max(y1 => y1.a.CreatedTime)).c.ValueSet,
                             //Area = c != null ? c.Value : "",
                             Proportion = grp.FirstOrDefault(x1 => x1.a.CreatedTime == grp.Max(y1 => y1.a.CreatedTime)).a.Proportion,
                             //Proportion = a.Proportion,
                             TotalCanImp = grp.FirstOrDefault(x1 => x1.a.CreatedTime == grp.Max(y1 => y1.a.CreatedTime)).a.TotalCanImp,
                             //TotalCanImp = a.TotalCanImp,
                             Staff = grp.FirstOrDefault(x1 => x1.a.CreatedTime == grp.Max(y1 => y1.a.CreatedTime)).b.Name,
                             //Staff = b != null ? b.Name : "",
                             CreatedTime = grp.Max(x1 => x1.a.CreatedTime),
                         });
            return query;
        }

        [HttpPost]
        public object GetListArea()
        {
            //var query = GetIncentProListMax();
            var query = from a in _context.EDMSCommonSettings
                        where a.Group == "AREA"
                        && a.IsDeleted != true
                        select new
                        {
                            Code = a.CodeSet,
                            Name = a.ValueSet
                        };
            return Json(query);
        }


        //[HttpGet]
        //public object GetItem(int id)
        //{
        //    try
        //    {

        //        var user = _context.Maintain_material_detailss.Single(x => x.Id == id);

        //        var temp = new
        //        {
        //            user.Id,
        //            user.ProductCode,
        //            user.ProductName,
        //            user.Create_time,
        //            user.Description,
        //            user.Image,
        //            user.Quantity,
        //            user.Brand,
        //            user.Status,
        //            user.BarCode,
        //            user.Price,
        //            user.Update_time

        //        };

        //        return Json(temp);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
        //    }
        //}


        //[HttpPost]
        //public JsonResult Update([FromBody]Maintain_material_details obj)
        //{
        //    var company_code = HttpContext.GetSessionUser()?.CompanyCode;
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {

        //        Maintain_material_details obj2 = _context.Maintain_material_detailss.FirstOrDefault(x => x.Id == obj.Id);
        //        if (obj2 != null)
        //        {
        //            obj2.ProductCode = obj.ProductCode;
        //            obj2.ProductName = obj.ProductName;
        //            obj2.Status = obj.Status;
        //            obj2.BarCode = obj.BarCode;
        //            obj2.Brand = obj.Brand;
        //            obj2.Price = obj.Price;
        //            obj2.Quantity = obj.Quantity;
        //            obj2.Description = obj.Description;
        //            obj2.Company_code = company_code;
        //            obj2.Update_time = DateTime.Now;

        //            _context.Maintain_material_detailss.Update(obj2);
        //            _context.Entry(obj2).State = EntityState.Modified;
        //        }

        //        var a = _context.SaveChanges();
        //        msg.Title = "Sửa khoản mục thành công";
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Title = "Có lỗi khi sửa khoản mục";
        //    }
        //    return Json(msg);
        //}

        //[HttpPost]
        //public object DeleteChecked([FromBody]List<int> listIdI)
        //{
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {



        //        foreach (var id in listIdI)
        //        {
        //            Maintain_material_details obj = _context.Maintain_material_detailss.FirstOrDefault(x => x.Id == id);
        //            if (obj != null)
        //            {
        //                obj.Flag = 1;
        //                _context.Maintain_material_detailss.Update(obj);
        //                _context.SaveChanges();
        //            }
        //        }
        //        msg.Title = "Xóa sản phẩm thành công";
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Object = ex;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_LIST_FAIL"), CommonUtil.ResourceValue("RESOURCE"));
        //        //_logger.LogError(LoggingEvents.LogDb, "Delete list Resource fail");
        //        //_actionLog.InsertActionLogDeleteItem("AdResource", "An error occurred while Delete list Resource", null, null, "Error");

        //    }
        //    return Json(msg);
        //}

        //[HttpPost]
        //public object LockProduct([FromBody]int id)
        //{
        //    var msg = new JMessage() { Error = false };
        //    try
        //    {


        //        Maintain_material_details obj = _context.Maintain_material_detailss.FirstOrDefault(x => x.Id == id);
        //        if (obj != null)
        //        {
        //            obj.Status = (obj.Status == 1 ? 2 : 1);

        //            _context.Maintain_material_detailss.Update(obj);
        //            _context.SaveChanges();
        //        }

        //        msg.Title = "Thay đổi trạng thái sản phẩm thành công";
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Error = true;
        //        msg.Object = ex;
        //        msg.Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_LIST_FAIL"), CommonUtil.ResourceValue("RESOURCE"));
        //        //_logger.LogError(LoggingEvents.LogDb, "Delete list Resource fail");
        //        //_actionLog.InsertActionLogDeleteItem("AdResource", "An error occurred while Delete list Resource", null, null, "Error");

        //    }
        //    return Json(msg);
        //}


        //[HttpPost]
        //public JsonResult Insert([FromBody]Maintain_material_details obj)
        //{
        //    var msg = new JMessage() { Error = false };
        //    var company_code = HttpContext.GetSessionUser()?.CompanyCode;
        //    try
        //    {
        //        Maintain_material_details rm = new Maintain_material_details
        //        {
        //            ProductCode = obj.ProductName,
        //            ProductName = obj.ProductName,
        //            Status = obj.Status,
        //            BarCode = obj.BarCode,
        //            Brand = obj.Brand,
        //            Price = obj.Price,
        //            Quantity = obj.Quantity,
        //            Description = obj.Description,
        //            Company_code = company_code,
        //            Create_time = DateTime.Now

        //        };
        //        _context.Maintain_material_detailss.Add(rm);
        //        var a = _context.SaveChanges();
        //        msg.Title = "Thêm sản phẩm thành công";
        //        msg.Object = obj;
        //        msg.ID = 1;

        //    }
        //    catch (Exception ex)
        //    {
        //        msg.ID = 0;
        //        msg.Error = true;
        //        msg.Object = ex;
        //        msg.Title = "Có lỗi khi thêm khoản mục";
        //    }
        //    return Json(msg);
        //}



        //Export Excel
        [HttpGet]
        public ActionResult ExportExcel(string page, string row, string customerName, string areaExport, string staff, string fromDate, string toDate, string orderBy)
        {
            int pageInt = int.Parse(page);
            int length = int.Parse(row);
            //Get data View
            var listData = FuncJTable(customerName, areaExport, staff, fromDate, toDate).OrderUsingSortExpression(orderBy).AsNoTracking().ToList();
            var listExport = new List<ProporCurrentExportModel>();
            var no = 1;
            foreach (var item in listData)
            {
                var itemExport = new ProporCurrentExportModel();
                itemExport.No = no;
                itemExport.CustomerName = item.CustomerName;
                itemExport.Area = item.Area;
                itemExport.Staff = item.Staff;
                itemExport.CreatedTime = item.CreatedTime != null ? item.CreatedTime.Value.ToString("dd/MM/yyyy HH:mm") : "";
                itemExport.Proportion = item.Proportion.ToString();
                itemExport.TotalCanImp = item.TotalCanImp.ToString();

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
            application.DefaultVersion = ExcelVersion.Excel2010;

            IWorkbook workbook = application.Workbooks.Create(1);
            workbook.Version = ExcelVersion.Excel97to2003;
            IWorksheet sheetRequest = workbook.Worksheets.Create("TyTrongButSon");
            workbook.Worksheets[0].Remove();
            IMigrantRange migrantRange = workbook.Worksheets[0].MigrantRange;
            sheetRequest.Range["A1"].ColumnWidth = 24;
            sheetRequest.Range["B1"].ColumnWidth = 24;
            sheetRequest.Range["C1"].ColumnWidth = 24;
            sheetRequest.Range["D1"].ColumnWidth = 24;
            sheetRequest.Range["E1"].ColumnWidth = 24;
            sheetRequest.Range["F1"].ColumnWidth = 24;
            sheetRequest.Range["G1"].ColumnWidth = 24;
            //sheetRequest.Range["H1"].ColumnWidth = 24;
            //sheetRequest.Range["I1"].ColumnWidth = 24;
            //sheetRequest.Range["J1"].ColumnWidth = 24;


            sheetRequest.Range["A1:G1"].Merge(true);

            sheetRequest.Range["A1"].Text = "Tỷ trọng";
            sheetRequest.Range["A1"].CellStyle.Font.FontName = "Calibri";
            sheetRequest.Range["A1"].CellStyle.Font.Bold = true;
            sheetRequest.Range["A1"].CellStyle.Font.Size = 24;
            sheetRequest.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 176, 240);
            sheetRequest.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheetRequest.ImportData(listExport, 2, 1, true);

            sheetRequest["A2"].Text = "TT";
            sheetRequest["B2"].Text = "NPP/ĐL/CH";
            sheetRequest["C2"].Text = "KHU VỰC";
            sheetRequest["D2"].Text = "Tỷ trọng (%)";
            sheetRequest["E2"].Text = "TRỮ LƯỢNG TRỐNG (T)";
            sheetRequest["F2"].Text = "NHÂN VIÊN";
            sheetRequest["G2"].Text = "THỜI GIAN NHẬP DỮ LIỆU";


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
            sheetRequest["A2:G2"].CellStyle = tableHeader;
            sheetRequest.Range["A2:G2"].RowHeight = 20;
            sheetRequest.UsedRange.AutofitColumns();

            string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "ExportTyTrongButSon" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            workbook.Close();
            excelEngine.Dispose();
            ms.Position = 0;
            return File(ms, ContentType, fileName);
        }
    }
}