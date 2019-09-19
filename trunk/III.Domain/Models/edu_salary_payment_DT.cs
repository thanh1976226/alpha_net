using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_salary_payment_DT")]
    public class edu_salary_payment_DT
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_salary_payment_HD { get; set; }
        public int? id_employee { get; set; }
        public double? luongtinhtheoUsd { get; set; }
        public string chitietcackhoanluong { get; set; }
        public double? sotien { get; set; }
        public double? tigiatiente { get; set; }
        public bool? dathulaiphieuchichua { get; set; }
        public string ghichu { get; set; }
        public DateTime? ngaytra { get; set; }
        public string sophieuchi { get; set; }
        public string note { get; set; }
        public string recipient { get; set; }
        public string nguoitra { get; set; }
    }
}
