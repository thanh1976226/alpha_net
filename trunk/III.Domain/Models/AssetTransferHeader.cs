using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("ASSET_TRANSFER_HEADER")]
    public class AssetTransferHeader
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetID { get; set; }

        [StringLength(maximumLength: 100)]
        public string Ticketcode { get; set; }

        [StringLength(maximumLength: 255)]
        public string QRcode { get; set; }

        [StringLength(maximumLength: 255)]
        public string Ticket { get; set; }

        [StringLength(maximumLength: 255)]
        public string DepartTransf { get; set; }

        [StringLength(maximumLength: 255)]
        public string UserTransf { get; set; }

        [StringLength(maximumLength: 255)]
        public string LocationTransf { get; set; }

        public DateTime? StartTime { get; set; }

        [StringLength(maximumLength: 255)]
        public string DepartReceived { get; set; }

        [StringLength(maximumLength: 255)]
        public string UserReceived { get; set; }

        public DateTime? ReceivedTime { get; set; }

        [StringLength(maximumLength: 255)]
        public string ReceivedLocation { get; set; }

        [StringLength(maximumLength: 255)]
        public string Status { get; set; }

        [StringLength(maximumLength: 255)]
        public string Note { get; set; }

        [StringLength(maximumLength: 50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

        [StringLength(maximumLength: 50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public Boolean IsDeleted { get; set; }


        [NotMapped]
        [StringLength(maximumLength: 50)]
        public string sStartTime { get; set; }
        [NotMapped]
        [StringLength(maximumLength: 50)]
        public string sReceivedTime { get; set; }

        [StringLength(maximumLength: 50)]
        public DateTime? DeletedTime { get; set; }

        [StringLength(maximumLength: 50)]
        public string DeletedBy { get; set; }
    }
}
