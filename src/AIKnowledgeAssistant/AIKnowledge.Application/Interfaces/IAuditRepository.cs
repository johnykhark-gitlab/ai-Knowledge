using AIKnowledge.Domain.Entities;

namespace AIKnowledge.Application.Interfaces;

public interface IAuditRepository
{
    Task AddAsync(AuditLog auditLog);
}