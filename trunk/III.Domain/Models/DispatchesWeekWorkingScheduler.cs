using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESEIM.Models
{
    [Table("DISPATCHES_WEEK_WORKING_SCHEDULER")]
    public class DispatchesWeekWorkingScheduler
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Content { get; set; }

        [StringLength(255)]
        public string Composition { get; set; }

        [StringLength(50)]
        public string Chair { get; set; }

        [StringLength(255)]
        public string Room { get; set; }

        public DateTime CreatedTime { get; set; }

        [StringLength(20)]
        public string TimeStart { get; set; }

        [StringLength(20)]
        public string TimeEnd { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }
    }
}
