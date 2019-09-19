using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace ESEIM.Models
{
    [Table("edu_class_tutor")]
    public  class edu_class_tutor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int? idcrouse { get; set; }
        public int? numberstudent { get; set; }
        public DateTime creattime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? userid { get; set; }
        public byte flag { get; set; }
        public string schoolstart { get; set; }
        public string schoolend { get; set; }
        public string schoolday { get; set; }
        public bool? status { get; set; }
        public int? classroom_id { get; set; }
        public int? teacher_id { get; set; }
        public double? fee { get; set; }
        public DateTime? datestart { get; set; }
        public DateTime? dateend { get; set; }
        public int? totalMinutes { get; set; }
        public int? totalMinutes_free { get; set; }
        public int? rateInfrequently { get; set; }
        public int? rateFrequently { get; set; }
        public int? parentId { get; set; }
        public int? studentId { get; set; }
        public string skills { get; set; }
        public string note { get; set; }
        public string totalMinutesExp { get; set; }
        public string totalMinutesFreeExp { get; set; }
        public int? debt_id { get; set; }
        public int? invoice_id { get; set; }
    }
}
