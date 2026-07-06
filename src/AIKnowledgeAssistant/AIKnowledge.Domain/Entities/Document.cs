namespace AIKnowledge.Domain.Entities;

public class Document
{
    public int DocumentId { get; set; }

    public int UserId { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;

    public string StoredFileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string FileExtension { get; set; } = string.Empty;

    public string MimeType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public int? TotalPages { get; set; }

    public string AIProcessingStatus { get; set; } = "Pending";

    public DateTime UploadedOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
}