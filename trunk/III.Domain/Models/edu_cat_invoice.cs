using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_cat_invoice")]
    public  class edu_cat_invoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public bool? type { get; set; }
        public int? parent_id { get; set; }
        public bool? flag { get; set; }
        public bool? status { get; set; }
        public string short_code { get; set; }
        public string obj_type { get; set; }
    }
}
