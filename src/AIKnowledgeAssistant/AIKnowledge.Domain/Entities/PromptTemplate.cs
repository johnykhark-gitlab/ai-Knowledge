namespace AIKnowledge.Domain.Entities;

public class PromptTemplate
{
    public int PromptTemplateId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Prompt { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
}