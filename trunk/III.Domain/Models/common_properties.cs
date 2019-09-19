using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace ESEIM.Models
{
    [Table("EDU_COMMON_PROPERTIES")]
    public  class common_properties
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public string code { get; set; }
        public int? parent_id { get; set; }
        public int? flag { get; set; }
        public int? order { get; set; }
        public DateTime? createtime { get; set; }
    }
}
