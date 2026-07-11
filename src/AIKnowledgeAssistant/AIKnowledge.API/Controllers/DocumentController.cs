using AIKnowledge.Application.Interfaces;
using AIKnowledge.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AIKnowledge.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;

    public DocumentController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] UploadDocumentRequestDto request)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _documentService.UploadDocumentAsync(
            userId,
            request.File);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _documentService.GetDocumentsAsync(userId);

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _documentService.GetDocumentByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _documentService.DeleteDocumentAsync(id);

        return Ok(new
        {
            Message = "Document deleted successfully."
        });
    }


}