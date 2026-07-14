using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;
using AIKnowledge.Infrastructure.Data.Connection;
using Dapper;

namespace AIKnowledge.Infrastructure.Data.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ChatRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CreateChatAsync(Chat chat)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
INSERT INTO Chats
(
    UserId,
    DocumentId,
    Title,
    CreatedOn,
    CreatedBy,
    IsDeleted
)
VALUES
(
    @UserId,
    @DocumentId,
    @Title,
    GETDATE(),
    'System',
    0
);

SELECT CAST(SCOPE_IDENTITY() AS INT);
";

        return await connection.ExecuteScalarAsync<int>(sql, chat);
    }

    public async Task<Chat?> GetChatAsync(int chatId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
SELECT *
FROM Chats
WHERE ChatId=@ChatId
AND IsDeleted=0";

        return await connection.QueryFirstOrDefaultAsync<Chat>(
            sql,
            new { ChatId = chatId });
    }

    public async Task<List<Chat>> GetUserChatsAsync(int userId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
SELECT *
FROM Chats
WHERE UserId=@UserId
AND IsDeleted=0
ORDER BY CreatedOn DESC";

        var result = await connection.QueryAsync<Chat>(
            sql,
            new { UserId = userId });

        return result.ToList();
    }

    public async Task<bool> ChatExistsAsync(int chatId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
SELECT COUNT(1)
FROM Chats
WHERE ChatId=@ChatId
AND IsDeleted=0";

        return await connection.ExecuteScalarAsync<bool>(
            sql,
            new { ChatId = chatId });
    }
}