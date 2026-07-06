namespace AIKnowledge.Domain.Entities;

public class RefreshToken
{
    public int RefreshTokenId { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiryDate { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime CreatedOn { get; set; }
}