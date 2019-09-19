using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace ESEIM.Models
{
    [Table("edu_reserved_sum")]
    public class edu_reserved_sum
    {
               [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public double price { get; set; }
        public byte? flag { get; set; }
        public int? heir { get; set; }
    }
}
