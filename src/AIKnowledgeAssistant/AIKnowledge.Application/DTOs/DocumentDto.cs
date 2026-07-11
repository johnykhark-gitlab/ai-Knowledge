namespace AIKnowledge.Application.DTOs;

public class DocumentDto
{
    public int DocumentId { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;

    public string FileExtension { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string AIProcessingStatus { get; set; } = string.Empty;

    public DateTime UploadedOn { get; set; }
}