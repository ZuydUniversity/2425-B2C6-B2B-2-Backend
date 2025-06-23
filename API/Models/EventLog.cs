using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class EventLog
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; } // Case ID

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Activity { get; set; }

        public string Details { get; set; }

        public Order Order { get; set; }
    }
}
