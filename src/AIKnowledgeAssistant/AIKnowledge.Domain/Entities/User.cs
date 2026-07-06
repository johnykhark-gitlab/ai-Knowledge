namespace AIKnowledge.Domain.Entities;

public class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
}