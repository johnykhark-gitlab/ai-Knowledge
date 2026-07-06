namespace AIKnowledge.Domain.Entities;

public class Chat
{
    public int ChatId { get; set; }

    public int UserId { get; set; }

    public int? DocumentId { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
}