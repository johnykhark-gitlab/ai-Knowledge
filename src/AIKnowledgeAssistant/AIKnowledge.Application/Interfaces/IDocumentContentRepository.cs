using AIKnowledge.Domain.Entities;

namespace AIKnowledge.Application.Interfaces;

public interface IDocumentContentRepository
{
    Task SaveContentAsync(DocumentContent content);

    Task<DocumentContent?> GetContentByDocumentIdAsync(int documentId);

    Task UpdateAIProcessedAsync(int documentId);
    Task<string?> GetDocumentTextAsync(int documentId);
}