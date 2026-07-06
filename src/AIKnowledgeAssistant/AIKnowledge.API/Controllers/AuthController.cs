using AIKnowledge.Application.Features.Auth.Register;
using AIKnowledge.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AIKnowledge.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result =
            await _authService.RegisterAsync(request);

        return Ok(result);
    }
}