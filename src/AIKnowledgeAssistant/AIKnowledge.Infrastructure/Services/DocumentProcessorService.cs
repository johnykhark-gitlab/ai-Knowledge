using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;

namespace AIKnowledge.Infrastructure.Services;

public class DocumentProcessorService
{
    private readonly IDocumentTextExtractor _extractor;
    private readonly IDocumentContentRepository _contentRepository;
    private readonly IDocumentRepository _documentRepository;

    public DocumentProcessorService(
        IDocumentTextExtractor extractor,
        IDocumentContentRepository contentRepository,
        IDocumentRepository documentRepository)
    {
        _extractor = extractor;
        _contentRepository = contentRepository;
        _documentRepository = documentRepository;
    }

    public async Task ProcessDocumentAsync(int documentId, string filePath)
    {
        string text = await _extractor.ExtractTextAsync(filePath);

        var content = new DocumentContent
        {
            DocumentId = documentId,
            ExtractedText = text,
            TotalCharacters = text.Length,
            TotalWords = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length,
            AIProcessed = true,
            CreatedOn = DateTime.Now
        };

        await _contentRepository.SaveContentAsync(content);

        await _documentRepository.UpdateAIStatusAsync(
            documentId,
            "Completed");
    }
}