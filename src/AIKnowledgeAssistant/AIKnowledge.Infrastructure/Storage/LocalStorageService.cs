using AIKnowledge.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AIKnowledge.Infrastructure.Storage;

public class LocalStorageService : IStorageService
{
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        string folder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Uploads",
            "Documents");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string fileName =
            Guid.NewGuid() +
            Path.GetExtension(file.FileName);

        string path = Path.Combine(folder, fileName);

        using var stream = new FileStream(path, FileMode.Create);

        await file.CopyToAsync(stream);

        return fileName;
    }

    public Task DeleteFileAsync(string filePath)
    {
        string path = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Uploads",
            "Documents",
            filePath);

        if (File.Exists(path))
            File.Delete(path);

        return Task.CompletedTask;
    }
}