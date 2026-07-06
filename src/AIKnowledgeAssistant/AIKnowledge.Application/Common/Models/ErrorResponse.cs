namespace AIKnowledge.Application.Common.Models;

public class ErrorResponse
{
    public bool Success => false;

    public string Message { get; set; } = string.Empty;

    public List<string>? Errors { get; set; }
}