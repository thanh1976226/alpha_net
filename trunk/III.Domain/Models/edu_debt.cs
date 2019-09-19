using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_debt")]
    public class edu_debt
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? customer_id { get; set; }
        public string reasons { get; set; }
        public string nextpay { get; set; }
        public float? promotion { get; set; }
        public float? total { get; set; }
        public int? type { get; set; }
        public bool status { get; set; }
        public int location_id { get; set; }
        public string extra { get; set; }
    }
}
