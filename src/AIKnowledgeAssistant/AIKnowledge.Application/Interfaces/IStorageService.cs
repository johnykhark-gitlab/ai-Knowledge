using Microsoft.AspNetCore.Http;

namespace AIKnowledge.Application.Interfaces;

public interface IStorageService
{
    Task<string> SaveFileAsync(IFormFile file);

    Task DeleteFileAsync(string filePath);
}