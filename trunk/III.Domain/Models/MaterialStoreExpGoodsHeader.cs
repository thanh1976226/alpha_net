using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("MATERIAL_STORE_EXP_GOODS_HEADER")]
    public class MaterialStoreExpGoodsHeader
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public string ExpCode { get; set; }

        public int? SupplierId { get; set; }

        public string UserId { get; set; }

        public DateTime? TimeImpCreate { get; set; }

        public decimal? Total { get; set; }

        public decimal? Vat { get; set; }

        [StringLength(1000)]
        public string Note { get; set; }

        public bool? Debt { get; set; }

        [StringLength(255)]
        public string ShipperId { get; set; }

        public DateTime? DeadlinePayment { get; set; }

        public int? StoreId { get; set; }

        public decimal? TotalPayed { get; set; }

        public decimal? TotalPayment { get; set; }

        public int? CoseShipping { get; set; }

        public DateTime? InsurantTime { get; set; }
    }
}
