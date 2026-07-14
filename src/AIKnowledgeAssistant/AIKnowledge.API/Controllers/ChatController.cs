using AIKnowledge.Application.Features.Chat;
using AIKnowledge.Application.Features.Chat.DTOs;
using AIKnowledge.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AIKnowledge.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(
        CreateChatRequest request)
    {
        int userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _chatService.CreateChatAsync(
            userId,
            request);

        return Ok(result);
    }

    [HttpPost("ask")]
    public async Task<IActionResult> Ask(
        AskQuestionRequest request)
    {
        int userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _chatService.AskQuestionAsync(
            userId,
            request);

        return Ok(result);
    }
}