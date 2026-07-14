using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;
using AIKnowledge.Infrastructure.Data.Connection;
using Dapper;

namespace AIKnowledge.Infrastructure.Data.Repositories;

public class DocumentContentRepository : IDocumentContentRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public DocumentContentRepository(
        ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task SaveContentAsync(DocumentContent content)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
INSERT INTO DocumentContents
(
DocumentId,
ExtractedText,
TotalCharacters,
TotalWords,
AIProcessed,
CreatedOn
)

VALUES
(
@DocumentId,
@ExtractedText,
@TotalCharacters,
@TotalWords,
0,
GETDATE()
)";
        await connection.ExecuteAsync(sql, content);
    }

    public async Task<DocumentContent?> GetContentByDocumentIdAsync(int documentId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
SELECT *
FROM DocumentContents
WHERE DocumentId=@DocumentId";

        return await connection.QueryFirstOrDefaultAsync<DocumentContent>(
            sql,
            new { DocumentId = documentId });
    }

    public async Task UpdateAIProcessedAsync(int documentId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
UPDATE DocumentContents
SET AIProcessed=1
WHERE DocumentId=@DocumentId";

        await connection.ExecuteAsync(sql, new { DocumentId = documentId });
    }
    public async Task<string?> GetDocumentTextAsync(int documentId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
SELECT Content
FROM DocumentContents
WHERE DocumentId=@DocumentId";

        return await connection.QueryFirstOrDefaultAsync<string>(
            sql,
            new { DocumentId = documentId });
    }
}