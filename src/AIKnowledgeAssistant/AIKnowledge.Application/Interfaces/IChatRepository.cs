using AIKnowledge.Domain.Entities;

namespace AIKnowledge.Application.Interfaces;

public interface IChatRepository
{
    Task<int> CreateChatAsync(Chat chat);

    Task<Chat?> GetChatAsync(int chatId);

    Task<List<Chat>> GetUserChatsAsync(int userId);

    Task<bool> ChatExistsAsync(int chatId);
}