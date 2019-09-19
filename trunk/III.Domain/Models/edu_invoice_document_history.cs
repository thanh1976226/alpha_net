using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_invoice_document_history")]
    public class edu_invoice_document_history
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public double? price { get; set; }
        public string note { get; set; }
        public int? usercreate { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
        public int? number { get; set; }
        public int? inventory { get; set; }
        public int? location_id { get; set; }
        public int? inventory_real { get; set; }
        public int? invoice_document_id { get; set; }
    }
}
