namespace Encadri_Backend.Models
{
    /// <summary>
    /// Supervisor Availability Model
    /// Represents office hours and availability slots set by supervisors
    /// </summary>
    public class SupervisorAvailability
    {
        public string? Id { get; set; }
        public string SupervisorEmail { get; set; }
        public string DayOfWeek { get; set; } // "Monday", "Tuesday", etc.
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsRecurring { get; set; } // If true, repeats weekly
        public DateTime? SpecificDate { get; set; } // For one-time availability
        public string? MeetingType { get; set; } // "virtual", "in-person", or "both"
        public string? Location { get; set; } // Physical location for in-person meetings
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
