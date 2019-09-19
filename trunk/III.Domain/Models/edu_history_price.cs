using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_history_price")]
    public class edu_history_price
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? course_id { get; set; }
        public double? price_old { get; set; }
        public double? price_new { get; set; }
        public DateTime? create_time { get; set; }
    }
}
