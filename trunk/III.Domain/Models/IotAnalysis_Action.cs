using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("IOT_ANALYSIS_ACTION")]
    public class IotAnalysis_Action
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string Address { get; set; }

        public string FaceId { get; set; }

        public string NameFace { get; set; }
        public string LocationGps { get; set; }
        public string LocationText { get; set; }

        public bool ObjType { get; set; }

        public decimal Total { get; set; }
        public string Action { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

    }
}
