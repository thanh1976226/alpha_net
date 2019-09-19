using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESEIM.Models
{
    [Table("MATERIAL_STORE_IMP_GOODS_DETAILS")]
    public class MaterialStoreImpGoodsDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string ImpCode { get; set; }

        [StringLength(50)]
        public string GoodsCode { get; set; }

        public int? Quantity { get; set; }

        public decimal? Cose { get; set; }

        public decimal? Saleoff { get; set; }

        public decimal? Vat { get; set; }

        public DateTime? ExpireDate { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [StringLength(100)]
        public string Unit { get; set; }

        [StringLength(12)]
        public string Barcode { get; set; }

        public int? BlockId { get; set; }
    }
}
