namespace AIKnowledge.Application.Features.Chat.DTOs;

public class AskQuestionResponse
{
    public string Answer { get; set; } = string.Empty;

    public int PromptTokens { get; set; }

    public int CompletionTokens { get; set; }

    public int TotalTokens { get; set; }

    public int ResponseTimeMs { get; set; }
}