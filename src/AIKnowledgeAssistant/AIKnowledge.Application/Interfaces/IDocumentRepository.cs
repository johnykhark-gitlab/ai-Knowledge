using AIKnowledge.Domain.Entities;

namespace AIKnowledge.Application.Interfaces;

public interface IDocumentRepository
{
    Task<int> UploadDocumentAsync(Document document);

    Task<List<Document>> GetDocumentsAsync(int userId);

    Task<Document?> GetDocumentByIdAsync(int documentId);

    Task DeleteDocumentAsync(int documentId);
    Task UpdateAIStatusAsync(int documentId, string status);
}