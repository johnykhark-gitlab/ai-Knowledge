using System.Text.Json.Serialization;

namespace AIKnowledge.Infrastructure.AI.Models;

public class OllamaResponse
{
    [JsonPropertyName("response")]
    public string Response { get; set; } = string.Empty;
}