using AIKnowledge.Application.Features.Auth.Login;
using AIKnowledge.Application.Features.Auth.Me;
using AIKnowledge.Application.Features.Auth.Register;

namespace AIKnowledge.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);

    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task<MeResponse> GetCurrentUserAsync(int userId);
}