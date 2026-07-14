using System.Text;
using System.Text.Json;
using AIKnowledge.Infrastructure.AI.Interfaces;
using AIKnowledge.Infrastructure.AI.Models;
using Microsoft.Extensions.Configuration;

namespace AIKnowledge.Infrastructure.AI.Ollama;

public class OllamaService : IOllamaService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public OllamaService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> AskAsync(string prompt)
    {
        var request = new OllamaRequest
        {
            model = _configuration["Ollama:Model"]!,
            prompt = prompt,
            stream = false
        };

        string json = JsonSerializer.Serialize(request);

        var response = await _httpClient.PostAsync(
            $"{_configuration["Ollama:BaseUrl"]}/api/generate",
            new StringContent(
                json,
                Encoding.UTF8,
                "application/json"));

        response.EnsureSuccessStatusCode();

        string result = await response.Content.ReadAsStringAsync();

        var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(result);

        return ollamaResponse?.Response ?? "";
    }

    public async Task<string> AskQuestionAsync(
       string documentContext,
       string conversationHistory,
       string question)
    {
        string prompt = $"""
You are an AI Knowledge Assistant.

Answer ONLY using the uploaded document.

Previous Conversation:
{conversationHistory}

Document:
{documentContext}

Current Question:
{question}

If the answer is not found in the document, reply:
'I couldn't find the answer in the uploaded document.'
""";

        return await AskAsync(prompt);
    }
}