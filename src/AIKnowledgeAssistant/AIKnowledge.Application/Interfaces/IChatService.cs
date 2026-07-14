using AIKnowledge.Application.DTOs;
using AIKnowledge.Application.Features.Chat;
using AIKnowledge.Application.Features.Chat.DTOs;

namespace AIKnowledge.Application.Interfaces;

public interface IChatService
{
    Task<CreateChatResponse> CreateChatAsync(
        int userId,
        CreateChatRequest request);

    Task<AskQuestionResponse> AskQuestionAsync(
        int userId,
        AskQuestionRequest request);
}

