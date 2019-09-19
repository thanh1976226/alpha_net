using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_token_mobile")]
    public  class edu_token_mobile
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id_User { get; set; }
        public string Token { get; set; }
        public int Id { get; set; }
    }
}
