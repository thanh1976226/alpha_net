using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_currency")]
    public class edu_currency
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int currencyid { get; set; }
        public string currency_name { get; set; }
        public string currency_code { get; set; }
        public string currency_symbol { get; set; }
        public int? prority { get; set; }
        public int? userid { get; set; }
        public string action { get; set; }
        public DateTime? createtime { get; set; }
    }
}
