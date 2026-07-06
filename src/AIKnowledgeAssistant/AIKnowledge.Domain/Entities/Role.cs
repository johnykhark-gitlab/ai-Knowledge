namespace AIKnowledge.Domain.Entities;

public class Role
{
    public int RoleId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
}