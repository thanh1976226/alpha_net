using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_AppointmentModify")]
    public partial class edu_AppointmentModify
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string subject { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public DateTime? start { get; set; }
        public DateTime? finish { get; set; }
        public int? status { get; set; }
        public int? fileId { get; set; }
        public int? id_class { get; set; }
        public int? id_TeachSchedule { get; set; }
        public string id_Skill { get; set; }
        public DateTime? createTime { get; set; }
        public DateTime? updateTime { get; set; }
        public string type { get; set; }
        public string action { get; set; }
        public byte? flag { get; set; }
        public int? confirm { get; set; }
        public int id_AppointmentTeach { get; set; }
        public int? id_room { get; set; }
        public int? type_class { get; set; }
    }
}
