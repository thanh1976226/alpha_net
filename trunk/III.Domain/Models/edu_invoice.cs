using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_invoice")]
    public class edu_invoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ticket_id { get; set; }
        public string ticket_code { get; set; }
        public string ticket_title { get; set; }
        public double? pay { get; set; }
        public int? currency { get; set; }
        public int? ticket_type { get; set; }
        public DateTime? createtime { get; set; }
        public int? usercreate { get; set; }
        public string note { get; set; }
        public int? debt_id { get; set; }
        public string cat_code { get; set; }
        public string person_pay { get; set; }
        public int? payment_mode { get; set; }
    }
}
