using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_salary_tutor_HD")]
    public class edu_salary_tutor_HD
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
    }
}
