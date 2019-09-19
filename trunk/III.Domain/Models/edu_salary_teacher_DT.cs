using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_salary_teacher_DT")]
    public class edu_salary_teacher_DT
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_salary_teacher_hd { get; set; }
        public int? id_teacher { get; set; }
        public double? total_schedule { get; set; }
        public double? total_not_schedule { get; set; }
        public double? basic_salary { get; set; }
        public double? begin_course_salary { get; set; }
        public double? schedule_salary { get; set; }
        public double? not_schedule_salary { get; set; }
        public double? course_end_bonus { get; set; }
        public string course_end_bonus_note { get; set; }
        public double? course_start_bonus { get; set; }
        public double? bonus_other { get; set; }
        public double? total_salary_bonus { get; set; }
        public double? money_photo { get; set; }
        public double? bonus_sidelines { get; set; }
        public double? total_money_receive { get; set; }
        public double? salary_advances { get; set; }
        public double? total_money_transfer { get; set; }
        public string bank_account_owner { get; set; }
        public string bank_account { get; set; }
        public string bank { get; set; }
        public string note { get; set; }
        public DateTime? date_receive { get; set; }
        public bool? confirm_receive { get; set; }
    }
}
