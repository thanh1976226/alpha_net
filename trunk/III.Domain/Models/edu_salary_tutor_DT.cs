using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_salary_tutor_DT")]
    public class edu_salary_tutor_DT
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_salary_tutor_HD { get; set; }
        public int? id_teacher { get; set; }
        public double? total_hour { get; set; }
        public double? salary_of_day { get; set; }
        public double? salary_of_class { get; set; }
        public double? total_salary { get; set; }
        public double? money_photo { get; set; }
        public double? total_tutor_recipient { get; set; }
        public DateTime? date_receive { get; set; }
        public string note { get; set; }
        public string class_name { get; set; }
    }
}
