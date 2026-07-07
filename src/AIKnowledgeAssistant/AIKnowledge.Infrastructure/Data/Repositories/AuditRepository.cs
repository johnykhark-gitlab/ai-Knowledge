using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;
using AIKnowledge.Infrastructure.Data.Connection;
using Dapper;

namespace AIKnowledge.Infrastructure.Data.Repositories;

public class AuditRepository : IAuditRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public AuditRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(AuditLog auditLog)
    {
        using var connection = _connectionFactory.CreateConnection();

        string sql = @"

INSERT INTO AuditLogs
(
UserId,
Action,
TableName,
RecordId,
Description,
IPAddress,
CreatedOn
)

VALUES
(
@UserId,
@Action,
@TableName,
@RecordId,
@Description,
@IPAddress,
GETDATE()
)
";

        await connection.ExecuteAsync(sql, auditLog);
    }
}

