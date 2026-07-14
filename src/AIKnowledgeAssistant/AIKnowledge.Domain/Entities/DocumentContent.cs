namespace AIKnowledge.Domain.Entities;

public class DocumentContent
{
    public int DocumentContentId { get; set; }

    public int DocumentId { get; set; }

    public string ExtractedText { get; set; } = string.Empty;

    public int TotalCharacters { get; set; }

    public int TotalWords { get; set; }

    public bool AIProcessed { get; set; }

    public DateTime CreatedOn { get; set; }
}