using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_invoice11")]
    public class edu_invoice11
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public DateTime? date_create { get; set; }
        public int? model { get; set; }
        public string note { get; set; }
        public int? method { get; set; }
        public byte? status { get; set; }
        public DateTime? next_pay { get; set; }
        public int? id_tms_crm_currency { get; set; }
        public double? money { get; set; }
        public int? user_id { get; set; }
        public byte? flag { get; set; }
        public string trans_code { get; set; }
        public int? trans_bank { get; set; }
        public string trans_note { get; set; }
    }
}
