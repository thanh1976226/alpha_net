using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_salary_teacher_year")]
    public class edu_salary_teacher_year
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? year_salary { get; set; }
        public int? total_schedule { get; set; }
        public int? total_not_schedule { get; set; }
        public int? level_bonus { get; set; }
        public double? bonus { get; set; }
        public int? id_teacher { get; set; }
        public double? salary_schedule { get; set; }
        public double? salary_not_schedule { get; set; }
        public string note { get; set; }
        public int? userid { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public bool? status { get; set; }
        public byte? flag { get; set; }
    }
}
