using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("ORDER_REQUEST_RAW")]
    public class OrderRequestRaw
    { 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public Guid ReqCode { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        public string Content { get; set; }

        [StringLength(255)]
        public string File1 { get; set; }

        [StringLength(255)]
        public string FileName1 { get; set; }

        [StringLength(255)]
        public string File2 { get; set; }

        [StringLength(255)]
        public string FileName2 { get; set; }

        [StringLength(255)]
        public string Ip { get; set; }

        [StringLength(255)]
        public string Device { get; set; }

        public int? Priority { get; set; }

        [StringLength(255)]
        public string RequestType { get; set; }

        public DateTime? RequestTime { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(255)]
        public string Keyword { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }
    }
}
