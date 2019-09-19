using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("CONTRACT_HEADER")]
    public class ContractHeader 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContractHeaderID { get; set; }

        [StringLength(100)]
        public string ContractCode { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(50)]
        public string ContractType { get; set; }

        public DateTime? ContractDate { get; set; }

        [StringLength(100)]
        public string ContractNo { get; set; }

        [StringLength(10)]
        public string Duration { get; set; }

        public int? Version { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(100)]
        public string Signer { get; set; }

        [StringLength(255)]
        public string MainService { get; set; }

        [StringLength(20)]
        public string Currency { get; set; }

        [StringLength(255)]
        public string LocationHardCopy { get; set; }

        [StringLength(500)]
        public string Describe { get; set; }

        [StringLength(100)]
        public string CusCode { get; set; }

        [StringLength(255)]
        public string ContractRelative { get; set; }

        [StringLength(50)]
        public string Tags { get; set; }

        [DataType("decimal(18,3)")]
        public decimal? Budget { get; set; }
        
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }

        [StringLength(50)]
        public string DeletedBy { get; set; }

        public DateTime? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
