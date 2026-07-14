namespace AIKnowledge.Infrastructure.AI.Interfaces;

public interface IOllamaService
{
    Task<string> AskAsync(string prompt);

    Task<string> AskQuestionAsync(
    string documentContext,
    string conversationHistory,
    string question);
}