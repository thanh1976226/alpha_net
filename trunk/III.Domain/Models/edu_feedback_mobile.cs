using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_feedback_mobile")]
    public class edu_feedback_mobile
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_user { get; set; }
        public string message { get; set; }
        public int? id_message { get; set; }
        public DateTime? timesend { get; set; }
    }
}
