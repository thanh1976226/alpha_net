using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("CONTRACT_FILE")]
    public class ContractFile
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContractFileID { get; set; }

        [StringLength(50)]
        public string ContractCode { get; set; }

        public int? ContractVersion { get; set; }

        [StringLength(100)]
        public string FileCode { get; set; }

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

        [StringLength(500)]
        public string Description { get; set; }

        public int? Category { get; set; }

        [StringLength(50)]
        public string Tags { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(255)]
        public string FileUrl { get; set; }
    }
}
