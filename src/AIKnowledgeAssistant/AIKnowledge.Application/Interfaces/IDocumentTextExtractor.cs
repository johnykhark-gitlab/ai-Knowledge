namespace AIKnowledge.Application.Interfaces;

public interface IDocumentTextExtractor
{
    Task<string> ExtractTextAsync(string filePath);
}