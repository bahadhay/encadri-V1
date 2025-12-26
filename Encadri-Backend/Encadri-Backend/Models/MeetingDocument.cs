namespace Encadri_Backend.Models
{
    /// <summary>
    /// Meeting Document Model
    /// Represents documents uploaded for a meeting (presentations, agendas, etc.)
    /// </summary>
    public class MeetingDocument
    {
        public string? Id { get; set; }
        public string MeetingId { get; set; }
        public string? MeetingRequestId { get; set; }
        public string FileName { get; set; }
        public string BlobUrl { get; set; } // Azure Blob Storage URL
        public long FileSize { get; set; }
        public string ContentType { get; set; }
        public string UploadedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
