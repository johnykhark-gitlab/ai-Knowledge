using AIKnowledge.Application.Features.Chat;
using AIKnowledge.Application.Features.Chat.DTOs;
using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;
using AIKnowledge.Infrastructure.AI.Interfaces;
using System.Diagnostics;

namespace AIKnowledge.Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IOllamaService _ollamaService;
    private readonly IDocumentContentRepository _documentContentRepository;

    public ChatService(
        IChatRepository chatRepository,
        IChatMessageRepository chatMessageRepository,
        IOllamaService ollamaService,
        IDocumentContentRepository documentContentRepository)
    {
        _chatRepository = chatRepository;
        _chatMessageRepository = chatMessageRepository;
        _ollamaService = ollamaService;
        _documentContentRepository = documentContentRepository;
    }



    public async Task<AskQuestionResponse> AskQuestionAsync(
        int userId,
        AskQuestionRequest request)
    {
        bool exists = await _chatRepository.ChatExistsAsync(request.ChatId);

        if (!exists)
            throw new Exception("Chat not found.");

        var stopwatch = Stopwatch.StartNew();

        var chat = await _chatRepository.GetChatAsync(request.ChatId);

        if (chat == null)
            throw new Exception("Chat not found.");

        if (!chat.DocumentId.HasValue)
            throw new Exception("Document not attached.");

        string? documentText =
            await _documentContentRepository.GetDocumentTextAsync(
                chat.DocumentId.Value);

        if (string.IsNullOrWhiteSpace(documentText))
            throw new Exception("Document content not found.");

        var previousMessages =
    await _chatMessageRepository.GetMessagesAsync(request.ChatId);

        var history = string.Join(
            Environment.NewLine,
            previousMessages.Select(m =>
                $"User: {m.UserQuestion}\nAI: {m.AIResponse}"));

        string answer =
     await _ollamaService.AskQuestionAsync(
         documentText,
         history,
         request.Question);

        stopwatch.Stop();

        await _chatMessageRepository.SaveMessageAsync(
            new ChatMessage
            {
                ChatId = request.ChatId,
                UserQuestion = request.Question,
                AIResponse = answer,
                PromptTokens = 0,
                CompletionTokens = 0,
                TotalTokens = 0,
                ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds
            });

        return new AskQuestionResponse
        {
            Answer = answer,
            PromptTokens = 0,
            CompletionTokens = 0,
            TotalTokens = 0,
            ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds
        };
    }

    public async Task<CreateChatResponse> CreateChatAsync(
    int userId,
    CreateChatRequest request)
    {
        var chat = new Chat
        {
            UserId = userId,
            DocumentId = request.DocumentId,
            Title = request.Title,
            CreatedOn = DateTime.Now,
            CreatedBy = "System",
            IsDeleted = false
        };

        int chatId = await _chatRepository.CreateChatAsync(chat);

        return new CreateChatResponse
        {
            ChatId = chatId,
            Message = "Chat created successfully."
        };
    }
}

