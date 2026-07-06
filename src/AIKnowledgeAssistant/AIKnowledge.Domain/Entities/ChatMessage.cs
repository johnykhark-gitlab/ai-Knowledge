namespace AIKnowledge.Domain.Entities;

public class ChatMessage
{
    public int MessageId { get; set; }

    public int ChatId { get; set; }

    public string UserQuestion { get; set; } = string.Empty;

    public string AIResponse { get; set; } = string.Empty;

    public int? PromptTokens { get; set; }

    public int? CompletionTokens { get; set; }

    public int? TotalTokens { get; set; }

    public int? ResponseTimeMs { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
}