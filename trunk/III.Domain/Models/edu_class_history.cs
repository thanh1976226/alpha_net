using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_class_history")]
    public  class edu_class_history
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_edu_class { get; set; }
        public string namechanged { get; set; }
        public string valueold { get; set; }
        public string valuenew { get; set; }
        public DateTime? datecreate { get; set; }
    }
}
