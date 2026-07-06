namespace AIKnowledge.Domain.Entities;

public class AuditLog
{
    public int AuditLogId { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string TableName { get; set; } = string.Empty;

    public int? RecordId { get; set; }

    public string? Description { get; set; }

    public string? IPAddress { get; set; }

    public DateTime CreatedOn { get; set; }
}

