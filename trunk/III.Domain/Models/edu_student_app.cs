using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_student_app")]
    public  class edu_student_app
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string SATscore { get; set; }
        public string TOEFL_IELTSscore { get; set; }
        public string GPA9 { get; set; }
        public string extracurricular_activities { get; set; }
        public string honor_awards { get; set; }
        public string family_contribution { get; set; }
        public string result_app { get; set; }
        public string note { get; set; }
        public int? classify { get; set; }
        public int? student_id { get; set; }
        public int? usercreate { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
        public int? season_app { get; set; }
        public string update_SAT { get; set; }
        public string update_TOEFL_IELTS { get; set; }
        public string GPA10 { get; set; }
        public string GPA11 { get; set; }
        public bool? type { get; set; }
    }
}
