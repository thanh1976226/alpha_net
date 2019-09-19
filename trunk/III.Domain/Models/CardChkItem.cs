using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("CARD_CHK_ITEM")]
    public class CardChkItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string ChkListCode { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        public int? Status { get; set; }

        [StringLength(255)]
        public string MemberId { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public bool Flag { get; set; }

        [StringLength(255)]
        public string MemberChecked { get; set; }

        public DateTime? TimeChecked { get; set; }
    }
}
