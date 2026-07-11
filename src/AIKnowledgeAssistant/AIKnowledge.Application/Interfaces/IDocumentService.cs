using AIKnowledge.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace AIKnowledge.Application.Interfaces;

public interface IDocumentService
{
    Task<DocumentDto> UploadDocumentAsync(
        int userId,
        IFormFile file);

    Task<List<DocumentDto>> GetDocumentsAsync(int userId);

    Task<DocumentDto?> GetDocumentByIdAsync(int documentId);

    Task DeleteDocumentAsync(int documentId);
}