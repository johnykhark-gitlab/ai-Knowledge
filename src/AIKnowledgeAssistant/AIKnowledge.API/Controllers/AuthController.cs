using AIKnowledge.Application.Features.Auth.Login;
using AIKnowledge.Application.Features.Auth.Register;
using AIKnowledge.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIKnowledge.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Register New User
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);

        return Ok(result);
    }

    /// <summary>
    /// Login User
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        return Ok(result);
    }

    /// <summary>
    /// Test API
    /// </summary>
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Authentication Module Working Successfully");
    }
}

