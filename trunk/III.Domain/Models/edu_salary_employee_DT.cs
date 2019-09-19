using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_salary_employee_DT")]
    public  class edu_salary_employee_DT
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_salary_employee_HD { get; set; }
        public int? id_employee { get; set; }
        public double? luongcoban { get; set; }
        public double? luongdongbaohiem { get; set; }
        public double? minute_lamviecthang { get; set; }
        public double? minute_lamviecthucte { get; set; }
        public double? minute_nghilehuongnguyenluong { get; set; }
        public double? minute_nghiphephuongBHXH { get; set; }
        public double? minute_nghikhonghuongluong { get; set; }
        public double? minute_lamthem { get; set; }
        public double? salary_dilam { get; set; }
        public double? salary_ngayle { get; set; }
        public double? salary_ngayphep { get; set; }
        public double? salary_lamthemgio { get; set; }
        public double? salary_total_tienluong { get; set; }
        public double? salary_phucap { get; set; }
        public double? salary_thuong1 { get; set; }
        public double? salary_thuong2 { get; set; }
        public double? total_tongluongthuong { get; set; }
        public double? BHXH { get; set; }
        public double? BHYT { get; set; }
        public double? ungluong { get; set; }
        public double? buluong { get; set; }
        public double? total_employee_receive { get; set; }
        public double? total_trichdongBH { get; set; }
        public string chutaikhoan { get; set; }
        public string sotaikhoan { get; set; }
        public string noimotaikhoan { get; set; }
        public DateTime? ngaynhantien { get; set; }
        public string tinhtrang { get; set; }
        public double? BHTN { get; set; }
    }
}
