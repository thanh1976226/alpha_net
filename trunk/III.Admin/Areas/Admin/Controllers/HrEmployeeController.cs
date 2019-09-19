using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace III.Admin.Controllers
{
    [Area("Admin")]
    public class HrEmployeeController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly EIMDBContext _context;
        private readonly ILogger _logger;
        private readonly IActionLogService _actionLog;
        private readonly AppSettings _appSettings;
        private readonly IUploadService _upload;
        private static int? id_emp;
        public class Address
        {

            public int id { get; set; }
            public string Permanent_Address { get; set; }


            public string Now_Address { get; set; }


            public string Phone { get; set; }
            public string Start_Time { get; set; }
            public string End_Time { get; set; }
            public int Employee_Id { get; set; }
            public DateTime? Created_Time { get; set; }
            public DateTime? Updated_Time { get; set; }

            public int? flag { get; set; }


            public string Created_By { get; set; }


            public string Updated_By { get; set; }
        }

        public class Contact
        {
            public int id { get; set; }
            public string Name { get; set; }

            public string Relationship { get; set; }


            public string Address { get; set; }


            public string Phone { get; set; }


            public string Job_Name { get; set; }


            public string Fax { get; set; }

            public string Email { get; set; }


            public string Note { get; set; }

            public int Employee_Id { get; set; }

            public DateTime? Created_Time { get; set; }
            public DateTime? Updated_Time { get; set; }


            public string Created_By { get; set; }


            public string Updated_By { get; set; }
            public string Birthday { get; set; }
            public int? flag { get; set; }


        }

        public class Contract
        {
            public int id { get; set; }
            public double? Insuarance { get; set; }
            public string Dates_of_pay { get; set; }

            public string Place_Work { get; set; }

            public string Exp_time_work { get; set; }

            public double? Salary_Ratio { get; set; }


            public string Payment { get; set; }

            public int? Contract_Type { get; set; }

            public int? Signer_Id { get; set; }

            public int? Employee_Id { get; set; }
            public double? Salary { get; set; }
            public string Start_Time { get; set; }
            public string End_Time { get; set; }
            public string DateOf_LaborBook { get; set; }

            public string Place_LaborBook { get; set; }


            public string Work_Content { get; set; }

            public string Allowance { get; set; }


            public string Contract_Code { get; set; }

            public string LaborBook_Code { get; set; }

            public string File { get; set; }

            public string Other_Agree { get; set; }


            public string Info_Insuarance { get; set; }


            public string Info_Contract { get; set; }


            public double? Bonus { get; set; }

            public string Tools_Work { get; set; }

            public int? Active { get; set; }


            public string Type_Money { get; set; }
            public int? Value_time_work { get; set; }

            public int? flag { get; set; }
        }

        public class working_process
        {
            public int id_qt { get; set; }
            //public int Employee_Id { get; set; }
            public string Start_Time1 { get; set; }
            public string End_Date1 { get; set; }
            public string Description1 { get; set; }
            public string Wage_Level { get; set; }
            public double? Salary_Ratio { get; set; }
            //public DateTime? Created_Time { get; set; }
            //public DateTime? Updated_Time { get; set; }
            public int? flag { get; set; }
        }

        public class workflows
        {
            public int id_cv { get; set; }
            public string Working_Process { get; set; }
            public int Employee_Id { get; set; }

            public string Description2 { get; set; }

            public string Name_Job { get; set; }

            public string Info_Details { get; set; }

            public string Created_By { get; set; }

            public string Updated_By { get; set; }
            public DateTime? Created_Time { get; set; }
            public DateTime? Updated_Time { get; set; }

            public int? flag { get; set; }
        }

        public class training_course
        {
            public int id_bccc { get; set; }
            public string Result { get; set; }
            //public int Employee_Id { get; set; }
            public string Start_Time3 { get; set; }
            public string End_Time3 { get; set; }
            public string Received_Place { get; set; }
            public string Traing_Place { get; set; }
            public string Certificate_Name { get; set; }
            public string Education_Name { get; set; }
            public string Info_Details1 { get; set; }

            public string Created_By { get; set; }
            public string Updated_By { get; set; }
            public DateTime? Created_Time { get; set; }
            public DateTime? Updated_Time { get; set; }
            public int? flag { get; set; }

        }
        public HrEmployeeController(EIMDBContext context, IUploadService upload, ILogger<HrEmployeeController> logger, IOptions<AppSettings> appSettings, IHostingEnvironment hostingEnvironment, IActionLogService actionLog)
        {
            _context = context;
            _upload = upload;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _actionLog = actionLog;
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            return View("Index");
        }
        public class JTableModelCustom : JTableModel
        {
            public string Key { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }
        #region Index
        [HttpPost]
        public object JTable([FromBody]JTableModelCustom jTablePara)
        {
            var fromDate = !string.IsNullOrEmpty(jTablePara.FromDate) ? DateTime.ParseExact(jTablePara.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            var toDate = !string.IsNullOrEmpty(jTablePara.ToDate) ? DateTime.ParseExact(jTablePara.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.HREmployees
                        where a.flag == 1
                        && (string.IsNullOrEmpty(jTablePara.Key) || a.fullname.ToLower().Contains(jTablePara.Key.ToLower()))
                        && ((fromDate == null || (a.createtime >= fromDate)) && (toDate == null || (a.createtime <= toDate)))
                        select new
                        {
                            a.Id,
                            a.fullname,
                            a.gender,
                            a.phone,
                            a.permanentresidence,
                            a.picture,
                        };

            var count = query.Count();
            var data = query.OrderUsingSortExpression(jTablePara.QueryOrderBy).AsNoTracking().ToList();
            var data1 = data.Skip(intBeginFor).Take(jTablePara.Length).ToList();
            var jdata = JTableHelper.JObjectTable(data1, jTablePara.Draw, count, "Id", "fullname", "gender", "phone", "picture", "permanentresidence");
            return Json(jdata);
        }

        [HttpPost]
        public object Gettreedataunit()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.AdGroupUsers.OrderBy(x => x.GroupUserId);
                msg.Object = data;
                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }

        [HttpGet]
        public object GetItem(int? id)
        {
            if (id == null || id < 0)
            {
                return Json("");
            }
            var a = _context.HREmployees.AsNoTracking().Single(m => m.Id == id);
            id_emp = id;
            return Json(a);
        }

        [HttpPost]
        public object GetPosition()
        {
            var query = _context.Roles.Where(x => x.Status == true).Select(x => new { x.Code, x.Title }).AsParallel();
            return query;
        }

        [HttpGet]
        public object SetEmployeeId(int id)
        {
            id_emp = id;
            return null;
        }

        [HttpPost]
        public object GetItemT()
        {

            if (id_emp == null || id_emp < 0)
            {
                return Json("");
            }
            var a = _context.HREmployees.AsNoTracking().Single(m => m.Id == id_emp);
            return Json(a);
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
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_UPLOAD_FILE"));
                return Json(msg);
            }
        }

        [HttpPost]
        public object UploadFile(IFormFile fileUpload)
        {
            var msg = new JMessage();
            try
            {
                var upload = _upload.UploadFile(fileUpload, Path.Combine(_hostingEnvironment.WebRootPath, "uploads\\files"));
                msg.Error = false;
                msg.Title = "";
                msg.Object = upload.Object;
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_UPLOAD_FILE"));
                msg.Object = ex;
                return Json(msg);
            }
        }
        [HttpPost]
        public object Insert([FromBody]HREmployee obj1)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var rs = _context.HREmployees.FirstOrDefault(x => x.Id == obj1.Id);
                if (rs == null)
                {
                    obj1.createtime = DateTime.Now;
                    obj1.flag = 1;
                    _context.HREmployees.Add(obj1);
                    _context.SaveChanges();

                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("HR_HR_MAN_MSG_EMPLOYEE"));
            }
                else
                {
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_EXITS"), CommonUtil.ResourceValue("HR_HR_MAN_MSG_CODE"));
                    msg.Error = true;
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
        public object Update([FromBody]HREmployee obj)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var rs = _context.HREmployees.FirstOrDefault(x => x.Id == obj.Id);

                if (rs != null)
                {
                    rs.Id = obj.Id;

                    rs.fullname = obj.fullname;
                    rs.nickname = obj.nickname;
                    rs.gender = obj.gender;
                    rs.nation = obj.nation;
                    rs.religion = obj.religion;
                    rs.birthday = obj.birthday;

                    rs.birthofplace = obj.birthofplace;
                    rs.permanentresidence = obj.permanentresidence;
                    rs.phone = obj.phone;
                    rs.factiondate = obj.factiondate;
                    rs.educationallevel = obj.educationallevel;

                    rs.languagelevel = obj.languagelevel;
                    rs.health = obj.health;
                    rs.identitycard = obj.identitycard;
                    rs.identitycarddate = obj.identitycarddate;
                    rs.identitycardplace = obj.identitycardplace;


                    rs.socialinsurance = obj.socialinsurance;
                    rs.socialinsurancedate = obj.socialinsurancedate;
                    rs.socialinsuranceplace = obj.socialinsuranceplace;
                    rs.identification = obj.identification;
                    rs.unit = obj.unit;

                    rs.wage = obj.wage;
                    rs.salarytype = obj.salarytype;
                    rs.salarygroup = obj.salarygroup;
                    rs.salaryfactor = obj.salaryfactor;
                    rs.trainingschool = obj.trainingschool;

                    rs.trainingtime = obj.trainingtime;
                    rs.trainingtype = obj.trainingtype;
                    rs.disciplines = obj.disciplines;
                    rs.specialized = obj.specialized;
                    rs.taxcode = obj.taxcode;


                    rs.position = obj.position;
                    rs.employeekind = obj.employeekind;
                    rs.emailuser = obj.emailuser;
                    rs.emailpassword = obj.emailpassword;
                    rs.nationlaty = obj.nationlaty;

                    rs.employeetype = obj.employeetype;
                    rs.bank = obj.bank;
                    rs.accountholder = obj.accountholder;
                    rs.accountopenplace = obj.accountopenplace;
                    rs.accountnumber = obj.accountnumber;

                    rs.maritalstatus = obj.maritalstatus;
                    rs.computerskill = obj.computerskill;
                    rs.employeegroup = obj.employeegroup;
                    rs.language = obj.language;
                    rs.picture = obj.picture;
                    rs.updatetime = DateTime.Now;
                    _context.HREmployees.Update(rs);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_SUCCESS"));
                }
                else
                {
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ERR_RETRY"));
                    msg.Error = true;
                }

            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_FAIL"));
            }
            return Json(msg);
        }

        [HttpPost]
        public object Delete(int id)
        {
            var msg = new JMessage { Error = false };
            try
            {
                var data = _context.HREmployees.FirstOrDefault(x => x.Id == id);
                data.flag = 0;
                _context.HREmployees.Update(data);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("HR_HR_MAN_MSG_EMPLOYEE"));
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_DELETE"));
                return Json(msg);
            }
        }
        [HttpPost]
        public object DeleteItems([FromBody]List<int> listIdI)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                foreach (var id in listIdI)
                {
                    HREmployee obj = _context.HREmployees.FirstOrDefault(x => x.Id == id);
                    if (obj != null)
                    {
                        obj.flag = 0;
                        _context.HREmployees.Update(obj);
                        _context.SaveChanges();
                    }
                }
                msg.Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_LIST_FAIL"), CommonUtil.ResourceValue("HR_HR_MAN_MSG_CATEGORY"));
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_DELETE"));
            }
            return Json(msg);
        }
        #endregion

        #region Address
        [HttpPost]
        public object JTableAddress([FromBody]JTableModelCustom jTablePara)
        {

            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.HRAddress
                        where a.flag == 1 && a.Employee_Id == id_emp
                        select new
                        {
                            id_dc = a.id,
                            Permanent_Address = a.Permanent_Address,
                            Now_Address = a.Now_Address,
                            Start_Time = a.Start_Time,
                            End_Time = a.End_Time,
                            Phone = a.Phone

                        };
            var count = query.Count();
            var data = query
                .OrderUsingSortExpression("id_dc").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id_dc", "Phone", "Permanent_Address", "Now_Address", "Start_Time", "End_Time");
            return Json(jdata);
        }

        [HttpGet]
        public object GetitemAddress(int id)
        {
            try
            {
                var data = _context.HRAddress.SingleOrDefault(x => x.id == id);
                return data;
            }
            catch (Exception ex)
            {
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
            }
        }

        [HttpPost]
        public JsonResult InsertAddress([FromBody]Address obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var obj1 = new HRAddress();
                obj1.Permanent_Address = obj.Permanent_Address;
                obj1.Now_Address = obj.Now_Address;
                obj1.End_Time = string.IsNullOrEmpty(obj.End_Time) ? (DateTime?)null : DateTime.ParseExact(obj.End_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj1.Start_Time = string.IsNullOrEmpty(obj.Start_Time) ? (DateTime?)null : DateTime.ParseExact(obj.Start_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj1.Phone = obj.Phone;
                obj1.flag = 1;
                obj1.Created_Time = DateTime.Now;
                obj1.Employee_Id = id_emp;
                _context.HRAddress.Add(obj1);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("HR_HR_MAN_MSG_ADDRESS"));
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_ADD"));
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult UpdateAddress([FromBody]Address obj)
        {

            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var rs = _context.HRAddress.FirstOrDefault(x => x.id == obj.id);
                if (rs != null)
                {
                    rs.Now_Address = obj.Now_Address;
                    rs.Phone = obj.Phone;
                    rs.End_Time = string.IsNullOrEmpty(obj.End_Time) ? (DateTime?)null : DateTime.ParseExact(obj.End_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    rs.Start_Time = string.IsNullOrEmpty(obj.Start_Time) ? (DateTime?)null : DateTime.ParseExact(obj.Start_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    obj.Updated_Time = DateTime.Now;
                    _context.HRAddress.Update(rs);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("HR_HR_MAN_MSG_ADDRESS"));
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_FAIL"));
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult DeleteAddress(int id)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var data = _context.HRAddress.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                _context.HRAddress.Remove(data);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("HR_HR_MAN_MSG_ADDRESS"));
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_DELETE"));
            }
            return Json(msg);

        }
        #endregion

        #region Contact

        [HttpPost]
        public object JTableLH([FromBody]JTableModelCustom jTablePara)
        {

            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.HRContacts
                        where a.flag == 1 && a.Employee_Id == id_emp
                        select new
                        {
                            id_lh = a.id,
                            Name = a.Name,
                            Relationship = a.Relationship,
                            Address = a.Address,
                            Phone1 = a.Phone

                        };
            var count = query.Count();
            var data = query
                .OrderUsingSortExpression("id_lh").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id_lh", "Name", "Relationship", "Address", "Phone1");
            return Json(jdata);
        }

        [HttpGet]
        public object GetitemLH(int id)
        {
            try
            {
                var data = _context.HRContacts.FirstOrDefault(x => x.id == id);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
            }
        }
        [HttpPost]
        public JsonResult InsertLH([FromBody]Contact obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var obj1 = new HRContact();
                obj1.Name = obj.Name;
                obj1.Relationship = obj.Relationship;
                obj1.Address = obj.Address;
                obj1.Birthday = string.IsNullOrEmpty(obj.Birthday) ? (DateTime?)null : DateTime.ParseExact(obj.Birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                obj1.Phone = obj.Phone;
                obj1.Job_Name = obj.Job_Name;
                obj1.Fax = obj.Fax;
                obj1.Email = obj.Email;
                obj1.Note = obj.Note;
                obj1.flag = 1;
                obj1.Created_Time = DateTime.Now;
                obj1.Employee_Id = id_emp;
                _context.HRContacts.Add(obj1);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ADD_SUCCESS"), CommonUtil.ResourceValue("HR_HR_MAN_MSG_CONTACT"));
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_ADD"));
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult UpdateLH([FromBody]Contact obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                var rs = _context.HRContacts.SingleOrDefault(x => x.id == obj.id);
                if (rs != null)
                {
                    rs.Name = obj.Name;
                    rs.Phone = obj.Phone;
                    rs.Relationship = obj.Relationship;
                    rs.Updated_Time = DateTime.Now;
                    rs.Address = obj.Address;
                    rs.Job_Name = obj.Job_Name;
                    rs.Fax = obj.Fax;
                    rs.Email = obj.Email;
                    rs.Note = obj.Note;
                    rs.Birthday = string.IsNullOrEmpty(obj.Birthday) ? (DateTime?)null : DateTime.ParseExact(obj.Birthday, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    _context.HRContacts.Update(rs);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"), CommonUtil.ResourceValue("HR_HR_MAN_MSG_CONTACT"));
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"));
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult DeleteLH(int id)
        {
            var msg = new JMessage { Error = false };
            try
            {
                var data = _context.HRContacts.FirstOrDefault(x => x.id == id);
                _context.HRContacts.Remove(data);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_UPDATE_SUCCESS"));

                    
                    
                    
                    
                    
                    
                   
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_DELETE"));
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult DeleteItemsLH([FromBody]List<int> listIdI)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                foreach (var id in listIdI)
                {
                    var obj = _context.HRContacts.FirstOrDefault(x => x.id == id);
                    if (obj != null)
                    {
                        _context.HRContacts.Remove(obj);
                        _context.SaveChanges();
                    }
                }
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_DELETE_SUCCESS"), CommonUtil.ResourceValue("HR_HR_MAN_MSG_CONTACT"));
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_DELETE"));
            }
            return Json(msg);
        }
        #endregion

        #region Work progress
        [HttpPost]
        public object JTableQTLV([FromBody]JTableModelCustom jTablePara)
        {

            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.HRWorkingProcesss
                        where a.flag == 1 && a.Employee_Id == id_emp
                        select new
                        {
                            id_qt = a.id,
                            Start_Time1 = a.Start_Time,
                            End_Date1 = a.End_Date,
                            Wage_Level = a.Wage_Level,
                            Salary_Ratio = a.Salary_Ratio

                        };
            var count = query.Count();
            var data = query
                .OrderUsingSortExpression("id_qt").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id_qt", "Start_Time1", "End_Date1", "Wage_Level", "Salary_Ratio");
            return Json(jdata);
        }
        [HttpPost]
        public JsonResult InsertQTLV([FromBody]working_process obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                HRWorkingProcess obj1 = new HRWorkingProcess();
                obj1.Start_Time = string.IsNullOrEmpty(obj.Start_Time1) ? (DateTime?)null : DateTime.ParseExact(obj.Start_Time1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj1.End_Date = string.IsNullOrEmpty(obj.End_Date1) ? (DateTime?)null : DateTime.ParseExact(obj.End_Date1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj1.Description = obj.Description1;
                //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                obj1.Wage_Level = obj.Wage_Level;
                obj1.Salary_Ratio = obj.Salary_Ratio;
                obj1.flag = 1;
                obj1.Created_Time = DateTime.Now;
                obj1.Employee_Id = id_emp;
                _context.HRWorkingProcesss.Add(obj1);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ADD_SUCCESS"));
        }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_ADD"));
            }
            return Json(msg);
        }
        [HttpPost]
        public object DeleteQTLV(int id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.HRWorkingProcesss.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                _context.HRWorkingProcesss.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_DELETE_SUCCESS"));
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_DELETE"));
                return Json(msg);
            }
        }
        [HttpPost]
        public object DeleteItemsQTLV([FromBody]List<int> listIdI)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                foreach (var id in listIdI)
                {
                    HRWorkingProcess obj = _context.HRWorkingProcesss.FirstOrDefault(x => x.id == id);
                    if (obj != null)
                    {
                        obj.flag = 0;
                        _context.HRWorkingProcesss.Update(obj);
                        _context.SaveChanges();
                    }
                }
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_DELETE_SUCCESS"));
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_LIST_FAIL"), CommonUtil.ResourceValue("RESOURCE"));
                //_logger.LogError(LoggingEvents.LogDb, "Delete list Resource fail");
                //_actionLog.InsertActionLogDeleteItem("AdResource", "An error occurred while Delete list Resource", null, null, "Error");

            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult UpdateQTLV([FromBody]working_process obj)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var rs = _context.HRWorkingProcesss.SingleOrDefault(x => x.id == obj.id_qt);
                if (rs != null)
                {
                    rs.id = obj.id_qt;
                    rs.Start_Time = string.IsNullOrEmpty(obj.Start_Time1) ? (DateTime?)null : DateTime.ParseExact(obj.Start_Time1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    rs.End_Date = string.IsNullOrEmpty(obj.End_Date1) ? (DateTime?)null : DateTime.ParseExact(obj.End_Date1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    rs.Description = obj.Description1;
                    rs.Updated_Time = DateTime.Now;
                    rs.Wage_Level = obj.Wage_Level;
                    rs.Salary_Ratio = obj.Salary_Ratio;
                    _context.HRWorkingProcesss.Update(rs);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_SUCCESS"));
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_FAIL"));
            }
            return Json(msg);
        }

        [HttpGet]
        public object GetitemQTLV(int id)
        {
            var msg = new JMessage { Error = false, Title = "" };
            try
            {
                var data = _context.HRWorkingProcesss.FirstOrDefault(x => x.id == id);
                if (data != null)
                {
                    var model = new working_process
                    {
                        id_qt = data.id,
                        Start_Time1 = data.Start_Time.HasValue ? data.Start_Time.Value.ToString("dd/MM/yyyy") : null,
                        End_Date1 = data.End_Date.HasValue ? data.End_Date.Value.ToString("dd/MM/yyyy") : null,
                        Wage_Level = data.Wage_Level,
                        Salary_Ratio = data.Salary_Ratio,
                        Description1 = data.Description,
                    };
                    msg.Object = model;
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_FAIL"));
            }
            return Json(msg);
        }
        #endregion

        #region Workflow
        [HttpPost]
        public object JTableQTCV([FromBody]JTableModelCustom jTablePara)
        {

            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.HRWorkFlows
                        where a.flag == 1 && a.Employee_Id == id_emp
                        select new
                        {
                            id_cv = a.id,
                            Name_Job = a.Name_Job,
                            Working_Process = a.Working_Process,

                        };
            var count = query.Count();
            var data = query
                .OrderUsingSortExpression("id_cv").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id_cv", "Name_Job", "Working_Process");
            return Json(jdata);
        }
        [HttpPost]
        public JsonResult InsertQTCV([FromBody]workflows obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                HRWorkFlows obj1 = new HRWorkFlows();
                obj1.Working_Process = obj.Working_Process;
                obj1.Name_Job = obj.Name_Job;
                obj1.Description = obj.Description2;
                //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                obj1.Info_Details = obj.Info_Details;

                obj1.flag = 1;
                obj1.Created_Time = DateTime.Now;
                obj1.Employee_Id = id_emp;
                _context.HRWorkFlows.Add(obj1);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ADD_SUCCESS"));
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_ADD"));
            }
            return Json(msg);
        }
        [HttpPost]
        public object DeleteQTCV(int id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.HRWorkFlows.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                _context.HRWorkFlows.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_DELETE_SUCCESS"));
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_DELETE"));
                return Json(msg);
            }
        }
        [HttpPost]
        public object DeleteItemsQTCV([FromBody]List<int> listIdI)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                foreach (var id in listIdI)
                {
                    HRWorkFlows obj = _context.HRWorkFlows.FirstOrDefault(x => x.id == id);
                    if (obj != null)
                    {
                        obj.flag = 0;
                        _context.HRWorkFlows.Update(obj);
                        _context.SaveChanges();
                    }
                }
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_DELETE_SUCCESS"));
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_LIST_FAIL"), CommonUtil.ResourceValue("RESOURCE"));
                //_logger.LogError(LoggingEvents.LogDb, "Delete list Resource fail");
                //_actionLog.InsertActionLogDeleteItem("AdResource", "An error occurred while Delete list Resource", null, null, "Error");

            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult UpdateQTCV([FromBody]workflows obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                var rs = _context.HRWorkFlows.SingleOrDefault(x => x.id == obj.id_cv);
                if (rs != null)
                {
                    rs.id = obj.id_cv;
                    rs.Working_Process = obj.Working_Process;
                    rs.Name_Job = obj.Name_Job;
                    rs.Description = obj.Description2;
                    rs.Updated_Time = DateTime.Now;
                    rs.Info_Details = obj.Info_Details;

                    _context.HRWorkFlows.Update(rs);

                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_SUCCESS"));
                    msg.Error = false;

                }
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_FAIL"));

            }
            return Json(msg);
        }
        [HttpGet]
        public object GetitemQTCV(int id)
        {
            try
            {
                var booking = _context.HRWorkFlows.SingleOrDefault(x => x.id == id);
                var query = from a in _context.HRWorkFlows
                            join b in _context.HREmployees
                          on a.Employee_Id equals b.Id
                            where b.Id == id_emp
                            select new
                            {
                                id_cv = a.id,
                                Working_Process = a.Working_Process,

                                Name_Job = a.Name_Job,
                                Description2 = a.Description,
                                Info_Details = a.Info_Details,
                                Id = b.Id,
                                fullname = b.fullname,
                                nickname = b.nickname,
                                gender = b.gender,
                                nation = b.nation,
                                religion = b.religion,
                                birthday = b.birthday,
                                birthofplace = b.birthofplace,
                                permanentresidence = b.permanentresidence,
                                phone = b.phone,
                                factiondate = b.factiondate,
                                educationallevel = b.educationallevel,
                                languagelevel = b.languagelevel,
                                health = b.health,
                                identitycard = b.identitycard,

                                identitycarddate = b.identitycarddate,
                                identitycardplace = b.identitycardplace,
                                socialinsurance = b.socialinsurance,
                                socialinsurancedate = b.socialinsurancedate,
                                socialinsuranceplace = b.socialinsuranceplace,
                                identification = b.identification,
                                unit = b.unit,
                                wage = b.wage,
                                salarytype = b.salarytype,
                                salarygroup = b.salarygroup,
                                salaryfactor = b.salaryfactor,
                                trainingschool = b.trainingschool,
                                trainingtime = b.trainingtime,
                                trainingtype = b.trainingtype,

                                disciplines = b.disciplines,
                                picture = b.picture,
                                taxcode = b.taxcode,
                                position = b.position,
                                employeekind = b.employeekind,
                                emailuser = b.emailuser,
                                emailpassword = b.emailpassword,
                                nationlaty = b.nationlaty,
                                status = b.status,
                                employeetype = b.employeetype,
                                bank = b.bank,
                                accountholder = b.accountholder,
                                accountopenplace = b.accountopenplace,
                                accountnumber = b.accountnumber,

                                maritalstatus = b.maritalstatus,
                                computerskill = b.computerskill,
                                employeegroup = b.employeegroup,
                                language = b.language,

                            };
                var data = query.Where(x => x.id_cv == id);

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
            }
        }
        #endregion

        #region Degree
        [HttpPost]
        public object JTableBCCC([FromBody]JTableModelCustom jTablePara)
        {

            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.HRTrainingCourses
                        where a.flag == 1 && a.Employee_Id == id_emp
                        select new
                        {
                            id_bccc = a.id,
                            Education_Name = a.Education_Name,
                            Result = a.Result
                        };
            var count = query.Count();
            var data = query
                .OrderUsingSortExpression("id_bccc").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id_bccc", "Education_Name", "Result");
            return Json(jdata);
        }
        [HttpPost]
        public JsonResult InsertBCCC([FromBody]training_course obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                HRTrainingCourse obj1 = new HRTrainingCourse();
                obj1.Result = obj.Result;
                obj1.Start_Time = string.IsNullOrEmpty(obj.Start_Time3) ? (DateTime?)null : DateTime.ParseExact(obj.Start_Time3, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                obj1.End_Time = string.IsNullOrEmpty(obj.End_Time3) ? (DateTime?)null : DateTime.ParseExact(obj.End_Time3, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                obj1.Received_Place = obj.Received_Place;

                obj1.Traing_Place = obj.Traing_Place;
                obj1.Certificate_Name = obj.Certificate_Name;
                obj1.Education_Name = obj.Education_Name;
                //   obj1.Birthday = Convert.ToDateTime(obj.Birthday);
                obj1.Info_Details = obj.Info_Details1;

                obj1.flag = 1;
                obj1.Created_Time = DateTime.Now;
                obj1.Employee_Id = id_emp;
                _context.HRTrainingCourses.Add(obj1);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ADD_SUCCESS"));
        }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_ADD"));
            }
            return Json(msg);
        }
        [HttpPost]
        public object DeleteBCCC(int id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.HRTrainingCourses.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                _context.HRTrainingCourses.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_DELETE_SUCCESS"));
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_DELETE"));
                return Json(msg);
            }
        }
        [HttpPost]
        public object DeleteItemsBCCC([FromBody]List<int> listIdI)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                foreach (var id in listIdI)
                {
                    HRTrainingCourse obj = _context.HRTrainingCourses.FirstOrDefault(x => x.id == id);
                    if (obj != null)
                    {
                        obj.flag = 0;
                        _context.HRTrainingCourses.Update(obj);
                        _context.SaveChanges();
                    }
                }
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_DELETE_SUCCESS"));
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_LIST_FAIL"), CommonUtil.ResourceValue("RESOURCE"));
                //_logger.LogError(LoggingEvents.LogDb, "Delete list Resource fail");
                //_actionLog.InsertActionLogDeleteItem("AdResource", "An error occurred while Delete list Resource", null, null, "Error");

            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult UpdateBCCC([FromBody]training_course obj)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {
                var rs = _context.HRTrainingCourses.SingleOrDefault(x => x.id == obj.id_bccc);
                if (rs != null)
                {
                    rs.id = obj.id_bccc;
                    rs.Result = obj.Result;
                    rs.Start_Time = string.IsNullOrEmpty(obj.Start_Time3) ? (DateTime?)null : DateTime.ParseExact(obj.Start_Time3, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    rs.End_Time = string.IsNullOrEmpty(obj.End_Time3) ? (DateTime?)null : DateTime.ParseExact(obj.End_Time3, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    rs.Updated_Time = DateTime.Now;
                    rs.Info_Details = obj.Info_Details1;

                    rs.Received_Place = obj.Received_Place;
                    rs.Traing_Place = obj.Traing_Place;
                    rs.Certificate_Name = obj.Certificate_Name;
                    rs.Education_Name = obj.Education_Name;
                    _context.HRTrainingCourses.Update(rs);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_SUCCESS"));
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_FAIL"));
            }
            return Json(msg);
        }
        [HttpGet]
        public object GetitemBCCC(int id)
        {
            try
            {
                var booking = _context.HRTrainingCourses.SingleOrDefault(x => x.id == id);
                var query = from a in _context.HRTrainingCourses
                            join b in _context.HREmployees
                           on a.Employee_Id equals b.Id
                            where b.Id == id_emp
                            select new
                            {
                                id_bccc = a.id,
                                Result = a.Result,

                                Start_Time3 = a.Start_Time,
                                End_Time3 = a.End_Time,
                                Received_Place = a.Received_Place,
                                Traing_Place = a.Traing_Place,
                                Certificate_Name = a.Certificate_Name,

                                Education_Name = a.Education_Name,
                                Info_Details1 = a.Info_Details,


                                Id = b.Id,
                                fullname = b.fullname,
                                nickname = b.nickname,
                                gender = b.gender,
                                nation = b.nation,
                                religion = b.religion,
                                birthday = b.birthday,
                                birthofplace = b.birthofplace,
                                permanentresidence = b.permanentresidence,
                                phone = b.phone,
                                factiondate = b.factiondate,
                                educationallevel = b.educationallevel,
                                languagelevel = b.languagelevel,
                                health = b.health,
                                identitycard = b.identitycard,

                                identitycarddate = b.identitycarddate,
                                identitycardplace = b.identitycardplace,
                                socialinsurance = b.socialinsurance,
                                socialinsurancedate = b.socialinsurancedate,
                                socialinsuranceplace = b.socialinsuranceplace,
                                identification = b.identification,
                                unit = b.unit,
                                wage = b.wage,
                                salarytype = b.salarytype,
                                salarygroup = b.salarygroup,
                                salaryfactor = b.salaryfactor,
                                trainingschool = b.trainingschool,
                                trainingtime = b.trainingtime,
                                trainingtype = b.trainingtype,

                                disciplines = b.disciplines,
                                picture = b.picture,
                                taxcode = b.taxcode,
                                position = b.position,
                                employeekind = b.employeekind,
                                emailuser = b.emailuser,
                                emailpassword = b.emailpassword,
                                nationlaty = b.nationlaty,
                                status = b.status,
                                employeetype = b.employeetype,
                                bank = b.bank,
                                accountholder = b.accountholder,
                                accountopenplace = b.accountopenplace,
                                accountnumber = b.accountnumber,

                                maritalstatus = b.maritalstatus,
                                computerskill = b.computerskill,
                                employeegroup = b.employeegroup,
                                language = b.language,
                            };
                var data = query.Where(x => x.id_bccc == id);

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
            }
        }
        #endregion

        #region Contract
        [HttpPost]
        public object JTableHD([FromBody]JTableModelCustom jTablePara)
        {

            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.HRContracts
                        where a.flag == 1 && a.Employee_Id == id_emp
                        select new
                        {
                            id = a.id,
                            Contract_Code = a.Contract_Code,
                            Start_Time = a.Start_Time,
                            End_Time = a.End_Time,
                            Salary = a.Salary,
                            Insuarance = a.Insuarance,//mức đống bảo hiểm xã hội
                            Type_Money = a.Type_Money,
                            Active = a.Active
                        };
            var count = query.Count();
            var data = query
                .OrderUsingSortExpression("id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "Contract_Code", "Start_Time", "End_Time", "Salary", "Insuarance", "Type_Money", "Active");
            return Json(jdata);
        }
        public object InsertHD(Contract obj, IFormFile File)
        {
            var msg = new JMessage() { Error = false, Title = "" };
            try
            {

                var rs = _context.HRContracts.FirstOrDefault(x => x.Contract_Code == obj.Contract_Code);
                if (rs == null)
                {
                    var rs1 = new HRContract();
                    rs1.File = obj.File;

                    rs1.Insuarance = obj.Insuarance;
                    rs1.Dates_of_pay = string.IsNullOrEmpty(obj.Dates_of_pay) ? (DateTime?)null : DateTime.ParseExact(obj.Dates_of_pay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    rs1.Place_Work = obj.Place_Work;
                    rs1.Exp_time_work = obj.Exp_time_work;
                    rs1.Salary_Ratio = obj.Salary_Ratio;
                    rs1.Payment = obj.Payment;
                    rs1.Contract_Type = obj.Contract_Type;
                    rs1.Signer_Id = obj.Signer_Id;

                    rs1.Salary = obj.Salary;
                    rs1.Start_Time = string.IsNullOrEmpty(obj.Start_Time) ? (DateTime?)null : DateTime.ParseExact(obj.Start_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    rs1.End_Time = string.IsNullOrEmpty(obj.End_Time) ? (DateTime?)null : DateTime.ParseExact(obj.End_Time, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //rs1.DateOf_LaborBook = obj.DateOf_LaborBook;
                    rs1.Place_LaborBook = obj.Place_LaborBook;
                    rs1.Work_Content = obj.Work_Content;
                    rs1.Allowance = obj.Allowance;
                    rs1.Contract_Code = obj.Contract_Code;

                    rs1.LaborBook_Code = obj.LaborBook_Code;
                    rs1.Other_Agree = obj.Other_Agree;
                    rs1.Info_Insuarance = obj.Info_Insuarance;
                    rs1.Info_Contract = obj.Info_Contract;
                    rs1.Bonus = obj.Bonus;
                    rs1.Tools_Work = obj.Tools_Work;

                    rs1.Type_Money = obj.Type_Money;
                    rs1.Value_time_work = obj.Value_time_work;

                    rs1.Employee_Id = id_emp;
                    rs1.Created_Time = DateTime.Now;
                    rs1.flag = 1;
                    if (rs1.End_Time > DateTime.Now)
                    {
                        rs1.Active = 1;
                    }
                    else
                    {
                        rs1.Active = 0;
                    }
                    _context.HRContracts.Add(rs1);
                    _context.SaveChanges();
                    msg.Title = "Thêm mới hợp đồng thành công";
                }
                else
                {
                    msg.Title = "Mã đã tồn tại";
                    msg.Error = true;
                }
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_ADD"));
        }
            return Json(msg);
        }
        public object UpdateHD(HRContract obj, IFormFile File)
        {
            var msg = new JMessage() { Error = false, ID = 1 };
            try
            {

                HRContract rs = _context.HRContracts.FirstOrDefault(x => x.id == obj.id);


                if (rs != null)
                {

                    //var icfile_path = "";

                    //if (File != null && File.Length > 0)
                    //{
                    //    var pathUpload = Path.Combine(_hostingEnvironment.WebRootPath, "File\\avatar");
                    //    if (!Directory.Exists(pathUpload)) Directory.CreateDirectory(pathUpload);

                    //    var fileName = DateTimeOffset.Now.ToUnixTimeMilliseconds() + File.FileName;
                    //    var filePath = Path.Combine(pathUpload, fileName);
                    //    using (var stream = new FileStream(filePath, FileMode.Create))
                    //    {
                    //        await File.CopyToAsync(stream);
                    //    }
                    //    icfile_path = "/File/avatar/" + fileName;
                    //}
                    //if (icfile_path != "")
                    //{
                    //    rs.File = icfile_path;
                    //}
                    rs.File = obj.File;

                    rs.id = obj.id;
                    rs.Insuarance = obj.Insuarance;
                    rs.Dates_of_pay = obj.Dates_of_pay;
                    rs.Place_Work = obj.Place_Work;
                    rs.Exp_time_work = obj.Exp_time_work;
                    rs.Salary_Ratio = obj.Salary_Ratio;

                    rs.Payment = obj.Payment;
                    rs.Contract_Type = obj.Contract_Type;
                    rs.Signer_Id = obj.Signer_Id;
                    rs.Salary = obj.Salary;
                    rs.Start_Time = obj.Start_Time;
                    rs.End_Time = obj.End_Time;

                    rs.DateOf_LaborBook = obj.DateOf_LaborBook;
                    rs.Place_LaborBook = obj.Place_LaborBook;
                    rs.Work_Content = obj.Work_Content;
                    rs.Allowance = obj.Allowance;
                    rs.Contract_Code = obj.Contract_Code;
                    rs.LaborBook_Code = obj.LaborBook_Code;

                    rs.Other_Agree = obj.Other_Agree;
                    rs.Info_Insuarance = obj.Info_Insuarance;
                    rs.Info_Contract = obj.Info_Contract;
                    rs.Bonus = obj.Bonus;
                    rs.Tools_Work = obj.Tools_Work;


                    rs.Type_Money = obj.Type_Money;
                    rs.Value_time_work = obj.Value_time_work;
                    rs.Employee_Id = id_emp;
                    rs.Updated_Time = DateTime.Now;
                    if (obj.End_Time > DateTime.Now)
                    {
                        rs.Active = 1;
                    }
                    else
                    {
                        rs.Active = 0;
                    }
                    _context.HRContracts.Update(rs);
                    _context.SaveChanges();
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_SUCCESS"));
                msg.Error = false;
                    _actionLog.InsertActionLog("HRContact", "update HRContact successfully", rs, obj, "Update");
                }
                else
                {
                    msg.Title = String.Format(CommonUtil.ResourceValue("COM_MSG_ERR_RETRY"));
                    msg.Error = true;
                }


            }
            catch (Exception ex)
            {
                msg.ID = 0;
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_UPDATE_FAIL"));
                _actionLog.InsertActionLog("HRContact", "update HRContact fail", null, obj, "Update");
            }
            return Json(msg);
        }
        [HttpGet]
        public object GetItemHD(int? id)
        {
            if (id == null || id < 0)
            {
                return Json("");
            }
            var a = _context.HRContracts.AsNoTracking().Where(x => x.id == id).Select(x => new
            {
                x.Contract_Code,
                x.LaborBook_Code,
                x.Insuarance,
                x.Dates_of_pay,
                x.Place_Work,
                x.Exp_time_work,
                x.Salary_Ratio,
                x.Payment,
                x.Contract_Type,
                x.Work_Content,
                x.Other_Agree,
                x.Signer_Id,
                x.Salary,
                Start_Time = x.Start_Time.HasValue ? x.Start_Time.Value.ToString("dd/MM/yyyy") : null,
                End_Time = x.End_Time.HasValue ? x.End_Time.Value.ToString("dd/MM/yyyy") : null,
                x.Allowance,
                x.Bonus,
                x.Tools_Work,
                x.Type_Money,
                x.File,
                x.Info_Insuarance,
                x.Info_Contract,
            }).FirstOrDefault();
            return Json(a);
        }

        [HttpPost]
        public object GetCurrencyHD()
        {
            return GetCurrencyBase();
        }

        [HttpPost]
        public JsonResult DeleteHD([FromBody]int id)
        {
            var msg = new JMessage { Error = false };
            try
            {
                var data = _context.HRContracts.FirstOrDefault(x => x.id == id);
                _context.HRContracts.Remove(data);
                _context.SaveChanges();
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_DELETE_SUCCESS"));
        }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_ERR_DELETE"));
            }
            return Json(msg);
        }
        [HttpPost]
        public JsonResult DeleteItemsHD([FromBody]List<int> listIdI)
        {
            var msg = new JMessage() { Error = false };
            try
            {
                foreach (var id in listIdI)
                {
                    HRContract obj = _context.HRContracts.FirstOrDefault(x => x.id == id);
                    if (obj != null)
                    {
                        obj.flag = 0;
                        _context.HRContracts.Update(obj);
                        _context.SaveChanges();
                    }
                }
                msg.Title = String.Format(CommonUtil.ResourceValue("COM_DELETE_SUCCESS"));
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = String.Format(CommonUtil.ResourceValue("MSG_DELETE_LIST_FAIL"), CommonUtil.ResourceValue("RESOURCE"));
            }
            return Json(msg);
        }
        #endregion
    }
}
