using AIKnowledge.Domain.Entities;

namespace AIKnowledge.Application.Interfaces;

public interface IChatMessageRepository
{
    Task<int> SaveMessageAsync(ChatMessage message);

    Task<List<ChatMessage>> GetMessagesAsync(int chatId);
}