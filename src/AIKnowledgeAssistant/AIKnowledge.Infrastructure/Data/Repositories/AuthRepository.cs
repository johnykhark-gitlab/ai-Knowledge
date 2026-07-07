using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;
using AIKnowledge.Infrastructure.Data.Connection;
using Dapper;

namespace AIKnowledge.Infrastructure.Data.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public AuthRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"SELECT * FROM Users
                       WHERE Email=@Email
                       AND IsDeleted=0";

        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new { Email = email });
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"SELECT COUNT(1)
                       FROM Users
                       WHERE Email=@Email AND IsDeleted=0";

        return await connection.ExecuteScalarAsync<bool>(
            sql,
            new { Email = email });
    }

    public async Task<int> RegisterUserAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
INSERT INTO Users
(
FirstName,
LastName,
Email,
PasswordHash,
PhoneNumber,
RoleId,
IsActive,
CreatedOn,
CreatedBy,
IsDeleted
)

VALUES
(
@FirstName,
@LastName,
@Email,
@PasswordHash,
@PhoneNumber,
@RoleId,
1,
GETDATE(),
'System',
0
);

SELECT CAST(SCOPE_IDENTITY() AS INT);
";

        return await connection.ExecuteScalarAsync<int>(sql, user);
    }

    public async Task<string?> GetRoleNameAsync(int roleId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"SELECT Name
                       FROM Roles
                       WHERE RoleId=@RoleId";

        return await connection.ExecuteScalarAsync<string>(
            sql,
            new { RoleId = roleId });
    }

    public async Task UpdateLastLoginAsync(int userId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
        UPDATE Users
        SET LastLogin = GETDATE()
        WHERE UserId=@UserId";

        await connection.ExecuteAsync(sql,
            new { UserId = userId });
    }
    public async Task SaveRefreshTokenAsync(RefreshToken token)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"

            INSERT INTO RefreshTokens
            (
            UserId,
            Token,
            ExpiryDate,
            IsRevoked,
            CreatedOn
            )

            VALUES
            (
            @UserId,
            @Token,
            @ExpiryDate,
            0,
            GETDATE()
            )
            ";

        await connection.ExecuteAsync(sql, token);
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
SELECT *
FROM Users
WHERE UserId=@UserId
AND IsDeleted=0";

        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new { UserId = userId });
    }
}


