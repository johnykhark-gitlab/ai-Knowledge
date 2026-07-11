using Microsoft.AspNetCore.Http;

namespace AIKnowledge.Application.DTOs;

public class UploadDocumentRequestDto
{
    public IFormFile File { get; set; } = default!;
}