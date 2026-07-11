using AIKnowledge.Application.DTOs;
using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AIKnowledge.Infrastructure.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _repository;
    private readonly IStorageService _storageService;
    private readonly IAuditRepository _auditRepository;

    public DocumentService(
        IDocumentRepository repository,
        IStorageService storageService,
        IAuditRepository auditRepository)
    {
        _repository = repository;
        _storageService = storageService;
        _auditRepository = auditRepository;
    }

    public async Task DeleteDocumentAsync(int documentId)
    {
        var document = await _repository.GetDocumentByIdAsync(documentId);

        if (document == null)
            throw new Exception("Document not found.");

        await _storageService.DeleteFileAsync(document.StoredFileName);

        await _repository.DeleteDocumentAsync(documentId);

        await _auditRepository.AddAsync(new AuditLog
        {
            UserId = document.UserId,
            Action = "DELETE_DOCUMENT",
            TableName = "Documents",
            RecordId = documentId,
            Description = $"Deleted document : {document.OriginalFileName}",
            IPAddress = ""
        });
    }

    public async Task<DocumentDto?> GetDocumentByIdAsync(int documentId)
    {
        var document = await _repository.GetDocumentByIdAsync(documentId);

        if (document == null)
            return null;

        return new DocumentDto
        {
            DocumentId = document.DocumentId,
            OriginalFileName = document.OriginalFileName,
            FileExtension = document.FileExtension,
            FileSize = document.FileSize,
            AIProcessingStatus = document.AIProcessingStatus,
            UploadedOn = document.UploadedOn
        };
    }

    public async Task<List<DocumentDto>> GetDocumentsAsync(int userId)
    {
        var documents = await _repository.GetDocumentsAsync(userId);

        return documents.Select(x => new DocumentDto
        {
            DocumentId = x.DocumentId,
            OriginalFileName = x.OriginalFileName,
            FileExtension = x.FileExtension,
            FileSize = x.FileSize,
            AIProcessingStatus = x.AIProcessingStatus,
            UploadedOn = x.UploadedOn
        }).ToList();
    }

    public async Task<DocumentDto> UploadDocumentAsync(int userId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Please select a file.");

        string extension = Path.GetExtension(file.FileName).ToLower();

        if (extension != ".pdf" && extension != ".docx")
            throw new Exception("Only PDF and DOCX files are allowed.");

        if (file.Length > 20 * 1024 * 1024)
            throw new Exception("Maximum file size is 20 MB.");

        // Save file locally
        string storedFileName = await _storageService.SaveFileAsync(file);

        var document = new Document
        {
            UserId = userId,
            OriginalFileName = file.FileName,
            StoredFileName = storedFileName,
            FilePath = Path.Combine("Uploads", "Documents", storedFileName),
            FileExtension = extension,
            MimeType = file.ContentType,
            FileSize = file.Length,
            TotalPages = null,
            AIProcessingStatus = "Pending",
            UploadedOn = DateTime.Now,
            CreatedOn = DateTime.Now,
            CreatedBy = "System",
            IsDeleted = false
        };

        int documentId = await _repository.UploadDocumentAsync(document);

        await _auditRepository.AddAsync(new AuditLog
        {
            UserId = userId,
            Action = "UPLOAD_DOCUMENT",
            TableName = "Documents",
            RecordId = documentId,
            Description = $"Uploaded document : {file.FileName}",
            IPAddress = ""
        });

        return new DocumentDto
        {
            DocumentId = documentId,
            OriginalFileName = document.OriginalFileName,
            FileExtension = document.FileExtension,
            FileSize = document.FileSize,
            AIProcessingStatus = document.AIProcessingStatus,
            UploadedOn = document.UploadedOn
        };
    }
}