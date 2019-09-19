using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ESEIM.Models;
using ESEIM.Utils;
using Microsoft.EntityFrameworkCore;
using FTU.Utils.HelperNet;
using Microsoft.Extensions.Logging;
using System.IO;
using III.Admin.Controllers;

namespace III.Admin.Controllers
{
    public class CommonSettingGroupModel
    {
        public string GroupCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    [Area("Admin")]
    public class CommonSettingController : BaseController
    {
        private readonly EIMDBContext _context;

        public CommonSettingController(EIMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class JTableModelSetting : JTableModel
        {
            public string SettingCode { get; set; }
            public string SettingValue { get; set; }
            public int? SettingGroup { get; set; }
        }

        [HttpPost]
        public object JTableContract([FromBody]JTableModelSetting jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.CommonSettings
                        where (a.AssetCode == "CONTRACT")
                        && (string.IsNullOrEmpty(jTablePara.SettingCode) || (a.CodeSet.ToLower().Contains(jTablePara.SettingCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SettingValue) || (a.ValueSet.ToLower().Contains(jTablePara.SettingValue.ToLower())))
                        select new
                        {
                            a.SettingID,
                            a.CodeSet,
                            a.ValueSet,
                            a.GroupNote,
                            a.CreatedTime,
                        };

            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBegin).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "SettingID", "CodeSet", "ValueSet", "GroupNote", "CreatedTime");
            return Json(jdata);
        }

        [HttpPost]
        public object JTableSupplier([FromBody]JTableModelSetting jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.CommonSettings
                        where (a.AssetCode == "SUPPLIER")
                          && (string.IsNullOrEmpty(jTablePara.SettingCode) || (a.CodeSet.ToLower().Contains(jTablePara.SettingCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SettingValue) || (a.ValueSet.ToLower().Contains(jTablePara.SettingValue.ToLower())))
                        select new
                        {
                            a.SettingID,
                            a.CodeSet,
                            a.ValueSet,
                            a.GroupNote,
                            a.CreatedTime
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBegin).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "SettingID", "CodeSet", "ValueSet", "GroupNote", "CreatedTime");
            return Json(jdata);
        }

        [HttpPost]
        public object JTableFile([FromBody]JTableModelSetting jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.CommonSettings
                        where (a.AssetCode == "FILE")
                          && (string.IsNullOrEmpty(jTablePara.SettingCode) || (a.CodeSet.ToLower().Contains(jTablePara.SettingCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SettingValue) || (a.ValueSet.ToLower().Contains(jTablePara.SettingValue.ToLower())))
                        select new
                        {
                            a.SettingID,
                            a.CodeSet,
                            a.ValueSet,
                            a.GroupNote,
                            a.CreatedTime
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBegin).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "SettingID", "CodeSet", "ValueSet", "GroupNote", "CreatedTime");
            return Json(jdata);
        }

        [HttpPost]
        public object JTableService([FromBody]JTableModelSetting jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.CommonSettings
                        where ( a.AssetCode == "SERVICE")
                          && (string.IsNullOrEmpty(jTablePara.SettingCode) || (a.CodeSet.ToLower().Contains(jTablePara.SettingCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SettingValue) || (a.ValueSet.ToLower().Contains(jTablePara.SettingValue.ToLower())))
                        select new
                        {
                            a.SettingID,
                            a.CodeSet,
                            a.ValueSet,
                            a.GroupNote,
                            a.CreatedTime
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBegin).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "SettingID", "CodeSet", "ValueSet", "GroupNote", "CreatedTime");
            return Json(jdata);
        }

        [HttpPost]
        public object JTableAsset([FromBody]JTableModelSetting jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.CommonSettings
                        where ( a.AssetCode == "ASSET")
                          && (string.IsNullOrEmpty(jTablePara.SettingCode) || (a.CodeSet.ToLower().Contains(jTablePara.SettingCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SettingValue) || (a.ValueSet.ToLower().Contains(jTablePara.SettingValue.ToLower())))
                        select new
                        {
                            a.SettingID,
                            a.CodeSet,
                            a.ValueSet,
                            a.GroupNote,
                            a.CreatedTime
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBegin).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "SettingID", "CodeSet", "ValueSet", "GroupNote", "CreatedTime");
            return Json(jdata);
        }

        [HttpPost]
        public object JTablePayment([FromBody]JTableModelSetting jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.CommonSettings
                        where (a.AssetCode == "PAYMENT")
                          && (string.IsNullOrEmpty(jTablePara.SettingCode) || (a.CodeSet.ToLower().Contains(jTablePara.SettingCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SettingValue) || (a.ValueSet.ToLower().Contains(jTablePara.SettingValue.ToLower())))
                        select new
                        {
                            a.SettingID,
                            a.CodeSet,
                            a.ValueSet,
                            a.GroupNote,
                            a.CreatedTime
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBegin).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "SettingID", "CodeSet", "ValueSet", "GroupNote", "CreatedTime");
            return Json(jdata);
        }

        [HttpPost]
        public object JTablePublish([FromBody]JTableModelSetting jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.CommonSettings
                        where (a.AssetCode == "PUBLISH")
                          && (string.IsNullOrEmpty(jTablePara.SettingCode) || (a.CodeSet.ToLower().Contains(jTablePara.SettingCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SettingValue) || (a.ValueSet.ToLower().Contains(jTablePara.SettingValue.ToLower())))
                        select new
                        {
                            a.SettingID,
                            a.CodeSet,
                            a.ValueSet,
                            a.GroupNote,
                            a.CreatedTime
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBegin).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "SettingID", "CodeSet", "ValueSet", "GroupNote", "CreatedTime");
            return Json(jdata);
        }

        [HttpPost]
        public object JTableProject([FromBody]JTableModelSetting jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.CommonSettings
                        where ( a.AssetCode == "PROJECT")
                          && (string.IsNullOrEmpty(jTablePara.SettingCode) || (a.CodeSet.ToLower().Contains(jTablePara.SettingCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SettingValue) || (a.ValueSet.ToLower().Contains(jTablePara.SettingValue.ToLower())))
                        select new
                        {
                            a.SettingID,
                            a.CodeSet,
                            a.ValueSet,
                            a.GroupNote,
                            a.CreatedTime
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBegin).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "SettingID", "CodeSet", "ValueSet", "GroupNote", "CreatedTime");
            return Json(jdata);
        }

        [HttpPost]
        public object JTableCustomer([FromBody]JTableModelSetting jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.CommonSettings
                        where (a.AssetCode == "CUSTOMER")
                          && (string.IsNullOrEmpty(jTablePara.SettingCode) || (a.CodeSet.ToLower().Contains(jTablePara.SettingCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SettingValue) || (a.ValueSet.ToLower().Contains(jTablePara.SettingValue.ToLower())))
                        select new
                        {
                            a.SettingID,
                            a.CodeSet,
                            a.ValueSet,
                            a.GroupNote,
                            a.CreatedTime
                        };
            int count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBegin).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "SettingID", "CodeSet", "ValueSet", "GroupNote", "CreatedTime");
            return Json(jdata);
        }

        [HttpPost]
        public object JTableCardjob([FromBody]JTableModelSetting jTablePara)
        {
            int intBegin = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.CommonSettings
                        where ( a.AssetCode == "CARDJOB")
                        && (string.IsNullOrEmpty(jTablePara.SettingCode) || (a.CodeSet.ToLower().Contains(jTablePara.SettingCode.ToLower())))
                        && (string.IsNullOrEmpty(jTablePara.SettingValue) || (a.ValueSet.ToLower().Contains(jTablePara.SettingValue.ToLower())))
                        select new
                        {
                            a.SettingID,
                            a.CodeSet,
                            a.ValueSet,
                            a.GroupNote,
                            a.CreatedTime,
                        };

            int count = query.Count();
            var data = query.OrderBy("GroupNote").AsNoTracking().ToList();
            var data1 = data.Skip(intBegin).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "SettingID", "CodeSet", "ValueSet", "GroupNote", "CreatedTime");
            return Json(jdata);
        }
        [HttpGet]
        public object GetCatFund()
        {
            var data = _context.CommonSettings.Where(x => x.Group == "CAT_FUND" && x.IsDeleted == false).AsNoTracking().ToList();
            //var data = _context.FundCatReptExpss.Select(x => new { CatCode = x.CatCode}).AsNoTracking().ToList();
            return data;
        }
        [HttpGet]
        public object GetGroup(string groupCode)
        {
            var data = new List<CommonSettingGroupModel>();
            #region Contract
            var contractType = new CommonSettingGroupModel
            {
                GroupCode = "CONTRACT",
                Code = "CONTRACT_TYPE",
                Name = "Loại hợp đồng",
            };
            data.Add(contractType);
            var contractStatus = new CommonSettingGroupModel
            {
                GroupCode = "CONTRACT",
                Code = "CONTRACT_STATUS",
                Name = "Trạng thái của hợp đồng",
            };
            data.Add(contractStatus);
            var contractGroup = new CommonSettingGroupModel
            {
                GroupCode = "CONTRACT",
                Code = "CONTRACT_EXTEND_GROUP",
                Name = "Nhóm khách hàng mở rộng",
            };
            data.Add(contractGroup);
            #endregion

            #region Supplier
            var supplierGroup = new CommonSettingGroupModel
            {
                GroupCode = "SUPPLIER",
                Code = "SUPPLIER_GROUP",
                Name = "Nhóm nhà cung cấp",
            };
            data.Add(supplierGroup);
            var supplierStatus = new CommonSettingGroupModel
            {
                GroupCode = "SUPPLIER",
                Code = "SUPPLIER_STATUS",
                Name = "Trạng thái nhà cung cấp",
            };
            data.Add(supplierStatus);
            #endregion

            #region Customer 
            var customerGroup = new CommonSettingGroupModel
            {
                GroupCode = "CUSTOMER",
                Code = "CUSTOMER_GROUP",
                Name = "Nhóm khách hàng",
            };
            data.Add(customerGroup);
            #endregion

            #region Service
            var serviceMain = new CommonSettingGroupModel
            {
                GroupCode = "SERVICE",
                Code = "MAIN_SERVICE",
                Name = "Dịch vụ chính",
            };
            data.Add(serviceMain);

            var serviceGroup = new CommonSettingGroupModel
            {
                GroupCode = "SERVICE",
                Code = "SERVICE_GROUP",
                Name = "Nhóm dịch vụ",
            };
            data.Add(serviceGroup);

            var serviceUnit = new CommonSettingGroupModel
            {
                GroupCode = "SERVICE",
                Code = "SERVICE_UNIT",
                Name = "Đơn vị dịch vụ",
            };
            data.Add(serviceUnit);

            #endregion

            #region Asset
            var assetType = new CommonSettingGroupModel
            {
                GroupCode = "ASSET",
                Code = "ASSET_TYPE",
                Name = "Loại tài sản",
            };
            data.Add(assetType);

            var assetGroup = new CommonSettingGroupModel
            {
                GroupCode = "ASSET",
                Code = "ASSET_GROUP",
                Name = "Nhóm tài sản",
            };
            data.Add(assetGroup);

            var assetActivityType = new CommonSettingGroupModel
            {
                GroupCode = "ASSET",
                Code = "ASSET_ACTIVITY_TYPE",
                Name = "Kiểu hoạt động",
            };
            data.Add(assetActivityType);

            #endregion

            #region Payment
            var paymentObj = new CommonSettingGroupModel
            {
                GroupCode = "PAYMENT",
                Code = "PAY_OBJ_TYOE",
                Name = "Đối tượng thanh toán",
            };
            data.Add(paymentObj);

            var paymentType = new CommonSettingGroupModel
            {
                GroupCode = "PAYMENT",
                Code = "PAY_TYPE",
                Name = "Loại thanh toán",
            };
            data.Add(paymentType);

            #endregion

            #region Publish
            var status = new CommonSettingGroupModel
            {
                GroupCode = "PUBLISH",
                Code = "STATUS",
                Name = "Trạng thái",
            };
            data.Add(status);

            var origin = new CommonSettingGroupModel
            {
                GroupCode = "PUBLISH",
                Code = "ORIGIN",
                Name = "Xuất xứ",
            };
            data.Add(origin);
            var curency = new CommonSettingGroupModel
            {
                GroupCode = "PUBLISH",
                Code = "CURRENCY_TYPE",
                Name = "Loại tiền",
            };
            data.Add(curency);
            var unit = new CommonSettingGroupModel
            {
                GroupCode = "PUBLISH",
                Code = "UNIT",
                Name = "Đơn vị",
            };
            data.Add(unit);

            var productGroup = new CommonSettingGroupModel
            {
                GroupCode = "PUBLISH",
                Code = "PRODUCT_GROUP",
                Name = "Nhóm sản phẩm",
            };
            data.Add(productGroup);

            var task = new CommonSettingGroupModel
            {
                GroupCode = "PUBLISH",
                Code = "TASK",
                Name = "Công việc",
            };
            data.Add(task);

            var programLanguage = new CommonSettingGroupModel
            {
                GroupCode = "PUBLISH",
                Code = "PROGRAM_LANGUAGE",
                Name = "Ngôn ngữ",
            };
            data.Add(programLanguage);

            var transpoter = new CommonSettingGroupModel
            {
                GroupCode = "PUBLISH",
                Code = "VC_TRANSPOTER_WEIGHT",
                Name = "Trọng lượng,loại xe",
            };
            data.Add(transpoter);
            var vcArea = new CommonSettingGroupModel
            {
                GroupCode = "PUBLISH",
                Code = "VC_AREA",
                Name = "Địa bàn Vicem",
            };
            data.Add(vcArea);

            #endregion

            #region Project
            var ProjectCurency = new CommonSettingGroupModel
            {
                GroupCode = "PROJECT",
                Code = "PRO_CURENCY",
                Name = "Đơn vị",
            };
            data.Add(ProjectCurency);

            //var ProjectLanguage = new CommonSettingGroupModel
            //{
            //    GroupCode = "PROJECT",
            //    Code = "PRO_LANGUAGE",
            //    Name = "Ngôn ngữ",
            //};
            //data.Add(ProjectLanguage);

            var ProjectStatus = new CommonSettingGroupModel
            {
                GroupCode = "PROJECT",
                Code = "PRO_STATUS",
                Name = "Trạng thái",
            };
            data.Add(ProjectStatus);

            #endregion

            #region CardJob
            var CardJobDependency = new CommonSettingGroupModel
            {
                GroupCode = "CARDJOB",
                Code = "OBJ_DEPENDENCY",
                Name = "Đối tượng",
            };
            data.Add(CardJobDependency);
            var CardJobWorkType = new CommonSettingGroupModel
            {
                GroupCode = "CARDJOB",
                Code = "OBJ_WORKTYPE",
                Name = "Kiểu công việc",
            };
            data.Add(CardJobWorkType);
            var CardJobRelative = new CommonSettingGroupModel
            {
                GroupCode = "CARDJOB",
                Code = "OBJ_RELATIVE",
                Name = "Quan hệ",
            };
            data.Add(CardJobRelative);
            var CardJobStatus = new CommonSettingGroupModel
            {
                GroupCode = "CARDJOB",
                Code = "STATUS",
                Name = "Trạng thái",
            };
            data.Add(CardJobStatus);
            var CardJobLevel = new CommonSettingGroupModel
            {
                GroupCode = "CARDJOB",
                Code = "LEVEL",
                Name = "Cấp độ",
            };
            data.Add(CardJobLevel);
            #endregion
            var list = data.Where(x => x.GroupCode == groupCode);
            return list;
        }

        #region insert,edit,delete

        [HttpPost]
        public object GetItem(int id)
        {
            var data = _context.CommonSettings.FirstOrDefault(x => x.SettingID == id);
            return Json(data);
        }
        [HttpPost]
        public object Insert([FromBody]CommonSetting data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var checkExist = _context.CommonSettings.FirstOrDefault(x => x.CodeSet.ToLower() == data.CodeSet.ToLower());
                if (checkExist != null)
                {
                    msg.Error = true;
                    msg.Title = "Mã cài đặt đã tồn tại!";
                }
                else
                {
                    data.CreatedBy = ESEIM.AppContext.UserName;
                    data.CreatedTime = DateTime.Now;
                    _context.CommonSettings.Add(data);
                    _context.SaveChanges();
                    msg.Title = "Thêm mới cài đặt thành công";
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi thêm!";
            }
            return Json(msg);
        }

        [HttpPost]
        public object Update([FromBody]CommonSetting data)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                data.UpdatedBy = ESEIM.AppContext.UserName;
                data.UpdatedTime = DateTime.Now;
                _context.CommonSettings.Update(data);
                _context.SaveChanges();

                msg.Title = "Cập nhật thành công";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi cập nhật!";
                msg.Object = ex;
                return Json(msg);
            }
        }

        [HttpPost]
        public JsonResult Delete([FromBody]int id)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var data = _context.CommonSettings.FirstOrDefault(x => x.SettingID == id);
                _context.CommonSettings.Remove(data);
                _context.SaveChanges();
                msg.Title = "Xóa cài đặt thành công";
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi xảy ra khi xóa!";
            }
            return Json(msg);
        }
        #endregion
    }
}