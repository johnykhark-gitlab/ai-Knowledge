namespace AIKnowledge.Application.Features.Chat.DTOs;

public class AskQuestionRequest
{
    public int ChatId { get; set; }

    public string Question { get; set; } = string.Empty;
}