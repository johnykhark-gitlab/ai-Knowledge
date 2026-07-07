using System.Data;

namespace AIKnowledge.Application.Interfaces;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}