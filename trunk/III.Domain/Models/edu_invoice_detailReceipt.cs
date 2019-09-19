using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_invoice_detailReceipt")]
    public class edu_invoice_detailReceipt
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_course { get; set; }
        public int? id_student { get; set; }
        public int? id_invoice { get; set; }
    }
}
