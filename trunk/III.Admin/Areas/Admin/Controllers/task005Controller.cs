
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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace ESEIM.Controllers
{
    [Area("Admin")]
    public class task005Controller : Controller
    {
        private readonly EIMDBContext _context;
        public task005Controller(EIMDBContext context)
        {
            _context = context;
           
        }
        public IActionResult Index()
        {
            return View("Index");
        }
        public class JTableModelCustom : JTableModel
        {
            public string Key { get; set; }
            public string Key4 { get; set; }

        }

        //view dữ liệu ra bảng
        public object jtable([FromBody]JTableModelCustom jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.edu_student_contact
                        join b in _context.common_properties
                        on a.relationship equals b.id
                        where a.flag == 1 && b.code.Contains("relative_") && a.studentID ==4 && (jTablePara.Key == null || jTablePara.Key == "" || a.name.ToLower().Contains(jTablePara.Key.ToLower()))
                      
                        select new
                        {
                            id = a.id,
                            name = a.name,
                            relationship =b.value,
                            address = a.address,
                            company = a.company,
                
                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "name", "relationship", "address", "company");
            return Json(jdata);
        }




        public object jtableHis([FromBody]JTableModelCustom jTablePara)
        {
            int intBeginFor = (jTablePara.CurrentPage - 1) * jTablePara.Length;
            var query = from a in _context.edu_student_contact_history
                       join b in _context.edu_student_contact
                       on a.edu_student_contact_id equals b.id
                        where b.studentID == 4
                        select new
                        {
                           a.id,
                           a.date_change,
                           a.column_change,
                           a.old_value,
                           a.new_value

                        };
            var count = query.Count();
            var data = query
                  .OrderByDescending("id").Skip(intBeginFor).Take(jTablePara.Length).AsNoTracking().ToList();
            var jdata = JTableHelper.JObjectTable(data, jTablePara.Draw, count, "id", "date_change", "column_change", "old_value", "new_value");
            return Json(jdata);
        }

        //insert dữ liệu

        [HttpPost]
        public JsonResult insert([FromBody]edu_student_contact obj)
        {
            var msg = new JMessage() { Error = false };
            try
            {

                edu_student_contact obj1 = new edu_student_contact();
                    obj1.name = obj.name;
                    obj1.company = obj.company;
                    obj1.telephone = obj.telephone;
                   obj1.career = obj.career;
                    obj1.mobilephone = obj.mobilephone;
                    obj1.email = obj.email;
                    obj1.relationship = obj.relationship;
                    obj1.address = obj.address;
                    obj1.note = obj.note;
                    obj1.position = obj.position;
                    obj1.address = obj.address;
                    obj1.note = obj.note;
                   obj1.studentID = 4;
                    obj1.flag = 1;
                  
                    _context.edu_student_contact.Add(obj1);
                    _context.SaveChanges();
                    msg.Title = "Thêm thành công";
          
               
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Object = ex;
                msg.Title = "Có lỗi khi thêm ";
            }
            return Json(msg);
        }

        //update dữ liệu
        [HttpPost]
        public JsonResult update([FromBody]edu_student_contact obj)
        {
            var msg = new JMessage() { Error = true };
            try
            {
                edu_student_contact_history his = new edu_student_contact_history();
                var rs1 = _context.edu_student_contact.SingleOrDefault(x => x.id == obj.id);
                var rs = _context.edu_student_contact.SingleOrDefault(x => x.id == obj.id);
                if (rs != null)
                {
                    rs.id = obj.id;
                    rs.name = obj.name;
                    rs.company = obj.company;
                    rs.telephone = obj.telephone;                 
                    rs.mobilephone = obj.mobilephone;
                    rs.email = obj.email;
                    rs.relationship = obj.relationship;                  
                    rs.address = obj.address;
                    rs.note = obj.note;
                    rs.studentID = obj.studentID;
                    rs.position = obj.position;
                    rs.career = obj.career;
                    _context.edu_student_contact.Update(rs);
                    if(rs1.name != obj.name )
                    {
                        his.date_change = DateTime.Now;
                        his.column_change = "name";
                        his.old_value = rs1.name;
                        his.old_value = obj.name;
                        his.edu_student_contact_id = obj.id;
                        _context.edu_student_contact_history.Add(his);
                    }
                    if (rs1.company != obj.company)
                    {
                        his.date_change = DateTime.Now;
                        his.column_change = "company";
                        his.old_value = rs1.company;
                        his.old_value = obj.company;
                        his.edu_student_contact_id = obj.id;
                        _context.edu_student_contact_history.Add(his);
                    }
                    if (rs1.telephone != obj.telephone)
                    {
                        his.date_change = DateTime.Now;
                        his.column_change = "telephone";
                        his.old_value = rs1.telephone;
                        his.old_value = obj.telephone;
                        his.edu_student_contact_id = obj.id;
                        _context.edu_student_contact_history.Add(his);
                    }
                    if (rs1.mobilephone != obj.mobilephone)
                    {
                        his.date_change = DateTime.Now;
                        his.column_change = "mobilephone";
                        his.old_value = rs1.mobilephone;
                        his.old_value = obj.mobilephone;
                        his.edu_student_contact_id = obj.id;
                        _context.edu_student_contact_history.Add(his);
                    }
                    if (rs1.email != obj.email)
                    {
                        his.date_change = DateTime.Now;
                        his.column_change = "email";
                        his.old_value = rs1.email;
                        his.old_value = obj.email;
                        his.edu_student_contact_id = obj.id;
                        _context.edu_student_contact_history.Add(his);
                    }
                    if (rs1.relationship != rs.relationship)
                    {
                        his.date_change = DateTime.Now;
                        his.column_change = "relationship";
                        his.old_value = rs1.relationship.ToString();
                        his.old_value = obj.relationship.ToString();
                        his.edu_student_contact_id = obj.id;
                        _context.edu_student_contact_history.Add(his);
                    }

                    if (rs1.address != obj.address)
                    {
                        his.date_change = DateTime.Now;
                        his.column_change = "address";
                        his.old_value = rs1.address;
                        his.old_value = obj.address;
                        his.edu_student_contact_id = obj.id;
                        _context.edu_student_contact_history.Add(his);
                    }

                    if (rs1.note != obj.note)
                    {
                        his.date_change = DateTime.Now;
                        his.column_change = "note";
                        his.old_value = rs1.note;
                        his.old_value = obj.note;
                        his.edu_student_contact_id = obj.id;
                        _context.edu_student_contact_history.Add(his);
                    }

                    if (rs1.position != rs.position)
                    {
                        his.date_change = DateTime.Now;
                        his.column_change = "position";
                        his.old_value = rs1.position.ToString();
                        his.old_value = obj.position.ToString();
                        his.edu_student_contact_id = obj.id;
                        _context.edu_student_contact_history.Add(his);
                    }

                    if (rs1.career != rs.career)
                    {
                        his.date_change = DateTime.Now;
                        his.column_change = "career";
                        his.old_value = rs1.career.ToString();
                        his.old_value = obj.career.ToString();
                        his.edu_student_contact_id = obj.id;
                        _context.edu_student_contact_history.Add(his);
                    }
                    _context.SaveChanges();
                    msg.Title = "Cập nhật thông tin thành công";
                    msg.Error = false;

                }
            }
            catch (Exception ex)
            {
                msg.Object = ex;
                msg.Title = "Có lỗi khi cập nhật";

            }
            return Json(msg);
        }


        //getItem


        [HttpGet]
        public object getitem(int id)
        {
            try
            {
          //      var booking = _context.Location.SingleOrDefault(x => x.id == id);
                var query = from a in _context.edu_student_contact

                            where a.id == id
                            select new
                            {
                                id=a.id,
                                name = a.name,
                                company = a.company,
                                telephone = a.telephone,
                                mobilephone = a.mobilephone,
                                email = a.email,
                                relationship = a.relationship,
                                address = a.address,
                                note = a.note,

                                studentID = a.studentID,
                                position = a.position,
                                career = a.career,
                            };
                var data = query.Where(x => x.id == id);

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new JMessage() { Error = true, Title = String.Format(CommonUtil.ResourceValue("MSG_LOAD_FAIL"), CommonUtil.ResourceValue("USER_USERNAME").ToLower()), Object = ex });
            }
        }





        //delete
        [HttpPost]
        public object delete(int id)
        {
            var msg = new JMessage { Error = true };
            try
            {
                var data = _context.edu_student_contact.FirstOrDefault(x => x.id == id);
                data.flag = 0;
                _context.edu_student_contact.Update(data);
                _context.SaveChanges();
                msg.Error = false;
                msg.Title = "xoá thành công!";
                return Json(msg);
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Title = "Có lỗi khi xóa.";
                return Json(msg);
            }
        }

        [HttpPost]
        public object gettreedataRelationship()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.common_properties.OrderBy(x => x.id).Where(x => x.flag == 1 && x.code.Contains("relative_"));
                msg.Object = data;
                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }

        [HttpPost]
        public object gettreedataPosition()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.common_properties.OrderBy(x => x.id).Where(x => x.flag == 1 && x.code.Contains("POSITION_EMPLOYEE_"));
                msg.Object = data;
                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }

        [HttpPost]
        public object gettreedataCareer()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.common_properties.OrderBy(x => x.id).Where(x => x.flag == 1 && x.code.Contains("CAREER_EMPLOYEE_"));
                msg.Object = data;
                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }


        [HttpPost]
        public object gettreedataLevel()
        {
            var msg = new JMessage { Error = true };

            try
            {
                var data = _context.common_properties.OrderBy(x => x.id).Where(x => x.flag == 1 && x.code.Contains("LevelStudent_"));
                msg.Object = data;
                msg.Error = false;
            }
            catch (Exception ex)
            {
                msg.Title = "Get Parent Group fail ";
            }

            return Json(msg);
        }


    }
}