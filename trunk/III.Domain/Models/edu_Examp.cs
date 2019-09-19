using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_Examp")]
    public class edu_Examp
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_filestore { get; set; }
        public int? id_Examp_Result_Student { get; set; }
        public int? user_id { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
    }
}
