namespace Encadri_Backend.Models
{
    /// <summary>
    /// Meeting Request Model
    /// Represents a request from a student to schedule a meeting with their supervisor
    /// </summary>
    public class MeetingRequest
    {
        public string? Id { get; set; }
        public string ProjectId { get; set; }
        public string StudentEmail { get; set; }
        public string SupervisorEmail { get; set; }
        public string? Title { get; set; }
        public string? Agenda { get; set; }
        public DateTime PreferredDate { get; set; }
        public int? DurationMinutes { get; set; }
        public string? MeetingType { get; set; } // "virtual" or "in-person"
        public string? Location { get; set; } // Physical location for in-person meetings
        public string Status { get; set; } // "pending", "approved", "rejected", "cancelled"
        public string? RejectionReason { get; set; }
        public string? MeetingId { get; set; } // Reference to created Meeting if approved
        public List<string>? DocumentUrls { get; set; } // URLs to uploaded documents in Azure Blob Storage
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
