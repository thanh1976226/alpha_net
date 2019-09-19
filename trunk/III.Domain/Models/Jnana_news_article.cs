using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("Jnana_news_article")]
    public class Jnana_news_article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [StringLength(maximumLength: 250)]
        public string article_code { get; set; }


        [StringLength(maximumLength: 255)]
        public string article_title { get; set; }

        public int? artile_order { get; set; }

        public int? created_by { get; set; }
        public int? update_by { get; set; }

        [StringLength(maximumLength: 255)]
        public string article_content { get; set; }

        [StringLength(maximumLength: 255)]
        public string article_avarta { get; set; }

      
        public DateTime? created_time { get; set; }
        public DateTime? update_time { get; set; }
        public int article_status { get; set; }

        [StringLength(maximumLength: 250)]
        public string cat_code { get; set; }

    }
}
