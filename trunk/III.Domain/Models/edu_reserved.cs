using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_reserved")]
    public  class edu_reserved
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string title { get; set; }
        public int? class_id { get; set; }
        public int? student_id { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public string note { get; set; }
        public int? user_create { get; set; }
        public double? price { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? debt_id { get; set; }
        public int? course_id { get; set; }
        public int? percent_deferment { get; set; }
        public byte? flag { get; set; }
        public int? heir { get; set; }
    }
}
