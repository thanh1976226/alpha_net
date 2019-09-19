using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("SUPPLIER_FILE")]
    public class SupplierFile
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? Category { get; set; }

        [StringLength(maximumLength: 255)]
        public string FilePath { get; set; }

        [StringLength(maximumLength: 255)]
        public string FileName { get; set; }

        [StringLength(maximumLength: 255)]
        public string FileType { get; set; }

        [StringLength(maximumLength: 255)]
        public string FileUrl { get; set; }

        [StringLength(maximumLength: 500)]
        public string Title { get; set; }

        public string Description { get; set; }

        public int? SupplierId { get; set; }

        [StringLength(maximumLength: 50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

        [StringLength(maximumLength: 50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }

        [StringLength(maximumLength: 50)]
        public string DeletedBy { get; set; }

        public DateTime? DeletedTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
