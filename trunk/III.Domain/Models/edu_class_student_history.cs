using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_class_student_history")]
    public  class edu_class_student_history
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime? date_change { get; set; }
        public int? user_change { get; set; }
        public string column_change { get; set; }
        public string old_value { get; set; }
        public string new_value { get; set; }
        public int? class_student_id { get; set; }
    }
}
