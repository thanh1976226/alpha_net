using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("TRACKING_BUILDING")]
    public class TrackingBuilding
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
    
        [StringLength(maximumLength: 100)]
        public string PrjBuildingCode { get; set; }
        [StringLength(maximumLength: 100)]
        public string TrkCode { get; set; }
        [StringLength(maximumLength: 100)]
        public string OptCode { get; set; }
        [StringLength(maximumLength: 255)]
        public string Progress { get; set; }
        public string Note { get; set; }
        [StringLength(maximumLength: 255)]
        public string LocationText { get; set; }
        [StringLength(maximumLength: 255)]
        public string LocationGps { get; set; }
        [StringLength(maximumLength: 100)]
        public string TeamCode { get; set; }
        [StringLength(maximumLength: 255)]
        public string Device { get; set; }


        public bool IsDeleted { get; set; }
        [StringLength(50)]
        public string DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}