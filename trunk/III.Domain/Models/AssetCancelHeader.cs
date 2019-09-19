using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("ASSET_CANCEL_HEADER")]
    public class AssetCancelHeader
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetID { get; set; }
        
        [StringLength(maximumLength: 100)]
        public string TicketCode { get; set; }
        
        [StringLength(100)]
        public string Title { get; set; }
        
        [StringLength(maximumLength: 255)]
        public string Branch { get; set; }
        
        [StringLength(maximumLength: 255)]
        public string Person { get; set; }
        
        [StringLength(maximumLength: 255)]
        public string Note { get; set; }

        [StringLength(100)]
        public string Status { get; set; }

        [StringLength(maximumLength: 50)]
        public string CreatedBy { get; set; }

        public int Quantity { get; set; }

        public DateTime? CreatedTime { get; set; }

        [StringLength(maximumLength: 50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }
       
        [StringLength(maximumLength: 50)]
        public string DeletedBy { get; set; }

        [StringLength(maximumLength: 50)]
        public DateTime? DeletedTime { get; set; }

        public Boolean IsDeleted { get; set; }

        [StringLength(maximumLength: 255)]
        public string Adress { get; set; }

        public DateTime? CancelTime { get; set; }

        [NotMapped]
        [StringLength(maximumLength: 50)]
        public string sStartTime { get; set; }
    }
}
