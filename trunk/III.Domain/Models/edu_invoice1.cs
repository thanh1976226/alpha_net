using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_invoice1")]
    public class edu_invoice1
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ticket_id { get; set; }
        public string ticket_code { get; set; }
        public string ticket_title { get; set; }
        public DateTime? createtime { get; set; }
        public int? userwork_id { get; set; }
        public int? ticket_type { get; set; }
        public double? total { get; set; }
        public int? currency { get; set; }
        public int? student_id { get; set; }
        public string note { get; set; }
        public string reasons { get; set; }
        public int? status { get; set; }
    }
}
