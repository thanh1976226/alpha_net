using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_salary_employee_HD")]
    public class edu_salary_employee_HD
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public DateTime? month_salary { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? user_id { get; set; }
        public bool? status { get; set; }
        public byte? flag { get; set; }
        public string note { get; set; }
        public double? total_luongcoban { get; set; }
        public double? total_luongbaohiem { get; set; }
        public double? total_minute_month { get; set; }
        public double? total_work_thucte { get; set; }
        public double? total_nghilehuongluong { get; set; }
        public double? total_nghiphephuongBHXH { get; set; }
        public double? total_nghikhongluong { get; set; }
        public double? total_lamthem { get; set; }
        public double? total_salary_dilam { get; set; }
        public double? total_salary_ngayle { get; set; }
        public double? total_salary_ngayphep { get; set; }
        public double? total_salary_lamthemgio { get; set; }
        public double? total_salary { get; set; }
        public double? total_phucap { get; set; }
        public double? total_thuong1 { get; set; }
        public double? total_thuong2 { get; set; }
        public double? total_tongluongthuong { get; set; }
        public double? total_salary_employee_receive { get; set; }
    }
}
