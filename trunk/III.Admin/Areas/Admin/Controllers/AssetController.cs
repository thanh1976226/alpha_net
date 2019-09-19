using System;
using System.Linq;
using ESEIM.Models;
using ESEIM.Utils;
using FTU.Utils.HelperNet;
using III.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class AssetController : BaseController
    {
        private readonly EIMDBContext _context;
        private readonly IUploadService _upload;

        public AssetController(EIMDBContext context, IUploadService upload)
        {
            _context = context;
            _upload = upload;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Index
        public class JTableModelAsset : JTableModel
        {
            public string AssetCode { get; set; }
            public string AssetName { get; set; }
            public string Status { get; set; }
        }

        [HttpPost]
        public object JTableAsset([FromBody]JTableModelAsset jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var listCommon = _context.CommonSettings.Select(x => new { x.CodeSet, x.ValueSet });
            var query = from a in _context.Assets
                        join b in listCommon on a.Status equals b.CodeSet into b1
                        from b in b1.DefaultIfEmpty()
                        join c in _context.UrencoAssetsCategorys on a.AssetType equals c.CatCode into c1
                        from c in c1.DefaultIfEmpty()
                        join d in listCommon on a.Currency equals d.CodeSet into d1
                        from d in d1.DefaultIfEmpty()
                        where (a.IsDeleted == false)
                           && (string.IsNullOrEmpty(jTablePara.AssetCode) || a.AssetCode.ToLower().Contains(jTablePara.AssetCode.ToLower()))
                           && (string.IsNullOrEmpty(jTablePara.AssetName) || a.AssetName.ToLower().Contains(jTablePara.AssetName.ToLower()))
                           && (string.IsNullOrEmpty(jTablePara.Status) || a.Status.ToLower().Contains(jTablePara.Status.ToLower()))

                        select new JtableAsset
                        {
                            id = a.AssetID,
                            code = a.AssetCode,
                            name = a.AssetName,
                            type = c != null ? c.CatName : "Không xác định",
                            description = a.Description,
                            status = b != null ? b.CodeSet == "ACTIVE" ? b.CodeSet : b.CodeSet == "DEACTIVE" ? b.CodeSet : b.ValueSet : "Không xác định",
                            cost = a.Cost,
                            currency = d != null ? d.ValueSet : "Không xác định",
                            img = a.PathIMG
                        };

            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();

            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "code", "name", "type", "description", "status", "cost", "currency", "img");
            return Json(jdata);
        }

        [HttpPost]
        public object DeleteAsset(int id)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var data = _context.Assets.FirstOrDefault(x => x.AssetID == id);
                data.DeletedBy = ESEIM.AppContext.UserName;
                data.DeletedTime = DateTime.Now;
                data.IsDeleted = true;
                _context.Assets.Update(data);
                _context.SaveChanges();

                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue(""));// "Xóa thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue(""));//"Có lỗi xảy ra khi xóa!";
                msg.Object = ex;
                return Json(msg);
            }
        }

        [HttpPost]
        public object GetAsset(int id)
        {
            var data = _context.Assets.FirstOrDefault(x => x.AssetID == id);
            return Json(data);
        }

        [HttpPost]
        public object InsertAsset([FromBody]Asset data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                if (_context.Assets.FirstOrDefault(x => x.AssetCode == data.AssetCode) == null)
                {
                    data.CreatedBy = ESEIM.AppContext.UserName;
                    data.CreatedTime = DateTime.Now;
                    _context.Assets.Add(data);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("ASSET_MSG_ADD_ASSET_SUCCESS"));//"Thêm tài sản thành công!";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_EXITS"), CommonUtil.ResourceValue("ASSET_CURD_LBL_ASSET_CODE"));//"Mã tài sản đã tồn tại!";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_FAILED"), CommonUtil.ResourceValue(""));//"Có lỗi xảy ra khi thêm mới tài sản!";
                msg.Object = ex;
            }
            return Json(msg);
        }

        [HttpPost]
        public object UpdateAsset([FromBody]Asset data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                data.UpdatedBy = ESEIM.AppContext.UserName;
                data.UpdatedTime = DateTime.Now;
                _context.Assets.Update(data);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue(""));//"Cập nhật thông tin thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_FAILED"), CommonUtil.ResourceValue(""));//"Có lỗi xảy ra khi cập nhật!";
                return Json(msg);
            }
        }

        [HttpPost]
        public object UploadImage(IFormFile fileUpload)
        {
            var msg = new JMessage();
            try
            {
                var upload = _upload.UploadImage(fileUpload);
                msg.Error = false;
                msg.Title = "";
                msg.Object = upload.Object;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_UPLOAD_FILE"));// "Có lỗi xảy ra khi upload file!";
                msg.Object = ex;
                return Json(msg);
            }
        }

        [HttpPost]
        public object GetAllSupplier()
        {
            var data = _context.Suppliers.Select(x => new { x.SupCode, x.SupName }).AsNoTracking();
            return Json(data);
        }
        [HttpPost]
        public object GetAssetType()
        {
            var data = _context.UrencoAssetsCategorys.Where(x => x.IsDeleted == false).Select(x => new { Code = x.CatCode, Name = x.CatName });
            return data;
        }
        [HttpPost]
        public object GetAssetGroup()
        {
            var data = _context.UrencoMaterialProductGroup.Where(x => x.IsDeleted == false).Select(x => new { Code = x.Code, Name = x.Name });
            return data;
        }
        [HttpPost]
        public object GetCurrency()
        {
            return GetCurrencyBase();
        }
        [HttpPost]
        public object GetStatus()
        {
            //var data = _context.CommonSettings.Where(x => x.Group == EnumHelper<PublishEnum>.GetDisplayValue(PublishEnum.Status)).Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            var data = _context.CommonSettings.Where(x => x.Group == "SERVICE_STATUS").Select(x => new { Code = x.CodeSet, Name = x.ValueSet });
            return data;
        }
        #endregion

        #region AssetAttribute
        public class JTableModelAttr : JTableModel
        {
            public string AssetCode { get; set; }
            public string AttrCode { get; set; }
            public string AttrValue { get; set; }
        }

        [HttpPost]
        public object JTableAttr([FromBody]JTableModelAttr jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.AssetAttributes
                        where (a.IsDeleted == false && a.AssetCode == jTablePara.AssetCode)
                            && (string.IsNullOrEmpty(jTablePara.AttrCode) || a.AttrCode.ToLower().Contains(jTablePara.AttrCode.ToLower()))
                            && (string.IsNullOrEmpty(jTablePara.AttrValue) || a.AttrValue.ToLower().Contains(jTablePara.AttrValue.ToLower()))
                        select new
                        {
                            id = a.AttrID,
                            code = a.AttrCode,
                            value = a.AttrValue,
                            groupAttr = a.AttrGroup,
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "code", "value", "groupAttr", "assetCode");
            return Json(jdata);
        }

        [HttpPost]
        public object GetAttr(int id)
        {
            var data = _context.AssetAttributes.FirstOrDefault(x => x.AttrID == id);
            return Json(data);
        }

        [HttpPost]
        public object InsertAttr([FromBody]AssetAttribute data)
        {
            var msg = new JMessage() { Error = false };

            try
            {
                var query = _context.AssetAttributes.FirstOrDefault(x => x.AttrCode == data.AttrCode);
                if (query == null)
                {
                    data.CreatedBy = ESEIM.AppContext.UserName;
                    data.CreatedTime = DateTime.Now;
                    _context.AssetAttributes.Add(data);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("ASSET_CURD_TAB_ATTRIBUTE"));//"Thêm thuộc tính thành công!";
                }
                else
                {
                    msg.Error = true;
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_EXITS"), CommonUtil.ResourceValue("ASSET_CURD_TAB_ATTRIBUTE_CURD_LBL_ATTR_CODE"));//"Mã thuộc tính đã tồn tại!";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("ASSET_CURD_TAB_ATTRIBUTE"));// "Có lỗi xảy ra khi xóa thuộc tính!";
            }
            return Json(msg);
        }


        [HttpPost]
        public object UpdateAttr([FromBody]AssetAttribute data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                data.UpdatedBy = ESEIM.AppContext.UserName;
                data.UpdatedTime = DateTime.Now;
                _context.AssetAttributes.Update(data);
                _context.SaveChanges();

                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue(""));//"Cập nhật thông tin thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_FAILED"), CommonUtil.ResourceValue(""));//"Có lỗi xảy ra khi xóa!";
                msg.Object = ex;
                throw;
            }
        }

        [HttpPost]
        public object DeleteAttr(int id)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var data = _context.AssetAttributes.FirstOrDefault(x => x.AttrID == id);
                data.DeletedBy = ESEIM.AppContext.UserName;
                data.DeletedTime = DateTime.Now;
                data.IsDeleted = true;
                _context.AssetAttributes.Remove(data);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("ASSET_CURD_TAB_ATTRIBUTE")); //"Xóa thuộc tính thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_FAIL"), CommonUtil.ResourceValue("ASSET_CURD_TAB_ATTRIBUTE")); //"Có lỗi xảy ra khi xóa!";
            }
            return Json(msg);
        }
        [HttpPost]
        public object JTableHistoryRun()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("draw", 1);
            dictionary.Add("recordsFiltered", 4);
            dictionary.Add("recordsTotal", 4);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<object> datas = new List<object>();
            data.Add("Id", "1");
            data.Add("Title", "Hợp đồng mua sắm");
            data.Add("TimeFrom", "05/06/2019");
            data.Add("TimeTo", "06/06/2019");
            data.Add("Expense", "100.000.000VND");
            data.Add("Status", "Đã thanh toán");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "2");
            data.Add("Title", "Cho mượn");
            data.Add("TimeFrom", "10/07/2019");
            data.Add("TimeTo", "17/07/2019");
            data.Add("Expense", "0VND");
            data.Add("Status", "Đã hoàn trả");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "3");
            data.Add("Title", "Bảo trì sửa chữa");
            data.Add("TimeFrom", "20/07/2019");
            data.Add("TimeTo", "20/07/2019");
            data.Add("Expense", "0VND");
            data.Add("Status", "Đã xong");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "3");
            data.Add("Title", "Bảo trì sửa chữa");
            data.Add("TimeFrom", "22/07/2019");
            data.Add("TimeTo", "22/07/2019");
            data.Add("Expense", "0VND");
            data.Add("Status", "Đã xong");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "4");
            data.Add("Title", "Thanh lý");
            data.Add("TimeFrom", "01/08/2019");
            data.Add("TimeTo", "10/08/2019");
            data.Add("Expense", "80.000.000VND");
            data.Add("Status", "Đã thanh lý");
            datas.Add(data);

            dictionary.Add("data", datas);
            var count = dictionary.Count();

            return Json(dictionary);
        } [HttpPost]
        public object JTableMaintenanceHistory()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("draw", 1);
            dictionary.Add("recordsFiltered", 2);
            dictionary.Add("recordsTotal", 2);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<object> datas = new List<object>();
            data.Add("Id", "1");
            data.Add("Maintenance", "Sửa chữa nhỏ");
            data.Add("Type", "Sửa chữa");
            data.Add("Unit", "VNĐ");
            data.Add("Time", "20/07/2019");
            data.Add("Note", "Gara Ô TÔ TÍN PHÁT");
            data.Add("Address", "");
            data.Add("Status", "");
            datas.Add(data);

            data = new Dictionary<string, string>();
            data.Add("Id", "2");
            data.Add("Maintenance", "Bảo dưỡng đinh kỳ");
            data.Add("Type", "Bảo dưỡng");
            data.Add("Unit", "VNĐ");
            data.Add("Time", "22/07/2019");
            data.Add("Note", "Gara Ô TÔ TÍN PHÁT");
            data.Add("Address", "");
            data.Add("Status", "");
            datas.Add(data);


            dictionary.Add("data", datas);
            var count = dictionary.Count();

            return Json(dictionary);
        }
        #endregion
    }

    public class JtableAsset
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string cost { get; set; }

        public string currency { get; set; }

        public string img { get; set; }

    }
}