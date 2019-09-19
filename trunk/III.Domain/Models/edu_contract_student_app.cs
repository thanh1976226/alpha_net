using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_contract_student_app")]
    public class edu_contract_student_app
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public double? price { get; set; }
        public DateTime? datestart { get; set; }
        public DateTime? dateend { get; set; }
        public int? type { get; set; }
        public int? kind { get; set; }
        public int? year_training { get; set; }
        public string note { get; set; }
        public int? student_id { get; set; }
        public int? usercreate { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
        public int? currency_id { get; set; }
    }
}
