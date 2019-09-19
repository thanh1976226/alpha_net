using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("MATERIAL_STORE_IMP_GOODS_HEADER")]
    public class MaterialStoreImpGoodsHeader
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public string ImpCode { get; set; }

        public int? SupplierId { get; set; }

        [StringLength(100)]
        public string UserId { get; set; }

        public DateTime? CreateTicketTime { get; set; }

        public decimal? Total { get; set; }

        public int? Vat { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public bool? Debt { get; set; }

        [StringLength(250)]
        public string ShipperId { get; set; }

        public DateTime? DeadlinePayment { get; set; }

        public int? StoreId { get; set; }

        public decimal? TotalPayed { get; set; }

        public decimal? TotalPayment { get; set; }

        public bool? Flag { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        [StringLength(50)]
        public string DeletedBy { get; set; }

        public DateTime? DeletedTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
