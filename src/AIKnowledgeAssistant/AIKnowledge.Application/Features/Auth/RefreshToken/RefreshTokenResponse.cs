namespace AIKnowledge.Application.Features.Auth.RefreshToken;

public class RefreshTokenResponse
{
    public string Token { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime Expiry { get; set; }
}