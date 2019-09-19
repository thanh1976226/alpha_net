using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("Jnana_news_article_file")]
    public class Jnana_news_article_file
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string file_code { get; set; }
        public string article_code { get; set; }
        public int? created_by { get; set; }

        public DateTime? created_time { get; set; }
       
    }
}
