using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("CARD_CHECK_LIST")]
    public class CardCheckList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string CardCode { get; set; }

        [StringLength(255)]
        public string CheckTitle { get; set; }

        [StringLength(255)]
        public string Status { get; set; }

        [StringLength(255)]
        public string MemberId { get; set; }

        public DateTime? Finishtime { get; set; }

        [StringLength(255)]
        public string ChkListCode { get; set; }

        public bool Flag { get; set; }
    }
}
