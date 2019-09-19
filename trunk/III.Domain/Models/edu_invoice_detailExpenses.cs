using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_invoice_detailExpenses")]
    public class edu_invoice_detailExpenses
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int id_client { get; set; }
        public int id_invoice { get; set; }
        public string receipt { get; set; }
    }
}
