using AIKnowledge.Application.Interfaces;

namespace AIKnowledge.Infrastructure.Services;

public class JwtService : IJwtService
{
    public string GenerateToken(int userId, string email, string role)
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }
}