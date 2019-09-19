using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("WORK_OS_CARD")]
    public class WORKOSCard
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CardID { get; set; }

        [StringLength(100)]
        public string CardCode { get; set; }

        [StringLength(255)]
        public string CardName { get; set; }

        [StringLength(100)]
        public string ListCode { get; set; }

        public DateTime? DueDate { get; set; }

        [StringLength(255)]
        public string Labels { get; set; }

        [StringLength(255)]
        public string Member { get; set; }

        [StringLength(255)]
        public string CheckList { get; set; }

        [StringLength(255)]
        public string AttachmentList { get; set; }

        [StringLength(255)]
        public string CommentList { get; set; }

        public bool IsDeleted { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(255)]
        public string CreatedBy { get; set; }

        [StringLength(255)]
        public string WorkType { get; set; }

        [StringLength(255)]
        public string Status { get; set; }

        [StringLength(255)]
        public string CardLevel { get; set; }
    }
}
