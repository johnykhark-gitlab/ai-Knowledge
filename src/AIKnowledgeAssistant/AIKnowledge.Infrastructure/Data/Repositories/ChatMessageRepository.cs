using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;
using AIKnowledge.Infrastructure.Data.Connection;
using Dapper;

namespace AIKnowledge.Infrastructure.Data.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ChatMessageRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> SaveMessageAsync(ChatMessage message)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
INSERT INTO ChatMessages
(
    ChatId,
    UserQuestion,
    AIResponse,
    PromptTokens,
    CompletionTokens,
    TotalTokens,
    ResponseTimeMs,
    CreatedOn,
    CreatedBy,
    IsDeleted
)
VALUES
(
    @ChatId,
    @UserQuestion,
    @AIResponse,
    @PromptTokens,
    @CompletionTokens,
    @TotalTokens,
    @ResponseTimeMs,
    GETDATE(),
    'System',
    0
);

SELECT CAST(SCOPE_IDENTITY() AS INT);
";

        return await connection.ExecuteScalarAsync<int>(sql, message);
    }

    public async Task<List<ChatMessage>> GetMessagesAsync(int chatId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
SELECT *
FROM ChatMessages
WHERE ChatId=@ChatId
AND IsDeleted=0
ORDER BY CreatedOn";

        var result = await connection.QueryAsync<ChatMessage>(
            sql,
            new { ChatId = chatId });

        return result.ToList();
    }

   
}