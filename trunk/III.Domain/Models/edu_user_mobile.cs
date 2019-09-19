using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace ESEIM.Models
{
    [Table("edu_user_mobile")]
    public  class edu_user_mobile
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public byte? active { get; set; }
        public byte? super { get; set; }
        public byte? status { get; set; }
        public int? type_user { get; set; }
        public string profile_picture { get; set; }
        public int? balence_credit { get; set; }
        public DateTime? register { get; set; }
        public string id_facebook { get; set; }
    }
}
