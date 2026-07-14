using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;
using AIKnowledge.Infrastructure.Data.Connection;
using Dapper;

namespace AIKnowledge.Infrastructure.Data.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public DocumentRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> UploadDocumentAsync(Document document)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"

INSERT INTO Documents
(
UserId,
OriginalFileName,
StoredFileName,
FilePath,
FileExtension,
MimeType,
FileSize,
TotalPages,
AIProcessingStatus,
UploadedOn,
CreatedOn,
CreatedBy,
IsDeleted
)

VALUES
(
@UserId,
@OriginalFileName,
@StoredFileName,
@FilePath,
@FileExtension,
@MimeType,
@FileSize,
@TotalPages,
@AIProcessingStatus,
GETDATE(),
GETDATE(),
'System',
0
);

SELECT CAST(SCOPE_IDENTITY() AS INT);

";

        return await connection.ExecuteScalarAsync<int>(sql, document);
    }

    public async Task<List<Document>> GetDocumentsAsync(int userId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"

SELECT *
FROM Documents
WHERE UserId=@UserId
AND IsDeleted=0
ORDER BY DocumentId DESC

";

        var result = await connection.QueryAsync<Document>(
            sql,
            new { UserId = userId });

        return result.ToList();
    }

    public async Task<Document?> GetDocumentByIdAsync(int documentId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"

SELECT *
FROM Documents
WHERE DocumentId=@DocumentId
AND IsDeleted=0

";

        return await connection.QueryFirstOrDefaultAsync<Document>(
            sql,
            new { DocumentId = documentId });
    }

    public async Task DeleteDocumentAsync(int documentId)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"

UPDATE Documents
SET
IsDeleted=1,
ModifiedOn=GETDATE(),
ModifiedBy='System'
WHERE DocumentId=@DocumentId

";

        await connection.ExecuteAsync(
            sql,
            new { DocumentId = documentId });
    }

    public async Task UpdateAIStatusAsync(
    int documentId,
    string status)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"
UPDATE Documents
SET AIProcessingStatus=@Status
WHERE DocumentId=@DocumentId";

        await connection.ExecuteAsync(sql,
            new
            {
                DocumentId = documentId,
                Status = status
            });
    }
}