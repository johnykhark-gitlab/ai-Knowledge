using AIKnowledge.Domain.Entities;

namespace AIKnowledge.Application.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetUserByEmailAsync(string email);

    Task<int> RegisterUserAsync(User user);

    Task<string?> GetRoleNameAsync(int roleId);

    Task<bool> EmailExistsAsync(string email);

    Task UpdateLastLoginAsync(int userId);
    Task SaveRefreshTokenAsync(RefreshToken token);
    Task<User?> GetUserByIdAsync(int userId);

    Task<RefreshToken?> GetRefreshTokenAsync(string token);

    Task UpdateRefreshTokenAsync(RefreshToken token);
}