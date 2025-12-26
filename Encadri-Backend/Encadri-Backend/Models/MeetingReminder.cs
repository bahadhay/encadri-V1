namespace Encadri_Backend.Models
{
    /// <summary>
    /// Meeting Reminder Model
    /// Tracks reminders sent for upcoming meetings
    /// </summary>
    public class MeetingReminder
    {
        public string? Id { get; set; }
        public string MeetingId { get; set; }
        public string RecipientEmail { get; set; }
        public int MinutesBeforeMeeting { get; set; } // e.g., 60 for 1 hour before, 1440 for 1 day before
        public bool IsSent { get; set; } = false;
        public DateTime? SentAt { get; set; }
        public DateTime? ScheduledFor { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
