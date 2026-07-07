using AIKnowledge.Domain.Entities;

namespace AIKnowledge.Application.Interfaces;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);

    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByIdAsync(int userId);

    Task<int> RegisterAsync(User user);

    Task UpdateLastLoginAsync(int userId);

    Task SaveRefreshTokenAsync(RefreshToken refreshToken);

    Task<Role?> GetRoleAsync(int roleId);
}