using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace ESEIM.Models
{
    [Table("edu_class")]
    public  class edu_class
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int? idcrouse { get; set; }
        public int? numberstudent { get; set; }
        public DateTime? creattime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? userid { get; set; }
        public int flag { get; set; }
        public string schoolstart { get; set; }
        public string schoolend { get; set; }
        public string schoolday { get; set; }
        public string status { get; set; }
        public string classroom_id { get; set; }
        public int? teacher_id { get; set; }
        public double? fee { get; set; }
        public DateTime? datestart { get; set; }
        public DateTime? dateend { get; set; }
        public double? sale_off_price { get; set; }
        public DateTime? start_sale_off { get; set; }
        public DateTime? end_sale_off { get; set; }
        public string sale_off_note { get; set; }
        public int? location_id { get; set; }
    }
}
