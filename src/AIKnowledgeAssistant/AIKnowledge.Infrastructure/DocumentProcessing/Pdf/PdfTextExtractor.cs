using AIKnowledge.Application.Interfaces;
using UglyToad.PdfPig;
using System.Text;

namespace AIKnowledge.Infrastructure.DocumentProcessing.Pdf;

public class PdfTextExtractor : IDocumentTextExtractor
{
    public async Task<string> ExtractTextAsync(string filePath)
    {
        return await Task.Run(() =>
        {
            StringBuilder builder = new();

            using var document = PdfDocument.Open(filePath);

            foreach (var page in document.GetPages())
            {
                builder.AppendLine(page.Text);
            }

            return builder.ToString();
        });
    }
}
