namespace AIKnowledge.Application.Features.Chat;

public class CreateChatRequest
{
    public int DocumentId { get; set; }

    public string Title { get; set; } = string.Empty;
}